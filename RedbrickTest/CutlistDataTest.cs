using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redbrick_Addin;
using System.Data;
using System.Diagnostics;
namespace RedbrickTest {
  [TestClass]
  public class CutlistDataTest {
    CutlistData cd = new CutlistData();
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
      int i = cd.GetHash("ZASC1505-02-06");
      Debug.Assert(i == 246268142);
    }

    [TestMethod]
    public void GetLegacyECODataTest() {
      eco eco = cd.GetLegacyECOData("8807");
      Debug.Assert(eco.RequestedBy == "S.PALMER");

      eco = cd.GetLegacyECOData("xxxx");
      Debug.Assert(eco.RequestedBy == string.Empty);
    }

    [TestMethod]
    public void GetMaterialIDTest() {
      int res = cd.GetMaterialID("SHT SST 430 #4 POLISH 24GA");
      Debug.Assert(res == 1339);

      res = cd.GetMaterialID("TBD MATERIAL");
      Debug.Assert(res == 3030);

      res = cd.GetMaterialID("ksladhfgakldfhasd");
      Debug.Assert(res == 3030);

      res = cd.GetMaterialID(string.Empty);
      Debug.Assert(res == 3030);
    }

    [TestMethod]
    public void GetMaterialByIDTest() {
      string res = cd.GetMaterialByID("1339");
      Debug.Assert(res == "SHT SST 430 #4 POLISH 24GA");

      res = cd.GetMaterialByID("9000");
      Debug.Assert(res == "TBD MATERIAL");
    }

    [TestMethod]
    public void GetOpDataByNameTest() {
      System.Collections.Generic.List<string> l = new System.Collections.Generic.List<string>();
      l = cd.GetOpDataByName("NOT");
      Debug.Assert(l[0] == "20");

      cd.OpType = 2;
      l = cd.GetOpDataByName("NOT");
      Debug.Assert(l[0] == "21");
    }

    [TestMethod]
    public void TestGetLastLegacyECO() {
      int x = cd.GetLastLegacyECR();
      Debug.Assert(x >= 8845);
    }

    [TestMethod]
    public void TestGetDrawingID() {
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\KOHLS\KOFO\KOFO1536-02.PDF")) == 51560);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\TARGET\INSTALL\AX7505FWTI-INSTALL.PDF")) == 16344);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\TARGET\INSTALL\AX7505sdflkjgsdf.PDF")) == 0);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\KOHLS\KOCO\KOCO1211-03.pdf")) == 6834);


      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"KOFO1536-02.PDF")) == 51560);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"AX7505FWTI-INSTALL.PDF")) == 16344);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"AX7505sdflkjgsdf.PDF")) == 0);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"KOCO1211-03.pdf")) == 6834);
    }


  }
}
