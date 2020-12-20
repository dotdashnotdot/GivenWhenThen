namespace FakeApplication.UnitTests.Advanced
{
    using FakeApplication.UnitTests.Creators;
    using GivenWhenThen;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Example showing how to ignore the return value of When statements
    /// </summary>
    public class IgnoringReturnTypes
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
                    Then.UsernameWasChecked);
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
            public static void UsernameWasChecked(
                TestState<LogonManager> testState,
                Action act)
            {
                act();
                var dependency = testState.GetDependency<Mock<IUsernameValidator>>(ValidatorDependency);
                dependency.Verify(m => m.IsValid(It.IsAny<string>()), Times.Once);
            }
        }
    }
}