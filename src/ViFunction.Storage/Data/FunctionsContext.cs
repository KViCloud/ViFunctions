// Data/FunctionsContext.cs

using Microsoft.EntityFrameworkCore;
using ViFunction.Storage.Models;

namespace ViFunction.Storage.Data
{
    public class FunctionsContext(DbContextOptions<FunctionsContext> options) : DbContext(options)
    {
        public DbSet<Function> Functions { get; set; }
    }
}