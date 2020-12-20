namespace GivenWhenThen
{
    using System;

    internal class WhenReturns<TTestClass, TReturn> : GivenWhenThenTestCase
            where TTestClass : class
    {
        private readonly Func<TestState<TTestClass>> givenFunc;
        private readonly Func<TTestClass, TReturn> whenFunc;
        private readonly Action<TestState<TTestClass>, Func<TReturn>> thenActionWhenReturns;
        private readonly Action<TestState<TTestClass>, Action> thenActionWhenIgnored;
        private readonly string name;

        internal WhenReturns(
            Func<TestState<TTestClass>> givenFunc,
            Func<TTestClass, TReturn> whenFunc,
            Action<TestState<TTestClass>, Func<TReturn>> thenAction)
        {
            this.givenFunc = givenFunc;
            this.whenFunc = whenFunc;
            this.thenActionWhenReturns = thenAction;
            this.name = $"Given{givenFunc.Method.Name},When{whenFunc.Method.Name},Then{thenAction.Method.Name}";
        }

        internal WhenReturns(
            Func<TestState<TTestClass>> givenFunc,
            Func<TTestClass, TReturn> whenFunc,
            Action<TestState<TTestClass>, Action> thenAction)
        {
            this.givenFunc = givenFunc;
            this.whenFunc = whenFunc;
            this.thenActionWhenIgnored = thenAction;
            this.name = $"Given{givenFunc.Method.Name},When{whenFunc.Method.Name},Then{thenAction.Method.Name}";
        }

        public override void RunTestCase()
        {
            var testState = givenFunc();
            if (thenActionWhenReturns != null)
            {
                thenActionWhenReturns(testState, () => whenFunc(testState.TestObject));
            }
            else
            {
                thenActionWhenIgnored(testState, () => whenFunc(testState.TestObject));
            }
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
