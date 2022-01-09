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
