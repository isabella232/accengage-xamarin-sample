using Foundation;
using UIKit;
using AccengageSDK;
using CoreLocation;
using System;

namespace Xamarin_Accengage_iOS_sample_demo
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{

		// class-level declarations

		public override UIWindow Window
		{
			get;
			set;
		}

		CLLocationManager locationManager;
		AccPushDelegate AccPushSource;

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			// Enable logs
			Accengage.SetLoggingEnabled(true);

			Accengage.Start();

			// Register for notification
			AccengageIOS.ACCNotificationOptions options = (AccengageIOS.ACCNotificationOptions.Alert | AccengageIOS.ACCNotificationOptions.Badge | AccengageIOS.ACCNotificationOptions.Sound);
			AccengageIOS.Accengage.Push.RegisterForUserNotificationsWithOptions(options);

			// Enable Geofence service if needed
			AccengageIOS.BMA4SLocationServices.SetGeofenceServiceEnabled(SampleHelpers.isGeofenceServiceEnabled());

			// Enable Beacon service if needed
			AccengageIOS.BMA4SLocationServices.SetBeaconServiceEnabled(SampleHelpers.isBeaconServiceEnabled());

			//init géoloc
			locationManager = new CLLocationManager();
			locationManager.Delegate = new LocationManagerDelegate();
			locationManager.DistanceFilter = CLLocationDistance.FilterNone;
			locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			locationManager.RequestAlwaysAuthorization();
			locationManager.StartUpdatingLocation();

			AccPushSource = new AccPushDelegate(this);
			AccengageIOS.Accengage.Push.Delegate = AccPushSource;

			return true;
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			locationManager.StopUpdatingLocation();
		}

		public override void WillEnterForeground(UIApplication application)
		{
			locationManager.StartUpdatingLocation();
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			locationManager.StopUpdatingLocation();
		}

		///--------------------------------------
		/// Handle incoming URLS
		///--------------------------------------

		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			handleUrl(url);
			return true;
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			handleUrl(url);
			return true;
		}

		///--------------------------------------
		/// Accengage push delegate
		///--------------------------------------

		public void DidOpenNotificationWithId(string notifId, NSDictionary @params)
		{
			handlePush(@params);
		}
		
		public class AccPushDelegate : AccengageIOS.ACCPushDelegate
		{
			AppDelegate _obj = null;

			public AccPushDelegate(AppDelegate obj)
			{
				_obj = obj;
			}

			public override void DidOpenNotificationWithId(string notifId, NSDictionary @params)
			{
				 _obj.DidOpenNotificationWithId(notifId, @params);
			}
		}

		///--------------------------------------
		/// Location manager delegate
		///--------------------------------------

		public class LocationManagerDelegate : CLLocationManagerDelegate
		{
			public override void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
			{
				// do not update with old location
				CLLocation newLocation = locations[locations.Length - 1];
				var locationAge = (DateTime) NSDate.Now - (DateTime) newLocation.Timestamp;

				if (locationAge.TotalSeconds > 5)
					return;

				// test that the horizontal accuracy does not indicate an invalid measurement
				if (newLocation.HorizontalAccuracy < 0)
					return;

				Accengage.UpdateGeolocation(newLocation);
			}
		}

        // Private

		void handlePush(NSDictionary infos)
		{
			if (infos == null)
				return;
			if (infos[new NSString("p")] != null)
				goToScreen(infos[new NSString("p")].ToString());
		}

		void handleUrl(NSUrl url)
		{
			if (url == null)
				return;
			
			if (url.Host == "p")
			{
				var array = url.PathComponents;

				if (array.Length == 2)
					goToScreen(array[1]);
			}
		}

		void goToScreen(string name)
		{
			UITabBarController tabBarController = (UITabBarController) Window.RootViewController;

			if (name == "2")
			{
				tabBarController.SelectedIndex = 1;
			}
			else
			{
				tabBarController.SelectedIndex = 0;

				UINavigationController nav = (UINavigationController) tabBarController.ViewControllers[0];
				MainViewController controller = (MainViewController) nav.ViewControllers[0];

				if (name == "1")
					controller.PerformSegue("goToSettings", controller);
				else if (name == "3")
					controller.PerformSegue("goToProductsList", controller);
			}
		}
	}
}

