using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Whimsy.Client.Core
{
    public class RequestHeader : IEnumerable<KeyValuePair<string, object>>
    {
        #region Fields

        private readonly List<KeyValuePair<string, object>> _parameters = new List<KeyValuePair<string, object>>();

        #endregion

        #region Interface

        public void Add(string name, object value) => _parameters.Add(new KeyValuePair<string, object>(name, value));

        #endregion

        #region Implementations

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _parameters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}