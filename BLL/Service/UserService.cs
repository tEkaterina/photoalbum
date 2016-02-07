using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using BLL.Interface.Service;
using BLL.Interface.Entity;
using BLL.BLLMapper;
using DAL.Interface;
using DAL.Interface.Repository;
using DAL.Interface.Entity;

namespace BLL.Service
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserRepository repository;

        public UserService(IUnitOfWork uow, IUserRepository repository)
        {
            this.uow = uow;
            this.repository = repository;
        }

        public UserBll GetUserEntity(int id)
        {
            if (id <= 0)
                throw new InvalidIdException();

            var foundUser = repository.GetById(id);
            if (foundUser == null)
                return null;
            return foundUser.ToBll();
        }

        public UserBll GetUserEntity(string username)
        {
            if (username == null)
                throw new ArgumentNullException("username");

            var foundUser = FindByUsername(username);
            if (foundUser == null)
                return null;
            return foundUser.ToBll();
        }

        public IEnumerable<UserBll> GetAllUserEntities()
        {
            var allUsers = repository.GetAll();
            if (allUsers == null)
                return new UserBll[0];
            return allUsers.Select(user => user.ToBll());
        }

        public void CreateUser(UserBll user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var userDal = user.ToDal();
            
            repository.Create(userDal);
            uow.Commit();
        }

        public void DeleteUser(UserBll user)
        {
            if (user.Id <= 0)
                throw new InvalidIdException();

            var userDal = repository.GetById(user.Id);
            if (userDal == null)
                throw new EntityNotFoundException("user", user.Id);
            repository.Delete(userDal);
        }
               
        private UserDal FindByUsername(string username)
        {
            return repository.GetFirst(user => user.Username == username);
        }
        
        public void UpdateUser(UserBll user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            UserDal currentUser = user.ToDal();
            UserDal existedUser = repository.GetById(user.Id);
            if (existedUser == null)
                throw new EntityNotFoundException("user", user.Id);
            
            existedUser.Username = currentUser.Username;
            existedUser.Email = currentUser.Email;
            existedUser.Password = currentUser.Password;

            repository.Update(existedUser);
            uow.Commit();
        }
    }
}
