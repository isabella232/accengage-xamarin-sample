using Foundation;
using System;
using UIKit;
using AccengageSDK;
using System.Collections.Generic;
using CoreGraphics;

namespace Xamarin_Accengage_iOS_sample_demo
{
	public partial class InboxDetailsViewController : UIViewController
	{
		public AccengageInboxMessage message;
		public AccengageInboxMessageContent content;

		public InboxDetailsViewController(IntPtr handle) : base(handle) {}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIImage logoImage = UIImage.FromBundle("header");
			NavigationItem.TitleView = new UIImageView(logoImage);

			UIImage backImage = UIImage.FromBundle("arrows-left");
			UIBarButtonItem myBackButton = new UIBarButtonItem(backImage, UIBarButtonItemStyle.Plain, (sender, e) =>
			{
				NavigationController.PopViewController(true);
			});
			myBackButton.TintColor = UIColor.White;
			NavigationItem.LeftBarButtonItem = myBackButton;

			// Setup tool bar with custom buttons
			setupToolBar();

            Subject.Text = message.Title;
			Sender.Text = "Expéditeur : " + message.From;

			// Category
			Category.Text = (message.Category != null) ? message.Category : "";
            Category.BackgroundColor = InboxTools.colorForCategory(message.Category);

			ReceiptDate.Text = "Reçu : " + InboxTools.labelTextForDate(message.Date);

			Details.Text = message.Text;

			switch (content.Type)
			{
				case AccengageInboxMessageContentType.Text:
					WebView.Hidden = true;
					TextView.Text = content.Body;
					break;
				case AccengageInboxMessageContentType.Web:
					TextView.Hidden = true;
					Loader.StartAnimating();
					WebView.Alpha = 0;
					WebView.ScrollView.Bounces = false;
					WebView.Delegate = new WebviewDelegate(WebView, Loader);
                    WebView.LoadRequest(new NSUrlRequest(new NSUrl(content.Body)));
					break;
			}

			if (message.IconUrl.Length > 0)
			{
				NSMutableUrlRequest request = new NSMutableUrlRequest(new NSUrl(message.IconUrl));
                NSUrlConnection.SendAsynchronousRequest(request, NSOperationQueue.MainQueue, (response, data, error) =>
                {
                    if (error == null)
                        MessageIcon.Image = UIImage.LoadFromData(data);
                });
			}
			else
			    MessageIcon.Hidden = true;

			NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidChangeStatusBarOrientationNotification, deviceOrientationDidChangeNotification);

			updateForDeviceOrientation();
		}

		void setupToolBar()
		{
			UIBarButtonItem space = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace, null, null);

			UIBarButtonItem markAsRead = createBarButton((message.Read) ? "Mark as unread" : "Mark as read", UIBarButtonItemStyle.Plain, markAsUnreadAction);

			UIBarButtonItem archive = createBarButton((message.Archived) ? "Restore" : "Delete", UIBarButtonItemStyle.Plain, archiveAction);

			List<UIBarButtonItem> buttons = new List<UIBarButtonItem> { space, markAsRead, space, archive, space };

			int i = 0;
			foreach (AccengageInboxButton button in content.Buttons)
			{
				UIBarButtonItem aButton = createBarButton(button.Title, UIBarButtonItemStyle.Plain, interact);
				aButton.Tag = i;
				buttons.Add(aButton);
				buttons.Add(space);
				i++;
			}

			Toolbar.SetItems(buttons.ToArray(), false);
		}

		void updateDetailsViewFrame()
		{
			if (!TransitionButton.Selected)
				HeaderHeight.Constant = 155;
			else
				HeaderHeight.Constant = TransitionButton.Frame.Size.Height;
		}

		void updateToolbarFrame()
		{
			CGRect toolBarFrame = Toolbar.Frame;

			ToolbarBottom.Constant = -1;

			if (!TransitionButton.Selected)
				ToolbarBottom.Constant -= toolBarFrame.Size.Height;
		}

		void updateDetailsAlpha()
		{
			foreach (UIView view in DetailCollection)
				view.Alpha = Convert.ToInt32(!TransitionButton.Selected);
		}

		void updateSubjectFrame()
		{
			CGRect subjectFrame = Subject.Frame;

			if (!TransitionButton.Selected)
			{
				subjectFrame.X = Sender.Frame.X;
				subjectFrame.Y = MessageIcon.Frame.Y;
				subjectFrame.Height = Sender.Frame.Height;
			}
			else
			{
				subjectFrame.X = MessageIcon.Frame.X;
				subjectFrame.Y = TransitionButton.Frame.Y;
				subjectFrame.Height = TransitionButton.Frame.Height;
			}

			Subject.Frame = subjectFrame;
		}

		void updateForDeviceOrientation()
		{
			UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;

			if (orientation == UIInterfaceOrientation.Portrait)
				showDetails(true);
			else if (orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight)
				showDetails(false);
		}

		// Tools

		UIBarButtonItem createBarButton(string title, UIBarButtonItemStyle style, EventHandler action)
		{
			return new UIBarButtonItem(title, style, action);
		}

		// Webview delegate

		class WebviewDelegate : UIWebViewDelegate
		{
			UIWebView webview;
			UIActivityIndicatorView loader;

			public WebviewDelegate(UIWebView arg1, UIActivityIndicatorView arg2)
			{
				webview = arg1;
				loader = arg2;
			}

			public override void LoadingFinished(UIWebView webView)
			{
				stopLoadingAnimation();
			}

			public override void LoadFailed(UIWebView webView, NSError error)
			{
				stopLoadingAnimation();
			}

			void stopLoadingAnimation()
			{
				if (loader.IsAnimating)
					webview.Alpha = 1;
				loader.StopAnimating();
			}
		}

		// Actions

		void markAsUnreadAction(object sender, EventArgs e)
		{
			if (message.Read)
				message.Read = false;
			else
				message.Read = true;

			setupToolBar();
		}

		void archiveAction(object sender, EventArgs e)
		{
			if (message.Archived)
				message.Archived = false;
			else
				message.Archived = true;

			setupToolBar();
		}

		void interact(object sender, EventArgs e)
		{
			UIBarButtonItem button = sender as UIBarButtonItem;
			var tag = (int)button.Tag;
			content.Buttons[tag].Interact();
		}

		partial void TransitionButton_TouchUpInside(UIButton sender)
		{
			showDetails(TransitionButton.Selected);
		}

		void showDetails(bool show)
		{
			if (show != TransitionButton.Selected)
				return;

			TransitionButton.Selected = !show;

            updateDetailsViewFrame();
			updateToolbarFrame();

			UIView.Animate(0.2, () =>
			{
				View.LayoutIfNeeded();
				updateDetailsAlpha();
				updateSubjectFrame();
			});
		}

		// Orientation change notification

		void deviceOrientationDidChangeNotification(NSNotification notification)
		{
			UIInterfaceOrientation newOrientation = UIApplication.SharedApplication.StatusBarOrientation;
			UIInterfaceOrientation oldOrientation = (UIInterfaceOrientation)Int32.Parse(notification.UserInfo[UIApplication.StatusBarOrientationUserInfoKey].ToString());

			if ((newOrientation == UIInterfaceOrientation.Portrait) != (oldOrientation == UIInterfaceOrientation.LandscapeLeft || oldOrientation == UIInterfaceOrientation.LandscapeRight))
				updateForDeviceOrientation();
		}

	}
}