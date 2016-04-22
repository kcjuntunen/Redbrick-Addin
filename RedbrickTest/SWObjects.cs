using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using Redbrick_Addin;

namespace RedbrickTest {
  [TestClass]
  public class SWObjects {
    [TestMethod]
    public void TestGetProps() {
      //SwProperties s = new SwProperties(new SldWorks());
      //s.Add(new SwProperty(new CustomPropertyManager(), "stuff", swCustomInfoType_e.swCustomInfoText, 
      //  "other stuff", true));
      //s.Add(new SwProperty(new CustomPropertyManager(), "thing", swCustomInfoType_e.swCustomInfoText, 
      //  "other thing", false));

      //Assert.AreEqual("other stuff", s.GetProperty("stuff").Value);
      //Assert.AreEqual("other thing", s.GetProperty("thing").Value);
      //Assert.IsTrue(s.Contains("stuff"));
      //Assert.IsFalse(s.Contains("other"));
    }
  }
}
