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

        public ModelDoc2 modeldoc { get; set; }
        public string configName { get; set; }
        public string PartName { get; set; }

        public SwProperties(SldWorks sw)
        {
            this.swApp = sw;
            this.cutlistData = new CutlistData();
            this._innerArray = new ArrayList();
        }

        /// <summary>
        /// Empty metadata can create a problem.
        /// </summary>
        public void CreateDefaultPartSet()
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
        }

        /// <summary>
        /// Empty metadata can create a problem.
        /// </summary>
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
        }

        /// <summary>
        /// This sucks in all the metadata from a SW doc.
        /// </summary>
        /// <param name="md">A ModelDoc2 object.</param>
        public void GetPropertyData(ModelDoc2 md)
        {
            this.modeldoc = md;
            if (md != null)
            {
                swDocumentTypes_e docType = (swDocumentTypes_e)md.GetType();
                // Drawings only have global props.
                if (docType == swDocumentTypes_e.swDocDRAWING)
                {
                    CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);
                    this.ParsePropertyData(g, docType);
                }
                else
                {
                    // Getting global and local props.
                    CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);
                    Configuration c = (Configuration)md.ConfigurationManager.ActiveConfiguration;
                    CustomPropertyManager s = md.Extension.get_CustomPropertyManager(c.Name);
                    
                    this.cutlistData.OpType = ParseDept(md);
                    this.configName = c.Name;
                    this.ParsePropertyData(g, docType);
                    this.ParsePropertyData(s, docType);
                }
            }
        }

        /// <summary>
        /// Gotta know the dept to know how to populate OPS and turn on/off the right fields.
        /// </summary>
        /// <param name="md">A ModelDoc2 object.</param>
        /// <returns>Returns the ID of an OpType.</returns>
        public int ParseDept(ModelDoc2 md)
        {
            CustomPropertyManager g = md.Extension.get_CustomPropertyManager(string.Empty);
            Configuration c = (Configuration)md.ConfigurationManager.ActiveConfiguration;
            CustomPropertyManager s = md.Extension.get_CustomPropertyManager(c.Name);

            string[] sa = g.GetNames();
            List<string> ss = new List<string>();

            // Noooo! Null stuff is trouble.
            if (sa != null)
                ss.AddRange(sa);

            int res;
            bool useCached = false;
            string val = "WOOD";                // If we can't find the prop we want, let's just presume a wood part.
            string resVal = "WOOD";
            bool wRes = false;

            if (ss.Contains("DEPARTMENT"))
            {
                res = g.Get5("DEPARTMENT", useCached, out val, out resVal, out wRes);
            }
            else // If it wasn't where it was supposed to be, let's look here. Maybe, in the future, we could traverse configs for it too.
            {
                sa = s.GetNames();
                
                if (sa != null)
                    ss.AddRange(sa);

                if (ss.Contains("DEPARTMENT"))
                {
                    res = s.Get5("DEPARTMENT", useCached, out val, out resVal, out wRes);
                }
            }

            int opt = this.cutlistData.GetOpTypeIDByName(resVal.ToUpper());
            this.GetProperty("DEPARTMENT").Value = opt.ToString();
            return opt;
        }
        
        /// <summary>
        /// This takes the property data and sets it up for the controls to read properly. This gets the descrs from the cutlist,
        ///  or, if we have descrs, it gets IDs instead.
        /// </summary>
        /// <param name="g">A CustomPropertyManager.</param>
        /// <param name="dt">Drawing or Model?</param>
        public void ParsePropertyData(CustomPropertyManager g, swDocumentTypes_e dt)
        {
            string valOut = string.Empty;
            string resValOut = string.Empty;
            bool wasResolved = false;
            int res = 0;
            string[] ss = g.GetNames();

            if (ss != null)
            {
                foreach (string s in ss)
                {
                    res = g.Get5(s, false, out valOut, out resValOut, out wasResolved);
                    SwProperty p = new SwProperty(s, swCustomInfoType_e.swCustomInfoText, valOut, true);
                    p.ResValue = resValOut;
                    p.Type = (swCustomInfoType_e)g.GetType2(s);

                    if (p.Name.ToUpper().StartsWith("OVER"))
                    {
                        p.Global = true;
                        p.Type = swCustomInfoType_e.swCustomInfoDouble;                         // Make sure OVERs are doubles.
                    }

                    if (p.Name.ToUpper().StartsWith("CUTLIST"))                                 // Cutlist material.
                    {
                        p.Global = false;
                        p.Type = swCustomInfoType_e.swCustomInfoNumber;
                        p.Table = "CLPARTID";
                        p.Field = "MATID";
                        int tp = 0;
                        if (int.TryParse(p.Value, out tp))                                      // Is it numerical? Then it's an ID.
                        {
                            if (tp > 0)
                            {
                                p.ResValue = this.cutlistData.GetMaterialByID(p.Value);
                            }
                            else                                                                // No mat.
                            {
                                p.ResValue = "2929";                                            // "TBD MATERIAL"
                            }
                        }
                        else                                                               
                        {
                            p.Value = this.cutlistData.GetMaterialID(p.ResValue).ToString();    // Not numerical; must have a descr. Let's get an ID.
                        }
                    }

                    if (p.Name.ToUpper().StartsWith("EDGE"))                                    // Cutlist edge.
                    {
                        p.Global = false;
                        p.Type = swCustomInfoType_e.swCustomInfoNumber;
                        p.Table = "CLPARTID";
                        p.Field = "EDGEID_{0}";
                        int tp = 0;
                        if (int.TryParse(p.Value, out tp))                                      // Is it numerical? We have an ID.
                        {
                            if (tp > 0)
                            {
                                p.ResValue = this.cutlistData.GetEdgeByID(p.Value);
                            }
                            else                                                                // No edge.
                            {
                                p.ResValue = string.Empty;                                      // It's OK for edging to not exist.
                            }
                        }
                        else
                        {
                            p.Value = this.cutlistData.GetEdgeID(p.ResValue).ToString();        // When life gives you descrs, make IDs.
                        }
                    }

                    if (p.Name.ToUpper().Contains("OP"))                                        // Handle ops. Good thing we've got the dept figured out!
                    {
                        p.Global = true;
                        p.Type = swCustomInfoType_e.swCustomInfoNumber;
                        p.Table = "CUT_PARTS";
                        p.Field = "OP{0}ID";
                        int tp = 0;
                        if (int.TryParse(p.Value, out tp))                                      // Are we numerical?
                        {
                            if (tp > 0)
                            {
                                p.ResValue = this.cutlistData.GetOpByID(p.Value);
                            }
                            else                                                                // Nothing in there.
                            {
                                p.ResValue = string.Empty;                                      // It's ok to nop.
                            }
                        }
                        else
                        {
                            if (p.ResValue.Length < 4 && p.ResValue != string.Empty)                        // When it's not numerical, it's probably
                            {                                                                               // abbreviated.
                                List<string> dr = this.cutlistData.GetOpDataByName(p.ResValue.ToString());  // Getting datarows leads to trouble here
                                p.Value = dr[0];                                                            // if they're empty.
                                p.ResValue = dr[2];
                            }
                            else
                            {
                                p.Value = "0";
                                p.ResValue = string.Empty;
                            }
                        }
                    }

                    p.SwApp = this.swApp;
                    if (!this.Contains(p))                                                                  // Let's add it if we don't already have it.
                    {
                        this._innerArray.Add(p);   
                    }
#if DEBUG   
                    System.Diagnostics.Debug.Print(s);
#endif
                }
            }

            if (dt == swDocumentTypes_e.swDocDRAWING)                                                           // Well, now, if it's a drawing...
            {
                ss = (string[])g.GetNames();
                if (ss != null)
                {
                    foreach (string s in ss)
                    {
                        res = g.Get5(s, false, out valOut, out resValOut, out wasResolved);                     // We can just pull data. Nice!
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
                else
                {
                    this.CreateDefaultDrawingSet();                                                             // Nothing in there. Let's make stuff up.
                }
            }
        }

        public void LinkControlToProperty(string property, bool global, System.Windows.Forms.Control c)
        {
            SwProperty p = this.GetProperty(property);
            if (this.Contains(p))
            {
#if DEBUG
                System.Diagnostics.Debug.Print(p.ToString());
#endif
                p.SwApp = this.swApp;
                if (p.Name.ToUpper() == "LENGTH" || p.Name.ToUpper() == "WIDTH"
                    || p.Name.ToUpper() == "THICKNESS" || p.Name.ToUpper() == "WALL THICKNESS")  // Only these fields get resolved to something.
                {
                    c.Text = p.Value;
                }
                else                                                                            // Otherwise I'm using ResValue to carry cutlist descrs.
                {
                    c.Text = p.ResValue;
                }
                p.Ctl = c;
            }
            else
            {
                SwProperty x = new SwProperty(property, swCustomInfoType_e.swCustomInfoText, string.Empty, global);
                x.SwApp = this.swApp;
                x.Ctl = c;
            }
        }

        /// <summary>
        /// Clear out everything the doc has on the config specific side.
        /// </summary>
        /// <param name="md">A ModelDoc2 object.</param>
        public void DelSpecific(ModelDoc2 md)
        {
            Configuration conf = md.ConfigurationManager.ActiveConfiguration;
            if (conf != null)
            {
                CustomPropertyManager sp = md.Extension.get_CustomPropertyManager(conf.Name);
                string[] ss = (string[])sp.GetNames();

                foreach (string s in ss)
                {
                    sp.Delete2(s);
                }   
            }
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
            SwProperty q = new SwProperty(name, swCustomInfoType_e.swCustomInfoText, string.Empty, true);
            return q;
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

        public void Write(ModelDoc2 md)
        {
            this.DelSpecific(md);
            foreach (SwProperty p in this._innerArray)
            {
                p.Del(md);
                p.Write(md);
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
