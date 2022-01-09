using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace SecurityManager
{
    public enum AuditEventTypes
    {
        StartSuccess = 0,
        StopSuccess = 1,
        RestartSuccess = 2
    }

    public class AuditEvents//pomocna klasa za rad sa resx fajlom
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string StartSuccess
        {
            get
            {
                // TO DO
                return ResourceMgr.GetString(AuditEventTypes.StartSuccess.ToString());
            }
        }

        public static string StopSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.StopSuccess.ToString());
            }
        }

        public static string RestartSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.RestartSuccess.ToString());
            }
        }
    }
}
