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
            Debug.Assert(eco.Changes == "ANOTHER STERLING NITPICK.");
        }

        [TestMethod]
        public void GetLegacyECODataTest() {
            eco eco = cd.GetLegacyECOData("8042");
            Debug.Assert(eco.RequestedBy == "S.PALMER");

            eco = cd.GetLegacyECOData("xxxx");
            Debug.Assert(eco.RequestedBy == string.Empty);
        }

        [TestMethod]
        public void GetMaterialIDTest() {
            int res = cd.GetMaterialID("SHT SST 430 #4 POLISH 24GA");
            Debug.Assert(res == 1311);

            res = cd.GetMaterialID("TBD MATERIAL");
            Debug.Assert(res == 2929);

            res = cd.GetMaterialID("ksladhfgakldfhasd");
            Debug.Assert(res == 2929);

            res = cd.GetMaterialID(string.Empty);
            Debug.Assert(res == 2929);
        }

        [TestMethod]
        public void GetMaterialByIDTest() {
            string res = cd.GetMaterialByID("1311");
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
    }
}
