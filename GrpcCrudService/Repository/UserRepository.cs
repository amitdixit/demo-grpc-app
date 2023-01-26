namespace GrpcCrudService.Repository;


public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
}

public class UserRepository
{
    private static readonly List<User> _users = new List<User>();
    public UserRepository()
    {
    }


    internal List<User> GetUsers() => _users;

    internal User GetUser(int id) => _users.FirstOrDefault(x => x.Id == id);

    internal User AddUser(User messageToCreate)
    {
        _users.Add(messageToCreate);

        return messageToCreate;
    }

    internal User UpdateUser(User messageToUpdate)
    {
        var msg = GetUser(messageToUpdate.Id);
        _users.Remove(msg);
        AddUser(messageToUpdate);
        return messageToUpdate;
    }

    internal void DeleteUser(int id)
    {
        var msg = GetUser(id);
        _users.Remove(msg);
    }
}
