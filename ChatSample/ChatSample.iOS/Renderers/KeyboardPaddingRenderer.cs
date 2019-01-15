using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ChatSample.Controls;
using ChatSample.iOS.Renderers;
using Foundation;
using UIKit;
using CoreGraphics;

[assembly: ExportRenderer(typeof(KeyboardPadding), typeof(KeyboardPaddingRenderer))]
namespace ChatSample.iOS.Renderers
{
    public class KeyboardPaddingRenderer : ViewRenderer
    {
        private NSObject _keyboardShowObserver;
        private NSObject _keyboardHideObserver;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }
        }

        private void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
            {
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            }

            if (_keyboardHideObserver == null)
            {
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
            }
        }

        private void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                var height = ((NSValue)args.Notification.UserInfo[UIKeyboard.FrameEndUserInfoKey]).CGRectValue.Size.Height;

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    height -= Window.SafeAreaInsets.Bottom;
                }

                Element.HeightRequest = height;
            }
        }

        private void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                Element.HeightRequest = 0;
            }
        }

        private void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }
    }
}
