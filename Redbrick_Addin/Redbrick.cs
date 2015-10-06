using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;

using System.Runtime.InteropServices;

namespace Redbrick_Addin
{
    public class Redbrick : ISwAddin
    {
        public SldWorks swApp;
        private int cookie;
        private TaskpaneView taskpaneView;
        private SWTaskPaneHost taskpaneHost;

        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            swApp = (SldWorks)ThisSW;
            cookie = Cookie;

            bool res = swApp.SetAddinCallbackInfo(0, this, cookie);
            this.UISetup();

            return true;
        }

        public bool DisconnectFromSW()
        {
            this.UITearDown();
            return true;
        }

        private void UISetup()
        {
            try
            {
                taskpaneView = swApp.CreateTaskpaneView2(Properties.Settings.Default.NetPath + 
                    Properties.Settings.Default.Icon
                    , Properties.Resources.Title);

                taskpaneHost = (SWTaskPaneHost)taskpaneView.AddControl(SWTaskPaneHost.SWTASKPANE_PROGID, string.Empty);
                taskpaneHost.OnRequestSW += new Func<SldWorks>(delegate { return this.swApp; });

                bool result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Ok, "OK");
                //result = taskpaneView.AddStandardButton((int)swTaskPaneBitmapsOptions_e.swTaskPaneBitmapsOptions_Close, "Close");

                taskpaneView.TaskPaneToolbarButtonClicked += taskpaneView_TaskPaneToolbarButtonClicked;

                taskpaneHost.Start();
            }
            catch (Exception e)
            {
                RedbrickErr.ErrMsg em = new RedbrickErr.ErrMsg(e);
                em.ShowDialog();
            }
        }

        int taskpaneView_TaskPaneToolbarButtonClicked(int ButtonIndex)
        {
            switch (ButtonIndex)
            {
                case 0:
                    this.taskpaneHost.Write();
                    break;
                case 1:
                    this.DisconnectFromSW();
                    break;
                default:
                    break;
            }
            return 1;
        }
        
        private void UITearDown()
        {
            taskpaneHost = null;
            taskpaneView.DeleteView();
            Marshal.ReleaseComObject(taskpaneView);
            taskpaneView = null;
        }

        [ComRegisterFunction()]
        private static void ComRegister(Type t)
        {
            string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);

            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath))
            {
                rk.SetValue(null, 1); // Load at startup
                rk.SetValue("Title", "Redbrick");
                rk.SetValue("Description", "Change properties the Amstore way.");
            }
        }

        [ComUnregisterFunction()]
        private static void ComUnregister(Type t)
        {
            string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);
        }
    }
}
