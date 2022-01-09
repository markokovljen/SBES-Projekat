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

		#region Read()
		
		public void Read()
		{
			try
			{
				factory.Read();
				Console.WriteLine("Read allowed.");
			}
			catch (SecurityAccessDeniedException e)
			{
				Console.WriteLine("Error while trying to Read. Error message : {0}", e.Message);
			}
		}

		#endregion

		#region Modify()
		
		public void Modify()
		{
			
			try
			{
				factory.Modify();
				Console.WriteLine("Modify allowed.");
			}
            catch (SecurityAccessDeniedException e)
            {
                Console.WriteLine("Error while trying to Modify. Error message : {0}", e.Message);
            }
            catch (FaultException e)
            {
                Console.WriteLine("Error while trying to Modify. Error message: {0}", e.Message);
            }
        }

            #endregion

        #region Delete()

            public void Delete()
		{
			try
			{
				factory.Delete();
				Console.WriteLine("Delete allowed.");
			}
			catch (SecurityAccessDeniedException e)
			{
				Console.WriteLine("Error while trying to Delete. Error message: {0}", e.Message);
			}
			catch (FaultException e)
			{
				Console.WriteLine("Error while trying to Delete. Error message: {0}", e.Message);
			}
		}

		#endregion

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
