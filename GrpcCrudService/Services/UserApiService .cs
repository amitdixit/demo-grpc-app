using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcCrudService.Repository;

namespace GrpcCrudService.Services;
public class UserApiService : UserService.UserServiceBase
{
    private readonly ILogger<UserApiService> _logger;
    private readonly UserRepository _userRepository;

    public UserApiService(ILogger<UserApiService> logger, UserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public override Task<ListReply> ListUsers(Empty request, ServerCallContext context)
    {
        var items = _userRepository.GetUsers();

        var listReply = new ListReply();
        var messageList = items.Select(item => new UserMessageReply() { Id = item.Id, UserName = item.UserName }).ToList();
        listReply.UserMessages.AddRange(messageList);
        return Task.FromResult(listReply);
    }

    public override Task<UserMessageReply> GetUser(GetUserMessageRequest request, ServerCallContext context)
    {
        var message = _userRepository.GetUser(request.Id);
        return Task.FromResult(new UserMessageReply() { Id = message.Id, UserName = message.UserName });
    }

    public override Task<UserMessageReply> CreateUser(CreateUserMessageRequest request, ServerCallContext context)
    {
        var messageToCreate = new User()
        {
            Id = request.Id,
            UserName = request.UserName
        };

        var createdMessage = _userRepository.AddUser(messageToCreate);

        var reply = new UserMessageReply() { Id = createdMessage.Id, UserName = createdMessage.UserName };

        return Task.FromResult(reply);
    }

    public override Task<UserMessageReply> UpdateUser(UpdateUserMessageRequest request, ServerCallContext context)
    {
        var existingMessage = _userRepository.GetUser(request.Id);

        if (existingMessage == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Advent message not found"));
        }

        var messageToUpdate = new User()
        {
            Id = request.Id,
            UserName = request.UserName
        };

        var createdMessage = _userRepository.UpdateUser(messageToUpdate);

        var reply = new UserMessageReply() { Id = createdMessage.Id, UserName = createdMessage.UserName };

        return Task.FromResult(reply);
    }

    public override Task<Empty> DeleteUser(DeleteUserMessageRequest request, ServerCallContext context)
    {
        var existingMessage = _userRepository.GetUser(request.Id);

        if (existingMessage == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        _userRepository.DeleteUser(request.Id);

        return Task.FromResult(new Empty());
    }

    public override async Task GetNewUsers(NewUserMessageRequest request, IServerStreamWriter<UserMessageRequest> responseStream, ServerCallContext context)
    {
        var newUsers = new List<UserMessageRequest>()
        {
            new UserMessageRequest{Id = 1, UserName ="Tom"},
            new UserMessageRequest{Id = 2, UserName ="Dick"},
            new UserMessageRequest{Id = 3, UserName ="Harry"},
            new UserMessageRequest{Id = 4, UserName ="Dexter"},
            new UserMessageRequest{Id = 5, UserName ="Debra"},
            new UserMessageRequest{Id = 6, UserName ="Angel"},
            new UserMessageRequest{Id = 7, UserName ="Quinn"},
            new UserMessageRequest{Id = 8, UserName ="Trinity"}
        };

        foreach (var user in newUsers)
        {
            await Task.Delay(2000);
            await responseStream.WriteAsync(user);
        }
    }
}
