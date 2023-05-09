using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace AuthWebApi.Models
{
    public class LoginModel
    {

        [Required]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }

    }
}
