using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    [Serializable]
    public class JQGridColumnData
    {
        public String[] ColumnNames { get; set; }
        public List<JQGridColumnModel> ColumnModels { get; set; }
        public List<String> ColspanColumns { get; set; }
    }

    [Serializable]
    public class JQGridColumnModel
    {
        
        public String name { get; set; }
        public String index { get; set; }

        private Int32 _width = 150;
        public Int32 width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private String _align = "left";
        public String align
        {
            get
            {
                return _align;
            }
            set
            {
                _align = value;
            }
        }

        private String _datefmt = "yyyy年mm月dd日";
        public String datefmt
        {
            get
            {
                return _datefmt;
            }
            set
            {
                _datefmt = value;
            }
        }

        private Boolean _sortable = true;
        public Boolean sortable
        {
            get
            {
                return _sortable;
            }
            set
            {
                _sortable = value;
            }
        }

        private Boolean _colspan = false;
        public Boolean colspan
        {
            get
            {
                return _colspan;
            }
            set
            {
                _colspan = value;
            }
        }
    }
}
