using Microsoft.EntityFrameworkCore;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;

namespace ViFunction.Store.Infras;

public class FunctionRepository(FunctionsContext context) : IRepository<Function>
{
    public async Task<List<Function>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Functions.ToListAsync(cancellationToken);
    }

    public async Task<List<Function>> GetAllAsync() => await context.Functions.ToListAsync();

    public async Task<Function> GetByIdAsync(Guid id) => await context.Functions.FindAsync(id);

    public async Task<Function> AddAsync(Function function)
    {
        context.Functions.Add(function);
        await context.SaveChangesAsync();
        return function;
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public async Task<bool> DeleteAsync(Guid id)
    {
        var function = await context.Functions.FindAsync(id);
        if (function == null) return false;

        context.Functions.Remove(function);
        await context.SaveChangesAsync();
        return true;
    }
}