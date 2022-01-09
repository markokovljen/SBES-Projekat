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
            throw new NotImplementedException();
        }

        public void Pokreni(string subjectName)
        {
            throw new NotImplementedException();
        }

        public void PromeniVreme(string subjectName, string vreme)
        {
            throw new NotImplementedException();
        }

        public void Resetuj(string subjectName)
        {
            throw new NotImplementedException();
        }

        public string VidiVreme(string subjectName)
        {
            throw new NotImplementedException();
        }
    }
}
