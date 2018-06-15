using Android.App;
using Android.OS;
using Android.Widget;
using Client.Common;
using Client.DB_Helper;

namespace Client.Activities
{
    [Activity]
    public class SignUp : Activity
    {
        private EditText edtFullname, edtUsername, edtEmail, edtPassword, edtMobile;
        private Button btnCreate, btnBack;
        Helper helper;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);
            
            edtFullname = FindViewById<EditText>(Resource.Id.edtfullname);
            edtUsername = FindViewById<EditText>(Resource.Id.edtusername);
            edtPassword = FindViewById<EditText>(Resource.Id.edtpassword);
            edtEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            edtMobile = FindViewById<EditText>(Resource.Id.edtMobile);
            btnCreate = FindViewById<Button>(Resource.Id.btnCreate);
            btnBack = FindViewById<Button>(Resource.Id.btnBack);
            helper = new Helper(this);

            btnBack.Click += delegate { StartActivity(typeof(MainActivity)); };

            btnCreate.Click += delegate 
            {
                Admin admin = new Admin()
                {
                    FullName = edtFullname.Text,
                    Username = edtUsername.Text,
                    Password = edtPassword.Text,
                    Email = edtEmail.Text,
                    Mobile = edtMobile.Text
                };
                string username = edtUsername.Text;
                string password = edtPassword.Text;
                if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Toast.MakeText(this, "Username and Password should not be empty.", ToastLength.Short).Show();
                }
                else
                {
                    helper.Register(this,admin);
                    var data = helper.GetAdmin(this);
                    admin = data[data.Count - 1];
                    Toast.MakeText(this, $"User {admin.FullName} registration successful!", ToastLength.Short).Show();
                    Clear();
                    Toast.MakeText(this, $"Total {data.Count} Admin founds.", ToastLength.Short).Show();
                }
            };
           
        }
        void Clear()
        {
            edtFullname.Text = "";
            edtUsername.Text = "";
            edtPassword.Text = "";
            edtMobile.Text = "";
            edtEmail.Text = "";
        }
    }
}