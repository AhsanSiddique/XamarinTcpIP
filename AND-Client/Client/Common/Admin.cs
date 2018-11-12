namespace Client.Common
{
    public class Admin
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public Admin() { }
        public Admin(string Id, string username, string fullName, string email, string password, string mobile) //Constructor with all parameters
        {
            ID = Id;
            Username = username;
            FullName = fullName;
            Email = email;
            Password = password;
            Mobile = mobile;
        }
        public Admin(string Password) //Constructor with one parameter
        {
            this.Password = Password;
        }
       
    }
}