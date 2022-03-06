using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Text;

namespace com.alexanderhirschfeld.Logging.MailLogging
{
    public sealed class MailLogger : ILogger
    {
        private readonly MailLoggerProvider _mailLoggerProvider;

        public MailLogger(MailLoggerProvider mailLoggerProvider)
        {
            _mailLoggerProvider = mailLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            MailLoggerOptions options = _mailLoggerProvider.GetOptions();

            StringBuilder builder = new StringBuilder();
            builder.Append($"EventId: {eventId}\r\n");
            builder.Append($"LogLevel: {logLevel}\r\n");
            builder.Append($"Application: {AppDomain.CurrentDomain.FriendlyName}\r\n");
            builder.Append($"Context: {_mailLoggerProvider.CategoryName}\r\n");
            builder.Append($"Time: {DateTimeOffset.Now}\r\n");
            builder.Append($"Message: {formatter(state, exception)}\r\n");
            if (exception is not null)
            {
                builder.Append($"Exception: {exception.Message}\r\n");
                builder.Append($"Trace: {exception.StackTrace}\r\n");
            }

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(options.From),
                Subject = $"[{logLevel.ToString().ToUpper()}] [{AppDomain.CurrentDomain.FriendlyName}] {_mailLoggerProvider.CategoryName}",
                Body = builder.ToString(),
            };
            options.GetRecipients().ForEach(recipient => mailMessage.To.Add(recipient));

            if (options.Debug)
            {
                Console.WriteLine("[DEBUG]");
                Console.WriteLine($"FROM: {mailMessage.From}");
                Console.WriteLine($"TO: {string.Join(";", mailMessage.To)}");
                Console.WriteLine($"SUBJECT: {mailMessage.Subject}");
                Console.WriteLine($"BODY: {mailMessage.Body}");
                return;
            }

            options.GetClient().Send(mailMessage);
        }
    }
}
