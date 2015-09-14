using System;
using System.Collections.Generic;
using System.Text;

namespace Redbrick_Addin
{
    public class eco
    {
        private int _ecrNum;

        public int EcrNumber
        {
            get { return _ecrNum; }
            set { _ecrNum = value; }
        }

        private string _reqBy;

        public string RequestedBy
        {
            get { return _reqBy; }
            set { _reqBy = value; }
        }

        private string _changes;

        public string Changes
        {
            get { return _changes; }
            set { _changes = value; }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _errDesc;

        public string ErrDescription
        {
            get { return _errDesc; }
            set { _errDesc = value; }
        }

        private string _rev;

        public string Revision
        {
            get { return _rev; }
            set { _rev = value; }
        }
		
    }
}
