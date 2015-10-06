using System;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin
{
    public class DrawingRev
    {
        public DrawingRev(SwProperty rev, SwProperty eco, SwProperty desc, SwProperty list, SwProperty date)
        {
            this.Revision = rev;
            this.Eco = eco;
            this.Description = desc;
            this.List = list;
            this.Date = date;

            this.Revision.SwApp = this.SwApp;
            this.Eco.SwApp = this.SwApp;
            this.Description.SwApp = this.SwApp;
            this.List.SwApp = this.SwApp;
            this.Date.SwApp = this.SwApp;
        }

        public void Del()
        {
            this.Revision.Del();
            this.Eco.Del();
            this.Description.Del();
            this.List.Del();
            this.Date.Del();
        }

        public void Del(SldWorks sw)
        {
            this.SwApp = sw;
            this.Revision.Del(sw);
            this.Eco.Del(sw);
            this.Description.Del(sw);
            this.List.Del(sw);
            this.Date.Del(sw);
        }

        public void Write()
        {
            //System.Diagnostics.Debug.Print("Writing " + this.Revision.Value);
            this.Revision.Write();
            this.Eco.Write();
            this.Description.Write();
            this.List.Write();
            this.Date.Write();
        }

        public void Write(ModelDoc2 md)
        {
            //System.Diagnostics.Debug.Print("Writing " + this.Revision.Value);
            this.Revision.Write(md);
            this.Eco.Write(md);
            this.Description.Write(md);
            this.List.Write(md);
            this.Date.Write(md);
        }

        public void Write(SldWorks sw)
        {
            this.SwApp = sw;
            System.Diagnostics.Debug.Print("Writing " + this.Revision.Value);
            this.Revision.Write(sw);
            this.Eco.Write(sw);
            this.Description.Write(sw);
            this.List.Write(sw);
            this.Date.Write(sw);
        }

        public override string ToString()
        {
            string o = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n", 
                this.Revision.ToString(),
                this.Eco.ToString(),
                this.Description.ToString(),
                this.List.ToString(),
                this.Date.ToString());

            return o;
        }

        private SwProperty _rev;

        public SwProperty Revision
        {
            get { return _rev; }
            set { _rev = value; }
        }

        private SwProperty _eco;

        public SwProperty Eco
        {
            get { return _eco; }
            set { _eco = value; }
        }

        private SwProperty _description;

        public SwProperty Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private SwProperty _list;

        public SwProperty List
        {
            get { return _list; }
            set { _list = value; }
        }

        private SwProperty _date;

        public SwProperty Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private SldWorks _swApp;

        public SldWorks SwApp
        {
            get { return _swApp; }
            set { _swApp = value; }
        }
	
    }
}
