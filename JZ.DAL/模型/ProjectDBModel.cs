using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    public class ProjectModel
    {
        public Boolean DataISAttach { get; set; }
        public Boolean DataIntegratedLogin { get; set; }
        public String DataAdapterType { get; set; }
        public String DataBaseType { get; set; }
        public String DataSource { get; set; }
        public String DataInstance { get; set; }
        public String DataUserName { get; set; }
        public String DataPassword { get; set; }
        public String ProjectName { get; set; }
        public String ProjectCode { get; set; }
        public String ProjectDBKey { get; set; }
    }
}
