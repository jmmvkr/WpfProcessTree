using System.Collections.Generic;

namespace VData
{
    public class BucketMap<K, V>
    {
        Dictionary<K, IList<V>> map;

        public BucketMap()
        {
            map = new Dictionary<K, IList<V>>();
        }

        public IEnumerable<V> this[K k]
        {
            get
            {
                IList<V> lst = null;
                map.TryGetValue(k, out lst);
                return lst;
            }
        }

        public void add(K k, V v)
        {
            IList<V> lst = null;
            map.TryGetValue(k, out lst);
            if (null == lst)
            {
                lst = new List<V>(10);
                map[k] = lst;
            }
            lst.Add(v);
        }

        public IEnumerable<K> keys { get { return map.Keys; } }
    }
}
