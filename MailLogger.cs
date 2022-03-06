using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;
using System.Text;

namespace com.alexanderhirschfeld.Logging.MailLogging
{
    public sealed class MailLogger : ILogger
    {
        private readonly MailLoggerProvider _mailLoggerProvider;
        private readonly string _categoryName;

        public MailLogger(MailLoggerProvider mailLoggerProvider, string categoryName)
        {
            _mailLoggerProvider = mailLoggerProvider;
            _categoryName = categoryName;
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

            StringBuilder body = new StringBuilder();
            body.Append($"EventId: {eventId}\r\n");
            body.Append($"LogLevel: {logLevel}\r\n");
            body.Append($"Application: {AppDomain.CurrentDomain.FriendlyName}\r\n");
            body.Append($"Context: {_categoryName}\r\n");
            body.Append($"Time: {DateTimeOffset.Now}\r\n");
            body.Append($"Message: {formatter(state, exception)}\r\n");
            if (exception is not null)
            {
                body.Append($"Exception: {exception.Message}\r\n");
                body.Append($"Trace: {exception.StackTrace}\r\n");
            }

            MailMessage mailMessage = new MailMessage
            {
                From = options.GetFrom(),
                Subject = $"[{logLevel.ToString().ToUpper()}] [{AppDomain.CurrentDomain.FriendlyName}] {_categoryName}",
                Body = body.ToString(),
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
