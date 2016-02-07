using System;
using System.Collections.Generic;
using BLL.Interface.Entity;

namespace BLL.Interface.Service
{
    public interface IAvatarService
    {
        AvatarBll GetById(int id);
        void CreateAvatar(AvatarBll avatar);
        void UpdateAvatar(AvatarBll avatar);
    }
}
