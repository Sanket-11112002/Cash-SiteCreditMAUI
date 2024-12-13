using Android.App;
using Android.Content.PM;
using Android.OS;

namespace CardGameCorner
{
    [Activity(Theme = "@style/Maui.SplashTheme",
          MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize,
          ScreenOrientation = ScreenOrientation.Portrait)] // Force portrait mode
public class MainActivity : MauiAppCompatActivity
{
}
}
