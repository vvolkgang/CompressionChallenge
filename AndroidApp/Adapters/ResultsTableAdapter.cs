using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ByteSizeLib;
using CodeCrafters.TableViews;
using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidApp.Adapters
{
    public class ResultViewModel : Java.Lang.Object
    {
        private ByteSize _size;
        private float _gainPerc;
        private float _executionTimeInMs;

        public ResultViewModel(TestResult result)
        {
            Method = result.Method;
            _size = result.Size;
            _gainPerc = result.GainPerc;
            _executionTimeInMs = result.ExecutionTimeInMs;
        }

        public string Method { get; private set; }

        public string Bytes => _size.Bytes.ToString();

        public string KBytes => _size.KiloBytes.ToString("F0");

        public string SizeDiff => GainToString(_gainPerc);

        public string TimeAvg => _executionTimeInMs.ToString();

        string GainToString(double gainPerc) => gainPerc switch
        {
            0 => "-",
            _ => gainPerc.ToString("P")
        };

        public static List<ResultViewModel> From(List<TestResult> result)
        {
            return result.Select(x => new ResultViewModel(x)).ToList();
        }

        public static Java.Util.IComparator GetBytesComparator() => new BytesComparator();

        public static Java.Util.IComparator GetSizeDiffComparator() => new SizeDiffComparator();
        
        public static Java.Util.IComparator GetTimeComparator() => new TimeComparator();

        private class BytesComparator : Java.Lang.Object, Java.Util.IComparator
        {
            public int Compare(Java.Lang.Object lhs, Java.Lang.Object rhs)
            {
                var r1 = (ResultViewModel)lhs;
                var r2 = (ResultViewModel)rhs;
                return r1._size.CompareTo(r2._size);
            }
        }

        private class SizeDiffComparator : Java.Lang.Object, Java.Util.IComparator
        {
            public int Compare(Java.Lang.Object lhs, Java.Lang.Object rhs)
            {
                var r1 = (ResultViewModel)lhs;
                var r2 = (ResultViewModel)rhs;
                return r1._gainPerc.CompareTo(r2._gainPerc);
            }
        }

        private class TimeComparator : Java.Lang.Object, Java.Util.IComparator
        {
            public int Compare(Java.Lang.Object lhs, Java.Lang.Object rhs)
            {
                var r1 = (ResultViewModel)lhs;
                var r2 = (ResultViewModel)rhs;
                return r1._executionTimeInMs.CompareTo(r2._executionTimeInMs);
            }
        }
    }

    public class ResultsTableAdapter : TableDataAdapter
    {
        private const int TextSize = 14;

        public ResultsTableAdapter(Context context, IList data)
            : base(context, data)
        {
        }

        public override View GetCellView(int rowIndex, int columnIndex, ViewGroup parentView)
        {
            var model = (ResultViewModel)GetRowData(rowIndex);
            View renderedView = null;

            switch (columnIndex)
            {
                case 0:
                    renderedView = RenderText(model.Method);
                    break;
                case 1:
                    renderedView = RenderText(model.Bytes);
                    break;
                case 2:
                    renderedView = RenderText(model.KBytes);
                    break;
                case 3:
                    renderedView = RenderText(model.SizeDiff);
                    break;
                case 4:
                    renderedView = RenderText(model.TimeAvg);
                    break;
            }

            return renderedView;
        }

        private View RenderText(string text)
        {
            var textView = new TextView(Context);
            textView.Text = text;
            textView.SetPadding(20, 10, 20, 10);
            textView.TextSize = TextSize;

            return textView;
        }

        //private View RenderPrice(Car car)
        //{
        //    var textView = new TextView(Context);
        //    textView.Text = (car.Price / 1000).ToString("R #,##0K");
        //    textView.SetPadding(20, 10, 20, 10);
        //    textView.TextSize = TextSize;

        //    if (car.Price < 500000)
        //    {
        //        textView.SetTextColor(Color.DarkGreen);
        //    }
        //    else if (car.Price > 1000000)
        //    {
        //        textView.SetTextColor(Color.DarkRed);
        //    }

        //    return textView;
        //}

        //private View RenderPower(Car car, ViewGroup parentView)
        //{
        //    var view = LayoutInflater.Inflate(Resource.Layout.TableCellPower, parentView, false);
        //    var kwView = view.FindViewById<TextView>(Resource.Id.kw_view);
        //    var psView = view.FindViewById<TextView>(Resource.Id.ps_view);

        //    kwView.Text = car.Kw + " kW";
        //    psView.Text = car.Ps + " PS";

        //    return view;
        //}

        //private View RenderName(Car car)
        //{
        //    var textView = new TextView(Context);
        //    textView.Text = car.Name;
        //    textView.SetPadding(20, 10, 20, 10);
        //    textView.TextSize = TextSize;
        //    return textView;
        //}

        //private View RenderProducerLogo(Car car, ViewGroup parentView)
        //{
        //    var view = LayoutInflater.Inflate(Resource.Layout.TableCellImage, parentView, false);
        //    var imageView = view.FindViewById<ImageView>(Resource.Id.imageView);
        //    imageView.SetImageResource(car.Producer.Logo);
        //    return view;
        //}
    }
}