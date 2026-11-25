using ERP.Context;
using Microsoft.EntityFrameworkCore;

namespace ERP.Tests
{
    internal static class TestContext
    {
        public static DataContext Create() => new(new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    }
}
