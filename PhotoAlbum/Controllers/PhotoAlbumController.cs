using BLL.Interface.Entity;
using BLL.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoAlbum.Controllers
{
    using Models;
    using Infrastructure;
    using System.Security.Cryptography;
    using System.Text;

    public class PhotoAlbumController : Controller
    {
        private readonly IPictureService pictureService;
        private readonly IPictureProfileService picProfileService;
        private readonly IUserService userService;

        public PhotoAlbumController(IUserService userService, IPictureService pictureService, IPictureProfileService picProfileService)
        {
            this.userService = userService;
            this.pictureService = pictureService;
            this.picProfileService = picProfileService;
        }

        public ActionResult ShowPhotoAlbum(string username)
        {
            if (username == null)
            {
                username = User.Identity.Name;
            }
            else
            {
                TempData["ViewedUser"] = username;
            }

            UserBll user = userService.GetUserEntity(username);
            if (user == null)
                throw new HttpException(404, "Not Found");

            ViewBag.PictureOnPageCount = 10;
            int count = picProfileService
                .GetAllUsersProfiles(user.Id)
                .Count();
            int pageCount = count / ViewBag.PictureOnPageCount;
            if (pageCount != 0 && count % ViewBag.PictureOnPageCount !=0)
            {
                pageCount++;
            }
            ViewBag.PageCount = pageCount;
            return View(user.Id);
        }

        public ActionResult AlbumPage(int userId = 0, int page = 1, int count = 9)
        {
            if (userId <= 0)
                throw new ArgumentException("Invalid user id");
            IEnumerable<PictureProfileModel> pictureProfiles = picProfileService
                .GetAllUsersProfiles(userId)
                .Skip((page - 1) * count)
                .Take(count)
                .Select((profile, index) => Map(profile, index + (page - 1) * count));

            ViewBag.CurrentPage = page;
            return PartialView(pictureProfiles);
        }
        
        [HttpGet]
        public ActionResult AddPicture()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPicture(AddPictureModel picture)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pictureId = CreatePicture(picture);
                    var user = userService.GetUserEntity(User.Identity.Name);

                    var profile = new PictureProfileBll()
                    {
                        Description = picture.Description,
                        LoadingDate = DateTime.Now,
                        PictureId = pictureId,
                        UserId = user.Id,
                    };
                    picProfileService.CreatePictureProfile(profile);

                    TempData["MessageType"] = MessageType.success;
                    TempData["StrongResultMessage"] = "Фото успешно загружено";
                }
                catch (Exception)
                {
                    TempData["MessageType"] = MessageType.info;
                    TempData["StrongResultMessage"] = "Не удалось загрузить новое фото";
                }
                return RedirectToAction("ShowPhotoAlbum", "PhotoAlbum");
            }
            return View(picture);
        }

        [AllowAnonymous]
        public ActionResult GetLastLoadedPictures(int count = 9)
        {
            IEnumerable<PictureProfileModel> pictureProfiles = picProfileService
                .GetAllProfiles()
                .OrderByDescending(pic => pic.LoadingDate)
                .Take(count)
                .Select((profile, index) => Map(profile, index));

            return PartialView("LastLoadedAlbumPage", pictureProfiles);
        }

        public ActionResult ShowPicture(int pictureId = 0, int userId = 0)
        {
            if (pictureId <= 0)
                throw new ArgumentException("Invalid picture id");
            if (userId <= 0)
                throw new ArgumentException("Invalid user id");
                
            UserBll user = userService.GetUserEntity(userId);
            if (user == null)
                throw new HttpException(404, "Not Found");

            var allProfiles = picProfileService.GetAllUsersProfiles(userId);
            var profile = allProfiles
                .Where(pic => pictureId == pic.PictureId)
                .Select((pic, index) => Map(pic, index))
                .FirstOrDefault();

            if (user == null || profile == null || user.Id != profile.UserId) 
                throw new HttpException(404, "Not Found");

            @TempData["ViewedUser"] = user.Username;
            return View(profile);
        }

        public ActionResult ShowPictureByIndex(int index = -1, int userId = 0)
        {
            if (index == -1) throw new ArgumentException("Invalid index");
            if (userId == 0) throw new ArgumentException("Invalid user id");

            UserBll user = userService.GetUserEntity(userId);
            if (user == null) throw new HttpException(404, "Not Found");

            var allProfiles = picProfileService.GetAllUsersProfiles(userId);
            var profile = allProfiles.Skip(index)
                .Select(pic => Map(pic, index))
                .FirstOrDefault();

            if (user == null || profile == null || user.Id != profile.UserId)
                throw new HttpException(404, "Not Found");

            @TempData["ViewedUser"] = user.Username;
            @ViewBag.PicturesCount = allProfiles.Count();

            return View(profile);
        }

        private int CreatePicture(AddPictureModel picture)
        {
            var imageBytes = picture.ImageFile.GetBytes();
            var newPicture = new PictureBll()
            {
                Name = picture.Name,
                Image = imageBytes,
                Hash = GetImageHash(imageBytes),
            };

            pictureService.CreatePicture(newPicture);

            PictureBll addedPic = pictureService
                                    .GetPicturesByHash(newPicture.Hash)
                                    .First(pic => pic.Name == picture.Name);
            return addedPic.Id;
        }

        private string GetImageHash(byte[] image)
        {
            var sha256 = new SHA256Cng();
            return Encoding.Default.GetString(sha256.ComputeHash(image));
        }

        private PictureProfileModel Map(PictureProfileBll profile, int index)
        {
            var modelProfile = new PictureProfileModel()
                {
                    Description = profile.Description,
                    LoadingDate = profile.LoadingDate,
                    PictureId = profile.PictureId,
                    Index = index,
                    UserId = profile.UserId,
                };

            PictureBll picture = pictureService.GetPictureById(profile.PictureId);
            if (picture == null)
                return null;

            modelProfile.Name = picture.Name;
            modelProfile.Image = Convert.ToBase64String(picture.Image);

            return modelProfile;
        }

    }
}
