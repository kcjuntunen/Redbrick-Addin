using System;
using System.Collections.Generic;
using System.Text;

namespace GetAssemblyInfoForNSIS {
  class Program {
    /// <summary>
    /// This program is used at compile-time by the NSIS Install Scripts.
    /// It copies the file properties of an assembly and writes that info a
    /// header file that the scripts use to make the installer match the program
    /// 
    /// I got it from <http://stackoverflow.com/questions/3039024/nsis-put-exe-version-into-name-of-installer#3040323>
    /// </summary>
    static void Main(string[] args) {
      try {
            String inputFile = args[0];
            String outputFile = args[1];
            System.Diagnostics.FileVersionInfo fileInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(inputFile);
            using (System.IO.TextWriter writer = new System.IO.StreamWriter(outputFile, false, Encoding.Default)) {
                writer.WriteLine("!define VERSION \"" + fileInfo.FileVersion + "\"");
                writer.WriteLine("!define DESCRIPTION \"" + fileInfo.FileDescription + "\"");
                writer.WriteLine("!define COPYRIGHT \"" + fileInfo.LegalCopyright + "\"");
                writer.Close();
            }
        } catch (Exception e) {
            Console.WriteLine(e.Message + "\n\n");
            Console.WriteLine("Usage: GetAssemblyInfoForNSIS.exe MyApp.exe MyAppVersionInfo.nsh\n");
        }
      }
    }
  }

