
using System;
using MyKafka.Common;

namespace MyProxyLoggerNameSpace {
	public class MyKafkaProducer_Proxy {
			private readonly IMyKafkaProducer _target;

		private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(MyKafkaProducer_Proxy));

		public MyKafkaProducer_Proxy(IMyKafkaProducer target)
		{
			this._target = target;
		}

		public void Start(System.Threading.CancellationToken cancellationToken)
		{
			try
            {
                Logger.Info($"{DateTime.Now}: Entering \"Start(System.Threading.CancellationToken cancellationToken)\" ");
                				this._target.Start(cancellationToken);
				            }
            catch (Exception ex)
            {
                Logger.Info($"{DateTime.Now}: {ex}");
                throw;
            }
            finally
            {
                Logger.Info($"{DateTime.Now}: Finishing \"Start(System.Threading.CancellationToken cancellationToken)\" ");
            }
		}
				}
		}
	