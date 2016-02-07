using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Entity
{
    public class AvatarDal: IDalEntity
    {
        public byte[] Image { get; set; }
        public int Id { get; set; }
    }
}
