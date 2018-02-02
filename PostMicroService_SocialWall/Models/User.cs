using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PostMicroService_SocialWall.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool Active { get; set; }
    }
}