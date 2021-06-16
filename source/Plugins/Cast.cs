using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// オブジェクト値をキャスト変換します。
	/// </summary>
	public class Cast
	{
		/// <summary>
		/// 指定した型の Enum に変換します。
		/// </summary>
		/// <typeparam name="T">Enum 型</typeparam>
		/// <param name="o">変換する object</param>
		/// <param name="initval">Enum が取れなかった場合の初期値</param>
		/// <returns>取得した Enum 値</returns>
		public static T Enumelate<T>(object o, T initval)
		{
			T	otype = initval;
			
			if (o != null && o != System.DBNull.Value)
			{
				if (Enum.IsDefined(typeof(T), o) == true)
				{
					otype = (T)Enum.Parse(typeof(T), o.ToString());
				}
				else
				{
					// "1" -> 1 にして調査
					int ival;
					if (int.TryParse(o.ToString(), out ival) == true)
					{
						if (Enum.IsDefined(typeof(T), ival) == true)
						{
							otype = (T)Enum.Parse(typeof(T), o.ToString());
						}
					}
				}
			}
			
			return otype;
		}
		
		/// <summary>
		/// DateTimeに変換します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値。エラーの場合はfalseが返ります。</returns>
		public static DateTime DateTime(object src)
		{
			return DateTime(src, System.DateTime.MinValue);
		}
		
		/// <summary>
		/// DateTimeに変換します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="err">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static DateTime DateTime(object src, DateTime err)
		{
			DateTime	dst;
			
			if (src == null)
			{
				return err;
			}
			
			try
			{
				if (src is DateTime)
				{
					dst = (DateTime)src;
				}
				else
				{
					if (System.DateTime.TryParse(src.ToString(), out dst) == false)
					{
						return err;
					}
				}
			}
			catch
			{
				return err;
			}
			
			return dst;
		}
		
		/// <summary>
		/// DateTimeに変換します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static DateTime? DateTimeNull(object src)
		{
			DateTime	dst;
			
			if (src == null)
			{
				return null;
			}
			
			try
			{
				if (src is DateTime)
				{
					dst = (DateTime)src;
				}
				else
				{
					if (System.DateTime.TryParse(src.ToString(), out dst) == false)
					{
						return null;
					}
				}
			}
			catch
			{
				return null;
			}
			
			return dst;
		}
		
		/// <summary>
		/// Boolに変換します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値。エラーの場合はfalseが返ります。</returns>
		public static bool Bool(object src)
		{
			return Bool(src, false);
		}
		
		/// <summary>
		/// Boolに変換します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="err">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static bool Bool(object src, bool err)
		{
			bool	dst;
			
			if (src == null)
			{
				return err;
			}
			if (bool.TryParse(src.ToString(), out dst) == false)
			{
				return err;
			}
			
			return dst;
		}
		
		/// <summary>
		/// Stringに変換します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static string String(object src)
		{
			return String(src, "");
		}
		
		/// <summary>
		/// Stringに変換します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errstr">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static string String(object src, string errstr)
		{
			if (src == null)
			{
				return errstr;
			}
			if (src == System.DBNull.Value)
			{
				return errstr;
			}
			return src.ToString();
		}
		
		/// <summary>
		/// Intに変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static int Int(object src)
		{
			return Int(src, 0);
		}
		
		/// <summary>
		/// Intに変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static int Int(object src, int errno)
		{
			int		dst;
			float	fdst;
			
			if (src == null)
			{
				return errno;
			}
			
			string	srcstr = src.ToString();
			
			if (int.TryParse(srcstr, out dst) == false)
			{
				if (float.TryParse(srcstr, out fdst) == false)
				{
					return errno;
				}
				if (Single.IsNaN(fdst) == true)
				{
					return errno;
				}
				
				dst = (int)fdst;
			}
			
			return dst;
		}
		
		/// <summary>
		/// Shortに変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static short Short(object src)
		{
			return Short(src, 0);
		}
		
		/// <summary>
		/// Shortに変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static short Short(object src, short errno)
		{
			short	dst;
			
			if (src == null)
			{
				return errno;
			}
			
			string	srcstr = src.ToString();
			
			if (short.TryParse(srcstr, out dst) == false)
			{
				return errno;
			}
			
			return dst;
		}
		
		/// <summary>
		/// UShortに変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static ushort UShort(object src)
		{
			return UShort(src, 0);
		}
		
		/// <summary>
		/// UShortに変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static ushort UShort(object src, ushort errno)
		{
			ushort	dst;
			
			if (src == null)
			{
				return errno;
			}
			
			string	srcstr = src.ToString();
			
			if (ushort.TryParse(srcstr, out dst) == false)
			{
				return errno;
			}
			
			return dst;
		}
		
		/// <summary>
		/// longに変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static long Long(object src)
		{
			return Long(src, 0);
		}
		
		/// <summary>
		/// longに変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static long Long(object src, int errno)
		{
			long		dst;
			
			if (src == null)
			{
				return errno;
			}
			if (long.TryParse(src.ToString(), out dst) == false)
			{
				return errno;
			}
			
			return dst;
		}
		
		/// <summary>
		/// Int32に変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static System.Int32 Int32(object src)
		{
			return Int32(src, 0);
		}
		
		/// <summary>
		/// Int32に変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static System.Int32 Int32(object src, int errno)
		{
			System.Int32		dst;
			
			if (src == null)
			{
				return errno;
			}
			if (System.Int32.TryParse(src.ToString(), out dst) == false)
			{
				return errno;
			}
			
			return dst;
		}
		
		/// <summary>
		/// Int64に変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static System.Int64 Int64(object src)
		{
			return Int64(src, 0);
		}
		
		/// <summary>
		/// Int64に変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static System.Int64 Int64(object src, int errno)
		{
			System.Int64		dst;
			
			if (src == null)
			{
				return errno;
			}
			if (System.Int64.TryParse(src.ToString(), out dst) == false)
			{
				return errno;
			}
			
			return dst;
		}
		
		/// <summary>
		/// Decimalに変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static decimal Decimal(object src)
		{
			return Decimal(src, 0);
		}
		
		/// <summary>
		/// Decimalに変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static decimal Decimal(object src, decimal errno)
		{
			decimal?	dst = DecimalNull(src);
			
			if (dst == null)
			{
				return errno;
			}
			
			return dst.Value;
		}
		
		/// <summary>
		/// Decimalに変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値。</param>
		/// <returns>変換値。変換できないものは null。</returns>
		public static decimal? DecimalNull(object src)
		{
			decimal		dst;
			
			if (src == null || src == System.DBNull.Value)
			{
				return null;
			}
			if (decimal.TryParse(src.ToString(), out dst) == false)
			{
				return null;
			}
			
			return dst;
		}
		
		/// <summary>
		/// Floatに変換します。エラーだと0を返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <returns>変換値</returns>
		public static float Float(object src)
		{
			return Float(src, 0.0f);
		}
		
		/// <summary>
		/// Floatに変換します。エラーだとerrnoを返します。
		/// </summary>
		/// <param name="src">元の値</param>
		/// <param name="errno">エラーの際に返る値</param>
		/// <returns>変換値</returns>
		public static float Float(object src, float errno)
		{
			float		dst;
			
			if (src == null)
			{
				return errno;
			}
			if (float.TryParse(src.ToString(), out dst) == false)
			{
				return errno;
			}
			
			return dst;
		}
	}
}
