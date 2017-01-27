using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Redbrick_Addin {
  class OpProperty : SwProperty {
    public override void Write(ModelDoc2 md) {
      if (md != null) {
        Configuration cf = md.ConfigurationManager.ActiveConfiguration;

        CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
        CustomPropertyManager scpm = md.Extension.get_CustomPropertyManager(string.Empty);

        if (SWCustPropMgr != null) {
          scpm = SWCustPropMgr;
        }

        // Null reference on drawings. Not good. Let's just make everything global if there's no config.
        if (cf != null)
          scpm = md.Extension.get_CustomPropertyManager(cf.Name);

        // Rather than changing values, we'll just completely overwrite them.
        swCustomPropertyAddOption_e ao = swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd;
        int res;
        string v = ID;
        res = gcpm.Add3(this.Name, (int)swCustomInfoType_e.swCustomInfoNumber, v, (int)ao);
      }
      //base.Write(md);
    }
    public override void Get(ModelDoc2 md, CutlistData cd) {
      Configuration cf = md.ConfigurationManager.ActiveConfiguration;

      CustomPropertyManager gcpm = md.Extension.get_CustomPropertyManager(string.Empty);
      CustomPropertyManager scpm;

      bool wasResolved;
      bool useCached = false;
      string tempval = string.Empty;
      string tempresval = string.Empty;

      if (cf != null) {
        scpm = md.Extension.get_CustomPropertyManager(cf.Name);
      } else {
        scpm = md.Extension.get_CustomPropertyManager(string.Empty);
      }

      if (SWCustPropMgr != null) {
        scpm = SWCustPropMgr;
      }

      int res;

      res = scpm.Get5(Name, useCached, out tempval, out tempresval, out wasResolved);

      int tp = 0;
      if (int.TryParse(Value, out tp)) {
        ID = Value;
        Descr = cd.GetOpAbbreviationByID(Value);
      } else {
        ID = cd.GetOpIDByName(Value).ToString();
        Descr = Value;
      }
      //base.Get(md, cd);
    }
  }
}
