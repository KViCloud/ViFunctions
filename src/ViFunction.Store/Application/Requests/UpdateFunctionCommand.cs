using MediatR;
using ViFunction.Store.Application.Entities;

namespace ViFunction.Store.Application.Requests;

public record UpdateFunctionCommand : IRequest
{
    public Guid Id { get; set; }
    public FunctionStatus Status { get; set; }
    public string Image { get; set; }
    public string Message { get; set; }
}