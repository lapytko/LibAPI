using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using LibAPI.Attributes;

namespace LibAPI.Models
{
    public class BaseModel<T, TSelf> : IBaseModel<T, TSelf> where TSelf : BaseModel<T, TSelf>, new()
    {
        protected BaseModel(T value = default)
        {
            Value = value;
        }

        public T Value { get; set; }

        public TSelf SetValue(T value = default)
        {
            Value = value;
            return (TSelf) this;
        }

        public virtual object ToObject()
        {
            return Value;
        }

        public virtual object ToObjectReflection(string[] args = null)
        {
            args = args?.Select(x => x.Trim().ToLower()).ToArray();

            var obj = new ExpandoObject() as IDictionary<string, object>;
            var propertyInfos = Value.GetType()
                .GetProperties(BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !(Attribute.IsDefined(x, typeof(IgnorePropertyAttribute)) || Attribute.IsDefined(x,
                                typeof(IgnoreIfNullDataAttribute)) && x.GetValue(Value) == null) &&
                            (args == null || args.Contains(x.Name.Trim().ToLower())));
            foreach (var propertyInfo in propertyInfos.OrderBy(x => x.Name))
            {
                obj.Add(char.ToLowerInvariant(propertyInfo.Name[0]) + propertyInfo.Name.Substring(1),
                    propertyInfo.GetValue(Value) is IBaseModel baseModel
                        ? baseModel.ToObjectReflection()
                        : propertyInfo.GetValue(Value));
            }

            return obj;
        }

        public object ToObject(Func<T, object> func)
        {
            return func.Invoke(Value);
        }

        public TR ToType<TR>(Func<T, TR> func)
        {
            return func.Invoke(Value);
        }

        public TSelf InvokeAction(Action<T> action)
        {
            action(Value);
            return (TSelf) this;
        }

        public TSelf SetProperty(Dictionary<string, object> propertyData)
        {
            var t = Value?.GetType();
            if (t == null) return (TSelf) this;
            foreach (var (key, value) in propertyData)
                t.GetProperty(key, BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance)
                    ?.SetValue(Value, value);
            return (TSelf) this;
        }
    }

    public interface IBaseModel<T, TSelf> : IBaseModel where TSelf : IBaseModel<T, TSelf>
    {
        T Value { get; set; }

        TSelf SetValue(T value = default);

        object ToObject(Func<T, object> func);

        TSelf InvokeAction(Action<T> action);

        TSelf SetProperty(Dictionary<string, object> propertyData);
    }

    public interface IBaseModel
    {
        object ToObject();
        object ToObjectReflection(string[] args = null);
    }
}
