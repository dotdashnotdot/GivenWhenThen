namespace FakeApplication.UnitTests.Advanced
{
    using FakeApplication.UnitTests.Creators;
    using GivenWhenThen;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Example showing how to store multiple types of dependency under the same key
    /// </summary>
    public class KeySharing
    {
        public const string ValidatorDependency = nameof(ValidatorDependency);

        [TestCaseSource(nameof(TestCases))]
        public void TestThat(
            GivenWhenThenTestCase toRun)
        {
            toRun.RunTestCase();
        }

        public static IEnumerable<GivenWhenThenTestCaseData> TestCases
        {
            get
            {
                yield return GivenWhenThen.CreateTestCase(
                    Given.UsernameValidationFails,
                    When.FredTriesToLogOn,
                    Then.UsernameWasCheckedAndDatabaseWasNotInvoked);
            }
        }

        public static class Given
        {
            public static TestState<LogonManager> UsernameValidationFails()
            {
                return CreateTestState(
                    usernameValidator: UsernameValidatorCreator.AlwaysInvalid());
            }

            private static TestState<LogonManager> CreateTestState(
                Mock<IDatabase> database = null,
                Mock<IUsernameValidator> usernameValidator = null)
            {
                database ??= DatabaseCreator.EmptyDatabase();
                usernameValidator ??= UsernameValidatorCreator.AlwaysValid();

                var result = new TestState<LogonManager>();
                result.StoreDependency(ValidatorDependency, usernameValidator);
                result.StoreDependency(ValidatorDependency, database);

                result.TestObject = new LogonManager(
                    database.Object,
                    usernameValidator.Object);

                return result;
            }
        }

        public static class When
        {
            public static bool FredTriesToLogOn(LogonManager testObject)
            {
                return testObject.TryLogon("Fred");
            }
        }

        public static class Then
        {
            public static void UsernameWasCheckedAndDatabaseWasNotInvoked(
                TestState<LogonManager> testState,
                Action act)
            {
                act();
                var validator = testState.GetDependency<Mock<IUsernameValidator>>(ValidatorDependency);
                validator.Verify(m => m.IsValid(It.IsAny<string>()), Times.Once);
                var database = testState.GetDependency<Mock<IDatabase>>(ValidatorDependency);
                database.Verify(m => m.UserExists(It.IsAny<string>()), Times.Never);
            }
        }
    }
}