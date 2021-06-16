using System;
using System.Drawing;
using System.Windows.Forms;

namespace Uindies.Library
{
	/// <summary>
	/// [作成者 fj]
	/// Formを扱うための標準クラスです。
	/// </summary>
	public class FormCommon
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern bool MoveWindow(
			IntPtr hWnd,
			int    X,
			int    Y,
			int    nWidth,
			int    nHeight,
			bool   bRepaint);
		
		/// <summary>
		/// キー名のImageIndexを取得します。
		/// </summary>
		/// <param name="imglist">イメージリスト</param>
		/// <param name="key">キー名</param>
		/// <returns>該当するイメージIndex。なければ-1</returns>
		public static int GetImageIndex(ImageList imglist, string key)
		{
			int		index = imglist.Images.IndexOfKey(key);
			
			return index;
		}
		
		/// <summary>
		/// キー名のImageを取得します。
		/// </summary>
		/// <param name="imglist">イメージリスト</param>
		/// <param name="key">キー名</param>
		/// <returns>該当するイメージ。なければnull</returns>
		public static Image GetImage(ImageList imglist, string key)
		{
			int		index = imglist.Images.IndexOfKey(key);
			
			if (index < 0)
			{
				return null;
			}
			
			return imglist.Images[index];
		}
		
		/// <summary>
		/// フォームのMdiClientコントロールを探して返します。
		/// </summary>
		/// <param name="form">MdiClientコントロールを探すフォーム</param>
		/// <returns>見つかったMdiClientコントロール。見つからなければnull。</returns>
		public static MdiClient GetMdiClient(Form form)
		{
			foreach (Control ctl in form.Controls)
			{
				if (ctl is MdiClient)
				{
					return (MdiClient)ctl;
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// ウィンドウの位置とサイズを変更します。
		/// </summary>
		/// <param name="form">位置とサイズを変更するウィンドウ</param>
		/// <param name="x">横位置</param>
		/// <param name="y">縦位置</param>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		public static void SetWindowBounds(Form form, int x, int y, int width, int height)
		{
			int		w, h;
			
			// MaximumSizeを大きくしておく
			w = form.MaximumSize.Width  < width  ? width  : form.MaximumSize.Width ;
			h = form.MaximumSize.Height < height ? height : form.MaximumSize.Height;
			
			form.MaximumSize = new Size(w, h);
			MoveWindow(form.Handle, x, y, width, height, true);
		}
		
		/// <summary>
		/// フォームに配置されているコントロールを名前で探します。
		/// </summary>
		/// <param name="ctl">コントロールの存在する親コントロール</param>
		/// <param name="name">コントロールのName</param>
		/// <returns>見つかった時は、コントロールのオブジェクト。
		/// 見つからなかった時は、null。</returns>
		public static object FindControlByName(Control ctl, string name)
		{
			// Form だけではなく親コントロールにぶらさがったコントロールも探せるよう修正 2010/2/14 by fj
			System.Type t = ctl.GetType();
			
			System.Reflection.FieldInfo fi = t.GetField(
				name,
				System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.NonPublic |
				System.Reflection.BindingFlags.Instance |
				System.Reflection.BindingFlags.DeclaredOnly);
			
			if (fi == null) return null;
			
			return fi.GetValue(ctl);
		}
		
		/// <summary>
		/// エンターフォーカス移動を許可します。
		/// </summary>
		/// <param name="frm">許可するフォーム</param>
		public static void EnableEnterFocus(Form frm)
		{
			frm.KeyPreview = true;
			
			frm.KeyDown  += new System.Windows.Forms.KeyEventHandler(EnterFocusKeyDown);
			frm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EnterFocusKeyPress);
		}
		
		/// <summary>
		/// エンターフォーカス移動を禁止します。
		/// </summary>
		/// <param name="frm">禁止するフォーム</param>
		public static void DisableEnterFocus(Form frm)
		{
			frm.KeyDown  -= new System.Windows.Forms.KeyEventHandler(EnterFocusKeyDown);
			frm.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(EnterFocusKeyPress);
		}
		
		/// <summary>
		/// 次のコントロールにフォーカスを移動します
		/// </summary>
		/// <param name="conctl">コントロールのあるフォーム</param>
		/// <param name="forward">true..順方向, false..逆方向</param>
		public static void SelectNextControl(ContainerControl conctl, bool forward)
		{
			SelectNextControl(conctl, conctl.ActiveControl, forward);
		}
		
		/// <summary>
		/// 次のコントロールにフォーカスを移動します
		/// </summary>
		/// <param name="conctl">コントロールのあるフォーム</param>
		/// <param name="ctl">アクティブなコントロール</param>
		/// <param name="forward">true..順方向, false..逆方向</param>
		public static void SelectNextControl(ContainerControl conctl, Control ctl, bool forward)
		{
			if (ctl is UserControl)
			{
				UserControl	uc  = (UserControl)ctl;
				
				// ユーザーコントロール内のタブ移動
				bool		ret = uc.SelectNextControl(uc.ActiveControl, forward, true, true, false);
				
				// ユーザーコントロール内の移動が終わったのなら次のコントロールへ
				if (ret == false)
				{
					#if DEBUG
						AppLog.Write("mx " + ctl.Name);
					#endif
					conctl.SelectNextControl(ctl, forward, true, true, true);
					#if DEBUG
						AppLog.WriteLine("→" + conctl.ActiveControl.Name);
					#endif
				}
			}
			else if (ctl == null)
			{
				ErrLog.WriteLine("フォーカスを受けたコントロールが、指定されたフォーム上にありません。");
			}
			else
			{
				// 通常のタブ移動
				#if DEBUG
					AppLog.Write("nx " + ctl.Name);
				#endif
				conctl.SelectNextControl(ctl, forward, true, true, true);
				#if DEBUG
					AppLog.WriteLine("→" + conctl.ActiveControl.Name);
				#endif
			}
		}
		
		/// <summary>
		/// EnterFocus 用 KeyDown イベントです。
		/// </summary>
		public static void EnterFocusKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			ContainerControl frm = (ContainerControl)sender;
			
			// [Enter] キーで次の TabIndex があるコントロールへフォーカスを移す
			if (e.KeyCode == Keys.Enter)
			{
				if (frm.ActiveControl != null)
				{
					if (!e.Control)
					{
						SelectNextControl(frm, !e.Shift);
						e.Handled = true;
					}
				}
			}
		}
		
		/// <summary>
		/// EnterFocus 用 KeyPress イベントです。
		/// </summary>
		public static void EnterFocusKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// フォーカス遷移後の音を消すためにキーイベントが処理されたことにする
			if (e.KeyChar == (char)Keys.Enter) 
			{
				e.Handled = true;
			}
		}
	}
}
