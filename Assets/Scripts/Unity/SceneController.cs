using System;
using Core.Unity;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Unity
{
    public static class SceneController
    {
        public static event Action<string> OnSceneStartLoading;
        public static event Action<string> OnSceneLoaded;
        
        public static void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            OnSceneStartLoading?.Invoke(sceneName);
            StopSideProceedings();
            SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            OnSceneLoaded?.Invoke(sceneName);
        }

        public static string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
        
        public static int GetActiveSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        public static bool IsSceneLoaded(string sceneName)
        {
            return GetActiveSceneName() == sceneName;
        }

        private static void StopSideProceedings()
        {
            DOTween.KillAll();
            TimeManager.ClearTasks();
        }
    }
}
