using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Srez.Models
{
    public class Reader
    {
        [JsonPropertyName ("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("middleName")]
        public string MiddleName { get; set; }
        [JsonPropertyName("photo")]

        public byte[] Photo { get; set; }
        public string fio
        {
            get
            {
                char a = FirstName.FirstOrDefault();
                char b = MiddleName.FirstOrDefault();
                return LastName + " " + a + ". " + b + ".";
            }
        }
    }
}
