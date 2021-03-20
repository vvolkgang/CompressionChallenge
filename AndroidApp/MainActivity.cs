using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using ByteSizeLib;
using CodeCrafters.TableViews;
using CodeCrafters.TableViews.Toolkit;
using Core;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;

namespace AndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        private RecyclerView _list;
        private ResultsAdapter _adapter = new ResultsAdapter();
        private ResultsTableAdapter _tableAdapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
            SetupTable(new List<TestResult>());

            var contacts = 1000;
            var repeatTests = 1;
            var now = DateTime.Now;
            var scheduler = new TestScheduler();
            var result = await scheduler.ExecuteTestsAsync(contacts, repeatTests);

            _tableAdapter.AddAll(result);
            textMessage.Text = "Done!";
        }

        private void SetupTable(List<TestResult> results)
        {
            var resultTableView = FindViewById<SortableTableView>(Resource.Id.tableView);

            var simpleTableHeaderAdapter = new SimpleTableHeaderAdapter(this, "Method", "Bytes", "KB", "Size Diff", "Time Avg (ms)");
            simpleTableHeaderAdapter.SetTextColor(ContextCompat.GetColor(this, Resource.Color.table_header_text));
            resultTableView.HeaderAdapter = simpleTableHeaderAdapter;

            var rowColorEven = ContextCompat.GetColor(this, Resource.Color.table_data_row_even);
            var rowColorOdd = ContextCompat.GetColor(this, Resource.Color.table_data_row_odd);
            resultTableView.SetDataRowBackgroundProvider(TableDataRowBackgroundProviders.AlternatingRowColors(rowColorEven, rowColorOdd));
            resultTableView.HeaderSortStateViewProvider = SortStateViewProviders.BrightArrows();

            resultTableView.SetColumnWeight(0, 2);
            resultTableView.SetColumnWeight(1, 3);
            resultTableView.SetColumnWeight(2, 3);
            resultTableView.SetColumnWeight(3, 2);
            resultTableView.SetColumnWeight(4, 2);

            resultTableView.SetColumnComparator(0, ResultViewModel.GetBytesComparator());
            resultTableView.SetColumnComparator(1, ResultViewModel.GetTimeComparator());
            _tableAdapter = new ResultsTableAdapter(this, ResultViewModel.From(results));
            resultTableView.DataClick += (sender, e) =>
            {
                var result = (ResultViewModel)e.ClickedData;
                var resultString = $"Tapped {result.Method}";
                Toast.MakeText(this, resultString, ToastLength.Short).Show();
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    //textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    //textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    //textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }
    }
}

