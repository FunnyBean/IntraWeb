namespace IntraWeb.Services.Email
{
    public class UnknownKeyException
        : FormatEmailException
    {

        public UnknownKeyException(string emailType, string key)
            : base("Neznámy kľúč v šablóne.")
        {
            EmailType = emailType;
            Key = key;
        }


        public string EmailType { get; }
        public string Key { get; }
    }
}
