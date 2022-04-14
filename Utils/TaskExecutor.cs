using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibAPI.Utils
{
    public class TaskExecutor
    {
        public TaskExecutor(IEnumerable<ATaskItem> items)
        {
            _items = items.ToList();
        }

        public TaskExecutor(IEnumerable<Task> tasks)
        {
            _items = tasks.Select(x => (ATaskItem) TaskItemBuilder.TaskItemInit(x)).ToList();
        }

        public async Task<IEnumerable<ATaskItem>> ExecuteAsync()
        {
            foreach (var item in _items.Where(x => !x.IsRunning))
                item.Start();
            foreach (var item in _items)
            {
                await item.ExecuteAsync();
            }

            return _items.Where(x => !x.IsVoid);
        }

        private readonly List<ATaskItem> _items;
    }

    public static class TaskItemBuilder
    {
        public static TaskItem TaskItemInit(Task t)
        {
            return new TaskItem(t);
        }

        // public static TaskItem<TResult> TaskItemInit<TResult>(Task<TResult> t)
        // {
        //     return new TaskItem<TResult>(t);
        // }
    }

    public class TaskItem : ATaskItem
    {
        public TaskItem(Task t)
        {
            T = t;
            if (T.GetType() is {IsGenericType: true} type)
                ResType = type.GetGenericArguments()[0];
        }
        //
        // public TaskItem(Task<TResult> t) : this((Task) t)
        // {
        //     ResType = _t.GetType().GetGenericParameterConstraints()[0];
        // }

        public override async Task ExecuteAsync()
        {
            Start();
            await T;
            if (!IsVoid)
                Res = T.GetType().GetProperty("Result")?.GetValue(T);
        }
    }

    public abstract class ATaskItem
    {
        public virtual void Start()
        {
            if (!IsRunning)
                T.Start();
        }

        public abstract Task ExecuteAsync();

        public bool IsVoid => T.GetType().GetProperty("Result")?.PropertyType.Name == "VoidTaskResult";

        public bool IsRunning => T.Status != TaskStatus.WaitingToRun;
        protected Task T;
        protected object Res { get; set; }
        protected Type ResType { get; set; } = typeof(object);

        public static ConvertResult<TResult> Convert<TResult>(ATaskItem item)
        {
            try
            {
                return ConvertResult<TResult>.Success((TResult) item.Res);
            }
            catch (Exception)
            {
                return ConvertResult<TResult>.Error();
            }
        }
    }

    public class ConvertResult<TResult>
    {
        public static ConvertResult<TResult> Success(TResult result)
        {
            return new()
            {
                Result = result,
                IsSuccess = true
            };
        }

        public static ConvertResult<TResult> Error()
        {
            return new();
        }

        public TResult Result { get; set; }
        public bool IsSuccess { get; set; }
    }
}