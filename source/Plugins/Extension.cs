using System.Collections.Generic;

/// <summary>
/// DictionaryExtensions
/// </summary>
public static class DictionaryExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public static bool ContainsKeyNullable<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
	{
		if (key == null)
		{
			return false;
		}
		return self.ContainsKey(key);
	}
	
	///// <summary>
	///// 
	///// </summary>
	//public static TValue GetValueNullable<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
	//{
	//	if (ContainsKeyNullable(self, key) == false)
	//	{
	//		return null;
	//	}
	//	return self[key];
	//}
}