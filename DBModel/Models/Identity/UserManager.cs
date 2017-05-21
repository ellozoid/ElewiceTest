using System;
using System.Collections.Generic;
using DBModel.Helpers;
using DBModel.Interfaces;
using DBModel.Models;
using Microsoft.AspNet.Identity;

namespace DBModel.Models.Identity
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