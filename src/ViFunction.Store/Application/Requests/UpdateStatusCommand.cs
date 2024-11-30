using MediatR;
using ViFunction.Store.Application.Entities;

namespace ViFunction.Store.Application.Requests;

public record UpdateStatusCommand : IRequest
{
    public Guid Id { get; set; }
    public FunctionStatus FunctionStatus { get; set; }
    public string Message { get; set; }
}