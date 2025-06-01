using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.DAL
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);       
        Task<int> CreateAsync(User user);
        Task<IEnumerable<User>> SearchUsers(string firstName, string secondName);
    }
}
