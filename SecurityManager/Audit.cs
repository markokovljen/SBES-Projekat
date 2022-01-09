using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SecurityManager
{
	public class Audit : IDisposable
	{

        private static EventLog customLog = null;
        const string SourceName = "SecurityManager.Audit";
        const string LogName = "MySecTest";//ovde stavim perin security test i mi ga napravi u event viewer

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void StartSuccess(string userName)
        {
            //TO DO

            if (customLog != null)
            {
                string UserAuthenticationSuccess =
                    AuditEvents.StartSuccess;
                string message = String.Format(UserAuthenticationSuccess,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.StartSuccess));
            }
        }

        public static void StopSuccess(string userName)
        {
            //TO DO
            if (customLog != null)
            {
                string AuthorizationSuccess =
                    AuditEvents.StopSuccess;
                string message = String.Format(AuthorizationSuccess,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.StopSuccess));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
        /// <param name="reason">permission name</param>
        public static void RestartSuccess(string userName)
        {
            if (customLog != null)
            {
                string AuthorizationFailed =
                    AuditEvents.RestartSuccess;
                string message = String.Format(AuthorizationFailed,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.RestartSuccess));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
