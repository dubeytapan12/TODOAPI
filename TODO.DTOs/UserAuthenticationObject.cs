using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TODO.TODO.DTOs
{
    public class UserAuthenticationObject
    {
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string BearerToken { get; set; }

        public bool canAccessTODO { get; set; }
        public bool canAccessDashboard { get; set; }
        public bool canAccessAdmin { get; set; }

    }
}
