using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace com.alexanderhirschfeld.Logging.MailLogging
{
    public static class MailLoggerExtensions
    {
        public static ILoggingBuilder AddMailLogging(this ILoggingBuilder builder, Action<MailLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, MailLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
