using ApiToDo.Infrastructure.Data;
using Helper.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toDoList.Entities.UserAccount;

namespace BusinessLayer.Extensions.DB
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AppDbContext _dbContext;

        public DatabaseInitializer(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            if (!_dbContext.UserAccounts.Any(u => u.Role == 1))
            {
                var adminUser = new UserAccount
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Role = 1,
                    Password = PasswordHasher.HashPassword("admin123")
                };

                _dbContext.UserAccounts.Add(adminUser);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
