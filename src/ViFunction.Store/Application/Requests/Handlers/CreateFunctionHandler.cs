using System.Text.RegularExpressions;
using MediatR;
using ViFunction.Store.Application.Dtos;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;
using Mapster;

namespace ViFunction.Store.Application.Requests.Handlers;

public class CreateFunctionHandler(IRepository<Function> repository) : IRequestHandler<CreateFunctionCommand, FunctionDto>
{
    public async Task<FunctionDto> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
    {
        var sanitizedFunctionName = Regex.Replace(request.Name, "[^a-zA-Z0-9]", "").ToLower();
        var function = new Function(
            name: request.Name,
            image: $"{sanitizedFunctionName}",
            language: request.Language,
            languageVersion: request.LanguageVersion,
            userId: request.UserId,
            kubernetesName: sanitizedFunctionName
        );
        
        function.SetCluster(request.Cluster);
        //Hard code first, should resolve by tier
        function.SetResource(request.Tier,"100m","128Mi", "200m", "256Mi");

        await repository.AddAsync(function);
        return function.Adapt<FunctionDto>();
    }
}