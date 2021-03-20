using AndroidX.RecyclerView.Widget;
using AndroidApp.Extensions;

namespace AndroidApp.DroidX
{
    public abstract class ListAdapter<T> : ListAdapter
    {
        protected ListAdapter(DiffUtil.ItemCallback diffCallback)
            : base(diffCallback)
        {
        }

        public new T GetItem(int position) => base.GetItem(position).Cast<T>();
    }
}
