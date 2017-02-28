using System;
using System.Collections.Generic;
using System.Text;

namespace Redbrick_Addin {
  class OpMethodHandler {
    ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter ccpta =
      new ENGINEERINGDataSetTableAdapters.CUT_CUTLIST_PARTSTableAdapter();

    ENGINEERINGDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter cctta =
      new ENGINEERINGDataSetTableAdapters.CUT_CUTLISTS_TIMETableAdapter();

    ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter cpoa =
      new ENGINEERINGDataSetTableAdapters.CUT_PART_OPSTableAdapter();

    ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter fcota =
      new ENGINEERINGDataSetTableAdapters.FRIENDLY_CUT_OPSTableAdapter();

    private ENGINEERINGDataSet.CUT_PART_OPSRow cUT_PART_OPSRow;
    private ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow fRIENDLY_CUT_OPSRow;
    private int partID = 0;
    private int opID = 0;
    private int order = 0;
    private double setupTime = 0.0F;
    private double runTime = 0.0F;
    private double opSetupTime = 0.0F;
    private double opRunTime = 0.0F;
    private int method = 0;

    private enum MaterialType {
      PocketBoringMachine,
      PanelSaw,
      EdgeBander
    }

    public OpMethodHandler(int partid, int op, int idx, double opsetup, double oprun) {
      partID = partid;
      opID = op;
      order = idx;
      opSetupTime = opsetup;
      opRunTime = oprun;
      cUT_PART_OPSRow = (ENGINEERINGDataSet.CUT_PART_OPSRow)cpoa.GetDataByPOPID(opID)[0];
      fRIENDLY_CUT_OPSRow = (ENGINEERINGDataSet.FRIENDLY_CUT_OPSRow)fcota.GetDataByOpID(opID).Rows[0];
      setupTime = (double)fRIENDLY_CUT_OPSRow[@"OPSETUP"];
      runTime = (double)fRIENDLY_CUT_OPSRow[@"OPRUN"];
      method = (int)fRIENDLY_CUT_OPSRow[@"OPMETHOD"];
    }

    public void PartOpAdd() {
      UpdateCutlistTime();
      cpoa.Insert(partID, order, opID, opSetupTime, opRunTime);
    }

    public void UpdateCutlistTime() {
      switch (method) {
        case 1:
          break;
        case 2:
          updPerCL();
          break;
        case 3:
          updPerMat(MaterialType.PanelSaw);
          break;
        case 4:
          updPerMat(MaterialType.EdgeBander);
          break;
        default:
          break;
      }
    }

    private void updPerCL() {
      int clid = 0;
      foreach (ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow row in ccpta.GetDataByPartID(partID)) {
        clid = (int)row[@"CLID"];
        updateCutlistTime(clid, countPartOP(clid) == 0);
      }
    }

    private void updateCutlistTime(int clid, bool countIsZero) {
      if (countIsZero) {
        cctta.DeleteQuery(clid, true, opID);
      } else {
        if (cctta.CountOfCTID(true, clid, opID) != 0) {
          cctta.UpdateQuery(clid, true, opID, setupTime, 0, string.Empty);
        } else {
          cctta.Insert(clid, true, opID, setupTime, 0, string.Empty);
        }
      }
    }

    private void updPerMat(MaterialType type) {
      int clid = 0;
      foreach (ENGINEERINGDataSet.CUT_CUTLIST_PARTSRow row in ccpta.GetDataByPartID((int)cUT_PART_OPSRow[@"POPPART"])) {
        clid = (int)row[@"CLID"];
        switch (type) {
          case MaterialType.PanelSaw:
            setupTime = setupTime * CountFlat(opID, clid);
            break;
          case MaterialType.EdgeBander:
            setupTime = setupTime * CountEdge(opID, clid);
            break;
          default:
            break;
        }
        updateCutlistTime(clid, setupTime == 0);
      }
    }

    private double CountFlat(int popop, int clid) {
      int count = 0;
      foreach (var item in ccpta.GetUniqueFlatMaterials(popop, clid)) {
        count += 1;
      }
      return count;
    }

    private double CountEdge(int popop, int clid) {
      List<object> edges = new List<object>();
      foreach (var item in ccpta.GetUniqueFrontEdge(popop, clid)) {
        if (!edges.Contains(item)) {
          edges.Add(item);
        }
      }

      foreach (var item in ccpta.GetUniqueBackEdge(popop, clid)) {
        if (!edges.Contains(item)) {
          edges.Add(item);
        }
      }

      foreach (var item in ccpta.GetUniqueLeftEdge(popop, clid)) {
        if (!edges.Contains(item)) {
          edges.Add(item);
        }
      }

      foreach (var item in ccpta.GetUniqueRightEdge(popop, clid)) {
        if (!edges.Contains(item)) {
          edges.Add(item);
        }
      }
      return edges.Count;
    }

    private int countPartOP(int clid) {
      return (int)ccpta.OpsPerCutlist(clid, opID);
    }

  }
}
