namespace FakeApplication.UnitTests.Creators
{
    using Moq;

    public class UsernameValidatorCreator
    {
        public static Mock<IUsernameValidator> AlwaysValid()
        {
            var mock = new Mock<IUsernameValidator>();

            mock.Setup(m => m.IsValid(It.IsAny<string>()))
                .Returns(true);

            return mock;
        }

        public static Mock<IUsernameValidator> AlwaysInvalid()
        {
            var mock = new Mock<IUsernameValidator>();

            mock.Setup(m => m.IsValid(It.IsAny<string>()))
                .Returns(false);

            return mock;
        }
    }
}
