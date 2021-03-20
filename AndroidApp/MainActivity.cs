using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using ByteSizeLib;
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

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            _list = FindViewById<RecyclerView>(Resource.Id.test_results_list);
            _list.SetAdapter(_adapter);
            textMessage.Text = "Executing tests...";

            var l = new List<TestResult>()
            {
                new TestResult("Test", new ByteSize(1000), 12, 40),
                new TestResult("Test2", new ByteSize(2000), 12, 40),
                new TestResult("Badjoras3", new ByteSize(3000), 12, 40),
            };
            _adapter.SubmitList(l);


            var contacts = 1000;
            var repeatTests = 1;
            var now = DateTime.Now;
            var scheduler = new TestScheduler();
            var result = await scheduler.ExecuteTestsAsync(contacts, repeatTests);

            _adapter.SubmitList(result);
            

            textMessage.Text = "Done!";

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

