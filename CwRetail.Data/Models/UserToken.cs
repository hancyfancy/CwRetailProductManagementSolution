using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Models
{
    public class UserToken
    {
        public long UserTokenId { get; set; }

        public string Token { get; set; }

        public DateTime RefreshAt { get; set; }
    }
}
