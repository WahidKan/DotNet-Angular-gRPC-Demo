using Grpc.Core;
using GrpcDemo.Services;

namespace GrpcDemo.Services
{
    public class UserServiceImpl : UserService.UserServiceBase
    {
        public override Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
        {
            var response = new UserResponse
            {
                UserId = request.UserId,
                Name = "John Doe",
                Email = "john.doe@example.com"
            };

            return Task.FromResult(response);
        }
    }
}
