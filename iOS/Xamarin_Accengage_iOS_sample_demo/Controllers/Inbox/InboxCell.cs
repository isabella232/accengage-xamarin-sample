using Foundation;
using System;
using UIKit;
using AccengageSDK;

namespace Xamarin_Accengage_iOS_sample_demo
{
	public partial class InboxCell : UITableViewCell
	{
		public int index;

		public InboxCell(IntPtr handle) : base (handle)
        {
		}

		public void setMessage(AccengageInboxMessage msg)
		{
			Subject.Text = msg.Title;
			Content.Text = msg.Text;
			Date.Text = InboxTools.labelTextForDate(msg.Date);

			string categorie = msg.Category;
			Category.Text = (categorie != null) ? categorie : "";
			Category.BackgroundColor = InboxTools.colorForCategory(Category.Text);

			if (msg.Read)
			{
				Subject.TextColor = UIColor.FromWhiteAlpha(0.4f, 1.0f);
				Content.TextColor = UIColor.FromWhiteAlpha(0.4f, 1.0f);
				MessageStatus.BackgroundColor = UIColor.White;
			}
			else
			{
				Subject.TextColor = UIColor.Black;
				Content.TextColor = UIColor.Black;
				MessageStatus.BackgroundColor = UIColor.FromRGB(0, 121, 255);
			}

			if (msg.Archived)
				MessageStatus.BackgroundColor = UIColor.Red;

			string iconUrl = msg.IconUrl;

			if (iconUrl.Length > 0)
			{
				MessageIcon.Hidden = false;
				var request = NSUrlRequest.FromUrl(new NSUrl(iconUrl));
				NSUrlConnection.SendAsynchronousRequest(request, NSOperationQueue.MainQueue,
														(response, data, error) =>
				{
					if (error == null)
						MessageIcon.Image = UIImage.LoadFromData(data);
				});
			}
			else
				MessageIcon.Hidden = true;
		}

		public void setLoading()
		{
			Subject.Text = "";
			Content.Text = "";
		}
	}
}