using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.DAL
{
    public class User
    {
        public int Id { get; set; }
        public string First_Name { get; set; } = null!;
        public string Second_Name { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string Biography { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
