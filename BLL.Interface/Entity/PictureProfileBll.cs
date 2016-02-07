using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entity
{
    public class PictureProfileBll: IBllEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public DateTime? LoadingDate { get; set; }
        public int PictureId { get; set; }
        public int UserId { get; set; }
    }
}
