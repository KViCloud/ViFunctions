// Data/FunctionsContext.cs

using Microsoft.EntityFrameworkCore;
using ViFunction.DataService.Models;

namespace ViFunction.DataService.Data
{
    public class FunctionsContext(DbContextOptions<FunctionsContext> options) : DbContext(options)
    {
        public DbSet<Function> Functions { get; set; }
    }
}