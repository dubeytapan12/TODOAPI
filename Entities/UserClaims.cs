using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TODO.Entities
{
    public partial class UserClaims
    {
        public int ClaimId { get; set; }
        public int? UserId { get; set; }
        public string ClaimType { get; set; }
        public bool? ClaimValue { get; set; }
        [JsonIgnore]
        public virtual UserDetails User { get; set; }
    }
}
