using System;
using System.Collections.Generic;
using Core.VBCM.Helper;
using Core.VBCM.Interfaces;

namespace Core.VBCM
{
    public sealed class Validator : IValidator
    {
        private readonly IDictionary<Type, List<WeakReference>> _validates = new Dictionary<Type, List<WeakReference>>();

        /// <inheritdoc />
        public void Add<TCallBackValue, TEventHub, TSendValue>(
            EventHubVbcm<TCallBackValue, TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>
        {
            var type = typeof(TEventHub);
            GetValidatesList<TEventHub>(type).Add(new WeakReference(validate));
        }

        /// <inheritdoc />
        public void Remove<TCallBackValue, TEventHub, TSendValue>(
            EventHubVbcm<TCallBackValue, TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TCallBackValue, TEventHub, TSendValue>
        {
            var type = typeof(TEventHub);
            _validates.GetList<TEventHub>(type).RemoveAll(obj => obj.IsAlive && obj.Target == validate);
        }

//============================================
        /// <inheritdoc />
        public void Add<TEventHub, TSendValue>(EventHubVbcm<TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>
        {
            var type = typeof(TEventHub);
            GetValidatesList<TEventHub>(type).Add(new WeakReference(validate));
        }

        /// <inheritdoc />
        public void Remove<TEventHub, TSendValue>(EventHubVbcm<TEventHub, TSendValue>.IValidate validate)
            where TEventHub : EventHubVbcm<TEventHub, TSendValue>
        {
            var type = typeof(TEventHub);
            GetValidatesList<TEventHub>(type).RemoveAll(obj => obj.IsAlive && obj.Target == validate);
        }

        public List<WeakReference> Remove<TEventHub>() where TEventHub : EventHubBase<TEventHub>
        {
            var type = typeof(TEventHub);
            if (!_validates.ContainsKey(type))
                return null;
            
            var result = GetValidatesList<TEventHub>(type);
            _validates.Remove(type);
            return result;
        }

        private List<WeakReference> GetValidatesList<TEventHub>(Type type)
            where TEventHub : EventHubBase<TEventHub>
        {
            List<WeakReference> list;
            var isExist = _validates.TryGetValue(type, out list);
            if (isExist)
                return list;

            list = new List<WeakReference>();
            _validates.Add(type, list);
            return list;
        }
    }
}