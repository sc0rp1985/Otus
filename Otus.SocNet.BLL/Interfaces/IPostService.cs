using Otus.SocNet.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.SocNet.BLL
{
    public interface IPostService
    {
        Task<Post> CreatePost(int autorid, string text);
        Task RebuildFeedForUser(int userId);
    }
}
