# MailLogging

How to use

Program.cs:
[...]
Host.CreateDefaultBuilder(args)
    .ConfigureLogging((hostBuilderContext, logging) =>
    {
        logging.ClearProviders();
        [...]
        logging.AddMailLogging(options =>
        {
            hostBuilderContext.Configuration.GetSection("Logging").GetSection("MailLogging").GetSection("Options").Bind(options);
        });
        [...]
    })
[...]

appsettings.json
{
  "Logging": {
    [...]
    "MailLogging": {
        "Options": {
            "SMTP": {
                "Host": "smtp.your-server.com",
                "Port": 587, // default 587
                "Username": "user",
                "Password": "password"
                "Timeout": 10000 // default 10000 (10sec)
            },
        "From": "sendermail@your-server.com",
        "To": [
            "recipient1@mail.com",
            "recipient2@mail.com"
        ],
        "Debug": false // default false
        },
        "LogLevel": {
            "Default": "Error"
        }
    },
    [...]
    "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Error"
    }
    [...]
  },
  [...]
}