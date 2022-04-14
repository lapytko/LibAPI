using System;
using System.Collections.Generic;
using System.Linq;

namespace LibAPI.Models
{
    public class EnumParserCollection<T> where T : Enum
    {
        protected EnumParserCollection(List<EnumParser<T>> elements)
        {
            _elements = elements;
        }

        public object this[string key]
        {
            get
            {
                var value = _elements.FirstOrDefault(x => x.IsKeyCorrect(key));
                if (value == null)
                    return null;
                return value.Value;
            }
        }

        public object this[T el]
        {
            get
            {
                var value = _elements.FirstOrDefault(x => Equals(x.Value, el));
                return value?.GetKey();
            }
        }

        private readonly List<EnumParser<T>> _elements;
    }

    public class EnumParser<T> where T : Enum
    {
        public EnumParser(string key, T value)
        {
            _key = key;
            Value = value;
        }

        public bool IsKeyCorrect(string key) => key == _key;
        public string GetKey() => _key;
        public T Value { get; }
        private readonly string _key;
    }
}