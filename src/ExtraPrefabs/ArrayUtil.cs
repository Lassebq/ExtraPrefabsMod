using System;

namespace ExtraPrefabs
{
	public static class ArrayUtil
	{
		public static void AddValue<T>(ref T[] array, T value)
		{
			int oldLength = array.Length;
			Array.Resize(ref array, array.Length + 1);
			array[oldLength] = value;
		}
		public static void AddValues<T>(ref T[] array, T[] values)
		{
			int oldLength = array.Length;
			Array.Resize(ref array, array.Length + values.Length);
			for(int i = 0; i < values.Length; i++)
			{
				array[oldLength + i] = values[i];
			}
		}
	}
}
