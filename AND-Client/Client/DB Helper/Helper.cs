using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Client.Common;
using System.Collections.Generic;

namespace Client.DB_Helper
{
    public class Helper : SQLiteOpenHelper
    {
        private static string _DatabaseName = "clientDatabase";

        public Helper(Context context) : base(context, _DatabaseName, null, 1) { }
        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(Helper.CreateQuery);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL(Helper.DeleteQuery);
            OnCreate(db);
        }
        
        private const string TableName = "adminTable";
        private const string ColumnID = "id";
        private const string ColumnUsername = "username";
        private const string ColumnFullName = "fullname";
        private const string ColumnPassword = "password";
        private const string ColumnEmail = "email";
        private const string ColumnMobile = "mobile";

        public const string CreateQuery = "CREATE TABLE " + TableName +
            " ( "
            + ColumnID + " INTEGER PRIMARY KEY,"
            + ColumnUsername + " TEXT,"
            + ColumnFullName + " TEXT,"
            + ColumnPassword + " TEXT,"
            + ColumnEmail + " TEXT,"
            + ColumnMobile + " TEXT)";

        public const string DeleteQuery = "DROP TABLE IF EXISTS " + TableName;

        public void Register(Context context, Admin admin)
        {
            SQLiteDatabase db = new Helper(context).WritableDatabase;
            ContentValues Values = new ContentValues();
            Values.Put(ColumnUsername, admin.Username);
            Values.Put(ColumnFullName, admin.FullName);
            Values.Put(ColumnPassword, admin.Password);
            Values.Put(ColumnEmail, admin.Email);
            Values.Put(ColumnMobile, admin.Mobile);
            db.Insert(TableName, null, Values);
            db.Close();
        }
        public Admin Authenticate(Context context, Admin admin)
        {
            SQLiteDatabase db = new Helper(context).ReadableDatabase;
            ICursor cursor = db.Query(TableName, new string[] 
            { ColumnID, ColumnFullName, ColumnUsername, ColumnPassword, ColumnEmail, ColumnMobile },
            ColumnUsername + "=?", new string[] { admin.Username }, null, null, null);
            if(cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
            {
                Admin admin1 = new Admin(cursor.GetString(3));
                if (admin.Password.Equals(admin1.Password))
                return admin1;
            }
            return null;
        }

        public List<Admin> GetAdmin(Context context)
        {
            List<Admin> admins = new List<Admin>();
            SQLiteDatabase db = new Helper(context).ReadableDatabase;
            string[] columns = new string[] {ColumnID,ColumnUsername,ColumnFullName,ColumnPassword,ColumnEmail,ColumnMobile };
            using(ICursor cursor = db.Query(TableName, columns, null, null, null, null, null))
            {
                while (cursor.MoveToNext())
                {
                    admins.Add(new Admin {
                        ID = cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnID)),
                        Username = cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnUsername)),
                        FullName = cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnFullName)),
                        Password = cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnPassword)),
                        Email = cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnEmail)),
                        Mobile = cursor.GetString(cursor.GetColumnIndexOrThrow(ColumnMobile))

                    });
                }
                db.Close();
                return admins;
            }
        }
    }
}