using Core.VBCM.Helper;

namespace Core.View.ViewPool
{
    public class ViewPoolable : global::View.View
    {
        public ViewPool UnityPoolManager { protected get; set; }

        public virtual void OnPop() // Constructor for the pool
        {
            if (gameObject != null && !gameObject.IsNull())
                gameObject.SetActive(true);
        }

        public virtual void OnPush() // Destructor for the pool
        {
            if (gameObject != null && !gameObject.IsNull())
                gameObject.SetActive(false);
        }

        /// <summary>
        /// Return to pool
        /// </summary>
        public void Push()
        {
            UnityPoolManager.Push(this);
            Clear();
        }
    }
}