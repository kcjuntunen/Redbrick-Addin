using System;
using Redbrick_Addin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedbrickTest {
  [TestClass]
  public class M2MDataTest {
    M2MData m2m = new M2MData();
    [TestMethod]
    public void M2MGetPartCount() {
      System.Diagnostics.Debug.Assert(m2m.GetPartCount("Z75794", "100") > 0);
      System.Diagnostics.Debug.Assert(m2m.GetPartCount("083511", "100") > 0);
      System.Diagnostics.Debug.Assert(m2m.GetPartCount("TAFX1423-06", "100") == 1);
      System.Diagnostics.Debug.Assert(m2m.GetPartCount("BBBBB", "100") == 0);
    }

    [TestMethod]
    public void M2MGetPurchased() {
      System.Diagnostics.Debug.Assert(m2m.GetPurchased("083511", "100") == true);
      System.Diagnostics.Debug.Assert(m2m.GetPurchased("TAFX1423-06", "100") == false);
      System.Diagnostics.Debug.Assert(m2m.GetPurchased("Z75794", "100") == false);
      System.Diagnostics.Debug.Assert(m2m.GetPurchased("BBBBB", "100") == false);
    }

    [TestMethod]
    public void M2MGetProductCl() {
      System.Diagnostics.Debug.Assert(m2m.GetProductClass("Z75794", "100") == "02");
      System.Diagnostics.Debug.Assert(m2m.GetProductClass("083511", "100") == "09");
      System.Diagnostics.Debug.Assert(m2m.GetProductClass("TAFX1423-06", "100") == "09");
      System.Diagnostics.Debug.Assert(m2m.GetProductClass("BBBBB", "100") == string.Empty);
    }

    [TestMethod]
    public void M2MGetPartType() {
      System.Diagnostics.Debug.Assert(m2m.GetPartType("Z75794", "100") == 3);
      System.Diagnostics.Debug.Assert(m2m.GetPartType("083511", "100") == 2);
      System.Diagnostics.Debug.Assert(m2m.GetPartType("TAFX1423-06", "100") == 1);
      System.Diagnostics.Debug.Assert(m2m.GetPartType("BBBBB", "100") == 7);
    }
  }
}
