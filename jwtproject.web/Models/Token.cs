using jwtproject.web.Models;
using System;

namespace jwtproject.Web.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
        public User User { get; set; }
    }
}
