// Data/FunctionsContext.cs

using Microsoft.EntityFrameworkCore;
using ViFunction.Store.Application.Entities;

namespace ViFunction.Store.Infras
{
    public class FunctionsContext(DbContextOptions<FunctionsContext> options) : DbContext(options)
    {
        public DbSet<Function> Functions { get; set; }
    }
}