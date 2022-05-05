using log4net;
using Microsoft.Extensions.Caching.Distributed;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Utilities;
//using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NY.Framework.Application.Services
{
    public class DistributedCacheService //: IDistributedCacheService
    {      
        NY.Framework.Infrastructure.Logging.Logger logger;
        IDistributedCache cache;
        public DistributedCacheService(IDistributedCache cache)
        {
            this.cache = cache;
            this.logger = new Infrastructure.Logging.Logger(typeof(DistributedCacheService));
        }

        public byte[] Get(string key)
        {
           return cache.Get(key);
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        //public async Task RemoveAll()
        //{
        //    try
        //    {
        //        logger.LogInfo("Connecting......Redis Server");
        //        var connection = RedisConnection.GetConnection();
        //        var server = connection.GetServer(Constants.Redis_ConnectionGetServer);
        //        if (server.IsConnected)
        //        {
        //            logger.LogInfo("Connected......Redis Server");
        //            await Task.Run(() =>
        //            {
        //                server.FlushAllDatabasesAsync();
        //            });
                    
        //        }
                
        //    }
        //   catch(Exception ex)
        //    {
        //        logger.Log(ex);
        //    }
           
        //}

        //public async Task RemoveAllProgram()
        //{
        //    try
        //    {
        //        var connection = RedisConnection.GetConnection();
        //        // EndPoint endPoint = connection.GetEndPoints().First();
        //        var server = connection.GetServer(Constants.Redis_ConnectionGetServer);
        //        if (server.IsConnected)
        //        {
                    
                    
        //            //    int cout = keys.Count();
        //            //    foreach(var k in keys)
        //            //    {
        //            //        string kk = k.ToString();
        //            //    }
        //            //}
                    
        //           await Task.Run(() =>
        //            {
        //                IEnumerable<RedisKey> keys = server.Keys(pattern: "*" + Constants.Cache_Program + "*");
        //                if (keys != null)
        //                {
        //                    foreach (var key in keys)
        //                    {
        //                        cache.RemoveAsync(key);
        //                    }
        //                }
        //            });
        //        }
              
               
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Log(ex);
        //    }
        //}
    }
}
