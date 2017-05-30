using System;
using AccengageIOS;
using Foundation;

namespace Xamarin_Accengage_iOS_sample_demo
{
	public static class SampleHelpers
	{
		static NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

		public static void setGeofenceServiceEnabled(bool arg)
		{
			BMA4SLocationServices.SetGeofenceServiceEnabled(arg);
			userDefaults.SetBool(arg, new NSString("com.accengage.sample.geofence.key"));
			userDefaults.Synchronize();
		}

		public static bool isGeofenceServiceEnabled()
		{
			return (userDefaults.ValueForKey(new NSString("com.accengage.sample.geofence.key")) != null) ? userDefaults.BoolForKey("com.accengage.sample.geofence.key") : true;
		}

		public static void setBeaconServiceEnabled(bool arg)
		{
			BMA4SLocationServices.SetBeaconServiceEnabled(arg);
			userDefaults.SetBool(arg, new NSString("com.accengage.sample.beacons.key"));
			userDefaults.Synchronize();
		}

		public static bool isBeaconServiceEnabled()
		{
			return (userDefaults.ValueForKey(new NSString("com.accengage.sample.beacons.key")) != null) ? userDefaults.BoolForKey("com.accengage.sample.beacons.key") : true;
		}
	}
}
