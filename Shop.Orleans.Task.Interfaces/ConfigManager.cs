using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Config
{
    public class ConfigManager
    {
        public static string ConsulAddress { get; set; } = "http://192.168.1.3:8500/";
        public static string GetByConsul(string key)
        {
            using (var client = new ConsulClient(it => { it.Address = new Uri(ConsulAddress); }))
            {
                var kvPair = client.KV.Get(key).Result;
                if (kvPair.Response != null && kvPair.Response.Value != null)
                {
                    return Encoding.UTF8.GetString(kvPair.Response.Value, 0, kvPair.Response.Value.Length);
                }
                return string.Empty;
            }
        }
        public static bool SetByConsul(string key, byte[] val)
        {
            using (var client = new ConsulClient(it => { it.Address = new Uri(ConsulAddress); }))
            {
                return client.KV.Put(new KVPair(key) { Value = val }).Result.Response;
            }
        }
    }

}
