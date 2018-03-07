using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    

    [Serializable]
    public class TestData
    {
        public double Time { get; set; }
        public string Value { get; set; }      
        public string color { get; set; }
        public string bgcolor { get; set; }
    }
}
