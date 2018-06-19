using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using XamarinForms.Client.Interface;

namespace XamarinForms.Client.iOS
{
    public class Message : ToastMessage
    {
        UIAlertController alert;
        public void Alert(string message)
        {
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
    }
}
