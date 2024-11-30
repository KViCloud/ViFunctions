using Mapster;
using MediatR;
using ViFunction.Store.Application.Dtos;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;

namespace ViFunction.Store.Application.Requests.Handlers;

public class GetAllFunctionsHandler(IRepository<Function> repository)
    : IRequestHandler<GetAllFunctionsQuery, List<FunctionDto>>
{
    public async Task<List<FunctionDto>> Handle(GetAllFunctionsQuery request, CancellationToken cancellationToken)
    {
        var functions = await repository.GetAllAsync();
        return functions.Adapt<List<FunctionDto>>();
    }
}