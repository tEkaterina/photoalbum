using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entity
{
    public class RoleBll: IBllEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
    }
}
