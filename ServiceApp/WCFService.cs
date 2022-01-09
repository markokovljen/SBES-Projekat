using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using System.Security.Permissions;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.ServiceModel.Security;
using System.ServiceModel;
using SecurityManager;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace ServiceApp
{
    public class WCFService : IWCFService
    {
        public void Pauziraj(string subjectName)
        {
            string group = CertManager.GetGroup(StoreName.My, StoreLocation.LocalMachine, subjectName);
            string[] permissions;
            if (RolesConfig.GetPermissions(group, out permissions))
            {
                foreach (string permision in permissions)
                {
                    if (permision.Equals("StartPause"))
                    {
                        StaticHelp.stopwatch.Stop();
                        try
                        {
                            Audit.StopSuccess(subjectName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        Console.WriteLine("Zaustavljena je stopwatch");
                       
                    }

                }
            }
            throw new FaultException("Nema klijent permisiju za ovu komandu!");
        }

        public void Pokreni(string subjectName)
        {
            StaticHelp.stopwatch = new Stopwatch();
            string group = CertManager.GetGroup(StoreName.My, StoreLocation.LocalMachine, subjectName);
            string[] permissions;
            if (RolesConfig.GetPermissions(group, out permissions))
            {
                foreach (string permision in permissions)
                {
                    if (permision.Equals("StartPause"))
                    {
                        StaticHelp.stopwatch.Start();
                        Console.WriteLine("Pokrenuta je stopwatch");
                        try
                        {
                            Audit.StartSuccess(subjectName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        return;
                    }

                }
            }
            throw new FaultException("Nema klijent permisiju za ovu komandu!");
        }

        public void PromeniVreme(string subjectName, string vreme)
        {
            throw new NotImplementedException();
        }

        public void Resetuj(string subjectName)
        {
            StaticHelp.stopwatch = new Stopwatch();
            string group = CertManager.GetGroup(StoreName.My, StoreLocation.LocalMachine, subjectName);
            string[] permissions;
            if (RolesConfig.GetPermissions(group, out permissions))
            {
                foreach (string permision in permissions)
                {
                    if (permision.Equals("Reset"))
                    {
                        StaticHelp.stopwatch.Reset();
                        Console.WriteLine("Resetovana je stopwatch");
                        try
                        {
                            Audit.RestartSuccess(subjectName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        return;
                    }

                }
            }
            throw new FaultException("Nema klijent permisiju za ovu komandu!");
        }

        public string VidiVreme(string subjectName)
        {
            string group = CertManager.GetGroup(StoreName.My, StoreLocation.LocalMachine, subjectName);
            string[] permissions;
            if (RolesConfig.GetPermissions(group, out permissions))
            {
                foreach (string permision in permissions)
                {
                    if (permision.Equals("Read"))
                    {
                        TimeSpan ts = StaticHelp.stopwatch.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                            ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);
                        return elapsedTime;
                    }

                }
            }
            throw new FaultException("Nema klijent permisiju za ovu komandu!");
        }
    }
}
