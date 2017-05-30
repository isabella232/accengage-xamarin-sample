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
    [Register ("InboxDetailsViewController")]
    partial class InboxDetailsViewController
    {
        [Outlet]
        UIKit.UIView [] DetailCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Category { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Details { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DetailView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint HeaderHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView Loader { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MessageIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ReceiptDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RichContentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Sender { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Subject { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView TextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIToolbar Toolbar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ToolbarBottom { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TransitionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView WebView { get; set; }

        [Action ("TransitionButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TransitionButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Category != null) {
                Category.Dispose ();
                Category = null;
            }

            if (Details != null) {
                Details.Dispose ();
                Details = null;
            }

            if (DetailView != null) {
                DetailView.Dispose ();
                DetailView = null;
            }

            if (HeaderHeight != null) {
                HeaderHeight.Dispose ();
                HeaderHeight = null;
            }

            if (Loader != null) {
                Loader.Dispose ();
                Loader = null;
            }

            if (MessageIcon != null) {
                MessageIcon.Dispose ();
                MessageIcon = null;
            }

            if (ReceiptDate != null) {
                ReceiptDate.Dispose ();
                ReceiptDate = null;
            }

            if (RichContentView != null) {
                RichContentView.Dispose ();
                RichContentView = null;
            }

            if (Sender != null) {
                Sender.Dispose ();
                Sender = null;
            }

            if (Subject != null) {
                Subject.Dispose ();
                Subject = null;
            }

            if (TextView != null) {
                TextView.Dispose ();
                TextView = null;
            }

            if (Toolbar != null) {
                Toolbar.Dispose ();
                Toolbar = null;
            }

            if (ToolbarBottom != null) {
                ToolbarBottom.Dispose ();
                ToolbarBottom = null;
            }

            if (TransitionButton != null) {
                TransitionButton.Dispose ();
                TransitionButton = null;
            }

            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }
        }
    }
}