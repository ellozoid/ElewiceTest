using DBModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetCurrentUser(string userName);
    }
}
