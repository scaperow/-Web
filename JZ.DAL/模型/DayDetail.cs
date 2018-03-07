using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JZ.BLL
{
    public class DayDetail
    {
        public int Id
        {
            get;
            set;
        }

        public int Repeatedweeks
        {
            get;
            set;
        }

        public string Meetingroom
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public DateTime Evtstart
        {
            get;
            set;
        }
        public DateTime Evtend
        {
            get;
            set;
        }


        public string Description
        {
            get;
            set;
        }


    }
}
