using Microsoft.ResourceManagement.Client;
using Microsoft.ResourceManagement.ObjectModel;
using Microsoft.ResourceManagement.ObjectModel.ResourceTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FIMModel
{
    [Serializable]
    public class PersonModel
    {
        public string Manager { get; set; }

        public string CostCenterCode { get; set; }

        public static List<PersonModel> GetFimPersonListFromCostCenter(string _cdcId, string _companyCode)
        {
            if (log.IsDebugEnabled) { log.Debug("Get user from DataTable with MetaID"); }
            PersonModel _per = null;
            List<PersonModel> _lperson = new List<PersonModel>();

            string xPathQuery = "/Person[labCostCenterId='" + _cdcId + "']";

            using (DefaultClient _client = new DefaultClient())
            {

                _client.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                _client.RefreshSchema();
                if (log.IsDebugEnabled) { log.Debug(string.Format("Enumerate with filter: {0}", xPathQuery)); }
                List<RmResource> _res = _client.Enumerate(xPathQuery).ToList();
                if (log.IsDebugEnabled) { log.Debug("End enumerate "); }

                if (_res == null)
                {
                    if (log.IsDebugEnabled) { log.Debug("Enumeration null!"); }
                    //throw new UserNotFoundException("");
                }
                else
                {
                    foreach (RmPerson person in _res)
                    {
                        _per = new PersonModel();
                        _per.Manager = (person.Manager != null) ? person.Manager.Value : string.Empty;
                        _per.CostCenterCode = person.CostCenter;
                        _lperson.Add(_per);
                    }
                }


            }

            return _lperson;
        }
    }
}
