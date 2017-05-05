using Microsoft.AspNet.Identity;

namespace ElewiceTest.Models.Identity
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store)
            : base(store)
        {
            UserValidator = new UserValidator<User, int>(this);
            PasswordValidator = new PasswordValidator();
        }
    }
}