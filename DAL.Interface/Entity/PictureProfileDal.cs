using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Entity
{
    public class PictureProfileDal: IDalEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public DateTime? LoadingDate { get; set; }
        public int PictureId { get; set; }
        public int UserId { get; set; }
    }
}
