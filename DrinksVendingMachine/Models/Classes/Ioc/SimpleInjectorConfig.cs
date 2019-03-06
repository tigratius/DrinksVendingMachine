using System.Reflection;
using System.Web.Mvc;
using DrinksVendingMachine.Controllers;
using DrinksVendingMachine.Models.Classes.Importers;
using DrinksVendingMachine.Models.Classes.Loggers;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using DrinksVendingMachine.Models.Interfaces;
using DrinksVendingMachine.Models.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace DrinksVendingMachine.Models.Classes.Ioc
{
    /// <summary>
    /// Документация
    /// https://simpleinjector.readthedocs.io/en/latest/
    /// </summary>
    public class SimpleInjectorConfig
    {
        public static void RegisterComponents()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.Register<VengingMachineDbContext, VengingMachineDbContext>(Lifestyle.Scoped);
            container.Register<IRepository<DrinkEntity>, DrinkRepo>(Lifestyle.Scoped);
            container.Register<IRepository<CoinEntity>, CoinsRepo>(Lifestyle.Scoped);
            container.Register<ISimpleRepo<CurrentStateEntity>, StateRepo>(Lifestyle.Scoped);

            var adminLog = Lifestyle.Singleton.CreateRegistration(() => new NlogLogger("Admin"), container);
            var userLog = Lifestyle.Singleton.CreateRegistration(() => new NlogLogger("User"), container);

            container.RegisterConditional(typeof(IVendingMachineLogger), adminLog, c => c.Consumer.ImplementationType == typeof(AdminService));
            container.RegisterConditional(typeof(IVendingMachineLogger), adminLog, c => c.Consumer.ImplementationType == typeof(AdminController));
            container.RegisterConditional(typeof(IVendingMachineLogger), userLog, c => c.Consumer.ImplementationType == typeof(UserController));
            container.RegisterConditional(typeof(IVendingMachineLogger), userLog, c => c.Consumer.ImplementationType == typeof(UserService));

            container.Register<IStrategy, CsvImport>();
            container.Register<IAdminService, AdminService>();
            container.Register<IUserService, UserService>();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}