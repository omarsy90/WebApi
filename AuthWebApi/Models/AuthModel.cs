namespace AuthWebApi.Models
{
    public class AuthModel
    {

        public string  UserName { get; set; }
        public string Email { get; set; }

        public List<string> Rolles { get; set; }

        public string Token { get; set; }

        public DateTime ExpireOn { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Message { get; set; }




    }
}
