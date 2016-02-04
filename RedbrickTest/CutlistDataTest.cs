using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redbrick_Addin;
using System.Data;
using System.Diagnostics;
namespace RedbrickTest {
  [TestClass]
  public class CutlistDataTest {
    //[TestMethod]
    //public void GetECODataTest() {
    //  eco eco = cd.GetECOData("8042");
    //  Debug.Assert(eco.RequestedBy == "S.PALMER");

    //  eco = cd.GetECOData("xxxx");
    //  Debug.Assert(eco.RequestedBy == string.Empty);

    //  eco = cd.GetECOData("9000");
    //  Debug.Assert(eco.Changes == "ANOTHER STERLING NITPICK.");
    //}

    [TestMethod]
    public void GetHashTest() {
      CutlistData cd = new CutlistData();
      int i = cd.GetHash("ZASC1505-02-06");
      Debug.Assert(i == 246268142);
    }

    [TestMethod]
    public void GetLegacyECODataTest() {
      CutlistData cd = new CutlistData();
      eco eco = cd.GetLegacyECOData("8807");
      Debug.Assert(eco.RequestedBy == "S.PALMER");

      eco = cd.GetLegacyECOData("xxxx");
      Debug.Assert(eco.RequestedBy == string.Empty);
    }

    [TestMethod]
    public void GetMaterialIDTest() {
      CutlistData cd = new CutlistData();
      int res = cd.GetMaterialID("SHT SST 430 #4 POLISH 24GA");
      Debug.Assert(res == 1311);

      res = cd.GetMaterialID("TBD MATERIAL");
      Debug.Assert(res == 3004);

      res = cd.GetMaterialID("ksladhfgakldfhasd");
      Debug.Assert(res == 3004);

      res = cd.GetMaterialID(string.Empty);
      Debug.Assert(res == 3004);
    }

    [TestMethod]
    public void GetMaterialByIDTest() {
      CutlistData cd = new CutlistData();
      string res = cd.GetMaterialByID("1311");
      Debug.Assert(res == "SHT SST 430 #4 POLISH 24GA");

      res = cd.GetMaterialByID("9000");
      Debug.Assert(res == "TBD MATERIAL");
    }

    [TestMethod]
    public void GetOpDataByNameTest() {
      CutlistData cd = new CutlistData();
      System.Collections.Generic.List<string> l = new System.Collections.Generic.List<string>();
      l = cd.GetOpDataByName("NOT");
      Debug.Assert(l[0] == "20");

      cd.OpType = 2;
      l = cd.GetOpDataByName("NOT");
      Debug.Assert(l[0] == "21");
    }

    [TestMethod]
    public void TestTypeEnum() {
      Debug.Print(string.Format("{0}\n{1}\n{3}\n{4}n{5}",
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoDate,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoDouble,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoNumber,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoUnknown,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoYesOrNo));
      System.Console.Write(string.Format("{0}\n{1}\n{3}\n{4}n{5}",
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoDate,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoDouble,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoNumber,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoText,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoUnknown,
        SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoYesOrNo));
      Debug.Assert(SolidWorks.Interop.swconst.swCustomInfoType_e.swCustomInfoDate == 0);
      System.Console.ReadKey();
    }
  }
}
