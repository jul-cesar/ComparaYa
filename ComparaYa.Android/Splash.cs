using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComparaYa.Droid
{
    [Activity(Label = "ComparaYa", Icon = "@mipmap/ic_launcher", Theme = "@style/newone", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            // Create your application here
        }
    }
}