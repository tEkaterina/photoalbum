using BLL.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Service
{
    public interface IPictureProfileService
    {
        PictureProfileBll GetProfileById(int id);
        IEnumerable<PictureProfileBll> GetAllProfiles();
        IEnumerable<PictureProfileBll> GetAllUsersProfiles(int userId);

        void CreatePictureProfile(PictureProfileBll profile);
        void DeletePictureProfile(PictureProfileBll profile);
        void UpdatePictureProfile(PictureProfileBll profile);
    }
}
