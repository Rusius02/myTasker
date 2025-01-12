using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Member
    {
        public int Id { get; set; } // Identifiant unique
        public string Name { get; set; } // Nom du membre
        public string Role { get; set; } // Rôle dans le projet (ex: Développeur, Designer)
        public string Email { get; set; } // Email du membre

        public Member()
        {
        }
    }
}
