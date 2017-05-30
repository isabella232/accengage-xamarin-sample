using AccengageSDK;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Square.Picasso;

namespace Android_Sample
{
    [Activity(Label = "InboxDetailActivity", LaunchMode = LaunchMode.SingleTask)]
    public class InboxDetailActivity : BaseActivity
    {
        private ImageView _iconImageView;
        private TextView _titleTextView;
        private TextView _dateTextView;
        private TextView _senderTextView;
        private TextView _textTextView;

        private WebView _contentWebView;
        private TextView _contenTextView;

        private LinearLayout _buttonsLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InboxDetail);

            _iconImageView = FindViewById<ImageView>(Resource.Id.msg_icon);
            _titleTextView = FindViewById<TextView>(Resource.Id.msg_title);
            _dateTextView = FindViewById<TextView>(Resource.Id.msg_date);
            _senderTextView = FindViewById<TextView>(Resource.Id.msg_sender);
            _textTextView = FindViewById<TextView>(Resource.Id.msg_text);

            _contentWebView = FindViewById<WebView>(Resource.Id.msg_webview);
            _contenTextView = FindViewById<TextView>(Resource.Id.msg_body);

            _buttonsLayout = FindViewById<LinearLayout>(Resource.Id.buttonsLayout);

            LoadMessage();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.InboxDetail, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.read:
                    InboxManager.CurrentMessage.Read = true;
                    return true;
                case Resource.Id.unread:
                    InboxManager.CurrentMessage.Read = false;
                    return true;
                case Resource.Id.archive:
                    InboxManager.CurrentMessage.Archived = true;
                    return true;
                case Resource.Id.unarchive:
                    InboxManager.CurrentMessage.Archived = false;
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void LoadMessage()
        {
            var message = InboxManager.CurrentMessage;
            var messageContent = InboxManager.CurrentMessageContent;

            _titleTextView.Text = message.Title;
            if (!TextUtils.IsEmpty(message.IconUrl))
                Picasso.With(this).Load(message.IconUrl).Into(_iconImageView);
            else
                _iconImageView.SetBackgroundColor(Color.DarkGray);

            _dateTextView.Text = message.Date.ToShortDateString() + ", " + message.Date.ToShortTimeString();
            _senderTextView.Text = message.From;
            _textTextView.Text = message.Text;

            if (messageContent.Type == AccengageInboxMessageContentType.Text)
            {
                _contenTextView.Visibility = ViewStates.Visible;
                _contenTextView.Text = messageContent.Body;
                _contentWebView.Visibility = ViewStates.Gone;
            }
            else if (messageContent.Type == AccengageInboxMessageContentType.Web)
            {
                _contentWebView.Visibility = ViewStates.Visible;
                _contentWebView.SetWebViewClient(new WebViewClient());
                var webSettings = _contentWebView.Settings;
                webSettings.JavaScriptEnabled = true;
                _contentWebView.LoadUrl(messageContent.Body);
                _contenTextView.Visibility = ViewStates.Gone;
            }

            var buttons = messageContent.Buttons;
            if (buttons.Count > 0)
            {
                _buttonsLayout.Visibility = ViewStates.Visible;
                foreach (var button in buttons)
                {
                    var btn = new Button(this);
                    btn.Text = button.Title;
                    btn.SetTextColor(Color.White);
                    btn.SetBackgroundColor(Color.ParseColor("#007AFF"));
                    btn.SetPadding(10, 2, 10, 2);
                    btn.Click += delegate {
                        button.Interact();
                    };

                    var lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                        ViewGroup.LayoutParams.WrapContent, 0.5f)
                    {
                        LeftMargin = 20,
                        RightMargin = 20
                    };
                    _buttonsLayout.AddView(btn, lp);
                }
            }
        }
    }
}