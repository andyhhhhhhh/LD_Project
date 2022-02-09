using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModels.Comm
{
    public class ChartData
    {
        public int Index { get; set; }
        public double XValue { get; set; }
        public double YValue { get; set; }
        
        public string Name { get; set; }

    }

    public class CharDataEventsArgs : EventArgs
    {
        public CharDataEventsArgs(List<ChartData> chardata, bool bFirst)
        {
            CharData = chardata;
            BFirst = bFirst;
        }
        public List<ChartData> CharData { get; set; }

        public bool BFirst { get; set; }
    }
}
