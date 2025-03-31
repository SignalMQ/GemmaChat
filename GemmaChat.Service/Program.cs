using GemmaChat.Application.Services;
using GemmaChat.Infrastructure.Contexts;
using GemmaChat.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using SQLitePCL;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
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

        // LLM Client
        services.AddHttpClient("llm-client", client =>
        {
            client.BaseAddress = new Uri(hostContext.Configuration["LLM"] ?? "");
            client.Timeout = TimeSpan.FromMinutes(5);
        });
        services.AddScoped<TelegramService>();
        services.AddScoped<LLMService>();
        services.AddHostedService<Worker>();
    })
    .Start();