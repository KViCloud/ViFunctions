using MediatR;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;

namespace ViFunction.Store.Application.Requests.Handlers;

public class UpdateStatusHandler(IRepository<Function> repository) : IRequestHandler<UpdateStatusCommand>
{
    public async Task Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var func = await repository.GetByIdAsync(request.Id);
        func.SetStatus(request.FunctionStatus, request.Message);
        await repository.SaveChangesAsync();
    }
}