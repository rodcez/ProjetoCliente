using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_CRUD.Models
{
    public class ServiceResponse<T>
    {
        public ServiceResponse()
        {
            Messages = new List<string>();
        }

        public T Value { get; set; }
        public bool Sucess { get; set; } = true;
        public List<string> Messages { get; set; }
    }
}
