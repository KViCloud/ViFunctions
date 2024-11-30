using MediatR;

namespace ViFunction.Store.Application.Requests;

public record DeleteFunctionCommand(Guid Id) : IRequest;