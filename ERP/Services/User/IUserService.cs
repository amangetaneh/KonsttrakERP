using ERP.Models;

namespace ERP.Services.User
{
    public interface IUserService
    {
        public Employee Employee { get; }

        string GetMyName();

        int GetMyId();

        int GetMySiteId();
    }
}
