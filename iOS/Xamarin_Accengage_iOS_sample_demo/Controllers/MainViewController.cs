using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using AccengageSDK;
using SCLAlertViewLib;

namespace Xamarin_Accengage_iOS_sample_demo
{
    public partial class MainViewController : UICollectionViewController 
    {
		List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
		string reuseIdentifier = "Cell";

		public MainViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ClearsSelectionOnViewWillAppear = true;

			UIImage logoImage = UIImage.FromBundle("header");
			NavigationItem.TitleView = new UIImageView(logoImage);

			list.Add(new KeyValuePair<string, string>("mostViewed.jpg", "Most viewed"));
			list.Add(new KeyValuePair<string, string>("media.jpg", "Multimedia"));
			list.Add(new KeyValuePair<string, string>("technology.jpg", "Technology"));
			list.Add(new KeyValuePair<string, string>("art.jpg", "Art & Design"));
			list.Add(new KeyValuePair<string, string>("business.jpg", "Business"));

			CollectionView.Delegate = new CollectionViewDelegate();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			Accengage.TrackScreenDisplay("DEMO");
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);

			Accengage.TrackScreenDismiss("DEMO");
		}

		partial void LikeButton_Activated(UIBarButtonItem sender)
		{
			Accengage.TrackLead("Facebook", "like");

			SCLAlertView alert = new SCLAlertView();
			alert.ShowSuccess(this.ParentViewController, "Thank you!", "Thanks for liking us on Facebook!", "Close", 3.0f);
		}

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return list.Count;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			UICollectionViewCell cell = (UICollectionViewCell) CollectionView.DequeueReusableCell(reuseIdentifier, indexPath);

			UIImageView image = (UIImageView) cell.ViewWithTag(1);
			image.Image = UIImage.FromBundle(list[indexPath.Row].Key);

			UILabel label = (UILabel) cell.ViewWithTag(2);
			label.Text = list[indexPath.Row].Value;

			cell.Layer.MasksToBounds = false;
			cell.Layer.ShadowOffset = new CoreGraphics.CGSize(0, 1);
			cell.Layer.ShadowRadius = 1;
			cell.Layer.ShadowColor = UIColor.Black.CGColor;
			cell.Layer.ShadowOpacity = 0.2f;

			return cell;
		}
    }

	public class CollectionViewDelegate: UICollectionViewDelegateFlowLayout
	{
		public override CoreGraphics.CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			double width = (collectionView.Bounds.Size.Width - 30) / 2;
			return new CoreGraphics.CGSize(width, width * 1.2);
		}
	}
}