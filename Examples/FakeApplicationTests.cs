namespace FakeApplication.UnitTests
{
    using FakeApplication.UnitTests.Creators;
    using GivenWhenThen;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    public class FakeApplicationTests
    {
        public const string DatabaseDependency = nameof(DatabaseDependency);
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
                yield return GivenWhenThen.Create(
                    Given.UsernameValidationFails,
                    When.FredTriesToLogOn,
                    Then.LogOnFailsBecauseTheUserIsNotValid);
                yield return GivenWhenThen.Create(
                    Given.NoUsersExist,
                    When.FredTriesToLogOn,
                    Then.LogOnFailsBecauseTheUserDidNotExist);
                yield return GivenWhenThen.Create(
                    Given.FredExists,
                    When.FredTriesToLogOn,
                    Then.LogOnSucceeds);
            }
        }

        /// <summary>
        /// Given methods arrange initial test state.
        /// They are responsible for creating the object under test and storing any information that may be needed at assertion time.
        /// </summary>
        public static class Given
        {
            public static TestState<LogonManager> UsernameValidationFails()
            {
                return CreateTestState(
                    usernameValidator: UsernameValidatorCreator.AlwaysInvalid());
            }

            public static TestState<LogonManager> NoUsersExist()
            {
                return CreateTestState(
                    database: DatabaseCreator.EmptyDatabase());
            }

            public static TestState<LogonManager> FredExists()
            {
                return CreateTestState(
                    database: DatabaseCreator.ContainsUser("Fred"));
            }

            private static TestState<LogonManager> CreateTestState(
                Mock<IDatabase> database = null,
                Mock<IUsernameValidator> usernameValidator = null)
            {
                database ??= DatabaseCreator.EmptyDatabase();
                usernameValidator ??= UsernameValidatorCreator.AlwaysValid();

                var result = new TestState<LogonManager>();
                result.StoreDependency(DatabaseDependency, database);
                result.StoreDependency(ValidatorDependency, usernameValidator);

                result.TestObject = new LogonManager(
                    database.Object,
                    usernameValidator.Object);

                return result;
            }
        }

        /// <summary>
        /// When methods are reponsible for acting upon the test object.
        /// These also return the result of the action if suitable.
        /// </summary>
        public static class When
        {
            public static bool FredTriesToLogOn(LogonManager testObject)
            {
                return testObject.TryLogon("Fred");
            }
        }

        /// <summary>
        /// Then methods are responsible for asserting upon the test state/result of the act.
        /// </summary>
        public static class Then
        {
            public static void LogOnFailsBecauseTheUserIsNotValid(
                TestState<LogonManager> testState,
                Func<bool> act)
            {
                var result = act();
                Assert.IsFalse(result);
                AssertUsernameWasChecked(testState);
                AssertDatabaseWasNotChecked(testState);
            }

            public static void LogOnFailsBecauseTheUserDidNotExist(
                TestState<LogonManager> testState,
                Func<bool> act)
            {
                var result = act();
                Assert.IsFalse(result);
                AssertUsernameWasChecked(testState);
                AssertDatabaseWasChecked(testState);
            }

            public static void LogOnSucceeds(
                TestState<LogonManager> testState,
                Func<bool> act)
            {
                var result = act();
                Assert.IsTrue(result);
                AssertUsernameWasChecked(testState);
                AssertDatabaseWasChecked(testState);
            }

            private static void AssertUsernameWasChecked(
                TestState<LogonManager> testState)
            {
                var dependency = testState.GetDependency<Mock<IUsernameValidator>>(ValidatorDependency);
                dependency.Verify(m => m.IsValid(It.IsAny<string>()), Times.Once);
            }

            private static void AssertDatabaseWasChecked(
                TestState<LogonManager> testState)
            {
                var dependency = testState.GetDependency<Mock<IDatabase>>(DatabaseDependency);
                dependency.Verify(m => m.UserExists(It.IsAny<string>()), Times.Once);
            }

            private static void AssertDatabaseWasNotChecked(
                TestState<LogonManager> testState)
            {
                var dependency = testState.GetDependency<Mock<IDatabase>>(DatabaseDependency);
                dependency.Verify(m => m.UserExists(It.IsAny<string>()), Times.Never);
            }
        }
    }
}