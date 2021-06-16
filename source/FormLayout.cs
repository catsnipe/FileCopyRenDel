using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ComponentForm
{
	#region *** Public Enumerator ***
	/// <summary>
	/// ���C�A�E�g�̒������[���B
	/// </summary>
	public enum FormLayoutRule
	{
		/// <summary>�ύX�Ȃ��B</summary>
		None = 0,
		/// <summary>�䗦�Œ����B</summary>
		Ratio,
		/// <summary>�����ɂ���悤�����B</summary>
		Center,
		/// <summary>�t�H�[���̍��[�A��[�Ɠ���������pixel�𔺂��Ē����B</summary>
		LinkTop,
		/// <summary>�t�H�[���̉E�[�A���[�Ɠ���������pixel�𔺂��Ē����B</summary>
		LinkBottom,
	}
	#endregion
	
	/// <summary>
	/// �������C�A�E�g��������R���g���[���̏��B
	/// </summary>
	public class FormLayoutCtlPosition
	{
		/// <summary>���C�A�E�g�ύX����R���g���[���B</summary>
		public Control			Control = null;
		
		/// <summary>X�ʒu�����C�A�E�g�ύX���邩�H</summary>
		public FormLayoutRule	RuleX = FormLayoutRule.None;
		/// <summary>Y�ʒu�����C�A�E�g�ύX���邩�H</summary>
		public FormLayoutRule	RuleY = FormLayoutRule.None;
		/// <summary>Width�����C�A�E�g�ύX���邩�H</summary>
		public FormLayoutRule	RuleW = FormLayoutRule.None;
		/// <summary>Height�����C�A�E�g�ύX���邩�H</summary>
		public FormLayoutRule	RuleH = FormLayoutRule.None;
		
		/// <summary>�R���g���[���̌���X�ʒu�B</summary>
		public int				X = 0;
		/// <summary>�R���g���[���̌���Y�ʒu�B</summary>
		public int				Y = 0;
		/// <summary>�R���g���[���̌���Width�B</summary>
		public int				W = 0;
		/// <summary>�R���g���[���̌���Height�B</summary>
		public int				H = 0;
	}
	
	/// <summary>
	/// [�쐬�� fj]
	/// �t�H�[���̈ړ��ɍ��킹�āA�����I�ɃR���g���[���̃��C�A�E�g�𒲐����܂��B
	/// </summary>
	public class FormLayout
	{
		// 
		// �t�H�[���ݒ�̗�B
		// 
		// > �t�H�[�������ɐL�΂������ɂ��܂����T�C�W���O����ݒ��B
		// �@�@������ �� �����@�����@����
		// �@�@LeftTop,Ratio�@Ratio,Ratio�@Ratio,LeftBottom
		// 
		
		#region *** Private Value ***
		/// <summary>���C�A�E�g�������������鎞�ɎQ�Ƃ���t�H�[���B</summary>
		Form						parent;
		/// <summary>�Q�Ƃ���t�H�[���̏���Width, Height�B</summary>
		int							pw, ph;
		/// <summary>���C�A�E�g�ύX����R���g���[���̃��X�g�B</summary>
		List<FormLayoutCtlPosition>	ctllist;
		/// <summary>�����Ɏg�p����Control�B</summary>
		Control						search_ctl;
		/// <summary>�Ȃ�炩�� StripControl �Őe�̕\���G���A�ɕω�������ꍇ�̕␳�l�B</summary>
		int							parentAdjustX;
		/// <summary>�Ȃ�炩�� StripControl �Őe�̕\���G���A�ɕω�������ꍇ�̕␳�l�BFunction ���z�u����Ă��鎞�ȂǂɎg���B</summary>
		int							parentAdjustY;
		/// <summary>EndAdd �����s�������H�@���Ă����� true</summary>
		bool						executeEndAdd;
		/// <summary>Resize ���ɃR�[������郆�[�U�[�p���\�b�h</summary>
		public event EventHandler	Resize;
		#endregion
		
		#region *** Public Method ***
		/// <summary>
		/// �R���X�g���N�^�B
		/// </summary>
		/// <param name="_parent">���C�A�E�g�������������鎞�ɎQ�Ƃ���t�H�[���B</param>
		public FormLayout(Form _parent)
		{
			//�� ���X�g�o�b�t�@�쐬�B
			ctllist = new List<FormLayoutCtlPosition>();
			
			//�� �e�t�H�[���̏����擾�B
			ResetFormSize(_parent);
			
			//�� �����l�ݒ�B
			search_ctl	  = null;
			parentAdjustX = 0;
			parentAdjustY = 0;
			executeEndAdd = false;
			Resize		  = null;
			
			// ���T�C�Y�ɍ��킹�ă��C�A�E�g�ύX���\�b�h���ĂԁB
			parent.Resize += new System.EventHandler(parent_Resize);
		}
		
		/// <summary>
		/// �S�ĊJ���B
		/// </summary>
		public void Dispose()
		{
			parent.Resize -= new System.EventHandler(parent_Resize);
			ctllist.Clear();
		}
		
		/// <summary>
		/// �t�H�[���T�C�Y�̃��Z�b�g�B
		/// </summary>
		/// <param name="_parent">�e�t�H�[��</param>
		public void ResetFormSize(Form _parent)
		{
			//�� �e�t�H�[���̏����擾�B
			parent	= _parent;
			pw		= parent.ClientSize.Width;
			ph		= parent.ClientSize.Height;
		}
		
		/// <summary>
		/// ���C�A�E�g��������R���g���[���̉������s���܂��B
		/// �R���g���[���Ɏq�R���g���[�������݂����ꍇ�A������ꏏ�ɍs���܂��B
		/// </summary>
		/// <param name="ctl">���C�A�E�g��������������R���g���[���B</param>
		public void RemoveControls(Control ctl)
		{
			// �e�R���g���[���B
			RemoveControl(ctl);
			
			// �q�R���g���[���B
			for (int i = 0; i < ctl.Controls.Count; i++)
			{
				// ����ɉ��̃R���g���[������������B
				if (ctl.Controls[i].Controls.Count > 0)
				{
					// �ċA�I�ɌĂяo���B
					RemoveControls(ctl.Controls[i]);
				}
				else
				{
					RemoveControl(ctl.Controls[i]);
				}
			}
		}
		
		/// <summary>
		/// ���C�A�E�g��������R���g���[���̉������s���܂��B
		/// </summary>
		/// <param name="ctl">���C�A�E�g��������������R���g���[���B</param>
		public void RemoveControl(Control ctl)
		{
			// ��r���̐ݒ�B
			search_ctl = ctl;
			
			//�� isMatchControl�ŊY������List��S�č폜����B
			ctllist.FindAll(isMatchControl).ForEach(removeControl);
		}
		
		/// <summary>
		/// ���C�A�E�g��������R���g���[���̒ǉ����s���܂��B
		/// </summary>
		/// <param name="ctl">���C�A�E�g��������R���g���[���B</param>
		/// <param name="_update_x">true..X�ʒu�𒲐�����B</param>
		/// <param name="_update_y">true..Y�ʒu�𒲐�����B</param>
		/// <param name="_update_w">true..Width�𒲐�����B</param>
		/// <param name="_update_h">true..Height�𒲐�����B</param>
		public FormLayoutCtlPosition AddControl(Control ctl, FormLayoutRule _update_x, FormLayoutRule _update_y, FormLayoutRule _update_w, FormLayoutRule _update_h)
		{
			return addControl(ctl, ctl.Left, ctl.Top, ctl.Width, ctl.Height, _update_x, _update_y, _update_w, _update_h);
		}
		
		/// <summary>
		/// ���C�A�E�g��������R���g���[���̒ǉ����s���܂��B
		/// ��������v�Z���̓t�H�[���Ƃ̑傫���̔䗦�ōs���܂��B
		/// </summary>
		/// <param name="ctl">���C�A�E�g��������R���g���[���B</param>
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
		/// �ǉ��I���̍��}�B�T�C�Y�����������܂��B
		/// </summary>
		public void EndAdd()
		{
			executeEndAdd = true;
			layoutUpdate();
		}
		
		/// <summary>
		/// �ǉ��I���̍��}�B�T�C�Y�����������܂��B
		/// </summary>
		/// <param name="adjx">�Ȃ�炩�� StripControl �Őe�̕\���G���A�ɕω�������ꍇ�̕␳�l�B</param>
		/// <param name="adjy">�Ȃ�炩�� StripControl �Őe�̕\���G���A�ɕω�������ꍇ�̕␳�l�BFunction ���z�u����Ă��鎞�ȂǂɎg���B</param>
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
		/// ���C�A�E�g�̍X�V�B���T�C�Y�ɍ��킹�Ď����I�ɌĂ΂�܂��B
		/// </summary>
		void parent_Resize(object sender, EventArgs e)
		{
			layoutUpdate();
			
			// ���[�U�[�R�[���o�b�N
			if (Resize != null)
			{
				Resize(sender, e);
			}
		}
		
		/// <summary>
		/// ���C�A�E�g��������R���g���[���̒ǉ����s���܂��B
		/// </summary>
		/// <param name="ctl">���C�A�E�g��������R���g���[���B</param>
		/// <param name="x">ctl.Left�̒l�B</param>
		/// <param name="y">ctl.Top�̒l�B</param>
		/// <param name="w">ctl.Width�̒l�B</param>
		/// <param name="h">ctl.Height�̒l�B</param>
		/// <param name="_update_x">Left�̈ʒu�������[���B</param>
		/// <param name="_update_y">Top�̈ʒu�������[���B</param>
		/// <param name="_update_w">Width�̈ʒu�������[���B</param>
		/// <param name="_update_h">Height�̈ʒu�������[���B</param>
		/// <returns>�o�^�������B</returns>
		FormLayoutCtlPosition addControl(Control ctl, int x, int y, int w, int h, FormLayoutRule _update_x, FormLayoutRule _update_y, FormLayoutRule _update_w, FormLayoutRule _update_h)
		{
			FormLayoutCtlPosition	ctlp;
			bool		newctl;
			
			//�� ���ɓo�^���Ȃ����ǂ������ׂ�B
			// ��r���̐ݒ�B
			search_ctl = ctl;
			
			ctlp = ctllist.Find(isMatchControl);
			
			// �o�^���Ȃ���ΐV�K�쐬�B
			if (ctlp == null)
			{
			    // �V�K�쐬�B
			    ctlp = new FormLayoutCtlPosition();
				
			    newctl = true;
			}
			else
			{
			    newctl = false;
			}
			
			//�� �f�[�^��o�^�B
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
			    // �V�K�쐬����ǉ��B
			    ctllist.Add(ctlp);
			}
			
			return ctlp;
		}
		
		/// <summary>
		/// �����p�̔�r���\�b�h�B
		/// </summary>
		/// <param name="ctlp">��r�����CtlPosition�B</param>
		/// <returns>��v������true�A�Ⴆ��false�B</returns>
		bool isMatchControl(FormLayoutCtlPosition ctlp)
		{
			return ctlp.Control == search_ctl;
		}
		
		/// <summary>
		/// �폜���\�b�h�B
		/// </summary>
		/// <param name="ctlp">�폜����CtlPosition�B</param>
		void removeControl(FormLayoutCtlPosition ctlp)
		{
			ctllist.Remove(ctlp);
		}
		
		/// <summary>
		/// ���C�A�E�g�̍X�V�B�����I�ɌĂяo�����Ɏg���܂��B
		/// </summary>
		void layoutUpdate()
		{
			if (executeEndAdd == false)
			{
//				ErrLog.WriteLine("EndAdd �����s���Ă��Ȃ����߁A���C�A�E�g�̍X�V���o���܂���B");
				
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
