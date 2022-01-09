using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Security;

namespace Contracts
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        void Pokreni(string subjectName);
        [OperationContract]
        void Pauziraj(string subjectName);
        [OperationContract]
        string VidiVreme(string subjectName);
        [OperationContract]
        void Resetuj(string subjectName);
        [OperationContract]
        void PromeniVreme(string subjectName, string vreme);
    }
}
