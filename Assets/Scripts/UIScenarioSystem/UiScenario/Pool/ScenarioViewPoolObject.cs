namespace Core.UiScenario.Pool
{
    public class ScenarioViewPoolObject: ScenarioView
    {
        public ScenarioViewPool UnityPoolManager { protected get; set; }

        public virtual void OnPop()
        {
        }

        public virtual void OnPush()
        {
        }

        /// <summary>
        /// Return to pool
        /// </summary>
        public void Push()
        {
            UnityPoolManager.Push(this);
        }
    }
}