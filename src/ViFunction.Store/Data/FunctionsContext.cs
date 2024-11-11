// Data/FunctionsContext.cs

using Microsoft.EntityFrameworkCore;
using ViFunction.Store.Models;

namespace ViFunction.Store.Data
{
    public class FunctionsContext : DbContext
    {
        public FunctionsContext(DbContextOptions<FunctionsContext> options) : base(options) { }

        public DbSet<Function> Functions { get; set; }
    }
}