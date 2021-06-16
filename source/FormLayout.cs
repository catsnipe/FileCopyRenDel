using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ComponentForm
{
	#region *** Public Enumerator ***
	/// <summary>
	/// レイアウトの調整ルール。
	/// </summary>
	public enum FormLayoutRule
	{
		/// <summary>変更なし。</summary>
		None = 0,
		/// <summary>比率で調整。</summary>
		Ratio,
		/// <summary>中央にくるよう調整。</summary>
		Center,
		/// <summary>フォームの左端、上端と同じ差分のpixelを伴って調整。</summary>
		LinkTop,
		/// <summary>フォームの右端、下端と同じ差分のpixelを伴って調整。</summary>
		LinkBottom,
	}
	#endregion
	
	/// <summary>
	/// 自動レイアウト調整するコントロールの情報。
	/// </summary>
	public class FormLayoutCtlPosition
	{
		/// <summary>レイアウト変更するコントロール。</summary>
		public Control			Control = null;
		
		/// <summary>X位置をレイアウト変更するか？</summary>
		public FormLayoutRule	RuleX = FormLayoutRule.None;
		/// <summary>Y位置をレイアウト変更するか？</summary>
		public FormLayoutRule	RuleY = FormLayoutRule.None;
		/// <summary>Widthをレイアウト変更するか？</summary>
		public FormLayoutRule	RuleW = FormLayoutRule.None;
		/// <summary>Heightをレイアウト変更するか？</summary>
		public FormLayoutRule	RuleH = FormLayoutRule.None;
		
		/// <summary>コントロールの元のX位置。</summary>
		public int				X = 0;
		/// <summary>コントロールの元のY位置。</summary>
		public int				Y = 0;
		/// <summary>コントロールの元のWidth。</summary>
		public int				W = 0;
		/// <summary>コントロールの元のHeight。</summary>
		public int				H = 0;
	}
	
	/// <summary>
	/// [作成者 fj]
	/// フォームの移動に合わせて、自動的にコントロールのレイアウトを調整します。
	/// </summary>
	public class FormLayout
	{
		// 
		// フォーム設定の例。
		// 
		// > フォームを横に伸ばした時にうまくリサイジングする設定例。
		// 　　■■■ → ■■　■■　■■
		// 　　LeftTop,Ratio　Ratio,Ratio　Ratio,LeftBottom
		// 
		
		#region *** Private Value ***
		/// <summary>レイアウトを自動調整する時に参照するフォーム。</summary>
		Form						parent;
		/// <summary>参照するフォームの初期Width, Height。</summary>
		int							pw, ph;
		/// <summary>レイアウト変更するコントロールのリスト。</summary>
		List<FormLayoutCtlPosition>	ctllist;
		/// <summary>検索に使用するControl。</summary>
		Control						search_ctl;
		/// <summary>なんらかの StripControl で親の表示エリアに変化がある場合の補正値。</summary>
		int							parentAdjustX;
		/// <summary>なんらかの StripControl で親の表示エリアに変化がある場合の補正値。Function が配置されている時などに使う。</summary>
		int							parentAdjustY;
		/// <summary>EndAdd を実行したか？　していたら true</summary>
		bool						executeEndAdd;
		/// <summary>Resize 時にコールされるユーザー用メソッド</summary>
		public event EventHandler	Resize;
		#endregion
		
		#region *** Public Method ***
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="_parent">レイアウトを自動調整する時に参照するフォーム。</param>
		public FormLayout(Form _parent)
		{
			//■ リストバッファ作成。
			ctllist = new List<FormLayoutCtlPosition>();
			
			//■ 親フォームの情報を取得。
			ResetFormSize(_parent);
			
			//■ 初期値設定。
			search_ctl	  = null;
			parentAdjustX = 0;
			parentAdjustY = 0;
			executeEndAdd = false;
			Resize		  = null;
			
			// リサイズに合わせてレイアウト変更メソッドを呼ぶ。
			parent.Resize += new System.EventHandler(parent_Resize);
		}
		
		/// <summary>
		/// 全て開放。
		/// </summary>
		public void Dispose()
		{
			parent.Resize -= new System.EventHandler(parent_Resize);
			ctllist.Clear();
		}
		
		/// <summary>
		/// フォームサイズのリセット。
		/// </summary>
		/// <param name="_parent">親フォーム</param>
		public void ResetFormSize(Form _parent)
		{
			//■ 親フォームの情報を取得。
			parent	= _parent;
			pw		= parent.ClientSize.Width;
			ph		= parent.ClientSize.Height;
		}
		
		/// <summary>
		/// レイアウト調整するコントロールの解除を行います。
		/// コントロールに子コントロールが存在した場合、それも一緒に行います。
		/// </summary>
		/// <param name="ctl">レイアウト調整を解除するコントロール。</param>
		public void RemoveControls(Control ctl)
		{
			// 親コントロール。
			RemoveControl(ctl);
			
			// 子コントロール。
			for (int i = 0; i < ctl.Controls.Count; i++)
			{
				// さらに下のコントロールがあったら。
				if (ctl.Controls[i].Controls.Count > 0)
				{
					// 再帰的に呼び出す。
					RemoveControls(ctl.Controls[i]);
				}
				else
				{
					RemoveControl(ctl.Controls[i]);
				}
			}
		}
		
		/// <summary>
		/// レイアウト調整するコントロールの解除を行います。
		/// </summary>
		/// <param name="ctl">レイアウト調整を解除するコントロール。</param>
		public void RemoveControl(Control ctl)
		{
			// 比較元の設定。
			search_ctl = ctl;
			
			//■ isMatchControlで該当したListを全て削除する。
			ctllist.FindAll(isMatchControl).ForEach(removeControl);
		}
		
		/// <summary>
		/// レイアウト調整するコントロールの追加を行います。
		/// </summary>
		/// <param name="ctl">レイアウト調整するコントロール。</param>
		/// <param name="_update_x">true..X位置を調整する。</param>
		/// <param name="_update_y">true..Y位置を調整する。</param>
		/// <param name="_update_w">true..Widthを調整する。</param>
		/// <param name="_update_h">true..Heightを調整する。</param>
		public FormLayoutCtlPosition AddControl(Control ctl, FormLayoutRule _update_x, FormLayoutRule _update_y, FormLayoutRule _update_w, FormLayoutRule _update_h)
		{
			return addControl(ctl, ctl.Left, ctl.Top, ctl.Width, ctl.Height, _update_x, _update_y, _update_w, _update_h);
		}
		
		/// <summary>
		/// レイアウト調整するコントロールの追加を行います。
		/// 調整する計算式はフォームとの大きさの比率で行います。
		/// </summary>
		/// <param name="ctl">レイアウト調整するコントロール。</param>
		public FormLayoutCtlPosition AddControlRatio(Control ctl)
		{
			return AddControl(
				ctl,
				FormLayoutRule.Ratio,
				FormLayoutRule.Ratio,
				FormLayoutRule.Ratio,
				FormLayoutRule.Ratio);
		}
		
		/// <summary>
		/// 追加終了の合図。サイズを初期化します。
		/// </summary>
		public void EndAdd()
		{
			executeEndAdd = true;
			layoutUpdate();
		}
		
		/// <summary>
		/// 追加終了の合図。サイズを初期化します。
		/// </summary>
		/// <param name="adjx">なんらかの StripControl で親の表示エリアに変化がある場合の補正値。</param>
		/// <param name="adjy">なんらかの StripControl で親の表示エリアに変化がある場合の補正値。Function が配置されている時などに使う。</param>
		public void EndAdd(int adjx, int adjy)
		{
			parentAdjustX = adjx;
			parentAdjustY = adjy;
			executeEndAdd = true;
			
			layoutUpdate();
		}
		#endregion

		#region *** Private Method ***
		/// <summary>
		/// レイアウトの更新。リサイズに合わせて自動的に呼ばれます。
		/// </summary>
		void parent_Resize(object sender, EventArgs e)
		{
			layoutUpdate();
			
			// ユーザーコールバック
			if (Resize != null)
			{
				Resize(sender, e);
			}
		}
		
		/// <summary>
		/// レイアウト調整するコントロールの追加を行います。
		/// </summary>
		/// <param name="ctl">レイアウト調整するコントロール。</param>
		/// <param name="x">ctl.Leftの値。</param>
		/// <param name="y">ctl.Topの値。</param>
		/// <param name="w">ctl.Widthの値。</param>
		/// <param name="h">ctl.Heightの値。</param>
		/// <param name="_update_x">Leftの位置調整ルール。</param>
		/// <param name="_update_y">Topの位置調整ルール。</param>
		/// <param name="_update_w">Widthの位置調整ルール。</param>
		/// <param name="_update_h">Heightの位置調整ルール。</param>
		/// <returns>登録した情報。</returns>
		FormLayoutCtlPosition addControl(Control ctl, int x, int y, int w, int h, FormLayoutRule _update_x, FormLayoutRule _update_y, FormLayoutRule _update_w, FormLayoutRule _update_h)
		{
			FormLayoutCtlPosition	ctlp;
			bool		newctl;
			
			//■ 既に登録がないかどうか調べる。
			// 比較元の設定。
			search_ctl = ctl;
			
			ctlp = ctllist.Find(isMatchControl);
			
			// 登録がなければ新規作成。
			if (ctlp == null)
			{
			    // 新規作成。
			    ctlp = new FormLayoutCtlPosition();
				
			    newctl = true;
			}
			else
			{
			    newctl = false;
			}
			
			//■ データを登録。
			ctlp.Control      = ctl;
			ctlp.X        = x;
			ctlp.Y        = y;
			ctlp.W        = w;
			ctlp.H        = h;
			ctlp.RuleX = _update_x;
			ctlp.RuleY = _update_y;
			ctlp.RuleW = _update_w;
			ctlp.RuleH = _update_h;
			
			if (newctl == true)
			{
			    // 新規作成分を追加。
			    ctllist.Add(ctlp);
			}
			
			return ctlp;
		}
		
		/// <summary>
		/// 検索用の比較メソッド。
		/// </summary>
		/// <param name="ctlp">比較されるCtlPosition。</param>
		/// <returns>一致したらtrue、違えばfalse。</returns>
		bool isMatchControl(FormLayoutCtlPosition ctlp)
		{
			return ctlp.Control == search_ctl;
		}
		
		/// <summary>
		/// 削除メソッド。
		/// </summary>
		/// <param name="ctlp">削除するCtlPosition。</param>
		void removeControl(FormLayoutCtlPosition ctlp)
		{
			ctllist.Remove(ctlp);
		}
		
		/// <summary>
		/// レイアウトの更新。明示的に呼び出す時に使います。
		/// </summary>
		void layoutUpdate()
		{
			if (executeEndAdd == false)
			{
//				ErrLog.WriteLine("EndAdd を実行していないため、レイアウトの更新が出来ません。");
				
				return;
			}
			
			int		left;
			int		top;
			int		width;
			int		height;
			
			foreach (FormLayoutCtlPosition ctlp in ctllist)
			{
				// X
				if (ctlp.RuleX == FormLayoutRule.Ratio)
				{
					left	= ctlp.X *  parent.ClientSize.Width / pw;
				}
				else if (ctlp.RuleX == FormLayoutRule.LinkTop)
				{
					left	= ctlp.X;
				}
				else if (ctlp.RuleX == FormLayoutRule.LinkBottom)
				{
					left	= ctlp.X + (parent.ClientSize.Width - pw);
				}
				else if (ctlp.RuleX == FormLayoutRule.Center)
				{
					left	= (parent.ClientSize.Width - parentAdjustX - ctlp.W) / 2;
				}
				else
				{
					left	= ctlp.Control.Left;
				}
				
				// Y
				if (ctlp.RuleY == FormLayoutRule.Ratio)
				{
					top		= ctlp.Y *  parent.ClientSize.Height / ph;
				}
				else if (ctlp.RuleY == FormLayoutRule.LinkTop)
				{
					top		= ctlp.Y;
				}
				else if (ctlp.RuleY == FormLayoutRule.LinkBottom)
				{
					top		= ctlp.Y + (parent.ClientSize.Height - ph);
				}
				else if (ctlp.RuleY == FormLayoutRule.Center)
				{
					top		= (parent.ClientSize.Height - parentAdjustY - ctlp.H) / 2;
				}
				else
				{
					top		= ctlp.Control.Top;
				}
				
				// W
				if (ctlp.RuleW == FormLayoutRule.Ratio)
				{
					width	= ctlp.W *  parent.ClientSize.Width / pw;
					width  += (ctlp.X *  parent.ClientSize.Width / pw) - left;
				}
				else if (ctlp.RuleW == FormLayoutRule.LinkTop)
				{
					width	= ctlp.W + (parent.ClientSize.Width - pw);
				}
				else if (ctlp.RuleW == FormLayoutRule.LinkBottom)
				{
					width	= parent.ClientSize.Width - left - (pw - (ctlp.X + ctlp.W));
				}
				else
				{
					width	= ctlp.Control.Width;
				}
				
				// H
				if (ctlp.RuleH == FormLayoutRule.Ratio)
				{
					height	= ctlp.H *  parent.ClientSize.Height / ph;
					height += (ctlp.Y *  parent.ClientSize.Height / ph) - top;
				}
				else if (ctlp.RuleH == FormLayoutRule.LinkTop)
				{
					height	= ctlp.H + (parent.ClientSize.Height - ph);
				}
				else if (ctlp.RuleH == FormLayoutRule.LinkBottom)
				{
					height	= parent.ClientSize.Height - top - (ph - (ctlp.Y + ctlp.H));
				}
				else
				{
					height	= ctlp.Control.Height;
				}
				
				ctlp.Control.SetBounds(left, top, width, height);
			}
		}
		#endregion
	}
}
