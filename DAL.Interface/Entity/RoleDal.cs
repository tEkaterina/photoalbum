using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Entity
{
    public class RoleDal: IDalEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
    }
}
