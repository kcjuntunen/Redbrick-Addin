#undef DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;

namespace Redbrick_Addin
{
    public class SwProperties : ICollection<SwProperty>
    {
        protected ArrayList _innerArray;
        protected SldWorks swApp;

        public SldWorks SwApp
        {
            get { return this.swApp; }
        }

        public SwProperties(SldWorks sw)
        {
            this.swApp = sw;
            this.cutlistData = new CutlistData();
            this._innerArray = new ArrayList();
        }

        public void CreateDefaultPartSet()
        {
            //this._innerArray.Add(new SwProperty("CUTLIST MATERIAL", swCustomInfoType_e.swCustomInfoNumber, "TBD MATERIAL", false));
            this._innerArray.Add(new SwProperty("EDGE FRONT (L)", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));
            this._innerArray.Add(new SwProperty("EDGE BACK (L)", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));
            this._innerArray.Add(new SwProperty("EDGE LEFT (W)", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));
            this._innerArray.Add(new SwProperty("EDGE RIGHT (W)", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));

            this._innerArray.Add(new SwProperty("Description", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("LENGTH", swCustomInfoType_e.swCustomInfoText, "\"D1@Sketch1\"", true));
            this._innerArray.Add(new SwProperty("WIDTH", swCustomInfoType_e.swCustomInfoText, "\"D2@Sketch1\"", true));
            this._innerArray.Add(new SwProperty("THICKNESS", swCustomInfoType_e.swCustomInfoText, "\"D1@Boss-Extrude1\"", true));
            this._innerArray.Add(new SwProperty("WALL THICKNESS", swCustomInfoType_e.swCustomInfoText, "\"Thickness@Sheet-Metal1\"", true));
            this._innerArray.Add(new SwProperty("COMMENT", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("BLANK QTY", swCustomInfoType_e.swCustomInfoNumber, "1", true));
            this._innerArray.Add(new SwProperty("CNC1", swCustomInfoType_e.swCustomInfoText, "NA", true));
            this._innerArray.Add(new SwProperty("CNC2", swCustomInfoType_e.swCustomInfoText, "NA", true));
            this._innerArray.Add(new SwProperty("OVERL", swCustomInfoType_e.swCustomInfoDouble, "0.0", true));
            this._innerArray.Add(new SwProperty("OVERW", swCustomInfoType_e.swCustomInfoDouble, "0.0", true));
            this._innerArray.Add(new SwProperty("OP1", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP2", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP3", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP4", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP5", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("DEPARTMENT", swCustomInfoType_e.swCustomInfoText, "WOOD", true));
            this._innerArray.Add(new SwProperty("UPDATE CNC", swCustomInfoType_e.swCustomInfoYesOrNo, "No", true));
            this._innerArray.Add(new SwProperty("INCLUDE IN CUTLIST", swCustomInfoType_e.swCustomInfoYesOrNo, "Yes", true));

            this._innerArray.Add(new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoText, "$PRP:\"SW-File Name\"", true));
            this._innerArray.Add(new SwProperty("MATERIAL", swCustomInfoType_e.swCustomInfoText, "\"SW-Material@{0}\"", true));
            this._innerArray.Add(new SwProperty("WEIGHT", swCustomInfoType_e.swCustomInfoText, "\"SW-Mass@{0}\"", true));
            this._innerArray.Add(new SwProperty("VOLUME", swCustomInfoType_e.swCustomInfoText, "\"SW-Volume@{0}\"", true));
            this._innerArray.Add(new SwProperty("COST-TOTALCOST", swCustomInfoType_e.swCustomInfoText, "\"SW-Cost-TotalCost@{0}\"", true));

            string s = string.Empty;
            foreach (SwProperty p in this._innerArray)
            {
                p.Get(this.swApp);
                s += p.Name + ": " + p.ResValue + "\n";
            }
#if DEBUG
            System.Windows.Forms.MessageBox.Show(s);
#endif
        }

        public void CreateDefaultPartSet2()
        {
            this._innerArray.Add(new SwProperty("MATID", swCustomInfoType_e.swCustomInfoNumber, "TBD MATERIAL", false));
            this._innerArray.Add(new SwProperty("EDGEID_LF", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));
            this._innerArray.Add(new SwProperty("EDGEID_LB", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));
            this._innerArray.Add(new SwProperty("EDGEID_WR", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));
            this._innerArray.Add(new SwProperty("EDGEID_WL", swCustomInfoType_e.swCustomInfoNumber, string.Empty, false));

            this._innerArray.Add(new SwProperty("Description", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("LENGTH", swCustomInfoType_e.swCustomInfoText, "\"D1@Sketch1\"", true));
            this._innerArray.Add(new SwProperty("WIDTH", swCustomInfoType_e.swCustomInfoText, "\"D2@Sketch1\"", true));
            this._innerArray.Add(new SwProperty("THICKNESS", swCustomInfoType_e.swCustomInfoText, "\"D1@Boss-Extrude1\"", true));
            this._innerArray.Add(new SwProperty("WALL THICKNESS", swCustomInfoType_e.swCustomInfoText, "\"Thickness@Sheet-Metal1\"", true));
            this._innerArray.Add(new SwProperty("COMMENT", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("BLANK QTY", swCustomInfoType_e.swCustomInfoNumber, "1", true));
            this._innerArray.Add(new SwProperty("CNC1", swCustomInfoType_e.swCustomInfoText, "NA", true));
            this._innerArray.Add(new SwProperty("CNC2", swCustomInfoType_e.swCustomInfoText, "NA", true));
            this._innerArray.Add(new SwProperty("OVERL", swCustomInfoType_e.swCustomInfoDouble, "0.0", true));
            this._innerArray.Add(new SwProperty("OVERW", swCustomInfoType_e.swCustomInfoDouble, "0.0", true));
            this._innerArray.Add(new SwProperty("OP1ID", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP2ID", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP3ID", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP4ID", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("OP5ID", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("DEPARTMENT", swCustomInfoType_e.swCustomInfoText, "WOOD", true));
            this._innerArray.Add(new SwProperty("UPDATE CNC", swCustomInfoType_e.swCustomInfoYesOrNo, "No", true));
            this._innerArray.Add(new SwProperty("INCLUDE IN CUTLIST", swCustomInfoType_e.swCustomInfoYesOrNo, "Yes", true));

            this._innerArray.Add(new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoText, "$PRP:\"SW-File Name\"", true));
            this._innerArray.Add(new SwProperty("MATERIAL", swCustomInfoType_e.swCustomInfoText, "\"SW-Material@{0}\"", true));
            this._innerArray.Add(new SwProperty("WEIGHT", swCustomInfoType_e.swCustomInfoText, "\"SW-Mass@{0}\"", true));
            this._innerArray.Add(new SwProperty("VOLUME", swCustomInfoType_e.swCustomInfoText, "\"SW-Volume@{0}\"", true));
            this._innerArray.Add(new SwProperty("COST-TOTALCOST", swCustomInfoType_e.swCustomInfoText, "\"SW-Cost-TotalCost@{0}\"", true));

#if DEBUG
            string s = string.Empty;
#endif
            foreach (SwProperty p in this._innerArray)
            {
                p.Get(this.swApp);
#if DEBUG
                s += p.Name + ": " + p.ResValue + "\n";
#endif
            }
#if DEBUG
            System.Windows.Forms.MessageBox.Show(s);
#endif

            if (this._innerArray.Contains("CUTLIST MATERIAL"))
            {
                int id = cutlistData.GetMaterialID(this.GetProperty("CUTLIST MATERIAL").Value);
                SwProperty p = new SwProperty("MATID", swCustomInfoType_e.swCustomInfoNumber, id.ToString(), false );
                this.Remove("CUTLIST MATERIAL");
                this._innerArray.Add(p);
            }

            if (this._innerArray.Contains("EDGE FRONT (L)"))
            {
                
            }
        }

        public void CreateDefaultDrawingSet()
        {
            this._innerArray.Add(new SwProperty("PartNo", swCustomInfoType_e.swCustomInfoNumber, "$PRP:\"SW-File Name\"", true));
            this._innerArray.Add(new SwProperty("CUSTOMER", swCustomInfoType_e.swCustomInfoNumber, string.Empty, true));
            this._innerArray.Add(new SwProperty("REVISION LEVEL", swCustomInfoType_e.swCustomInfoNumber, "100", true));
            this._innerArray.Add(new SwProperty("DrawnBy", swCustomInfoType_e.swCustomInfoNumber, string.Empty, true));
            this._innerArray.Add(new SwProperty("DATE", swCustomInfoType_e.swCustomInfoNumber, DateTime.Now.ToShortDateString(), true));

            this._innerArray.Add(new SwProperty("M1", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("FINISH 1", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("M2", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("FINISH 2", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("M3", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("FINISH 3", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("M4", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("FINISH 4", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("M5", swCustomInfoType_e.swCustomInfoText, string.Empty, true));
            this._innerArray.Add(new SwProperty("FINISH 5", swCustomInfoType_e.swCustomInfoText, string.Empty, true));

#if DEBUG
            string s = string.Empty;
#endif
            foreach (SwProperty p in this._innerArray)
            {
                p.Get(this.swApp);
#if DEBUG
                s += p.Name + ": " + p.ResValue + "\n";
#endif
            }
#if DEBUG
            System.Windows.Forms.MessageBox.Show(s);
#endif
        }

        public void GetPropertyData()
        {
            ModelDoc2 md = (ModelDoc2)swApp.ActiveDoc;
            CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);

            string[] ss = (string[])g.GetNames();
            if (ss != null)
            {
                foreach (string s in ss)
                {
                    SwProperty p = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
                    p.Get(this.swApp);

                    this._innerArray.Add(p);
                }
            }

            if ((swDocumentTypes_e)md.GetType() != swDocumentTypes_e.swDocDRAWING)
            {
                CustomPropertyManager c = md.Extension.get_CustomPropertyManager(md.ConfigurationManager.ActiveConfiguration.Name);
                ss = (string[])c.GetNames();
                foreach (string s in ss)
                {
                    SwProperty p = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, string.Empty, false);
                    p.Get(this.swApp);
                    this._innerArray.Add(p);
                }
            }
        }

        public void GetPropertyData(SldWorks sw)
        {
            this.swApp = sw;
            ModelDoc2 md = (ModelDoc2)swApp.ActiveDoc;
            CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);
            string valOut;
            string resValOut;
            bool wasResolved;
            int res;

            string[] ss = (string[])g.GetNames();
            foreach (string s in ss)
            {
                res = g.Get5(s, false, out valOut, out resValOut, out wasResolved);
                SwProperty p = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, valOut, true);
                p.ResValue = resValOut;
                p.Type = (swCustomInfoType_e)g.GetType2(s);

                if (p.Type == swCustomInfoType_e.swCustomInfoNumber && p.Name.ToUpper().Contains("OVER"))
                    p.Type = swCustomInfoType_e.swCustomInfoDouble;

                p.SwApp = this.swApp;
                this._innerArray.Add(p);
#if DEBUG
                System.Diagnostics.Debug.Print(s);
#endif
            }


            if ((swDocumentTypes_e)md.GetType() != swDocumentTypes_e.swDocDRAWING)
            {
                CustomPropertyManager c = md.Extension.get_CustomPropertyManager(md.ConfigurationManager.ActiveConfiguration.Name);
                ss = (string[])c.GetNames();
                foreach (string s in ss)
                {
                    res = c.Get5(s, false, out valOut, out resValOut, out wasResolved);
                    SwProperty p = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, valOut, false);
                    p.ResValue = resValOut;
                    p.Type = (swCustomInfoType_e)g.GetType2(s);
                    p.SwApp = this.swApp;
#if DEBUG
                    System.Diagnostics.Debug.Print(s);
#endif
                    this._innerArray.Add(p);
                }
            }
        }

        public bool SWContains(string property)
        {
            ModelDoc2 md = (ModelDoc2)swApp.ActiveDoc;
            Configuration conf = md.ConfigurationManager.ActiveConfiguration;
            CustomPropertyManager gp = md.Extension.get_CustomPropertyManager(string.Empty);
            CustomPropertyManager sp = md.Extension.get_CustomPropertyManager(conf.Name);
            string[] ss = (string[])gp.GetNames();

            foreach (string s in ss)
            {
                if (s.ToUpper() == property.ToUpper())
                {
                    return true;
                }
            }

            if ((swDocumentTypes_e)md.GetType() != swDocumentTypes_e.swDocDRAWING)
            {
                ss = (string[])sp.GetNames();

                foreach (string s in ss)
                {
                    if (s.ToUpper() == property.ToUpper())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual IEnumerator<SwProperty> GetEnumerator()
        {
            return (new List<SwProperty>(this).GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (new List<SwProperty>(this).GetEnumerator());
        }

        public void UpdateProperty(SwProperty property)
        {
            foreach (SwProperty p in this)
            {
                if (property.Name ==  p.Name)
                {
                    p.Ctl = property.Ctl;
                    p.Field = property.Field;
                    p.Global = property.Global;
                    p.ResValue = property.ResValue;
                    p.Table = property.Table;
                    p.Type = property.Type;
                    p.Value = property.Value;
                }
            }
        }

        public SwProperty GetProperty(string name)
        {
            foreach (SwProperty p in this._innerArray)
            {
                if (name.ToUpper() == p.Name.ToUpper())
                {
                    return p;
                }
            }

            return null;
        }

        public void ReadProperties()
        {
            foreach (SwProperty p in this._innerArray)
            {
                if (p.Ctl != null)
                {
#if DEBUG
                    System.Diagnostics.Debug.Print("To " + p.Ctl.Name + " <- " + p.ToString());
#endif
                    p.Ctl.Text = p.Value;
                }
            }
        }

        public void ReadControls()
        {
            foreach (SwProperty p in this._innerArray)
            {
                if (p.Ctl != null)
                {
                    p.Value = p.Ctl.Text;
                }
            }
        }

        public void Write()
        {
            foreach (SwProperty p in this._innerArray)
            {
                p.Write();
            }
        }

        public void Write(SldWorks sw)
        {
            foreach (SwProperty p in this._innerArray)
            {
                p.Write(sw);
            }
        }

        public override string ToString()
        {
            string ret = string.Empty;
            foreach (SwProperty p in this)
            {
                ret += string.Format("{0}: {1}\n", p.Name, p.Value);
            }
            return ret;
        }

        private int _clID;

        public int CutlistID
        {
            get { return _clID; }
            set { _clID = value; }
        }


        #region ICollection<SwProperty> Members

        public void Add(SwProperty item)
        {
            if (!this.Contains(item.Name))
            {
                this._innerArray.Add(item);   
            }
        }

        public void Clear()
        {
            this._innerArray.Clear();
        }

        public bool Contains(SwProperty item)
        {
            if (item == null)
                return false;

            string n = item.Name.ToUpper();
            foreach (SwProperty p in this._innerArray)
            {
                if (p.Name.ToUpper() == n)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Contains(string name)
        {
            string n = name.ToUpper();
            foreach (SwProperty p in this._innerArray)
            {
                if (p.Name.ToUpper() == n)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(SwProperty[] array, int arrayIndex)
        {
            this._innerArray.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._innerArray.Count; }
        }

        public bool IsReadOnly
        {
            get { return this._isReadOnly; }
        }

        public bool Remove(SwProperty item)
        {
            bool res = false;

            for (int i = 0; i < this._innerArray.Count; i++)
            {
                SwProperty obj = (SwProperty)this._innerArray[i];
                if (obj.Name == item.Name)
                {
                    this._innerArray.RemoveAt(i);
                    res = true;
                    break;
                }
            }

            return res;
        }

        public bool Remove(string name)
        {
            bool res = false;

            for (int i = 0; i < this._innerArray.Count; i++)
            {
                SwProperty obj = (SwProperty)this._innerArray[i];
                if (obj.Name == name)
                {
                    this._innerArray.RemoveAt(i);
                    res = true;
                    break;
                }
            }

            return res;
        }

        public virtual SwProperty this[int index]
        {
            get
            {
                return (SwProperty)_innerArray[index];
            }
            set
            {
                _innerArray[index] = value;
            }
        }

        private bool _isReadOnly;

        protected bool MyProperty
        {
            get { return _isReadOnly; }
            set { _isReadOnly = value; }
        }
	
        #endregion

        private CutlistData _cutlistData;

        public CutlistData cutlistData
        {
            get { return _cutlistData; }
            set { _cutlistData = value; }
        }
	
    }
}
