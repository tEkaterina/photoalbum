using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ORM
{
    public class PictureProfile : IEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public DateTime? LoadingDate { get; set; }

        public int PictureId { get; set; }
        public virtual Picture Picture { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
