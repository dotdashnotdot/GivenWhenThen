namespace FakeApplication.UnitTests
{
    using FakeApplication.UnitTests.Creators;
    using GivenWhenThen;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Example showing how it's possible to wrap return types.
    /// This can be useful when return types are shared or a when statement needs to return multiple values.
    /// </summary>
    public class WhenThenApproaches
    {
        public const string InitialNumberOfAttemptsRemaining = nameof(InitialNumberOfAttemptsRemaining);
        public const string InitialNumberOfDaysRemaining = nameof(InitialNumberOfDaysRemaining);

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
                    Given.DefaultBehaviour,
                    When.RemainingLogonAttemptsIsFetched,
                    Then.AttemptsRemainingIsUnchanged);
                yield return GivenWhenThen.Create(
                    Given.DefaultBehaviour,
                    When.SubscriptionLengthIsFetched,
                    Then.SubscriptionDaysRemainingIsUnchanged);
                /* Compilation Error
                yield return GivenWhenThen.Create(
                    Given.DefaultBehaviour,
                    When.SubscriptionLengthIsFetched,
                    Then.ThreeAttemptsRemain);
                */
            }
        }

        public static class Given
        {
            public static TestState<LogonManager> DefaultBehaviour()
            {
                return CreateTestState();
            }

            private static TestState<LogonManager> CreateTestState(
                Mock<IDatabase> database = null,
                Mock<IUsernameValidator> usernameValidator = null)
            {
                database ??= DatabaseCreator.EmptyDatabase();
                usernameValidator ??= UsernameValidatorCreator.AlwaysValid();

                var result = new TestState<LogonManager>();
                result.StoreDependency(InitialNumberOfDaysRemaining, 31);
                result.StoreDependency(InitialNumberOfAttemptsRemaining, 3);

                result.TestObject = new LogonManager(
                    database.Object,
                    usernameValidator.Object);

                return result;
            }
        }

        public static class When
        {
            public class RemainingLogonAttemptsResult
            {
                public int Result { get; set; }
            }

            public static RemainingLogonAttemptsResult RemainingLogonAttemptsIsFetched(LogonManager testObject)
            {
                return new RemainingLogonAttemptsResult
                {
                    Result = testObject.GetRemainingLogonAttempts("Fred"),
                };
            }

            public class SubscriptionLengthResult
            {
                public int Result { get; set; }
            }

            public static SubscriptionLengthResult SubscriptionLengthIsFetched(LogonManager testObject)
            {
                return new SubscriptionLengthResult
                {
                    Result = testObject.GetSubscriptionLength("Fred"),
                };
            }
        }

        public static class Then
        {
            public static void AttemptsRemainingIsUnchanged(
                TestState<LogonManager> testState,
                Func<When.RemainingLogonAttemptsResult> act)
            {
                var result = act();
                var expected = testState.GetDependency<int>(InitialNumberOfAttemptsRemaining);
                Assert.AreEqual(expected, result.Result);
            }

            public static void SubscriptionDaysRemainingIsUnchanged(
                TestState<LogonManager> testState,
                Func<When.SubscriptionLengthResult> act)
            {
                var result = act();
                var expected = testState.GetDependency<int>(InitialNumberOfDaysRemaining);
                Assert.AreEqual(expected, result.Result);
            }
        }
    }
}