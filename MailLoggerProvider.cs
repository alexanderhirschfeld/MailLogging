using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace com.alexanderhirschfeld.Logging.MailLogging
{
    [ProviderAlias("MailLogging")]
    public sealed class MailLoggerProvider : ILoggerProvider
    {
        private readonly MailLoggerOptions _mailLoggerOptions;

        public MailLoggerProvider(IOptions<MailLoggerOptions> mailLoggerOptions)
        {
            _mailLoggerOptions = mailLoggerOptions.Value;
            _mailLoggerOptions.Validate();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MailLogger(this, categoryName);
        }

        public MailLoggerOptions GetOptions() => _mailLoggerOptions;

        public void Dispose() {}
    }
}
