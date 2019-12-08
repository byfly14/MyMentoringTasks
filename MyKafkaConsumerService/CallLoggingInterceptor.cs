using System;
using Castle.DynamicProxy;

namespace MyKafkaConsumerService
{
    public class CallLoggingInterceptor : IInterceptor
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(CallLoggingInterceptor));
        public void Intercept(IInvocation invocation)
        {
            try
            {
                Logger.Info($"{DateTime.Now}: Entering {invocation.Method.Name} {string.Join(", ", invocation.Arguments)}");
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                Logger.Info($"{DateTime.Now}: {ex}");
            }
            finally
            {
                Logger.Info($"{DateTime.Now}: Finishing {invocation.Method.Name}; Return value: {invocation.ReturnValue}");
            }
        }
    }
}
