namespace FakeApplication.UnitTests.Creators
{
    using Moq;

    public class DatabaseCreator
    {
        public static Mock<IDatabase> EmptyDatabase()
        {
            var databaseMock = new Mock<IDatabase>();

            databaseMock.Setup(m => m.UserExists(It.IsAny<string>()))
                .Returns(false);

            return databaseMock;
        }

        public static Mock<IDatabase> ContainsUser(string username)
        {
            var databaseMock = new Mock<IDatabase>();

            databaseMock.Setup(m => m.UserExists(It.IsAny<string>()))
                .Returns(false);
            databaseMock.Setup(m => m.UserExists(username))
                .Returns(true);

            return databaseMock;
        }
    }
}
