
using Microsoft.Practices.Unity;
using Interfaces;
using TransactionSubsystem.Repositories;
using TransactionSubsystem.Repositories.Abstract;
using TransactionSubsystem.Repositories.Implementation;
using TransactionSubsystem.Services.Implementation;

namespace ParrotWings.App_Start
{
    public static class UnityConfig
    {
        private static UnityContainer _container;
        private static TransactionSubsystemContext _context;

        public static UnityContainer GetConfiguredContainer()
        {
            return _container ?? (_container = new UnityContainer());
        }

        public static TransactionSubsystemContext GetContext()
        {
            return _context ?? (_context = new TransactionSubsystemContext());
        }

        public static void RegisterComponents()
        {     
            // Repositories
            _container.RegisterType<IUserRepository, UserRepository>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(_context));
            _container.RegisterType<ITransactionRepository, TransactionRepository>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(_context));
            _container.RegisterType<ISecurityService, SecurityService>();

            var userRepository = _container.Resolve<IUserRepository>();
            var transactionRepository = _container.Resolve<ITransactionRepository>();

            // Services
            var securityService = _container.Resolve<ISecurityService>();

            _container.RegisterType<IAuthenticationService, AuthenticationService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(userRepository, securityService));
            _container.RegisterType<ITransactionService, TransactionService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(userRepository, transactionRepository));
            _container.RegisterType<IUserProvider, UserProvider>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(userRepository, securityService));            
        }
    }
}
