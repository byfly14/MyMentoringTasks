using System;
using System.Threading.Tasks;
using CompileTimeWeaver;

public class MyTimeWeaverInterceptorAttribute : AdviceAttribute
{
    private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(MyTimeWeaverInterceptorAttribute));
    public override object Advise(IInvocation invocation)
    {
        Logger.Info($"{DateTime.Now}: Entering {invocation.Method.Name} {string.Join(", ", invocation.Args)}");

        try
        {
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
        Logger.Info($"{DateTime.Now}: Entering {invocation.Method.Name} {string.Join(", ", invocation.Args)}");

        try
        {
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