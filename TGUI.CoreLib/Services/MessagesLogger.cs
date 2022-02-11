using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TGUI.CoreLib.Interfaces;

namespace TGUI.CoreLib.Services
{
    public class MessagesLogger : IDataLogger
    {
        private readonly IMongoDatabase mongoDatabase;
        private readonly ReplaceOptions replaceOptions = new ReplaceOptions() { IsUpsert = true };
        public MessagesLogger(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }

        public async Task Log<TData>(TData data, Expression<Func<TData, bool>> filter=null)
        {
            var collection = mongoDatabase.GetCollection<TData>(GetCollectionName<TData>());
            if (filter == null)
                await collection.InsertOneAsync(data);
            else await collection.ReplaceOneAsync(filter, data, replaceOptions);
        }

        public async Task<List<TData>> GetData<TData>(Expression<Func<TData, bool>> filter)
        {
            var collection = mongoDatabase.GetCollection<TData>(GetCollectionName<TData>());
            var result = await collection.FindAsync(Builders<TData>.Filter.Where(filter));
            return await result.ToListAsync();
        }

        public static string GetCollectionName<T>()
        {
            return typeof(T).Name + "s";
        }
    }
}
