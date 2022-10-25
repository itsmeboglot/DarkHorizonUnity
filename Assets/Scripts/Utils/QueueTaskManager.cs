using System.Collections.Generic;

namespace Core.Unity
{
    public class QueueTaskManager
    {
        private readonly Queue<QueueTask> _queueTasks = new Queue<QueueTask>();

        public void AddQueueTask(QueueTask task)
        {
            if (_queueTasks.Count == 0)
            {
                _queueTasks.Enqueue(task);
                ExecuteQueueTask(task);
            }
            else
            {
                _queueTasks.Enqueue(task);
            }
        }

        public void AddQueueTask(QueueTask.QueueTaskDelegate task)
        {
            AddQueueTask(new QueueTask(task));
        }

        public void ClearTasks()
        {
            _queueTasks.Clear();
        }

        public int GetTasksCount()
        {
            return _queueTasks.Count;
        }

        public QueueTask GetTask()
        {
            return _queueTasks.Peek();
        }

        private void OnCompleteQueueTask()
        {
            _queueTasks.Dequeue().OnComplete -= OnCompleteQueueTask;

            if (_queueTasks.Count > 0)
            {
                ExecuteQueueTask(_queueTasks.Peek());
            }
        }

        private void ExecuteQueueTask(QueueTask task)
        {
            task.OnComplete += OnCompleteQueueTask;
            task.executeFunc?.Invoke(task);
        }

        public bool HasTasks => _queueTasks.Count > 0;
    }

    public class QueueTask
    {
        public delegate void QueueTaskDelegate(QueueTask task);
        public QueueTaskDelegate executeFunc;
        public System.Action OnComplete;

        public QueueTask(QueueTaskDelegate executeFunc)
        {
            this.executeFunc = executeFunc;
        }

        public void Complete()
        {
            OnComplete?.Invoke();
        }
    }
}
