using System.ComponentModel.DataAnnotations;
namespace DynaDevAPI.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }

        //public class TokenResponse
        //{
        //    public string Token { get; set; }
        //    public string Role { get; set; }
        //}
    }
}
