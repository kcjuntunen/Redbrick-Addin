using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redbrick_Addin;
using System.Data;
using System.Diagnostics;

namespace RedbrickTest {
  [TestClass]
  public class CutlistDataTest {
    CutlistData cd = new CutlistData();
    [TestMethod]
    public void GetECODataTest() {
      eco eco = cd.GetECOData("8042");
      Debug.Assert(eco.RequestedBy == "S.PALMER");

      eco = cd.GetECOData("xxxx");
      Debug.Assert(eco.RequestedBy == string.Empty);

      eco = cd.GetECOData("9000");
      Debug.Assert(eco.Changes == "ADD EDGE BANDING TO BACK AND BOTTOM OF FIN (GE-35.21FINW.L & .R)");
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
      Debug.Assert(res == 1342);

      res = cd.GetMaterialID("TBD MATERIAL");
      Debug.Assert(res == 3042);

      res = cd.GetMaterialID("ksladhfgakldfhasd");
      Debug.Assert(res == 3042);

      res = cd.GetMaterialID(string.Empty);
      Debug.Assert(res == 3042);
    }

    [TestMethod]
    public void GetMaterialByIDTest() {
      string res = cd.GetMaterialByID("1342");
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
    public void GetLastLegacyECO() {
      int x = cd.GetLastLegacyECR();
      Debug.Assert(x >= 8845);
    }

    [TestMethod]
    public void GetDrawingID() {
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\KOHLS\KOFO\KOFO1536-02.PDF")) == 13320);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\TARGET\INSTALL\AX7505FWTI-INSTALL.PDF")) == 28634);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\TARGET\INSTALL\AX7505sdflkjgsdf.PDF")) == 0);
      Debug.Assert(cd.GetDrawingID(new System.IO.FileInfo(@"K:\KOHLS\KOCO\KOCO1211-03.pdf")) == 11568);


      Debug.Assert(cd.GetDrawingID(@"KOFO1536-02.PDF") == 13320);
      Debug.Assert(cd.GetDrawingID(@"AX7505FWTI-INSTALL.PDF") == 28634);
      Debug.Assert(cd.GetDrawingID(@"AX7505sdflkjgsdf.PDF") == 0);
      Debug.Assert(cd.GetDrawingID(@"KOCO1211-03.pdf") == 11568);
    }

    [TestMethod]
    public void GetDrawingData() {
      Debug.Assert((int)cd.GetDrawingData(@"KOFO1536-02.PDF")[0] == 13320);
      Debug.Assert(cd.GetDrawingData(@"AX7505FWTI-INSTALL.PDF")[2].ToString() == @"K:\TARGET\INSTALL\");
      Debug.Assert((int)cd.GetDrawingData(@"AX7505sdflkjgsdf.PDF")[0] == 0);
      Debug.Assert((int)cd.GetDrawingData(@"KOCO1211-03.pdf")[0] == 11568);
    }

    [TestMethod]
    public void ECRIsBogus() {
      Debug.Assert(cd.ECRIsBogus("9000") == false);
      Debug.Assert(cd.ECRIsBogus("1560") == true);
      Debug.Assert(cd.ECRIsBogus("0") == true);

      Debug.Assert(cd.ECRIsBogus(8996) == false);
      Debug.Assert(cd.ECRIsBogus(8995) == true);
      Debug.Assert(cd.ECRIsBogus(9000) == false);
      Debug.Assert(cd.ECRIsBogus(8830) == true);
    }

    //[TestMethod]
    //public void GetMaterials() {
      //DataTable d = cd.Materials.Tables[0];
      //System.Data.DataSet ds = cd.Materials;
      //System.Data.DataTable dt = ds.Tables[0];

      //string testString = dt.Rows[1482].ItemArray.GetValue(1).ToString();
      //Debug.WriteLine(testString);
      //Debug.Assert(testString == "SLB FM 5488-58 G2 M 0.750");

      //testString = dt.Rows[1517].ItemArray.GetValue(2).ToString();
      //Debug.WriteLine(testString);
      //Debug.Assert(testString == "SPECTRUM RED/FROSTY WHITE");
    //}

    [TestMethod]
    public void EcrItemExists() {
      if (cd != null) {
        Debug.Assert(cd.ECRItemExists(9000, "GE-35.21FINW.L", "100"));
        Debug.Assert(cd.ECRItemExists(9000, "GE-35.21FINW.R", "100"));
        Debug.Assert(cd.ECRItemExists(9000, "Z96895", "100"));
        Debug.Assert(cd.ECRItemExists(9308, "SC-190SBP", "100"));
        Debug.Assert(cd.ECRItemExists(8850, "SS1009-Z3", "100") == false);
        Debug.Assert(cd.ECRItemExists(8850, "WGFX1532-02", "101") == false);
        Debug.Assert(cd.ECRItemExists(8850, "WGFX1532-03", "101") == false);
        Debug.Assert(cd.ECRItemExists(7000, "WGFX1532-02", "101") == false);
        Debug.Assert(cd.ECRItemExists(8848, "WGFX1532-03", "101") == false);
      }
    }

    [TestMethod]
    public void TestGetCurrentAuthor() {
      int u = cd.GetCurrentAuthor();
      Debug.Assert(u == 27);
    }

    [TestMethod]
    public void TestGetCustomersForDrawing() {
      System.Collections.Generic.List<string> u = cd.GetCustomersForDrawing();

    }

    [TestMethod]
    public void TestGetOdometerTotalValue() {
      int x = cd.GetOdometerTotalValue((int)CutlistData.Functions.GreenCheck);
      Debug.Assert(x > 0);
    }

  }
}
