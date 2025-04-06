using GemmaChat.Application.Services;
using GemmaChat.Infrastructure.Contexts;
using GemmaChat.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using SQLitePCL;
using MapsterMapper;
using Mapster;

// Mapster configurations scanning
var mapperConfig = TypeAdapterConfig.GlobalSettings;
mapperConfig.Scan(typeof(GemmaChat.Application.Mappings.MappingConfig).Assembly);

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Mapster
        services.AddSingleton<IMapper>(new Mapper(mapperConfig));

        // Sqlite Initialization
        Batteries.Init();

        // Sqlite Context
        services.AddDbContext<DbContext, SqliteContext>((options) =>
        {
            options.UseSqlite(hostContext.Configuration.GetConnectionString("Sqlite"));
        });

        // Telegram Client
        services.AddScoped<ITelegramBotClient, TelegramBotClient>((s) =>
        {
            return new TelegramBotClient(hostContext.Configuration["BotToken"] ?? "");
        });

        // LMS Client
        services.AddHttpClient("lms-client", client =>
        {
            client.BaseAddress = new Uri(hostContext.Configuration["LMS"] ?? "");
            client.Timeout = TimeSpan.FromMinutes(5);
        });
        services.AddScoped<TelegramService>();
        services.AddScoped<LMSService>();
        services.AddHostedService<Worker>();
    })
    .Start();