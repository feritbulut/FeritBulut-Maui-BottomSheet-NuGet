using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeritBulut.Maui.BottomSheet.Helpers
{
    /// <summary>
    /// Manages keyboard events.
    /// </summary>
    public static class KeyboardHelper
    {
        /// <summary>
        /// It is triggered when the keyboard appears.
        /// </summary>
        public static event EventHandler<double>? KeyboardShown;

        /// <summary>
        /// It is triggered when the keyboard is hidden.
        /// </summary>
        public static event EventHandler? KeyboardHidden;

        private static bool _isInitialized;
        private static double _keyboardHeight;

        /// <summary>
        /// The keyboard initializes the helper.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

#if ANDROID
        InitializeAndroid();
#elif IOS
            InitializeiOS();
#endif

            _isInitialized = true;
        }

#if ANDROID
    private static void InitializeAndroid()
    {
        // Android için soft input mode değişikliklerini dinle
        if (Platform.CurrentActivity?.Window?.DecorView != null)
        {
            var decorView = Platform.CurrentActivity.Window.DecorView;
            decorView.ViewTreeObserver?.AddOnGlobalLayoutListener(new AndroidKeyboardListener());
        }
    }

    private class AndroidKeyboardListener : Java.Lang.Object, Android.Views.ViewTreeObserver.IOnGlobalLayoutListener
    {
        private int _previousHeight;

        public void OnGlobalLayout()
        {
            try
            {
                var activity = Platform.CurrentActivity;
                if (activity?.Window?.DecorView == null) return;

                var rect = new Android.Graphics.Rect();
                activity.Window.DecorView.GetWindowVisibleDisplayFrame(rect);

                var screenHeight = activity.Window.DecorView.RootView?.Height ?? 0;
                var keypadHeight = screenHeight - rect.Bottom;

                if (keypadHeight > screenHeight * 0.15)
                {
                    if (_previousHeight == 0)
                    {
                        _keyboardHeight = keypadHeight / Android.App.Application.Context.Resources?.DisplayMetrics?.Density ?? 1;
                        KeyboardShown?.Invoke(null, _keyboardHeight);
                    }
                    _previousHeight = keypadHeight;
                }
                else
                {
                    if (_previousHeight != 0)
                    {
                        KeyboardHidden?.Invoke(null, System.EventArgs.Empty);
                    }
                    _previousHeight = 0;
                }
            }
            catch
            {
                // Sessizce devam et
            }
        }
    }
#endif

#if IOS
        private static void InitializeiOS()
        {
            UIKit.UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
            {
                var keyboardFrame = args.FrameEnd;
                _keyboardHeight = keyboardFrame.Height;
                KeyboardShown?.Invoke(null, _keyboardHeight);
            });

            UIKit.UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
            {
                KeyboardHidden?.Invoke(null, System.EventArgs.Empty);
            });
        }
#endif

        /// <summary>
        /// Returns the current keyboard height.
        /// </summary>
        public static double GetKeyboardHeight() => _keyboardHeight;
    }
}
