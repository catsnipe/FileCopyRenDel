using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// コントロールの起動に必要なレジストリ値の保存・読み込みを行います。
	/// </summary>
	public class RegControl
	{
		#region *** Private Const Value ***
		/// <summary>
		/// コントロール情報を保存しておくレジストリキー
		/// </summary>
		public const string		REGCON_KEY = @"Controls\";
		#endregion
		
		#region *** Public Method ***
		/// <summary>
		/// XYの座標値をレジストリに登録します。
		/// </summary>
		public static void PushXY(Control ctl)
		{
			pushXYWH(ctl, true, true, false, false);
		}
		
		/// <summary>
		/// WHの座標値をレジストリに登録します。
		/// </summary>
		public static void PushWH(Control ctl)
		{
			pushXYWH(ctl, false, false, true, true);
		}
		
		/// <summary>
		/// XYWHの座標値をレジストリに登録します。
		/// </summary>
		public static void PushXYWH(Control ctl)
		{
			pushXYWH(ctl, true, true, true, true);
		}
		
		/// <summary>
		/// XYの座標値をレジストリから読み込みます。ない場合は無視します。
		/// </summary>
		public static bool PopXY(Control ctl)
		{
			return popXYWH(ctl, true, true, false, false);
		}
		
		/// <summary>
		/// WHの座標値をレジストリから読み込みます。ない場合は無視します。
		/// </summary>
		public static bool PopWH(Control ctl)
		{
			return popXYWH(ctl, false, false, true, true);
		}
		
		/// <summary>
		/// XYWHの座標値をレジストリから読み込みます。ない場合は無視します。
		/// </summary>
		public static bool PopXYWH(Control ctl)
		{
			return popXYWH(ctl, true, true, true, true);
		}
		
		/// <summary>
		/// レジストリに値を保存します。
		/// </summary>
		public static void Push(Control ctl, object val)
		{
			Push(ctl, ctl.Name, val);
		}
		
		/// <summary>
		/// レジストリに値を保存します。
		/// </summary>
		public static void Push(Control ctl, string key, object val)
		{
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("未初期化です。RegCommon.SetMasterKey()で初期化してください。");
				return;
			}
			
			// レジストリキー名を取得
			string		reg_key = getRegKey(ctl);
			
			RegCommon.Write(reg_key + key, val);
		}
		
		/// <summary>
		/// レジストリに保存した値を取得します。
		/// </summary>
		public static object Pop(Control ctl)
		{
			return Pop(ctl, ctl.Name);
		}
		
		/// <summary>
		/// レジストリに保存した値を取得します。
		/// </summary>
		public static object Pop(Control ctl, string key)
		{
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("未初期化です。RegCommon.SetMasterKey()で初期化してください。");
				return null;
			}
			
			// レジストリキー名を取得
			string		reg_key = getRegKey(ctl);
			
			return RegCommon.Read(reg_key + key);
		}
		#endregion
		
		#region *** Private Method ***
		/// <summary>
		/// レジストリキー名を取得します。
		/// 以下のようなルールでキーを作成します。
		/// ex) form1->formSub1->textBox1.x ... form1\formSub1\textBox1\x
		/// </summary>
		private static string getRegKey(Control ctl)
		{
			string		reg_key = "";
			
			for (Control c = ctl; c != null; c = c.Parent)
			{
				reg_key = reg_key.Insert(0, c.Name.ToString() + @"\");
			}
			reg_key = reg_key.Insert(0, REGCON_KEY);
			
			return reg_key;
		}
		
		/// <summary>
		/// XYWHの座標値をレジストリに登録します。
		/// </summary>
		/// <param name="ctl">コントロール名</param>
		/// <param name="bx">trueで保存, falseで保存しない</param>
		/// <param name="by">trueで保存, falseで保存しない</param>
		/// <param name="bw">trueで保存, falseで保存しない</param>
		/// <param name="bh">trueで保存, falseで保存しない</param>
		private static void pushXYWH(Control ctl, bool bx, bool by, bool bw, bool bh)
		{
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("未初期化です。RegCommon.SetMasterKey()で初期化してください。");
				return;
			}
			
			// レジストリキー名を取得
			string		reg_key = getRegKey(ctl);
			
			if (bx == true)
			{
				RegCommon.Write(reg_key + "_x", ctl.Left);
			}
			if (by == true)
			{
				RegCommon.Write(reg_key + "_y", ctl.Top);
			}
			if (bw == true)
			{
				RegCommon.Write(reg_key + "_w", ctl.Width);
			}
			if (bh == true)
			{
				RegCommon.Write(reg_key + "_h", ctl.Height);
			}
		}
		
		/// <summary>
		/// XYWHの座標値をレジストリから読み込みます。ない場合は無視します。
		/// </summary>
		/// <param name="ctl">コントロール名</param>
		/// <param name="bx">trueで読込, falseで読込しない</param>
		/// <param name="by">trueで読込, falseで読込しない</param>
		/// <param name="bw">trueで読込, falseで読込しない</param>
		/// <param name="bh">trueで読込, falseで読込しない</param>
		private static bool popXYWH(Control ctl, bool bx, bool by, bool bw, bool bh)
		{
			bool	success = true;
			
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("未初期化です。RegCommon.SetMasterKey()で初期化してください。");
				return false;
			}
			
			// レジストリキー名を取得
			string		reg_key = getRegKey(ctl);
			
			// レジストリからウィンドウのXYWHを持ってくる
			int			x, y, w, h;
			
			x = ctl.Left;
			y = ctl.Top;
			w = ctl.Width;
			h = ctl.Height;
			
			if (bx == true)
			{
				object ox = RegCommon.Read(reg_key + "_x");
				if (ox != null)
				{
					if (int.TryParse(ox.ToString(), out x) == false)
					{
						success = false;
					}
				}
				else
				{
					success = false;
				}
			}
			if (by == true)
			{
				object oy = RegCommon.Read(reg_key + "_y");
				if (oy != null)
				{
					if (int.TryParse(oy.ToString(), out y) == false)
					{
						success = false;
					}
				}
				else
				{
					success = false;
				}
			}
			if (bw == true)
			{
				object ow = RegCommon.Read(reg_key + "_w");
				if (ow != null)
				{
					if (int.TryParse(ow.ToString(), out w) == false)
					{
						success = false;
					}
				}
				else
				{
					success = false;
				}
			}
			if (bh == true)
			{
				object oh = RegCommon.Read(reg_key + "_h");
				if (oh != null)
				{
					if (int.TryParse(oh.ToString(), out h) == false)
					{
						success = false;
					}
				}
				else
				{
					success = false;
				}
			}
			
			// データ取得に一度でも失敗していたら設定しない
			if (success == true)
			{
				// モニタの領域が狭くなってしまっていた場合、画面に収まる位置にする
				if (x > Screen.GetWorkingArea(ctl).Width - w)
				{
					x = Screen.GetWorkingArea(ctl).Width - w;
				}
				if (y > Screen.GetWorkingArea(ctl).Height - h)
				{
					y = Screen.GetWorkingArea(ctl).Height - h;
				}
				
				// モニタを逆転させるとマイナスになる場合もあるので防ぐ
				if (x < 0)
				{
					x = 0;
				}
				if (y < 0)
				{
					y = 0;
				}
				
				ctl.SetBounds(x, y, w, h);
			}
			


			return success;
		}
		#endregion
	}
}
