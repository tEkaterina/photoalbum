using BLL.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Service
{
    public interface IUserService
    {
        UserBll GetUserEntity(int id);
        UserBll GetUserEntity(string username);
        IEnumerable<UserBll> GetAllUserEntities();
        
        void CreateUser(UserBll user);
        void DeleteUser(UserBll user);
        void UpdateUser(UserBll user);
    }
}
