using AccengageSDK;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace Android_Sample
{
    public class InboxCellView : LinearLayout
    {
        private TextView _titleTextView;
        private TextView _dateTextView;
        private View _coloredIndicatorView;
        private ImageView _imageView;

        private AccenageInboxMessage _message;

        public InboxCellView(Context context) :
            base(context)
        {
            Initialize();
        }

        private void Initialize()
        {
            LayoutInflater.From(Context).Inflate(Resource.Layout.InboxCell, this);

            _titleTextView = FindViewById<TextView>(Resource.Id.title);
            _dateTextView = FindViewById<TextView>(Resource.Id.date);
            _coloredIndicatorView = FindViewById<View>(Resource.Id.colored_indicator);
            _imageView = FindViewById<ImageView>(Resource.Id.img);
        }

        public void Bind(AccenageInboxMessage message)
        {
            _message = message;

            _titleTextView.Text = _message.Title;
            _dateTextView.Text = _message.Date.ToShortDateString() + ", " + _message.Date.ToShortTimeString();
            if (_message.Archived)
                _coloredIndicatorView.SetBackgroundColor(Color.Red);
            else
                _coloredIndicatorView.SetBackgroundColor(_message.Read ? Color.Transparent : Color.SkyBlue);

            if (!TextUtils.IsEmpty(_message.IconUrl))
            {
                _imageView.SetBackgroundColor(Color.Transparent);
                Picasso.With(Context).Load(_message.IconUrl).Into(_imageView);
            }
            else
            {
                _imageView.SetImageDrawable(null);
                _imageView.SetBackgroundColor(Color.DarkGray);
            }

            SetOnClickListener(new OnClick(Context, _message));
        }

        private class OnClick : Java.Lang.Object, IOnClickListener
        {
            private readonly Context _context;
            private readonly AccenageInboxMessage _message;

            public OnClick(Context context, AccenageInboxMessage message)
            {
                _context = context;
                _message = message;
            }

            void IOnClickListener.OnClick(View v)
            {
                _message?.InteractWithDisplayHandler(DisplayInboxDetailActivity);
            }

            private void DisplayInboxDetailActivity(AccengageInboxMessageContent content)
            {
                InboxManager.CurrentMessage = _message;
                InboxManager.CurrentMessageContent = content;

                _context.StartActivity(typeof(InboxDetailActivity));
            }
        }
    }
}