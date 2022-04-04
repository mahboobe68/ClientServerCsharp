using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerX
{
     class RedisConnectorHelper
    {
        static RedisConnectorHelper()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { "110.120.46.10" },
                AbortOnConnectFail = false

            };
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
