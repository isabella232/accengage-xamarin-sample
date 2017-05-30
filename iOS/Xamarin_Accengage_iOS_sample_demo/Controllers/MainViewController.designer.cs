// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Xamarin_Accengage_iOS_sample_demo
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem LikeButton { get; set; }

        [Action ("LikeButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LikeButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (LikeButton != null) {
                LikeButton.Dispose ();
                LikeButton = null;
            }
        }
    }
}