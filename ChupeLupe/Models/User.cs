using System;

namespace ChupeLupe.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string RegistrationToken { get; set; }
    }
}
