using Microsoft.AspNetCore.Identity;

namespace AuthWebApi.Data
{
    public class MyUser : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}