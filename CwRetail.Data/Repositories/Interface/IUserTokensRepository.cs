using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories.Interface
{
    public interface IUserTokensRepository
    {
        int InsertOrUpdate(long userId, string token);
    }
}
