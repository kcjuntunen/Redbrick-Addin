using System;
using System.Collections.Generic;
using System.Text;

namespace Redbrick_Addin {
    public class eco {
        private int _ecrNum = 0;

        public int EcrNumber {
            get { return _ecrNum; }
            set { _ecrNum = value; }
        }

        private string _reqBy = string.Empty;

        public string RequestedBy {
            get { return _reqBy; }
            set { _reqBy = value; }
        }

        private string _changes = string.Empty;

        public string Changes {
            get { return _changes; }
            set { _changes = value; }
        }

        private string _status = string.Empty;

        public string Status {
            get { return _status; }
            set { _status = value; }
        }

        private string _errDesc = string.Empty;

        public string ErrDescription {
            get { return _errDesc; }
            set { _errDesc = value; }
        }

        private string _rev = string.Empty;

        public string Revision {
            get { return _rev; }
            set { _rev = value; }
        }

        public override string ToString() {
            string outString = string.Format("EcrNum = {0}\nReqBy = {1}\nChanges = {2}\nStatus = {3}\nErrDesc = {4}\nRev = {5}",
                EcrNumber, RequestedBy, Changes, Status, ErrDescription, Revision);
            return outString;
        }
    }
}
