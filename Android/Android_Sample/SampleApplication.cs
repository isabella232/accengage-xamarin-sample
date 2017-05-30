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
    [Application]
    public class SampleApplication : Application
    {
        public SampleApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            // Nothing
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Accengage.SetContext(this);
        }
    }
}