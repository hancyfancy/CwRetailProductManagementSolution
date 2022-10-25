using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Constants
{
    public static class UserRoleConstant
    {
        static UserRoleConstant()
        {
            StandardUser = "Standard User";
            BronzeUser = "Bronze User";
            SilverUser = "Silver User";
            GoldUser = "Gold User";
            PlatinumUser = "Platinum User";
            StandardSpecialist = "Standard Specialist";
            StandardAdmin = "Standard Admin";
        }

        public static readonly string StandardUser;
        public static readonly string BronzeUser;
        public static readonly string SilverUser;
        public static readonly string GoldUser;
        public static readonly string PlatinumUser;
        public static readonly string StandardSpecialist;
        public static readonly string StandardAdmin;
    }
}
