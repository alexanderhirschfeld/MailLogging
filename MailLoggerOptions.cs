using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace com.alexanderhirschfeld.Logging.MailLogging
{
    public class MailLoggerOptions
    {
        public SMTPConfiguration SMTP { get; set; }
        public string From { get; set; }
        public List<string> To { get; set; }
        public bool Debug { get; set; } = false;

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
    }
}


