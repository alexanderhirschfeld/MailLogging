using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.alexanderhirschfeld.Logging.MailLogging
{
    [ProviderAlias("MailLogging")]
    public sealed class MailLoggerProvider : ILoggerProvider
    {
        private readonly MailLoggerOptions _mailLoggerOptions;
        public string CategoryName { get; private set; }

        public MailLoggerProvider(IOptions<MailLoggerOptions> mailLoggerOptions)
        {
            _mailLoggerOptions = mailLoggerOptions.Value;
        }

        public ILogger CreateLogger(string categoryName)
        {
            this.CategoryName = categoryName;
            return new MailLogger(this);
        }

        public MailLoggerOptions GetOptions() => _mailLoggerOptions;

        public void Dispose() {}
    }
}
