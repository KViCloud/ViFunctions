using Mapster;
using MediatR;
using ViFunction.Store.Application.Dtos;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;

namespace ViFunction.Store.Application.Requests.Handlers;

public class GetFunctionByIdHandler(IRepository<Function> repository) : IRequestHandler<GetFunctionByIdQuery, FunctionDto>
{
    public async Task<FunctionDto> Handle(GetFunctionByIdQuery request, CancellationToken cancellationToken)
    {
        var function = await repository.GetByIdAsync(request.Id);
        return function.Adapt<FunctionDto>();
    }
}