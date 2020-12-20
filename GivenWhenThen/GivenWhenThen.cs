namespace GivenWhenThen
{
    using System;

    public static class GivenWhenThen
    {
        public static GivenWhenThenTestCaseData Create<TTestClass, TReturn>(
            Func<TestState<TTestClass>> givenFunc,
            Func<TTestClass, TReturn> whenFunc,
            Action<TestState<TTestClass>, Func<TReturn>> thenAction)
            where TTestClass : class
        {
            var toReturn = new WhenReturns<TTestClass, TReturn>(givenFunc, whenFunc, thenAction);
            return new GivenWhenThenTestCaseData(toReturn);
        }

        public static GivenWhenThenTestCaseData Create<TTestClass>(
            Func<TestState<TTestClass>> givenFunc,
            Action<TTestClass> whenFunc,
            Action<TestState<TTestClass>, Action> thenAction)
            where TTestClass : class
        {
            var toReturn = new WhenReturnsVoid<TTestClass>(givenFunc, whenFunc, thenAction);
            return new GivenWhenThenTestCaseData(toReturn);
        }

        public static GivenWhenThenTestCaseData CreateTestCase<TTestClass, TReturn>(
            Func<TestState<TTestClass>> givenFunc,
            Func<TTestClass, TReturn> whenFunc,
            Action<TestState<TTestClass>, Action> thenAction)
            where TTestClass : class
        {
            var toReturn = new WhenReturns<TTestClass, TReturn>(givenFunc, whenFunc, thenAction);
            return new GivenWhenThenTestCaseData(toReturn);
        }
    }
}
