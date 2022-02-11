using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using Telegram.Bot;
using TGUI.CoreLib.Interfaces;
using TGUI.CoreLib.Services;

namespace TGUI.CoreLib
{
    public static class TGUIStarter
    {
        public static void Init(IServiceCollection services)
        {
            string botToken = Environment.GetEnvironmentVariable("TOKEN");
            string mongoCNNSTR = Environment.GetEnvironmentVariable("MONGO_DB_CNNSTR");
            TelegramBotClient client = new TelegramBotClient(botToken);
            MongoClient mongo = new MongoClient(mongoCNNSTR);
            if (client.BotId.HasValue)
            {
                IMongoDatabase db = mongo.GetDatabase(client.BotId.Value.ToString());
                services.AddSingleton<IMongoDatabase>(db);
            }
            else
            {
                throw new ApplicationException("Bot client creation failed!");
            }

            services.AddSingleton<ITelegramBotClient>(client);
            services.AddSingleton(mongo);
            //services.AddSingleton<IBotCore, BotCoreBase>();
            services.AddSingleton<IDataLogger, MessagesLogger>();
            services.AddSingleton<IMessagesSender, MessagesSender>();
            services.AddSingleton<ISendedItemFactory, SendedItemFactory>();
            services.AddHostedService<BotsEntryPoint>();
        }
    }
}
