using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Log4net.Loggers;
using Core.CrossCuttingConcerns.Security;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Aspects.Postsharp.LogAspects
{
    [Serializable]
    [MulticastAttributeUsage(MulticastTargets.Method, TargetMemberAttributes = MulticastAttributes.Instance)]
    public class LogAspect : OnMethodBoundaryAspect
    {
        private Type loggerType;
        private LoggerService loggerService;

        private LogDetail GetDetail(MethodExecutionArgs args)
        {
            var logParameters = args.Method.GetParameters().Select((t, i) => new LogParameter
            {
                Name = t.Name,
                Type = t.ParameterType.Name,
                Value = args.Arguments.GetArgument(i)
            }).ToList();

            var logDetail = new LogDetail
            {
                FullName = args.Method.DeclaringType?.Name,
                EntityName = logParameters != null && logParameters.Count > 0 ? logParameters[0].Type : string.Empty,
                MethodName = args.Method.Name,
                Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                UserId = Identity.PersonelId,
                UserName = Identity.KullaniciAd,
                Parameters = logParameters,
                Exception = args.Exception
            };

            return logDetail;
        }

        public LogAspect(Type loggerType = null)
        {
            this.loggerType = loggerType ?? typeof(FileLogger);
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            if (loggerType.BaseType != typeof(LoggerService))
            {
                throw new Exception("Yanlış Logger !");
            }

            loggerService = (LoggerService)Activator.CreateInstance(loggerType);

            base.RuntimeInitialize(method);
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (!loggerService.IsInfoEnabled)
            {
                return;
            }

            try
            {
                loggerService.Info(GetDetail(args));
            }
            catch { }
        }

        public override void OnException(MethodExecutionArgs args)
        {
            if (!loggerService.IsInfoEnabled)
            {
                return;
            }

            try
            {
                loggerService.Error(GetDetail(args));
            }
            catch { }
        }
    }
}
