using log4net;
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
        private static readonly ILog log = LogManager.GetLogger("FIMModel");

        public string ObjectID { get; set; }

        public string Manager { get; set; }

        public string CostCenterCode { get; set; }

        public string AccountName { get; set; }

        public static PersonModel GetManagerThroughSamAccount(string _managerSamAccount)
        {
            PersonModel _res = null;

            string xPathQuery = "/Person[AccountName='" + _managerSamAccount + "']";

            using (DefaultClient _client = new DefaultClient())
            {

                _client.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                _client.RefreshSchema();
                if (log.IsDebugEnabled) { log.Debug(string.Format("Enumerate with filter: {0}", xPathQuery)); }
                List<RmResource> _ls = _client.Enumerate(xPathQuery).ToList();
                if (log.IsDebugEnabled) { log.Debug("End enumerate "); }

                if (_ls.Count != 1)
                {
                    if (log.IsDebugEnabled) { log.Debug("Error found more than 1 account"); }
                    //throw new UserNotFoundException("");
                }
                else
                {
                    foreach (RmPerson person in _ls)
                    {
                        PersonModel _per = new PersonModel();
                        _per.ObjectID = person.ObjectID.Value;
                        _per.AccountName = person.AccountName;
                        _res = _per;
                    }
                }
            }
            return _res;
        }

        public static List<PersonModel> GetFimPersonListFromCostCenter(string _cdcCode)
        {
            PersonModel _per = null;
            List<PersonModel> _lperson = new List<PersonModel>();

            string xPathQuery = "/Person[CostCenter='" + _cdcCode + "']";

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
                        _per.ObjectID = person.ObjectID.Value;
                        _per.Manager = (person.Manager != null) ? person.Manager.Value : string.Empty;
                        _per.CostCenterCode = person.CostCenter;
                        _per.AccountName = person.AccountName;
                        _lperson.Add(_per);
                    }
                }


            }

            return _lperson;
        }

        public static void UpdateUser(string manager, PersonModel _fp)
        {

            using (DefaultClient _client = new DefaultClient())
            {
                _client.ClientCredential = CredentialCache.DefaultNetworkCredentials;
                _client.RefreshSchema();
                List<RmResource> _res = _client.Enumerate("/Person[ObjectID='" + _fp.ObjectID + "']").ToList();
                foreach (RmPerson _r in _res)
                {
                    RmResourceChanges changes = new RmResourceChanges(_r);
                    try
                    {
                        changes.BeginChanges();
                        if (string.IsNullOrWhiteSpace(manager))
                        {
                            RmAttributeName _attr = new RmAttributeName("Manager");
                            _r.Attributes.Remove(_attr);
                        }
                        else
                            _r.Manager = new RmReference(manager);

                        _client.Put(changes);
                        changes.AcceptChanges();
                    }
                    catch
                    {
                        changes.DiscardChanges();
                    }
                }
            }

        }
    }
}
