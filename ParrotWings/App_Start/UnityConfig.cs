using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Interfaces;
using TransactionSubsystem.Repositories;
using TransactionSubsystem.Repositories.Abstract;
using TransactionSubsystem.Repositories.Implementation;
using TransactionSubsystem.Services.Implementation;
using Unity.WebApi;

namespace ParrotWings.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Context
            var context = new TransactionSubsystemContext();

            // Repositories
            container.RegisterType<IUserRepository, UserRepository>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(context));
            container.RegisterType<ITransactionRepository, TransactionRepository>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(context));
            container.RegisterType<ISecurityService, SecurityService>();

            var userRepository = container.Resolve<IUserRepository>();
            var transactionRepository = container.Resolve<ITransactionRepository>();

            // Services
            var securityService = container.Resolve<ISecurityService>();
            
            container.RegisterType<IAuthenticationService, AuthenticationService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(userRepository, securityService));
            container.RegisterType<IAmountVerificationService, AmountVerificationService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(userRepository));
            container.RegisterType<ITransactionService, TransactionService>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(userRepository, transactionRepository));
            container.RegisterType<IUserProvider, UserProvider>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(userRepository, securityService));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
