using DBModel.Managers;
using DBModel.Models;
using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Threading.Tasks;


namespace DBModel.Models.Identity
{
    public class IdentityStore : IUserStore<User, int>,
        IUserPasswordStore<User, int>,
        IUserLockoutStore<User, int>,
        IUserTwoFactorStore<User, int>
    {
        private UserRepositoryManager userManager;

        public IdentityStore()
        {
            userManager = new UserRepositoryManager();
        }

        #region IUserStore<User, int>
        public Task CreateAsync(User user)
        {
            return Task.Run(() => userManager.Save(user));
        }

        public Task DeleteAsync(User user)
        {
            return Task.Run(() => userManager.Delete(user));
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.Run(() => userManager.Find(userId));
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                return userManager.Find(userName);
            });
        }

        public Task UpdateAsync(User user)
        {
            return Task.Run(() => userManager.Save(user));
        }
        #endregion

        #region IUserPasswordStore<User, int>
        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.Run(() => user.PasswordHash = passwordHash);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(true);
        }
        #endregion

        #region IUserLockoutStore<User, int>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task.FromResult(DateTimeOffset.MaxValue);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult(0);
        }
        #endregion

        #region IUserTwoFactorStore<User, int>
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }
        #endregion

        public void Dispose()
        {
            //do nothing
        }
    }
}