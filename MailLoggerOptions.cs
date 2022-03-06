using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;

namespace com.alexanderhirschfeld.Logging.MailLogging
{
    public class MailLoggerOptions
    {
        public SMTPConfiguration SMTP { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; }
        public bool Debug { get; set; } = false;

        public MailAddress GetFrom() => new MailAddress(From);
        public List<MailAddress> GetRecipients()
        {
            return To.Select(mailTo => new MailAddress(mailTo)).ToList();
        }

        public SmtpClient GetClient()
        {
            return new SmtpClient(SMTP.Host)
            {
                Port = SMTP.Port,
                Credentials = new NetworkCredential(SMTP.Username, SMTP.Password),
                EnableSsl = SMTP.UseSSL,
                Timeout = SMTP.Timeout,
            };
        }

        public void Validate()
        {
            GetFrom();
            GetRecipients();
            GetClient();
            //SMTPTest();
        }

        private void SMTPTest()
        {
            try
            {
                using (var client = new TcpClient())
                {
                    client.Connect(this.SMTP.Host, this.SMTP.Port);
                    using (var stream = client.GetStream())
                    using (var sslStream = new SslStream(stream))
                    {
                        sslStream.AuthenticateAsClient(this.SMTP.Host);
                        using (var writer = new StreamWriter(stream))
                        using (var reader = new StreamReader(stream))
                        {
                            writer.WriteLine("EHLO " + this.SMTP.Host);
                            writer.Flush();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}


