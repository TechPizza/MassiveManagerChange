using BOL;
using FIMModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MassiveManagerChange
{

    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger("MassiveManagerChange");

        static void Main(string[] args)
        {
            try
            {
                BusinessLayer _bl = new BusinessLayer();
                DataTable _dt = new DataTable();
                List<CostCenterModel> _ccl = new List<CostCenterModel>();
                List<PersonModel> _lp = new List<PersonModel>();
                PersonModel _manager = new PersonModel();

                _bl.CalculateDelta();
                _dt = _bl.GetDeltaChanges();
                _ccl = ConvertDataTable<CostCenterModel>(_dt);

                foreach (CostCenterModel ccm in _ccl)
                {
                    _lp = PersonModel.GetFimPersonListFromCostCenter(ccm.CostCenterCode);
                    if (ccm.NewManager is null)
                    {
                        foreach (PersonModel per in _lp)
                        {
                            PersonModel.UpdateUser(null, per);
                            if (per.AccountName.StartsWith("X_"))
                                _bl.UpdateExternalsDBSubject(per.AccountName, null);
                            else
                                _bl.UpdateInternalsDBSubject(per.AccountName, null);
                        }
                    }
                    else
                    {
                        _manager = PersonModel.GetManagerThroughSamAccount(ccm.NewManager);
                        foreach (PersonModel per in _lp)
                        {
                            if (!(_manager is null))
                            {
                                PersonModel.UpdateUser(_manager.ObjectID, per);
                                if (per.AccountName.StartsWith("X_"))
                                {
                                    _bl.UpdateExternalsDBSubject(per.AccountName, _manager.AccountName);
                                }
                                else
                                {
                                    _bl.UpdateInternalsDBSubject(per.AccountName, _manager.AccountName);
                                }
                            }
                        }
                    }
                }
                _bl.CopyInActual();
            }
            catch(Exception e)
            {

            }

        }

        //Convert datatable to a list of object (generic)
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        //Get items from datarow
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                PropertyInfo property = temp.GetProperty(column.ColumnName);
                if (property == null)
                    continue;

                if (String.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                {
                    property.SetValue(obj, null, null);
                }
                else
                {
                    property.SetValue(obj, dr[column.ColumnName], null);
                }
            }
            return obj;
        }

    }
}
