namespace SupportLibrary.Common.Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<K,V>(this IDictionary<K,V> dict, K key, V value) where K : class 
                                                                                        where V : class
        {
            dict.ThrowExceptionIfNull();
            key.ThrowExceptionIfNull();
            
            if(value.GetType() != typeof(String))
            {
                value.ThrowExceptionIfNull();
            }

            if(dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }            
        }
    }
}