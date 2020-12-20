namespace GivenWhenThen
{
    using System;

    internal class WhenReturnsVoid<TTestClass> : GivenWhenThenTestCase
            where TTestClass : class
    {
        private readonly Func<TestState<TTestClass>> givenFunc;
        private readonly Action<TTestClass> whenAction;
        private readonly Action<TestState<TTestClass>, Action> thenAction;

        internal WhenReturnsVoid(
            Func<TestState<TTestClass>> givenFunc,
            Action<TTestClass> whenAction,
            Action<TestState<TTestClass>, Action> thenAction)
        {
            this.givenFunc = givenFunc;
            this.whenAction = whenAction;
            this.thenAction = thenAction;
        }

        public override void RunTestCase()
        {
            var testState = givenFunc();
            thenAction(testState, () => whenAction(testState.TestObject));
        }

        public override string ToString()
        {
            return $"Given{givenFunc.Method.Name},When{whenAction.Method.Name},Then{thenAction.Method.Name}";
        }
    }
}
