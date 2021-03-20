using AndroidX.RecyclerView.Widget;
using AndroidApp.Extensions;
using Java.Lang;

namespace AndroidApp.DroidX
{
    public abstract class ItemCallback<T> : DiffUtil.ItemCallback
    {
        public abstract bool AreContentsTheSame(T oldItem, T newItem);

        public abstract bool AreItemsTheSame(T oldItem, T newItem);

        public override bool AreContentsTheSame(Object oldItem, Object newItem) => AreContentsTheSame(oldItem.Cast<T>(), newItem.Cast<T>());

        public override bool AreItemsTheSame(Object oldItem, Object newItem) => AreItemsTheSame(oldItem.Cast<T>(), newItem.Cast<T>());
    }
}
