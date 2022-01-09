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
using System.Security.Cryptography;

namespace ServiceApp
{
    public class WCFService : IWCFService
    {
        public static readonly string Key = "ow7dxys8glfor9tnc2ansdfo1etkfjcv";
        public static readonly Encoding Encoder = Encoding.UTF8;

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
            string rez = TripleDesDecrypt(vreme);
            Console.WriteLine(rez);
        }


        public static TripleDES CreateDes(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }

        public static string TripleDesDecrypt(string cypherText)
        {
            var des = CreateDes(Key);
            var ct = des.CreateDecryptor();
            var input = Convert.FromBase64String(cypherText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(output);
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
