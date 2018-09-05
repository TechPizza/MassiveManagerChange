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
        public int ID { get; set; }

        public string Manager { get; set; }
    }
}
