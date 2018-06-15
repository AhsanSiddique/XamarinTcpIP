using Android.App;
using Android.Database.Sqlite;
using Android.OS;
using Android.Widget;
using Client.Activities;
using Client.Common;
using Client.DB_Helper;

namespace Client
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity
    {
        private EditText txtUsername, txtPassword;
        private Button btnSignIn, btnCreate;
        Helper helper;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            txtUsername = FindViewById<EditText>(Resource.Id.txtusername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtpassword);
            btnCreate = FindViewById<Button>(Resource.Id.btnSignUp);
            btnSignIn = FindViewById<Button>(Resource.Id.btnSign);
            helper = new Helper(this);

            btnCreate.Click += delegate { StartActivity(typeof(SignUp)); };

            btnSignIn.Click += delegate
            {
                try
                {
                    string Username = txtUsername.Text.ToString();
                    string Password = txtPassword.Text.ToString();
                    var user = helper.Authenticate(this,new Admin(null,Username,null,null,Password,null));
                    if (user != null)
                    {
                        Toast.MakeText(this, "Login Successful", ToastLength.Short).Show();
                        StartActivity(typeof(Connect));
                    }
                    else
                    {
                        Toast.MakeText(this, "Login Unsuccessful! Please verify your Username and Password", ToastLength.Short).Show();
                    }
                }
                catch (SQLiteException ex)
                {
                    Toast.MakeText(this, ""+ex, ToastLength.Short).Show();
                }
                
            };
        }
    }
}

