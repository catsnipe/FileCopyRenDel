using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// �I�u�W�F�N�g�l���L���X�g�ϊ����܂��B
	/// </summary>
	public class Cast
	{
		/// <summary>
		/// �w�肵���^�� Enum �ɕϊ����܂��B
		/// </summary>
		/// <typeparam name="T">Enum �^</typeparam>
		/// <param name="o">�ϊ����� object</param>
		/// <param name="initval">Enum �����Ȃ������ꍇ�̏����l</param>
		/// <returns>�擾���� Enum �l</returns>
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
					// "1" -> 1 �ɂ��Ē���
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
		/// DateTime�ɕϊ����܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l�B�G���[�̏ꍇ��false���Ԃ�܂��B</returns>
		public static DateTime DateTime(object src)
		{
			return DateTime(src, System.DateTime.MinValue);
		}
		
		/// <summary>
		/// DateTime�ɕϊ����܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="err">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// DateTime�ɕϊ����܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
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
		/// Bool�ɕϊ����܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l�B�G���[�̏ꍇ��false���Ԃ�܂��B</returns>
		public static bool Bool(object src)
		{
			return Bool(src, false);
		}
		
		/// <summary>
		/// Bool�ɕϊ����܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="err">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// String�ɕϊ����܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static string String(object src)
		{
			return String(src, "");
		}
		
		/// <summary>
		/// String�ɕϊ����܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errstr">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// Int�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static int Int(object src)
		{
			return Int(src, 0);
		}
		
		/// <summary>
		/// Int�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// Short�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static short Short(object src)
		{
			return Short(src, 0);
		}
		
		/// <summary>
		/// Short�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// UShort�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static ushort UShort(object src)
		{
			return UShort(src, 0);
		}
		
		/// <summary>
		/// UShort�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// long�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static long Long(object src)
		{
			return Long(src, 0);
		}
		
		/// <summary>
		/// long�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// Int32�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static System.Int32 Int32(object src)
		{
			return Int32(src, 0);
		}
		
		/// <summary>
		/// Int32�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// Int64�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static System.Int64 Int64(object src)
		{
			return Int64(src, 0);
		}
		
		/// <summary>
		/// Int64�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// Decimal�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static decimal Decimal(object src)
		{
			return Decimal(src, 0);
		}
		
		/// <summary>
		/// Decimal�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
		/// Decimal�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l�B</param>
		/// <returns>�ϊ��l�B�ϊ��ł��Ȃ����̂� null�B</returns>
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
		/// Float�ɕϊ����܂��B�G���[����0��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <returns>�ϊ��l</returns>
		public static float Float(object src)
		{
			return Float(src, 0.0f);
		}
		
		/// <summary>
		/// Float�ɕϊ����܂��B�G���[����errno��Ԃ��܂��B
		/// </summary>
		/// <param name="src">���̒l</param>
		/// <param name="errno">�G���[�̍ۂɕԂ�l</param>
		/// <returns>�ϊ��l</returns>
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
