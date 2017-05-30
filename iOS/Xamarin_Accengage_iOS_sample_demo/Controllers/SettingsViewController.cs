using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using SCLAlertViewLib;
using AccengageSDK;
using CoreGraphics;

namespace Xamarin_Accengage_iOS_sample_demo
{
    public partial class SettingsViewController : UITableViewController
    {
		List<Dictionary<string, object>> settings = new List<Dictionary<string, object>>();

		string reuseIdentifierSample = "SimpleCell";
		string reuseIdentifierSwitch = "SwitchCell";
		string reuseIdentifierFooter = "FooterCell";

		string cellTypeKey = "cellTypeKey";
		string cellTitleKey = "cellTitleKey";
		string cellDetailsKey = "cellDetailsKey";


		enum SettingsCellType : int
		{
			Sample,
			Switch
		}

        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var userName = new Dictionary<string, object>();
			userName.Add(cellTypeKey, SettingsCellType.Sample);
			userName.Add(cellTitleKey, "Edit the user display name");

			var inappLock = new Dictionary<string, object>();
			inappLock.Add(cellTypeKey, SettingsCellType.Switch);
			inappLock.Add(cellTitleKey, "Lock InApp messages display");
			inappLock.Add(cellDetailsKey, "On: if you want to disable inApp Notifications Off: to enable inApp Notifications");

			var beacons = new Dictionary<string, object>();
			beacons.Add(cellTypeKey, SettingsCellType.Switch);
			beacons.Add(cellTitleKey, "Enable beacons service");
			beacons.Add(cellDetailsKey, "On: if you want to enable beacons service Off: to disable it");

			var geofences = new Dictionary<string, object>();
			geofences.Add(cellTypeKey, SettingsCellType.Switch);
			geofences.Add(cellTitleKey, "Enable geofencing service");
			geofences.Add(cellDetailsKey, "On: if you want to enable geofencing service Off: to disable it");

			settings.Add(userName);
			settings.Add(inappLock);
			settings.Add(beacons);
			settings.Add(geofences);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			Accengage.TrackScreenDisplay("SETTINGS");
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			Accengage.TrackScreenDismiss("SETTINGS");
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			updateSelectionsAnimated(false);
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return settings.Count;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = null;

			var item = settings[indexPath.Section];

			SettingsCellType type = (SettingsCellType) item[cellTypeKey];

			if (type == SettingsCellType.Switch)
			{
				cell = TableView.DequeueReusableCell(reuseIdentifierSwitch);
				((SettingsCell) cell).setTextLabel((string) item[cellTitleKey]);
			}
			else
			{
				cell = TableView.DequeueReusableCell(reuseIdentifierSample);
				cell.TextLabel.Text = (string) item[cellTitleKey];
			}

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			switch (indexPath.Section)
			{
				case 0:
					setUserName();
					TableView.DeselectRow(indexPath, true);
					break;
				default:
					setStatusForIndex(true, indexPath.Section);
					break;
			}
		}

		public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
		{
			var item = settings[indexPath.Section];
			SettingsCellType type = (SettingsCellType) item[cellTypeKey];

			if (type == SettingsCellType.Switch)
			{
				setStatusForIndex(false, indexPath.Section);
			}
		}

		public override UIView GetViewForFooter(UITableView tableView, nint section)
		{
			string index = section.ToString();
			UITableViewCell view = TableView.DequeueReusableCell(reuseIdentifierFooter);

			var item = settings[int.Parse(index)];

			if (!item.ContainsKey(cellDetailsKey))
				return null;

			string detail = (string) item[cellDetailsKey];

			view.TextLabel.Text = detail;
			view.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
			view.TextLabel.Lines = 0;
			return view;
		}

		public override nfloat GetHeightForFooter(UITableView tableView, nint section)
		{
			string index = section.ToString();
			if (settings[int.Parse(index)].ContainsKey(cellDetailsKey))
			    return heigthForText((string) settings[int.Parse(index)][cellDetailsKey]);
			
			return base.GetHeightForFooter(tableView, section);
		}

		void setUserName()
		{
			SCLAlertView alert = editionAlertView();

			UITextField textField = alert.AddTextField("User name");

			alert.AddButton("Send", () =>
			{
				string text = textField.Text;
				NSUserDefaults.StandardUserDefaults.SetValueForKey(new NSString(text), new NSString("user.name"));
				var list = new List<KeyValuePair<string, string>>();
				list.Add(new KeyValuePair<string, string>("user_name", text));
				Accengage.UpdateDeviceInfo(list);
			});

			var name = NSUserDefaults.StandardUserDefaults.ValueForKey(new NSString("user.name"));
			string subtitle = (name != null) ? "The current name is: " + name : "";
			alert.ShowEdit(this.ParentViewController, "User's name", subtitle, "cancel", 0);
		}

		void setStatusForIndex(bool status, int index)
		{
			Console.WriteLine(status);
			switch (index)
			{
				case 1:
					Accengage.SetInAppDisplayDisabled(status);
					break;
				case 2:
					SampleHelpers.setBeaconServiceEnabled(status);
					break;
				case 3:
					SampleHelpers.setGeofenceServiceEnabled(status);
					break;
			}
		}

		SCLAlertView editionAlertView()
		{
			SCLAlertView alert = new SCLAlertView();

			alert.ShouldDismissOnTapOutside = true;
			alert.CustomViewColor = UIColor.FromRGB(44, 147, 255);
			alert.HideAnimationType = SCLAlertViewHideAnimation.FadeOut;
			alert.ShowAnimationType = SCLAlertViewShowAnimation.FadeIn;

			return alert;
		}

		bool statusForIndex(int index)
		{
			switch (index)
			{
				case 1:
					return Accengage.IsInAppDisplayDisabled();
				case 2:
					return SampleHelpers.isBeaconServiceEnabled();
				case 3:
					return SampleHelpers.isGeofenceServiceEnabled();
			}
			return false;
		}

		void updateSelectionsAnimated(bool animated)
		{
			for (int i = 2; i < settings.Count; i++)
			{
				var indexPath = NSIndexPath.FromRowSection(0, i);
				if (statusForIndex(i))
					TableView.SelectRow(indexPath, animated, UITableViewScrollPosition.None);
				else
					TableView.DeselectRow(indexPath, animated);
			}
		}

		nfloat heigthForText(String text)
		{
			CGSize maximumLabelSize = new CGSize(TableView.Bounds.Size.Width, float.MaxValue);
			CGSize expectedLabelSize = text.StringSize(UIFont.SystemFontOfSize(14), maximumLabelSize, UILineBreakMode.WordWrap);
			return expectedLabelSize.Height + 10;
		}

		partial void CloseButton_Activated(UIBarButtonItem sender)
		{
			PresentingViewController.DismissViewController(true, null);
		}
	}
}