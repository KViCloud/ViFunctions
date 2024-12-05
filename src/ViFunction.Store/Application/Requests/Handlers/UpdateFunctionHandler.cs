using MediatR;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;

namespace ViFunction.Store.Application.Requests.Handlers;

public class UpdateFunctionCommandHandler(IRepository<Function> repository) : IRequestHandler<UpdateFunctionCommand>
{
    public async Task Handle(UpdateFunctionCommand request, CancellationToken cancellationToken)
    {
        var func = await repository.GetByIdAsync(request.Id);
        
        func.SetStatus(request.Status, request.Message);
        
        if (!string.IsNullOrWhiteSpace(request.Image))
            func.SetImage(request.Image);

        await repository.SaveChangesAsync();
    }
}