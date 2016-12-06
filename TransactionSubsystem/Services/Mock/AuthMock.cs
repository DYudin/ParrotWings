using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using TransactionSubsystem.Entities;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Services.Mock
{
    public class AuthMock : IAuthenticationService
    {
        public AuthMock(IUserRepository userRepository)
        {
            var user = userRepository.GetSingle(x => x.Name == "Dmitriy Yudin");
            CurrentUser = user;
        }
        public User CurrentUser { get; set; }

        public Task<bool> Login(string email, string password)
        {
            return Task.Run(() => false);
        }
    }
}
