using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TODO.Entities
{
    public partial class UserDetails
    {
        public UserDetails()
        {
            UserClaims = new HashSet<UserClaims>();
        }

        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserClaims> UserClaims { get; set; }
    }
}
