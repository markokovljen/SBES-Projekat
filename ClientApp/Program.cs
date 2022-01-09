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
                string temp = "";

                while (temp != "x")
                {
                    Console.WriteLine(
                        "Stisni 1: Pokreni\n" +
                        "Stisni 2: Pauziraj\n" +
                        "Stisni 3: Vidi vreme\n" +
                        "Stisni 4: Resetuj\n" +
                        "Stisni 5: Promeni vreme\n" +
                        "Stisni x: Izadji iz aplikacije\n"
                        );

                    temp = Console.ReadLine();

                    switch (temp)
                    {
                        case "1":
                            proxy.Pokreni(cltCertCN);
                            Console.WriteLine("Stoperica je pokrenuta");
                            break;
                        case "2":
                            proxy.Pauziraj(cltCertCN);
                            Console.WriteLine("Stoperica je stopirana");
                            break;
                        case "3":
                            Console.WriteLine("Trenutno vreme je :" + proxy.VidiVreme(cltCertCN));
                            break;
                        case "4":
                            proxy.Resetuj(cltCertCN);
                            Console.WriteLine("Stoperica je resetovana");
                            break;
                        case "5":
                            Console.WriteLine("Unesi novo vreme");
                            temp = Console.ReadLine();
                            proxy.PromeniVreme(cltCertCN, temp);
                            break;
                        default:
                            Console.WriteLine("Uneli ste pogresnu komandu!");
                            break;
                    }

                }
            }


		}
	}
}
