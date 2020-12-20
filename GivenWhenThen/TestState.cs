namespace GivenWhenThen
{
    using System;
    using System.Collections.Generic;

    public class TestState<TTestClass>
            where TTestClass : class
    {
        private Dictionary<string, Dictionary<Type, object>> storedState = new Dictionary<string, Dictionary<Type, object>>();
        public TTestClass TestObject = default;

        public T GetDependency<T>(string id)
        {
            var typeDictionary = this.storedState[id];
            return (T)typeDictionary[typeof(T)];
        }

        public void StoreDependency<T>(string id, T toStore)
        {
            Dictionary<Type, object> typeDictionary = null;
            if (this.storedState.ContainsKey(id))
            {
                typeDictionary = this.storedState[id];
            }
            else 
            {
                typeDictionary = new Dictionary<Type, object>();
                this.storedState[id] = typeDictionary;
            }

            typeDictionary[typeof(T)] = toStore;
        }
    }
}
