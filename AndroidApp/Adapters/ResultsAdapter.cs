using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.DroidX;
using AndroidX.RecyclerView.Widget;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidApp.Adapters
{
    public class ResultsAdapter : ListAdapter<TestResult>
    {
        public ResultsAdapter() : base(new DiffCallback())
        {
        }

        public override int GetItemViewType(int position) => 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var model = GetItem(position);
            var vh = (ResultViewHolder)holder;
            vh.Bind(model);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) => new ResultViewHolder(parent, Resource.Layout.test_result_item);

        private class DiffCallback : ItemCallback<TestResult>
        {
            public override bool AreContentsTheSame(TestResult oldItem, TestResult newItem) => oldItem.Method == newItem.Method;

            public override bool AreItemsTheSame(TestResult oldItem, TestResult newItem) => oldItem == newItem;
        }

        private class ResultViewHolder : RecyclerView.ViewHolder
        {
            private TextView Method => ItemView.FindViewById<TextView>(Resource.Id.result_method);
            private TextView Bytes => ItemView.FindViewById<TextView>(Resource.Id.result_bytes);
            private TextView KB => ItemView.FindViewById<TextView>(Resource.Id.result_kb);
            private TextView SizeDiff => ItemView.FindViewById<TextView>(Resource.Id.result_diff);
            private TextView Time => ItemView.FindViewById<TextView>(Resource.Id.result_time);

            public ResultViewHolder(ViewGroup parent, int layout)
                : base(Inflate(parent, layout))
            {
            }

            public void Bind(TestResult model)
            {
                Method.Text = model.Method;
                Bytes.Text = model.Size.Bytes.ToString();
                KB.Text = model.Size.KiloBytes.ToString("F0");
                SizeDiff.Text = GainToString(model.GainPerc);
                Time.Text = model.ExecutionTimeInMs.ToString();
            }

            string GainToString(double gainPerc) => gainPerc switch
            {
                0 => "-",
                _ => gainPerc.ToString("P")
            };

            private static View Inflate(ViewGroup parent, int layout) => LayoutInflater.From(parent.Context)?.Inflate(layout, parent, false);
        }
    }
}