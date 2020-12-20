namespace FakeApplication
{
    public class LogonManager
    {
        public IDatabase Database { get; }
        public IUsernameValidator UsernameValidator { get; }

        public LogonManager(
            IDatabase database,
            IUsernameValidator usernameValidator)
        {
            this.Database = database;
            this.UsernameValidator = usernameValidator;
        }

        public bool TryLogon(string username)
        {
            if(!this.UsernameValidator.IsValid(username))
            {
                return false;
            }

            if(!this.Database.UserExists(username))
            {
                return false;
            }

            return true;
        }

        public int GetSubscriptionLength(string username)
            => 31;

        public int GetRemainingLogonAttempts(string username)
            => 3;
    }
}
