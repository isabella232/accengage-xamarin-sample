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
    [Register ("InboxViewController")]
    partial class InboxViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem ArchiveButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView InboxTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem MarkAsReadButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIToolbar Toolbar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ToolbarBottom { get; set; }

        [Action ("ArchiveMessageButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ArchiveMessageButton_Activated (UIKit.UIBarButtonItem sender);

        [Action ("MarkAsReadButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void MarkAsReadButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (ArchiveButton != null) {
                ArchiveButton.Dispose ();
                ArchiveButton = null;
            }

            if (InboxTableView != null) {
                InboxTableView.Dispose ();
                InboxTableView = null;
            }

            if (MarkAsReadButton != null) {
                MarkAsReadButton.Dispose ();
                MarkAsReadButton = null;
            }

            if (Toolbar != null) {
                Toolbar.Dispose ();
                Toolbar = null;
            }

            if (ToolbarBottom != null) {
                ToolbarBottom.Dispose ();
                ToolbarBottom = null;
            }
        }
    }
}