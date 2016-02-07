using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entity
{
    public class AvatarBll: IBllEntity
    {
        public byte[] Image { get; set; }
        public int Id { get; set; }
    }
}
