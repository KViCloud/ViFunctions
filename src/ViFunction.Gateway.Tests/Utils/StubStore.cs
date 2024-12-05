using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Tests.Utils
{
    public class StubStore : IStore
    {
        public Task<List<FunctionDto>> GetAllFunctionsAsync()
        {
            // Return a hard-coded list of FunctionDto objects
            return Task.FromResult(new List<FunctionDto>
            {
                new FunctionDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Function1",
                    Language = "C#",
                    Status = FunctionStatus.Built
                },
                new FunctionDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Function2",
                    Language = "Python",
                    Status = FunctionStatus.Deployed
                }
            });
        }

        public Task<FunctionDto> GetFunctionByIdAsync(Guid id)
        {
            // Return a FunctionDto object with the provided id
            return Task.FromResult(new FunctionDto
            {
                Id = Guid.NewGuid(),
                Name = "SampleFunction",
                Language = "JavaScript",
                Status = FunctionStatus.None
            });
        }

        public Task<ApiResponse<FunctionDto>> CreateFunctionAsync(CreateFunctionRequest request)
        {
            // Simulate API response object
            var functionDto = new FunctionDto
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Language = request.Language,
                LanguageVersion = request.LanguageVersion,
                UserId = request.UserId,
                Cluster = request.Cluster,
                Tier = request.Tier,
                Status = FunctionStatus.Built
            };
            var response = new ApiResponse<FunctionDto>(
                new HttpResponseMessage(HttpStatusCode.Created),
                functionDto, new RefitSettings());
            return Task.FromResult(response);
        }

        public Task<IApiResponse> UpdateFunctionAsync(Guid id, UpdateFunctionRequest request)
        {
            var apiResponse = new ApiResponse<bool>(new HttpResponseMessage(HttpStatusCode.OK),
                true, new RefitSettings());
            return Task.FromResult(apiResponse as IApiResponse);
        }

        public Task<IApiResponse> DeleteFunctionAsync(Guid id)
        {
            var apiResponse = new ApiResponse<bool>(new HttpResponseMessage(HttpStatusCode.OK),
                true, new RefitSettings());
            return Task.FromResult(apiResponse as IApiResponse);
        }
    }
}