using GenshinVybyu;
using GenshinVybyu.Types;
using GenshinVybyu.Services;
using GenshinVybyu.Services.Interfaces;
using GenshinVybyu.Controllers;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

// Setup Bot configuration
var webhookConfigurationSection = builder.Configuration.GetSection(WebhookConfiguration.Configuration);
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
var sensitiveData = GetSensitiveData.LoadData();
var messagesConfig = GetMessagesConfig.LoadMessages();

builder.Services.Configure<WebhookConfiguration>(webhookConfigurationSection)
                .Configure<BotConfiguration>(botConfigurationSection)
                .Configure<SensitiveData>(sd =>
                {
                    sd.BotToken = sensitiveData.BotToken;
                    sd.SecretToken = sensitiveData.SecretToken;
                })
                .Configure<MessagesConfig>(mc =>
                {
                    mc.Splashes = messagesConfig.Splashes;
                    mc.Messages = messagesConfig.Messages;
                });

var webhookConfiguration = webhookConfigurationSection.Get<WebhookConfiguration>();

// Register named HttpClient to get benefits of IHttpClientFactory
// and consume it with ITelegramBotClient typed client.
// More read:
//  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests#typed-clients
//  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
builder.Services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {
                    WebhookConfiguration? webhookConfig = sp.GetConfiguration<WebhookConfiguration>();
                    TelegramBotClientOptions options = new(sensitiveData.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

// Dummy business-logic services

builder.Services
    .AddModelCalc()
    .AddState()
    .AddHandlingPipeline()
    .AddBotOutput()
    .AddUtility();

// There are several strategies for completing asynchronous tasks during startup.
// Some of them could be found in this article https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-part-1/
// We are going to use IHostedService to add and later remove Webhook
builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddLogging();
builder.Services.AddRedis(sensitiveData);

// The Telegram.Bot library heavily depends on Newtonsoft.Json library to deserialize
// incoming webhook updates and send serialized responses back.
// Read more about adding Newtonsoft.Json to ASP.NET Core pipeline:
//   https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-6.0#add-newtonsoftjson-based-json-format-support
builder.Services
    .AddControllers();

var app = builder.Build();
// Construct webhook route from the Route configuration parameter
// It is expected that BotController has single method accepting Update
app.MapBotWebhookRoute<BotController>(route: webhookConfiguration.Route);
app.MapControllers();
app.MapActions<IActionsCollection>();
app.Run();