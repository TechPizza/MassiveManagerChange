using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIMModel
{
    [Serializable]
    public class CostCenterModel
    {
        public int Id { get; set; }

        public string CostCenterCode { get; set; }

        public string NewManager { get; set; }
    }
}
