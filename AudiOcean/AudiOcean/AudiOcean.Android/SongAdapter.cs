using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace AudiOcean.Droid
{
    class SongAdapter : RecyclerView.Adapter
    {
        public event EventHandler<SongAdapterClickEventArgs> ItemClick;
        public event EventHandler<SongAdapterClickEventArgs> ItemLongClick;
        string[] items;

        public SongAdapter(string[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            //itemView = LayoutInflater.From(parent.Context).
            //       Inflate(id, parent, false);

            var vh = new SongAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as SongAdapterViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Length;

        void OnClick(SongAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(SongAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class SongAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public SongAdapterViewHolder(View itemView, Action<SongAdapterClickEventArgs> clickListener,
                            Action<SongAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new SongAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new SongAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class SongAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}