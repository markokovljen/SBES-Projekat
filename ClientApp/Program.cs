using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using SecurityManager;
using System.Security.Principal;

namespace ClientApp
{
	public class Program
	{
		static void Main(string[] args)
		{
            string srvCertCN = "pera";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/WCFService"),
                                      new X509CertificateEndpointIdentity(srvCert));

            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            using (WCFClient proxy = new WCFClient(binding, address))
            {

            }


			Console.ReadLine();
		}
	}
}
