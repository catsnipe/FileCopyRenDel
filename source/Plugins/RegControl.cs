using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Uindies.Library
{
	/// <summary>
	/// [�쐬�� fj]
	/// �R���g���[���̋N���ɕK�v�ȃ��W�X�g���l�̕ۑ��E�ǂݍ��݂��s���܂��B
	/// </summary>
	public class RegControl
	{
		#region *** Private Const Value ***
		/// <summary>
		/// �R���g���[������ۑ����Ă������W�X�g���L�[
		/// </summary>
		public const string		REGCON_KEY = @"Controls\";
		#endregion
		
		#region *** Public Method ***
		/// <summary>
		/// XY�̍��W�l�����W�X�g���ɓo�^���܂��B
		/// </summary>
		public static void PushXY(Control ctl)
		{
			pushXYWH(ctl, true, true, false, false);
		}
		
		/// <summary>
		/// WH�̍��W�l�����W�X�g���ɓo�^���܂��B
		/// </summary>
		public static void PushWH(Control ctl)
		{
			pushXYWH(ctl, false, false, true, true);
		}
		
		/// <summary>
		/// XYWH�̍��W�l�����W�X�g���ɓo�^���܂��B
		/// </summary>
		public static void PushXYWH(Control ctl)
		{
			pushXYWH(ctl, true, true, true, true);
		}
		
		/// <summary>
		/// XY�̍��W�l�����W�X�g������ǂݍ��݂܂��B�Ȃ��ꍇ�͖������܂��B
		/// </summary>
		public static bool PopXY(Control ctl)
		{
			return popXYWH(ctl, true, true, false, false);
		}
		
		/// <summary>
		/// WH�̍��W�l�����W�X�g������ǂݍ��݂܂��B�Ȃ��ꍇ�͖������܂��B
		/// </summary>
		public static bool PopWH(Control ctl)
		{
			return popXYWH(ctl, false, false, true, true);
		}
		
		/// <summary>
		/// XYWH�̍��W�l�����W�X�g������ǂݍ��݂܂��B�Ȃ��ꍇ�͖������܂��B
		/// </summary>
		public static bool PopXYWH(Control ctl)
		{
			return popXYWH(ctl, true, true, true, true);
		}
		
		/// <summary>
		/// ���W�X�g���ɒl��ۑ����܂��B
		/// </summary>
		public static void Push(Control ctl, object val)
		{
			Push(ctl, ctl.Name, val);
		}
		
		/// <summary>
		/// ���W�X�g���ɒl��ۑ����܂��B
		/// </summary>
		public static void Push(Control ctl, string key, object val)
		{
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("���������ł��BRegCommon.SetMasterKey()�ŏ��������Ă��������B");
				return;
			}
			
			// ���W�X�g���L�[�����擾
			string		reg_key = getRegKey(ctl);
			
			RegCommon.Write(reg_key + key, val);
		}
		
		/// <summary>
		/// ���W�X�g���ɕۑ������l���擾���܂��B
		/// </summary>
		public static object Pop(Control ctl)
		{
			return Pop(ctl, ctl.Name);
		}
		
		/// <summary>
		/// ���W�X�g���ɕۑ������l���擾���܂��B
		/// </summary>
		public static object Pop(Control ctl, string key)
		{
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("���������ł��BRegCommon.SetMasterKey()�ŏ��������Ă��������B");
				return null;
			}
			
			// ���W�X�g���L�[�����擾
			string		reg_key = getRegKey(ctl);
			
			return RegCommon.Read(reg_key + key);
		}
		#endregion
		
		#region *** Private Method ***
		/// <summary>
		/// ���W�X�g���L�[�����擾���܂��B
		/// �ȉ��̂悤�ȃ��[���ŃL�[���쐬���܂��B
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
		/// XYWH�̍��W�l�����W�X�g���ɓo�^���܂��B
		/// </summary>
		/// <param name="ctl">�R���g���[����</param>
		/// <param name="bx">true�ŕۑ�, false�ŕۑ����Ȃ�</param>
		/// <param name="by">true�ŕۑ�, false�ŕۑ����Ȃ�</param>
		/// <param name="bw">true�ŕۑ�, false�ŕۑ����Ȃ�</param>
		/// <param name="bh">true�ŕۑ�, false�ŕۑ����Ȃ�</param>
		private static void pushXYWH(Control ctl, bool bx, bool by, bool bw, bool bh)
		{
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("���������ł��BRegCommon.SetMasterKey()�ŏ��������Ă��������B");
				return;
			}
			
			// ���W�X�g���L�[�����擾
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
		/// XYWH�̍��W�l�����W�X�g������ǂݍ��݂܂��B�Ȃ��ꍇ�͖������܂��B
		/// </summary>
		/// <param name="ctl">�R���g���[����</param>
		/// <param name="bx">true�œǍ�, false�œǍ����Ȃ�</param>
		/// <param name="by">true�œǍ�, false�œǍ����Ȃ�</param>
		/// <param name="bw">true�œǍ�, false�œǍ����Ȃ�</param>
		/// <param name="bh">true�œǍ�, false�œǍ����Ȃ�</param>
		private static bool popXYWH(Control ctl, bool bx, bool by, bool bw, bool bh)
		{
			bool	success = true;
			
			if (RegCommon.RegTree == null)
			{
				ErrLog.WriteLine("���������ł��BRegCommon.SetMasterKey()�ŏ��������Ă��������B");
				return false;
			}
			
			// ���W�X�g���L�[�����擾
			string		reg_key = getRegKey(ctl);
			
			// ���W�X�g������E�B���h�E��XYWH�������Ă���
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
			
			// �f�[�^�擾�Ɉ�x�ł����s���Ă�����ݒ肵�Ȃ�
			if (success == true)
			{
				// ���j�^�̗̈悪�����Ȃ��Ă��܂��Ă����ꍇ�A��ʂɎ��܂�ʒu�ɂ���
				if (x > Screen.GetWorkingArea(ctl).Width - w)
				{
					x = Screen.GetWorkingArea(ctl).Width - w;
				}
				if (y > Screen.GetWorkingArea(ctl).Height - h)
				{
					y = Screen.GetWorkingArea(ctl).Height - h;
				}
				
				// ���j�^���t�]������ƃ}�C�i�X�ɂȂ�ꍇ������̂Ŗh��
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
