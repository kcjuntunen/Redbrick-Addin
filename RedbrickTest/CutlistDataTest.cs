using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redbrick_Addin;
using System.Data;
using System.Diagnostics;
namespace RedbrickTest {
    [TestClass]
    public class CutlistDataTest {

        [TestMethod]
        public void GetLegacyECODataTest() {
            CutlistData cd = new CutlistData();
            eco eco = cd.GetLegacyECOData("8042");
            Debug.Print(eco.RequestedBy);
            Debug.Assert(eco.RequestedBy == "S.PALMER");
        }

        [TestMethod]
        public void GetMaterialIDTest() {
            CutlistData cd = new CutlistData();
            int res = cd.GetMaterialID("SHT SST 430 #4 POLISH 24GA");
            Debug.Assert(res == 1311);
        }
        [TestMethod]
        public void GetMaterialByIDTest() {
            CutlistData cd = new CutlistData();
            string res = cd.GetMaterialByID("1311");
            Debug.Assert(res == "SHT SST 430 #4 POLISH 24GA");
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
    }
}
