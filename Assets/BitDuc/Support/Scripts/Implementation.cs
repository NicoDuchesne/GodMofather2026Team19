using System;
using Object = UnityEngine.Object;

namespace BitDuc.Support
{
    [Serializable]
    public struct Implementation<T, D> where T: class where D: T
    {
        public T Get => implementation as T;

        public Object implementation;

        public string type;
    }
}
