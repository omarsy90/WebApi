using System.Drawing;

namespace AuthWebApi.Models
{
    public class JWT
    {

        public string Key { get; set; }
        public string  Issuer { get; set; }

        public string Audience { get; set; }

        public int DurationOnDays { get; set; }

    }
}
