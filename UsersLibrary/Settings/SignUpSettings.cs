namespace UsersLibrary.Settings
{
    public class SignUpSettings : ISettings
    {
        public string Email { get; private set; }
        public string AuthPassword { get; private set; }

        public SignUpSettings() { }

        public SignUpSettings(string email, string password)
        {
            Email = email;
            AuthPassword = password;
        }
    }
}
