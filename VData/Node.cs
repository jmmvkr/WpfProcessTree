using System.Collections.Generic;

namespace VData
{
    public class Node<T>
    {
        static readonly Node<T>[] emptyList = { };

        public static Node<T> v(T val)
        {
            Node<T> n = new Node<T>();
            n.val = val;
            n.subNodes = emptyList;
            return n;
        }

        public T __value;

        public T val { get { return __value; } set { __value = value; } }
        public IList<Node<T>> subNodes { get; set; }

        public Node<T> addAsChild(T v)
        {
            var child = Node<T>.v(v);
            add(child);
            return child;
        }
        public Node<T> add(T v)
        {
            return add(Node<T>.v(v));
        }
        public Node<T> add(Node<T> aNode)
        {
            IList<Node<T>> lst = subNodes;
            if (emptyList == lst)
            {
                lst = new List<Node<T>>(1);
                subNodes = lst;
            }
            lst.Add(aNode);
            return this;
        }

        internal void remove(Node<T> n)
        {
            subNodes.Remove(n);
        }
    }
}
