using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Bawx.Util
{
    public class TaskManagerComponent : GameComponent
    {
        private readonly ConcurrentQueue<Action> _actionQueue = new ConcurrentQueue<Action>();

        public TaskManagerComponent(Game game) : base(game)
        {
            UpdateOrder = -1;
        }

        public override void Update(GameTime gameTime)
        {
            Action action;
            while (_actionQueue.TryDequeue(out action))
                action();
        }

        public GameTask<T> ExecuteInBackground<T>(Func<T> background, bool longRunning = false)
        {
            var task = new GameTask<T>(this,
                Task.Factory.StartNew(background,
                    longRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.PreferFairness));
            return task;
        }

        public GameTask ExecuteInBackground(Action background, bool longRunning = false)
        {
            var task = new GameTask(this,
                Task.Factory.StartNew(background,
                    longRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.PreferFairness));
            return task;
        }

        #region Nested

        public class GameTask
        {
            protected readonly Task Task;
            protected readonly TaskManagerComponent Manager;

            public bool Completed => Task.IsCompleted;
            public bool Canceled => Task.IsCanceled;
            public bool Faulted => Task.IsFaulted;
            public TaskStatus Status => Task.Status;

            internal GameTask(TaskManagerComponent manager, Task task)
            {
                Manager = manager;
                Task = task;
            }

            public void Then(Action toExecuteInGameThread)
            {
                Task.ContinueWith(t => Manager._actionQueue.Enqueue(toExecuteInGameThread),
                    TaskContinuationOptions.ExecuteSynchronously);
            }

            public AggregateException GetException()
            {
                return Task.Exception;
            }
        }

        public class GameTask<T> : GameTask
        {
            internal GameTask(TaskManagerComponent manager, Task task) : base(manager, task)
            {
            }

            public void Then(Action<T> toExecuteInGameThread)
            {
                ((Task<T>) Task).ContinueWith(t => Manager._actionQueue.Enqueue(() => toExecuteInGameThread(t.Result)),
                    TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        #endregion
    }
}