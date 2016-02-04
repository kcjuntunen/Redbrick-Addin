using System;
using Redbrick_Addin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedbrickTest {
  [TestClass]
  public class UnitTest1 {
    [TestMethod]
    public void M2MGetPartCount() {
      M2MData m2m = new Redbrick_Addin.M2MData();
      System.Diagnostics.Debug.Assert(m2m.GetPartCount("TAFX1423-06", "100") == 1);
      System.Diagnostics.Debug.Assert(m2m.GetPartCount("BBBBB", "100") == 0);
    }

    [TestMethod]
    public void M2MGetPurchased() {
      M2MData m2m = new Redbrick_Addin.M2MData();
      System.Diagnostics.Debug.Assert(m2m.GetPurchased("083551", "100") == true);
      System.Diagnostics.Debug.Assert(m2m.GetPurchased("TAFX1423-06", "100") == false);
    }

    [TestMethod]
    public void M2MGetPartType() {
      M2MData m2m = new Redbrick_Addin.M2MData();
      System.Diagnostics.Debug.Assert(m2m.GetPartType("083551", "100") == 2);
      System.Diagnostics.Debug.Assert(m2m.GetPartType("TAFX1423-06", "100") == 1);
    }
  }
}
