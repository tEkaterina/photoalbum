using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class InvalidIdException: Exception
    {
        public InvalidIdException() { }

        public override string Message
        {
            get
            {
                return "The id must be greater than zero";
            }
        }
    }
}
