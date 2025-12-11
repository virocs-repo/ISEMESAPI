namespace ISEMES.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool External { get; set; }
        public string Email { get; set; }
        public string IdToken { get; set; } // Added IdToken property for internal users
    }
}

