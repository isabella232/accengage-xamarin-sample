using Foundation;
using System;
using UIKit;
using SCLAlertViewLib;
using AccengageSDK;
using System.Collections.Generic;

namespace Xamarin_Accengage_iOS_sample_demo
{
    public partial class ProductsViewContoller : UITableViewController
    {
		string reuseIdentifier = "Cell";

        public ProductsViewContoller (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			UIImage logoImage = UIImage.FromBundle("header");
			NavigationItem.TitleView = new UIImageView(logoImage);

			UIImage backImage = UIImage.FromBundle("arrows-left");
			UIBarButtonItem myBackButton = new UIBarButtonItem(backImage, UIBarButtonItemStyle.Plain, (sender, e) => {
				NavigationController.PopViewController(true);
			});
			myBackButton.TintColor = UIColor.White;
			NavigationItem.LeftBarButtonItem = myBackButton;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			Accengage.TrackScreenDisplay("Product");
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			Accengage.TrackScreenDismiss("Product");
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return 100;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(reuseIdentifier);

			cell.TextLabel.Text = "Product";
			cell.DetailTextLabel.Text = indexPath.Row.ToString();

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			SCLAlertView alert = new SCLAlertView();
			UITextField textField = alert.AddTextField("Enter the quantity");
			textField.KeyboardType = UIKeyboardType.NumberPad;

			UITextField textField2 = alert.AddTextField("Enter the price");
			textField2.KeyboardType = UIKeyboardType.NumberPad;

			alert.ShouldDismissOnTapOutside = true;
			alert.CustomViewColor = UIColor.FromRGB(44, 147, 255);
			alert.HideAnimationType = SCLAlertViewHideAnimation.FadeOut;
			alert.ShowAnimationType = SCLAlertViewShowAnimation.FadeIn;

			alert.AddButton("Send", () => {
				if (textField.Text != "" && textField2.Text != "")
				{
					Console.WriteLine("Text Value: " + textField.Text);

					Int32 quantity = Int32.Parse(textField.Text);
					Double price = Double.Parse(textField2.Text);

					string product = "Product " + indexPath.Row.ToString();
					AccengageItem accengageItem = new AccengageItem(product, "Brand", "Product", price, "EUR", quantity);

					List<AccengageItem> listItem = new List<AccengageItem>();
					listItem.Add(accengageItem);

					Accengage.TrackPurchase("dePurchase", "EUR", quantity * price, listItem);
				}
			});

			alert.ShowEdit(this.ParentViewController, "How much ?", "", "Cancel", 0);

			TableView.DeselectRow(indexPath, true);
		}
    }
}