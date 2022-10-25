using Core.View.ViewPool;
using UnityEngine;

namespace View.ViewPool
{
    [CreateAssetMenu(fileName = "ViewPoolConfig", menuName = "ViewPoolConfig", order = 1)]
    public class ViewPoolConfig : ScriptableObject
    {
        public ViewPoolable[] prefabs;
    }
}