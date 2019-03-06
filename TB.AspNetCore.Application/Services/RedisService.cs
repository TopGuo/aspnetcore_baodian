using TB.AspNetCore.Infrastructrue.Logs;

namespace TB.AspNetCore.Application.Services
{
    public class RedisService
    {
        public static void SubscribeDoSomething(object query)
        {
            int num = 0;
            Log4Net.Info($"TestPubSub_通道订阅_{num}");
            num += 1;
        }

        public static void MemberChannel_SubscribeDoSomething(object query)
        {
            query= query as string;
            int num = 0;
            Log4Net.Info($"MemberChannel_SubscribeDoSomething_{query}_{num}");
            num += 1;
        }
    }
}
