using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class EntityNotFoundException : Exception
    {
        private readonly string entityName;
        private readonly int id;

        public EntityNotFoundException(string entityName, int id)
        {
            this.entityName = entityName;
            this.id = id;
        }

        public override string Message
        {
            get
            {
                return String.Format("The {0} with id = {1} doesn't exist", entityName, id);
            }
        }
    }
}
