using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccengageSDK;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Android_Sample
{
    public class BaseActivity : Activity
    {
        protected override void OnResume()
        {
            base.OnResume();
            Accengage.StartActivity(this);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Accengage.SetIntent(intent);
        }

        protected override void OnPause()
        {
            base.OnPause();
            Accengage.StopActivity(this);
        }
    }
}