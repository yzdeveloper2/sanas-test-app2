using System.Collections.Generic;
using System.Threading.Tasks;
using PrismApp.Models;

namespace PrismApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
    }
}
