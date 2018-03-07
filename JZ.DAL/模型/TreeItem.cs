using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    public class TreeItem
    {
        public string code
        {
            get;
            set;
        }

        public string text
        {
            get;
            set;
        }

        public string css
        {
            get;
            set;
        }

        public Boolean Checked { get; set; }

        public int isJL { get; set; }

        public int isCenter { get; set; }

        public List<TreeItem> children { get; set; }
    }
}
