using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

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
            string inputFile = args[0];
            string outputFile = args[1];
            System.Diagnostics.FileVersionInfo fileInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(inputFile);
            using (TextWriter writer = new StreamWriter(outputFile, false, Encoding.Default)) {
                writer.WriteLine("!define VERSION \"" + fileInfo.FileVersion + "\"");
                writer.WriteLine("!define DESCRIPTION \"" + fileInfo.FileDescription + "\"");
                writer.WriteLine("!define COPYRIGHT \"" + fileInfo.LegalCopyright + "\"");
                writer.Close();
            }

            string xmlFile = "version.xml";
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.CloseOutput = true;
            xws.ConformanceLevel = ConformanceLevel.Document;
            xws.Indent = true;
            xws.WriteEndDocumentOnClose = true;
            using (XmlWriter writer = XmlWriter.Create(xmlFile)) {
              writer.WriteStartDocument();
              writer.WriteStartElement("RedBrick");

              writer.WriteStartElement("version");
              writer.WriteString(fileInfo.FileVersion);
              writer.WriteEndElement();

              writer.WriteStartElement("url");
              writer.WriteString(@"file://\\AMSTORE-SVR-02\shared\shared\general\RedBrick\InstallRedBrick.exe");
              writer.WriteEndElement();

              writer.WriteEndElement();
              writer.WriteEndDocument();
              writer.Close();
            }
        } catch (Exception e) {
            Console.WriteLine(e.Message + "\n\n");
            Console.WriteLine("Usage: GetAssemblyInfoForNSIS.exe MyApp.exe MyAppVersionInfo.nsh\n");
        }
      }
  }
}

