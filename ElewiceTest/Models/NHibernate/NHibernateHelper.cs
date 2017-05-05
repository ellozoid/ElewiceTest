using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using ElewiceTest.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElewiceTest.Models.NHibernate
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;
        public NHibernateHelper()
        {
            sessionFactory = Fluently.Configure()
     .Database(MsSqlConfiguration.MsSql2008.ConnectionString("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Bogdan\\Documents\\Visual Studio 2015\\Projects\\ElewiceTest\\ElewiceTest\\App_Data\\ElewiceTest.mdf\";Integrated Security=True").ShowSql()
            )
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>())
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            .BuildSessionFactory();
        }
        public static ISession MakeSession()
        {
            return sessionFactory.OpenSession();
        }

        public IUserStore<User, int> Users
        {
            get { return new IdentityStore(MakeSession()); }
        }
    }
}