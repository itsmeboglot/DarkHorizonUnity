using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.VBCM.Helper
{
    // ReSharper disable RedundantNameQualifier
    // ReSharper disable RedundantCast
    public static class EngineHelper
    {
        private static readonly UnityEngine.Object Null = (UnityEngine.Object) null;

        public static bool IsNull(this object obj)
        {
            if (obj is UnityEngine.Object o)
                return Null == o;

            return obj == null;
        }
    }

    public static class WeakHelper
    {
        public static IEnumerable<TType> WeakCast<TType>(this List<WeakReference> target)
        {
            target.RemoveAll(weak => !weak.IsAlive);
            return target.Select(weak => (TType) weak.Target);
        }

        public static List<WeakReference> GetList<TEventHub>(this IDictionary<Type, List<WeakReference>> dictionary,
            Type type)
            where TEventHub : EventHubBase<TEventHub>
        {
            List<WeakReference> list;
            var isExist = dictionary.TryGetValue(type, out list);
            if (isExist)
                return list;

            list = new List<WeakReference>();
            dictionary.Add(type, list);
            return list;
        }
    }

    public sealed class WeakReferenceEqualityComparer : EqualityComparer<WeakReference>
    {
        public override bool Equals(WeakReference x, WeakReference y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            //=======================================

            var isGetX = x.IsAlive;
            var isGetY = y.IsAlive;

            var xTarget = x.Target;
            var yTarget = y.Target;

            if (ReferenceEquals(xTarget, yTarget)) return true;
            if (!isGetX || !isGetY) return false;
            if (xTarget.GetType() != yTarget.GetType()) return false;

            return xTarget.Equals(yTarget);
        }

        public override int GetHashCode(WeakReference obj)
        {
            if (obj == null)
                return 0;

            var isGet = obj.IsAlive;
            return isGet ? obj.Target.GetHashCode() : 0;
        }

        public static readonly EqualityComparer<WeakReference> BindsComparer = new WeakReferenceEqualityComparer();
    }
}