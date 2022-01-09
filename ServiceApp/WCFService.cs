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

namespace ServiceApp
{
	public class WCFService : IWCFService
	{
        public void Read()
		{
			Console.WriteLine("Read successfully executed.");

            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            try
            {
                Audit.AuthorizationSuccess(userName,
                    OperationContext.Current.IncomingMessageHeaders.Action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Modify")]
        public void Modify()
		{
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Modify"))
            {
                Console.WriteLine("Modify successfully executed.");

                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Modify method need Modify permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException("User " + userName +
                    " try to call Modify method. Modify method need  Modify permission.");
            }
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Delete")]
        public void Delete()
		{
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Delete"))
            {
                Console.WriteLine("Delete successfully executed.");

                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, "Delete method need Delete permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException("User " + userName +
                    " try to call Delete method. Delete method need Delete permission.");
            }
		}

	}
}
