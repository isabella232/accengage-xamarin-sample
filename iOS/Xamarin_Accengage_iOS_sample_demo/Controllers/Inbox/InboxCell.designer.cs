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
    [Register ("InboxCell")]
    partial class InboxCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Category { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Content { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Date { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MessageIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MessageStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Subject { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Category != null) {
                Category.Dispose ();
                Category = null;
            }

            if (Content != null) {
                Content.Dispose ();
                Content = null;
            }

            if (Date != null) {
                Date.Dispose ();
                Date = null;
            }

            if (MessageIcon != null) {
                MessageIcon.Dispose ();
                MessageIcon = null;
            }

            if (MessageStatus != null) {
                MessageStatus.Dispose ();
                MessageStatus = null;
            }

            if (Subject != null) {
                Subject.Dispose ();
                Subject = null;
            }
        }
    }
}