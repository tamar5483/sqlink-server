using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Services.Entities
{
    public class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identity { get; set; }
        public int Age { get; set; }
        public bool IsValid { get; set; }
    }
}
