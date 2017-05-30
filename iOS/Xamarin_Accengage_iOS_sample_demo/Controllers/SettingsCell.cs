using Foundation;
using System;
using UIKit;

namespace Xamarin_Accengage_iOS_sample_demo
{
    public partial class SettingsCell : UITableViewCell
	{
		public SettingsCell(IntPtr handle) : base(handle)
		{
		}

		public SettingsCell() : base()
		{
		}

		public void setTextLabel(string text)
		{
			TitleLabel.Text = text;
		}

		public void setStatus(bool status)
		{
			Switch.Enabled = status;
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}

		public override void SetSelected(bool selected, bool animated)
		{
			base.SetSelected(selected, animated);
			Switch.On = selected;
		}

		public static explicit operator SettingsCell(string v)
		{
			throw new NotImplementedException();
		}
	}
}