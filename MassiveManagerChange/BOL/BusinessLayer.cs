using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class BusinessLayer
    {
        private DataLayer _dl = new DataLayer();

        public void CopyInActual()
        {
            _dl.CopyInActual();
        }

        public void CalculateDelta()
        {
            _dl.CalculateDelta();
        }

        public DataTable GetDeltaChanges()
        {
            return _dl.GetDeltaChanges();
        }
    }
}
