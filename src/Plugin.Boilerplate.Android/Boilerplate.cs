using System;
using Acr.UserDialogs;
using Android.App;
using FFImageLoading.Forms.Droid;

namespace Plugin.Boilerplate
{
    public static class Boilerplate
    {
        public static void Init(Activity activity, bool enableFastRenderer = true)
        {
            CachedImageRenderer.Init(enableFastRenderer);
            UserDialogs.Init(activity);
        }

        public static void Init(Application application, bool enableFastRenderer = true)
        {
            CachedImageRenderer.Init(enableFastRenderer);
            UserDialogs.Init(application);
        }

        public static void Init(Func<Activity> topActivityFactory, bool enableFastRenderer = true)
        {
            CachedImageRenderer.Init(enableFastRenderer);
            UserDialogs.Init(topActivityFactory);
        }
    }
}
