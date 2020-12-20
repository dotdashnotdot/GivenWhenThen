namespace FakeApplication
{
    public interface IDatabase
    {
        bool UserExists(string username);
    }
}
