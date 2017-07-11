/// <summary>
/// Use case: In a list of key-value pairs, the whole list is visited sequencly and value will be updated,
/// but neither key will be changed, nor O(1) access to object is required.
/// </summary>
/// 
/// <remarks>
/// It is not designed to use with Dictionary, and confirmed as multi-thread safe yet
/// </remarks>

namespace ch3plusStudio.dotNETSupplement.Core.Collections.Generic
{
    public class KeyValuePairMutable<TKey, TValue>
    {
        internal KeyValuePairMutable(){ }

        public KeyValuePairMutable(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; private set; }
        public TValue Value { get; set; }
    }
}
