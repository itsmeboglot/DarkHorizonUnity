using System;
using UnityEngine;

namespace Core.Unity
{
    public class TimeManager : MonoBehaviour
    {
        public static event Action OnGamePause;
    
        private static QueueTaskManager _queuedManager = new QueueTaskManager();
        private static event Action<QueueTask> OnQueuedTaskComplete;

        public static void PauseGame()
        {
            QueueTask.QueueTaskDelegate func = delegate (QueueTask queuedTask)
            {
                OnQueuedTaskComplete += OnTaskComplete;
                Time.timeScale = 0;
                OnGamePause?.Invoke();
            };

            _queuedManager.AddQueueTask(new QueueTask(func));
        }

        public static void ResumeGame()
        {
            if (_queuedManager.GetTasksCount() > 0)
            {
                OnQueuedTaskComplete.Invoke(_queuedManager.GetTask());

                if(_queuedManager.GetTasksCount() == 0)
                {
                    Time.timeScale = 1;
                }
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public static void ClearTasks()
        {
            _queuedManager.ClearTasks();
            Time.timeScale = 1;
        }

        private static void OnTaskComplete(QueueTask task)
        {
            OnQueuedTaskComplete -= OnTaskComplete;
            task.Complete();
        }
    }
}
