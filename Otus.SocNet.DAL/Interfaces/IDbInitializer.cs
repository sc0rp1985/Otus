using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.DAL
{
    public interface IDbInitializer
    {
        Task InitializeAsync(string connectionString);
    }
}
