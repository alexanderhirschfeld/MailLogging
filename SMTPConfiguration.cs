namespace com.alexanderhirschfeld.Logging.MailLogging
{
    public class SMTPConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; } = 587;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; } = true;
        public int Timeout { get; set; } = 10000;
    }
}
