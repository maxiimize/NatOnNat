using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class AppConstants
    {
        public const string AppName = "NätOnNät";
        public const string Version = "1.0.0";
        public const string DefaultConnectionStringName = "DefaultConnection";

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Customer = "Customer";
        }

        public static class Policies
        {
            public const string RequireAdminRole = "RequireAdminRole";
        }
    }
}
