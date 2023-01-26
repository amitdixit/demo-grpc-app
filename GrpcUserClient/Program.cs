
using Grpc.Net.Client;

using (var channel = GrpcChannel.ForAddress("https://localhost:7234"))
{
    var client = new GrpcCrudServiceClient.UserService.UserServiceClient(channel);

    Console.WriteLine($"Saving Users");
    var reply = await client.CreateUserAsync(new GrpcCrudServiceClient.CreateUserMessageRequest
    {
        Id = 1,
        UserName = "Amit"
    });

    reply = await client.CreateUserAsync(new GrpcCrudServiceClient.CreateUserMessageRequest
    {
        Id = 2,
        UserName = "Aashita"
    });

    reply = await client.CreateUserAsync(new GrpcCrudServiceClient.CreateUserMessageRequest
    {
        Id = 3,
        UserName = "Pinaak"
    });


    Console.WriteLine($"Get All Users");

    var users = await client.ListUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());

    foreach (var user in users.UserMessages)
    {
        Console.WriteLine($"Hello {user.UserName}");
    }

    Console.WriteLine($"Get Single Users");

    var singleUser = await client.GetUserAsync(new GrpcCrudServiceClient.GetUserMessageRequest { Id = 1 });

    Console.WriteLine($"Hello {singleUser.UserName}");

    Console.WriteLine($"Updating the User");

    singleUser.UserName = "Updated Name";

    var updatedUser = await client.UpdateUserAsync(new GrpcCrudServiceClient.UpdateUserMessageRequest
    {
        Id = singleUser.Id,
        UserName = singleUser.UserName,
    });

    Console.WriteLine($"Users after update");

    users = await client.ListUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());

    foreach (var user in users.UserMessages)
    {
        Console.WriteLine($"Hello {user.UserName}");
    }

    var deleteUser = await client.DeleteUserAsync(new GrpcCrudServiceClient.DeleteUserMessageRequest { Id = 1 });


    Console.WriteLine($"Users after Delete");

    users = await client.ListUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());

    foreach (var user in users.UserMessages)
    {
        Console.WriteLine($"Hello {user.UserName}");
    }

}
//var client = new GrpcCrudServiceClient.UserService(channel);// UserApiService Greeter.GreeterClient(channel);
//var reply = await client.SayHelloAsync(
//                  new HelloRequest { Name = "GreeterClient" });
//Console.WriteLine("Greeting: " + reply.Message);
//Console.WriteLine("Press any key to exit...");
Console.ReadKey();
