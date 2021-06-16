using System;
using System.Drawing;
using System.Windows.Forms;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// Form���������߂̕W���N���X�ł��B
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
		/// �L�[����ImageIndex���擾���܂��B
		/// </summary>
		/// <param name="imglist">�C���[�W���X�g</param>
		/// <param name="key">�L�[��</param>
		/// <returns>�Y������C���[�WIndex�B�Ȃ����-1</returns>
		public static int GetImageIndex(ImageList imglist, string key)
		{
			int		index = imglist.Images.IndexOfKey(key);
			
			return index;
		}
		
		/// <summary>
		/// �L�[����Image���擾���܂��B
		/// </summary>
		/// <param name="imglist">�C���[�W���X�g</param>
		/// <param name="key">�L�[��</param>
		/// <returns>�Y������C���[�W�B�Ȃ����null</returns>
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
		/// �t�H�[����MdiClient�R���g���[����T���ĕԂ��܂��B
		/// </summary>
		/// <param name="form">MdiClient�R���g���[����T���t�H�[��</param>
		/// <returns>��������MdiClient�R���g���[���B������Ȃ����null�B</returns>
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
		/// �E�B���h�E�̈ʒu�ƃT�C�Y��ύX���܂��B
		/// </summary>
		/// <param name="form">�ʒu�ƃT�C�Y��ύX����E�B���h�E</param>
		/// <param name="x">���ʒu</param>
		/// <param name="y">�c�ʒu</param>
		/// <param name="width">��</param>
		/// <param name="height">����</param>
		public static void SetWindowBounds(Form form, int x, int y, int width, int height)
		{
			int		w, h;
			
			// MaximumSize��傫�����Ă���
			w = form.MaximumSize.Width  < width  ? width  : form.MaximumSize.Width ;
			h = form.MaximumSize.Height < height ? height : form.MaximumSize.Height;
			
			form.MaximumSize = new Size(w, h);
			MoveWindow(form.Handle, x, y, width, height, true);
		}
		
		/// <summary>
		/// �t�H�[���ɔz�u����Ă���R���g���[���𖼑O�ŒT���܂��B
		/// </summary>
		/// <param name="ctl">�R���g���[���̑��݂���e�R���g���[��</param>
		/// <param name="name">�R���g���[����Name</param>
		/// <returns>�����������́A�R���g���[���̃I�u�W�F�N�g�B
		/// ������Ȃ��������́Anull�B</returns>
		public static object FindControlByName(Control ctl, string name)
		{
			// Form �����ł͂Ȃ��e�R���g���[���ɂԂ炳�������R���g���[�����T����悤�C�� 2010/2/14 by fj
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
		/// �G���^�[�t�H�[�J�X�ړ��������܂��B
		/// </summary>
		/// <param name="frm">������t�H�[��</param>
		public static void EnableEnterFocus(Form frm)
		{
			frm.KeyPreview = true;
			
			frm.KeyDown  += new System.Windows.Forms.KeyEventHandler(EnterFocusKeyDown);
			frm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EnterFocusKeyPress);
		}
		
		/// <summary>
		/// �G���^�[�t�H�[�J�X�ړ����֎~���܂��B
		/// </summary>
		/// <param name="frm">�֎~����t�H�[��</param>
		public static void DisableEnterFocus(Form frm)
		{
			frm.KeyDown  -= new System.Windows.Forms.KeyEventHandler(EnterFocusKeyDown);
			frm.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(EnterFocusKeyPress);
		}
		
		/// <summary>
		/// ���̃R���g���[���Ƀt�H�[�J�X���ړ����܂�
		/// </summary>
		/// <param name="conctl">�R���g���[���̂���t�H�[��</param>
		/// <param name="forward">true..������, false..�t����</param>
		public static void SelectNextControl(ContainerControl conctl, bool forward)
		{
			SelectNextControl(conctl, conctl.ActiveControl, forward);
		}
		
		/// <summary>
		/// ���̃R���g���[���Ƀt�H�[�J�X���ړ����܂�
		/// </summary>
		/// <param name="conctl">�R���g���[���̂���t�H�[��</param>
		/// <param name="ctl">�A�N�e�B�u�ȃR���g���[��</param>
		/// <param name="forward">true..������, false..�t����</param>
		public static void SelectNextControl(ContainerControl conctl, Control ctl, bool forward)
		{
			if (ctl is UserControl)
			{
				UserControl	uc  = (UserControl)ctl;
				
				// ���[�U�[�R���g���[�����̃^�u�ړ�
				bool		ret = uc.SelectNextControl(uc.ActiveControl, forward, true, true, false);
				
				// ���[�U�[�R���g���[�����̈ړ����I������̂Ȃ玟�̃R���g���[����
				if (ret == false)
				{
					#if DEBUG
						AppLog.Write("mx " + ctl.Name);
					#endif
					conctl.SelectNextControl(ctl, forward, true, true, true);
					#if DEBUG
						AppLog.WriteLine("��" + conctl.ActiveControl.Name);
					#endif
				}
			}
			else if (ctl == null)
			{
				ErrLog.WriteLine("�t�H�[�J�X���󂯂��R���g���[�����A�w�肳�ꂽ�t�H�[����ɂ���܂���B");
			}
			else
			{
				// �ʏ�̃^�u�ړ�
				#if DEBUG
					AppLog.Write("nx " + ctl.Name);
				#endif
				conctl.SelectNextControl(ctl, forward, true, true, true);
				#if DEBUG
					AppLog.WriteLine("��" + conctl.ActiveControl.Name);
				#endif
			}
		}
		
		/// <summary>
		/// EnterFocus �p KeyDown �C�x���g�ł��B
		/// </summary>
		public static void EnterFocusKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			ContainerControl frm = (ContainerControl)sender;
			
			// [Enter] �L�[�Ŏ��� TabIndex ������R���g���[���փt�H�[�J�X���ڂ�
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
		/// EnterFocus �p KeyPress �C�x���g�ł��B
		/// </summary>
		public static void EnterFocusKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// �t�H�[�J�X�J�ڌ�̉����������߂ɃL�[�C�x���g���������ꂽ���Ƃɂ���
			if (e.KeyChar == (char)Keys.Enter) 
			{
				e.Handled = true;
			}
		}
	}
}
