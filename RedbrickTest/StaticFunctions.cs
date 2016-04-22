using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redbrick_Addin;

namespace RedbrickTest {
  [TestClass]
  public class StaticFunctions {
    [TestMethod]
    public void TestClip() {
      Redbrick.Clip("hey");
    }

    [TestMethod]
    public void TestGetHash() {
      int x = Redbrick.GetHash(@"C:\");
      Assert.IsTrue(-890689235 == x);
    }

    [TestMethod]
    public void TestFilterString() {
      string test = ";test%*" + '\u0022' + '\u0027';
      string target1 = '\u037E' + "TEST" + '\u066A' + '\u2217' + '\u2033' + '\u2032';
      string target2 = '\u037E' + "test" + '\u066A' + '\u2217' + '\u2033' + '\u2032';
      string x = CutlistData.FilterString(test);

      Assert.IsTrue(target1 == x);
      x = CutlistData.FilterString(test, false);
      Assert.IsTrue(target2 == x);
      x = CutlistData.FilterString(test, true);
      Assert.IsTrue(target1 == x);
    }


  }
}
