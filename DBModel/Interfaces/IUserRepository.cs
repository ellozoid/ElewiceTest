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
        void Delete(User user);
        User Find(int userId);
        User Find(string userName);
        User GetCurrentUser(string userName);
    }
}
