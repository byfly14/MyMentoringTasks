using System;
using System.Threading.Tasks;
using CompileTimeWeaver;

namespace MyKafkaProducerService
{
    public class MyCompileTimeWeaverAttribute : AdviceAttribute
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(MyCompileTimeWeaverAttribute));
        public override object Advise(IInvocation invocation)
        {
            try
            {
                Logger.Info($"{DateTime.Now}: Entering {invocation.Method.Name} {string.Join(", ", invocation.Args)}");
                return invocation.Proceed();
            }
            catch (Exception ex)
            {
                Logger.Info($"{DateTime.Now}: {ex}");
                throw;
            }
            finally
            {
                Logger.Info($"{DateTime.Now}: Finishing {invocation.Method.Name}; Return value: {invocation.ReturnValue}");
            }
        }

        public override async Task<object> AdviseAsync(IInvocation invocation)
        {
            try
            {
                Logger.Info($"{DateTime.Now}: Entering {invocation.Method.Name} {string.Join(", ", invocation.Args)}");
                return await invocation.ProceedAsync();
            }
            catch (Exception ex)
            {
                Logger.Info($"{DateTime.Now}: {ex}");
                throw;
            }
            finally
            {
                Logger.Info($"{DateTime.Now}: Finishing {invocation.Method.Name}; Return value: {invocation.ReturnValue}");
            }
        }
    }
}
