// Data/FunctionsContext.cs

using Microsoft.EntityFrameworkCore;
using ViFunction.Repository.Models;

namespace ViFunction.Repository.Data
{
    public class FunctionsContext(DbContextOptions<FunctionsContext> options) : DbContext(options)
    {
        public DbSet<Function> Functions { get; set; }
    }
}