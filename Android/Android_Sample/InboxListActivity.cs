using System.Collections.Generic;
using AccengageSDK;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Android.Views;
using Java.Lang;

namespace Android_Sample
{
    [Activity(Label = "InboxListActivity", MainLauncher = true, Icon = "@mipmap/ic_launcher", LaunchMode = LaunchMode.SingleTask)]
    public class InboxListActivity : BaseActivity
    {
        private ListView _inboxListView;
        private InboxListAdapter _inboxAdapter;

        private readonly List<AccenageInboxMessage> _messages = new List<AccenageInboxMessage>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InboxList);

            _inboxListView = FindViewById<ListView>(Resource.Id.list);
            _inboxAdapter = new InboxListAdapter(this, _messages);
            _inboxListView.Adapter = _inboxAdapter;
            SetActivityTitle(0);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Accengage.GetInbox(LoadMessages);
        }

        private void LoadMessages(AccengageInbox inbox)
        {
            InboxManager.Inbox = inbox;
            _messages.Clear();

            SetActivityTitle(inbox.UnreadMessageCount);
            for (var i = 0; i < inbox.Size; i++)
                inbox.ObtainMessageAtIndex(i, LoadMessage);
        }

        private void LoadMessage(AccenageInboxMessage message, int position)
        {
            _messages.Add(message);
            _inboxAdapter.NotifyDataSetChanged();

        }

        private void SetActivityTitle(int unreadMessages)
        {
            ActionBar.Title = "Inbox Messages (" + unreadMessages + ")";
        }

        private class InboxListAdapter : BaseAdapter
        {
            private readonly Context _context;
            private readonly List<AccenageInboxMessage> _messages;

            public InboxListAdapter(Context context, List<AccenageInboxMessage> messages)
            {
                _context = context;
                _messages = messages;
            }

            public override Object GetItem(int position)
            {
                return _messages[position];
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var inboxCellView = convertView as InboxCellView;
                if (inboxCellView == null)
                    inboxCellView = new InboxCellView(_context);

                inboxCellView.Bind(GetItem(position) as AccenageInboxMessage);
                return inboxCellView;
            }

            public override int Count => _messages.Count;
        }
    }
}

