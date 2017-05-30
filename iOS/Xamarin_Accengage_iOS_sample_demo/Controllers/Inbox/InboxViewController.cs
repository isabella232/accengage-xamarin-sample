using Foundation;
using System;
using UIKit;
using CoreGraphics;
using AccengageSDK;

namespace Xamarin_Accengage_iOS_sample_demo
{
	public partial class InboxViewController : UIViewController
	{
		AccengageInbox inbox;
		UIRefreshControl refreshControl;

		public InboxViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIImage logoImage = UIImage.FromBundle("header");
			NavigationItem.TitleView = new UIImageView(logoImage);

			NSNotificationCenter notficationCenter = NSNotificationCenter.DefaultCenter;

			// Register for inbox notication
			notficationCenter.AddObserver(AccengageIOS.Constants.BMA4SInBoxDataChanged, inboxChanged);

			// Register for cell notification
			notficationCenter.AddObserver(new NSString("RefreshInbox"), inboxRefresh);

			// Set Title
			// Ex : Inbox (3)
			if (inbox != null)
			{
				Title = "Inbox " + inbox.UnreadMessageCount.ToString();
			}

			// Initialize the refresh controll
			refreshControl = new UIRefreshControl();
			refreshControl.BackgroundColor = UIColor.FromWhiteAlpha(0, 0.1f);
			refreshControl.AddTarget((sender, e) => { reloadData(); }, UIControlEvent.ValueChanged);
			InboxTableView.AddSubview(refreshControl);

			InvokeOnMainThread(delegate
			{
				refreshControl.BeginRefreshing();
				refreshControl.EndRefreshing();
			});

			// Add Edit Buttonn
			UIBarButtonItem rigthButton = new UIBarButtonItem("edit", UIBarButtonItemStyle.Plain, startEditing);
			rigthButton.TintColor = UIColor.White;

			if (NavigationItem != null)
			{
				NavigationItem.RightBarButtonItem = rigthButton;
			}

			InboxTableView.Delegate = new InboxTableViewDelegate(this);
			reloadData();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// Desable edition
			activeEditionState(false);
			View.AddSubview(Toolbar);
		}

		void startEditing(object sender, EventArgs e)
		{
			activeEditionState(!InboxTableView.Editing);
		}

		partial void MarkAsReadButton_Activated(UIBarButtonItem sender)
		{
            if (InboxTableView.IndexPathsForSelectedRows != null)
            {
                foreach (NSIndexPath indexPath in InboxTableView.IndexPathsForSelectedRows)
                {

                    inbox.ObtainMessageAtIndex(indexPath.Row, (message, requestedIndex) =>
                {
                    if (message.Read)
                        message.Read = false;
                    else
                        message.Read = true;
                });
                }
            }
		}

		partial void ArchiveMessageButton_Activated(UIBarButtonItem sender)
		{
			if (InboxTableView.IndexPathsForSelectedRows != null)
			{
                foreach (NSIndexPath indexPath in InboxTableView.IndexPathsForSelectedRows)
                {

                    inbox.ObtainMessageAtIndex(indexPath.Row, (message, requestedIndex) =>
                {
                    if (message.Archived)
                        message.Archived = false;
                    else
                        message.Archived = true;
                });
                }
			}
		}

		void reloadData()
		{
			Accengage.GetInbox((arg1) =>
			{
				inbox = arg1;
				updateUIandData();
			});
		}

		void updateUIandData()
		{
			// update table content
			InboxTableView.DataSource = new InboxTableViewSource(inbox);
			InboxTableView.ReloadData();

			//title updating
			if (inbox != null)
			{
				Title = "Inbox " + inbox.UnreadMessageCount.ToString();
				NavigationController.TabBarItem.BadgeValue = inbox.UnreadMessageCount.ToString();
			}

			// End the refreshing
			if (refreshControl != null)
			{
				string title = "Last update: " + DateTime.Now.ToString("MMM d, hh:mm");
				NSAttributedString attributedTitle = new NSAttributedString(title, null, UIColor.DarkGray);
				refreshControl.AttributedTitle = attributedTitle;
				refreshControl.EndRefreshing();
			}
		}

		void updateToolBarPosition()
		{
			CGRect frame = Toolbar.Frame;
			ToolbarBottom.Constant = -1;

			if (!InboxTableView.Editing)
				ToolbarBottom.Constant -= frame.Size.Height;

			UIView.Animate(0.3, () =>
			{
				View.LayoutIfNeeded();
			});
		}

		void activeEditionState(bool actived)
		{
			InboxTableView.SetEditing(actived, true);

			UIImage image = UIImage.FromBundle("edit");

			if (InboxTableView.Editing)
				image = UIImage.FromBundle("save");

			NavigationItem.RightBarButtonItem.Image = image;

			updateToolBarPosition();
			updateUIandData();
		}

		void inboxChanged(NSNotification obj)
		{
			updateUIandData();
		}

		void inboxRefresh(NSNotification obj)
		{
			InboxTableView.ReloadData();
		}

		class InboxTableViewSource : UITableViewDataSource
		{
			AccengageInbox inbox;

			public InboxTableViewSource(AccengageInbox arg1)
			{
				inbox = arg1;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				InboxCell cell = (InboxCell)tableView.DequeueReusableCell("inboxCell", indexPath);

				cell.index = indexPath.Row;
				cell.setLoading();
				inbox.ObtainMessageAtIndex(indexPath.Row, (message, requestedIndex) =>
				{
					if (cell.index == (int)requestedIndex)
						cell.setMessage(message);
				});

				cell.Accessory = UITableViewCellAccessory.None;
				return cell;
			}

			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return (nint)((inbox != null) ? inbox.Size : 0);
			}
		}

		class InboxTableViewDelegate : UITableViewDelegate
		{
			InboxViewController InboxViewController;

			public InboxTableViewDelegate(InboxViewController value)
			{
				InboxViewController = value;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				if (!tableView.Editing)
				{
					tableView.DeselectRow(indexPath, false);
					InboxViewController.inbox.ObtainMessageAtIndex(indexPath.Row, (message, requestedIndex) =>
					{
						message.InteractWithDisplayHandler((content) =>
						{
							InboxDetailsViewController controller = (InboxDetailsViewController)InboxViewController.Storyboard.InstantiateViewController("InboxDetailsViewController");
							if (controller != null)
							{
								controller.message = message;
								controller.content = content;
								InboxViewController.NavigationController.PushViewController(controller, true);
							}
						});
					});
				}

			}
		}
	}
}