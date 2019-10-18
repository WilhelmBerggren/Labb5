using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb5
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public UserModel(string Name, string Email)
        {
            this.Name = Name;
            this.Email = Email;
        }
    }
}
