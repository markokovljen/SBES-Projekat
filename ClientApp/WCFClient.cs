using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Security;
using System.Security;
using SecurityManager;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ClientApp
{
	public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
	{

        public static readonly string Key = "ow7dxys8glfor9tnc2ansdfo1etkfjcv";
        public static readonly Encoding Encoder = Encoding.UTF8;

        IWCFService factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);//ovde ce biti user mika npr(tj onaj koji je trenutno pokrenuo klijenta)

            Console.WriteLine(cltCertCN);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ServerClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
		}

        public void Pauziraj(string subjectName)
        {
            try
            {
                factory.Pauziraj(subjectName);
                Console.WriteLine("Pauziraj allowed.");
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to Pauziraj. Error message: {0}", e.Message);
            }
            catch (FaultException e)
            {
                Console.WriteLine("Error while trying to Pauziraj. Error message: {0}", e.Message);
            }
        }

        public void Pokreni(string subjectName)
        {
            try
            {
                factory.Pokreni(subjectName);
                Console.WriteLine("Pokreni allowed.");
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to Pokreni. Error message: {0}", e.Message);

            }
            catch (FaultException e)
            {
                Console.WriteLine("Fault exp Error while trying to Pokreni. Error message: {0}", e.Message);
            }
        }

        public void PromeniVreme(string subjectName, string vreme)
        {
            try
            {
                string encMessage = TripleDesEncrypt(vreme);
                factory.PromeniVreme(subjectName, encMessage);
                Console.WriteLine("Resetuj allowed.");
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to Resetuj. Error message: {0}", e.Message);
            }
            catch (FaultException e)
            {
                Console.WriteLine("Error while trying to Resetuj. Error message: {0}", e.Message);
            }
        }

        public void Resetuj(string subjectName)
        {
            try
            {
                factory.Resetuj(subjectName);
                Console.WriteLine("Resetuj allowed.");
            }
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to Resetuj. Error message: {0}", e.Message);
            }
            catch (FaultException e)
            {
                Console.WriteLine("Error while trying to Resetuj. Error message: {0}", e.Message);
            }
        }

        public string VidiVreme(string subjectName)
        {
            try
            {
                string povratna = factory.VidiVreme(subjectName);
                Console.WriteLine("VidiVreme allowed.");
                return povratna;
            }
            catch (SecurityAccessDeniedException e)
            {
                return "Error while trying to Pauziraj. Error message: " + e.Message;
            }
            catch (FaultException e)
            {
                return "Error while trying to Pauziraj. Error message: " + e.Message;
            }
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

        public static string TripleDesEncrypt(string plainText)
        {
            var des = CreateDes(Key);
            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(plainText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(output);
        }

        public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			this.Close();
		}

       
    }
}
