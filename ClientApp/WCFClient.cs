using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Security;
using System.Security;

namespace ClientApp
{
	public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
	{
		IWCFService factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
			factory = this.CreateChannel();
		}

		

		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			this.Close();
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
            throw new NotImplementedException();
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
    }
}
