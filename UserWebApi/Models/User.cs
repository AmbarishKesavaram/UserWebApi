using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWebApi.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string CountryName{ get; set; }

        public string PhoneNumber { get; set; }

    }
}
