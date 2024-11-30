using MediatR;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;

namespace ViFunction.Store.Application.Requests.Handlers;

public class DeleteFunctionHandler(IRepository<Function> repository)
    : IRequestHandler<DeleteFunctionCommand>
{
    public async Task Handle(DeleteFunctionCommand request, CancellationToken cancellationToken) => 
        await repository.DeleteAsync(request.Id);
}