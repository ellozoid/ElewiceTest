using DBModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBModel.Models;
using DBModel.Helpers;
using Microsoft.AspNet.Identity;
using DBModel.Models.Identity;
using NHibernate;

namespace DBModel.Managers
{
    public class UserRepositoryManager : IUserRepository
    {
        private ISession session;
        private NHibernateHelper helper;

        public IUserStore<User, int> Users
        {
            get { return new IdentityStore(); }
        }

        public UserRepositoryManager()
        {
            helper = new NHibernateHelper();
            session = helper.MakeSession();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetCurrentUser(string userName)
        {
            return Users.FindByNameAsync(userName).Result;
        }

        public void Save(User user)
        {
            session.SaveOrUpdate(user);
        }

        public void Delete(User user)
        {
            session.Delete(user);
        }

        public User Find(int userId)
        {
            return session.Get<User>(userId);
        }

        public User Find(string userName)
        {
            return session.QueryOver<User>()
                    .Where(u => u.UserName == userName)
                    .SingleOrDefault();
        }
    }
}
