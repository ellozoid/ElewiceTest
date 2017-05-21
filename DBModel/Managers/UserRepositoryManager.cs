using DBModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBModel.Models;
using DBModel.Helpers;

namespace DBModel.Managers
{
    public class UserRepositoryManager : IUserRepository
    {
        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetCurrentUser(string userName)
        {
            return new NHibernateHelper().Users.FindByNameAsync(userName).Result;
        }
    }
}
