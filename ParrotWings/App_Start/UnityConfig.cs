using System;
using Microsoft.Practices.Unity;
using TransactionSubsystem.Repositories;
using System.Data.Entity;
using TransactionSubsystem.Infrastructure.UnitOfWork.Abstract;
using TransactionSubsystem.Infrastructure.UnitOfWork.Implementation;
using TransactionSubsystem.Infrastructure.Repositories.Abstract;
using TransactionSubsystem.Infrastructure.Services.Abstract;
using TransactionSubsystem.Infrastructure.Repositories.Implementation;
using TransactionSubsystem.Infrastructure.Services.Implementation;

namespace ParrotWings.App_Start
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterComponents(container);
            return container;
        });

        private static TransactionSubsystemContext _context;

        public static IUnityContainer GetConfiguredContainer()
        {
            return _container.Value;
        }

        public static TransactionSubsystemContext GetContext()
        {
            return _context ?? (_context = new TransactionSubsystemContext());
        }

        public static void RegisterComponents(IUnityContainer container)
        {
            // Context, UoW factory
            container.RegisterType<DbContext, TransactionSubsystemContext>(
                new PerRequestLifetimeManager());
            var context = container.Resolve<DbContext>();
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>(
                new PerRequestLifetimeManager(),
                new InjectionConstructor(context));

            // Repositories
            container.RegisterType<IUserRepository, UserRepository>(
                new PerRequestLifetimeManager(),
                new InjectionConstructor(context));
            var userRepository = container.Resolve<IUserRepository>();

            // Services
            container.RegisterType<ISecurityService, SecurityService>(new PerRequestLifetimeManager());
            var securityService = container.Resolve<ISecurityService>();

            container.RegisterType<IAuthenticationService, AuthenticationService>(
                new PerRequestLifetimeManager(),
                new InjectionConstructor(userRepository, securityService));

            container.RegisterType<IUserProvider, UserProvider>(
                new PerRequestLifetimeManager(),
                new InjectionConstructor(userRepository, securityService));
        }
    }
}
