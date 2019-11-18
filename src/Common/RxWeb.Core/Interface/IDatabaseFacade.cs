using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RxWeb.Core
{
    public interface IDatabaseFacade
    {
        DatabaseFacade Database { get; }
    }
}
