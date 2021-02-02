using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Generic;

namespace UbiClub.Feedback.Tests.Fakes
{
    public class FakeQueryCollection : Dictionary<string, StringValues> , IQueryCollection
    {
        public new StringValues this[string key] => base[key];

        public new int Count => base.Count;

        public new ICollection<string> Keys => base.Keys;

        public new bool ContainsKey(string key)
        {
            return base.ContainsKey(key);
        }

        public new IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
        {
            return base.GetEnumerator();
        }

        public new bool TryGetValue(string key, out StringValues value)
        {
            return base.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return base.GetEnumerator() as IEnumerator;
        }
    }
}