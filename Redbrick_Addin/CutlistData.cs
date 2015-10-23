using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Redbrick_Addin {
  public class CutlistData : IDisposable {
    private bool ENABLE_DB_WRITE = Properties.Settings.Default.EnableDBWrite;
    private object threadLock = new object();
    private OdbcConnection conn;

    public OdbcConnection Connection {
      get { return conn; }
      private set { conn = value; }
    }

    public CutlistData() {
      conn = new OdbcConnection(Properties.Settings.Default.ConnectionString);
      OpType = 1;
      conn.Open();
    }

    public void Dispose() {
      conn.Close();
    }

    private DataSet GetMaterials() {
      if (_materials == null) {
        lock (threadLock) {
#if DEBUG
          DateTime start;
          DateTime end;
          start = DateTime.Now;
#endif
          string SQL = "SELECT MATID,DESCR,COLOR FROM CUT_MATERIALS ORDER BY DESCR;";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
              using (DataSet ds = new DataSet()) {
                da.Fill(ds);
#if DEBUG
                end = DateTime.Now;
                System.Diagnostics.Debug.Print("*** MAT ***<<< " + (end - start).ToString() + " >>>");
#endif
                return ds;
              }
            }
          }
        }
      } else {
        return _materials;
      }
    }

    private DataSet GetEdges() {
      lock (threadLock) {
#if DEBUG
        DateTime start;
        DateTime end;
        start = DateTime.Now;
#endif
        string SQL = "SELECT EDGEID,DESCR,COLOR,THICKNESS FROM CUT_EDGES ORDER BY DESCR;";

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
            using (DataSet ds = new DataSet()) {
              da.Fill(ds);

              DataRow dar = ds.Tables[0].NewRow();
              dar[0] = 0;
              dar[1] = string.Empty;
              dar[2] = "None";
              dar[3] = 0.0;

              ds.Tables[0].Rows.Add(dar); ;
#if DEBUG
              end = DateTime.Now;
              System.Diagnostics.Debug.Print("*** EDG ***<<< " + (end - start).ToString() + " >>>");
#endif
              return ds;
            }
          }
        }
      }
    }

    private DataSet GetOps(int OpType) {
      lock (threadLock) {
        if (this.OpType != OpType) {
          this.OpType = OpType;
          string SQL = string.Format("SELECT OPID, OPNAME, OPDESCR, OPTYPE FROM CUT_PART_TYPES "
              + "INNER JOIN CUT_OPS ON CUT_PART_TYPES.TYPEID = CUT_OPS.OPTYPE WHERE CUT_PART_TYPES.TYPEID = {0} ORDER BY OPDESCR", OpType);
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
              using (DataSet ds = new DataSet()) {
                da.Fill(ds);
                //conn.Close();
                DataRow dar = ds.Tables[0].NewRow();
                dar[0] = 0;
                dar[1] = string.Empty;
                dar[2] = string.Empty;
                ds.Tables[0].Rows.Add(dar);
                this.Ops = ds;
                return this.Ops;
              }
            }
          }
        } else {
          return this.Ops;
        }
      }
    }

    private DataSet GetOps() {
      lock (threadLock) {
        string SQL = string.Format("SELECT * FROM CUT_OPS WHERE OPTYPE = {0} ORDER BY OPDESCR", OpType);
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
            using (DataSet ds = new DataSet()) {

              try {
                da.Fill(ds);
              } catch (StackOverflowException sofe) {
                throw sofe;
              } finally {
                DataRow dar = ds.Tables[0].NewRow();
                dar[0] = 0;
                dar[1] = string.Empty;
                dar[2] = string.Empty;
                ds.Tables[0].Rows.Add(dar);
                this.Ops = ds;
              }
            }
          }
        }
        return this.Ops;
      }
    }

    public DataSet GetOpTypes() {
      lock (threadLock) {
        string SQL = "SELECT * FROM CUT_PART_TYPES ORDER BY TYPEID";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
            using (DataSet ds = new DataSet()) {
              da.Fill(ds);
              OpTypes = ds;
              return OpTypes;
            }
          }
        }
      }
    }

    public DataSet GetWherePartUsed(int partID) {
      string SQL = string.Format("SELECT CUT_CUTLISTS.CLID, (CUT_CUTLISTS.PARTNUM + ' REV' + CUT_CUTLISTS.REV) AS PARTNUM, CUT_CUTLISTS.DESCR, CUT_CUTLISTS.LENGTH, " +
          "CUT_CUTLISTS.WIDTH, CUT_CUTLISTS.HEIGHT, CUT_CUTLISTS.CDATE, CUT_CUTLISTS.CUSTID, CUT_CUTLISTS.SETUP_BY, CUT_CUTLISTS.STATE_BY, " +
          "CUT_CUTLISTS.DRAWING, CUT_CUTLIST_PARTS.QTY, CUT_CUTLISTS.STATEID FROM " +
          "(CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID) INNER JOIN " +
          "CUT_CUTLISTS ON CUT_CUTLIST_PARTS.CLID = CUT_CUTLISTS.CLID WHERE " +
          "(((CUT_PARTS.PARTID)={0}));", partID);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public DataSet GetWherePartUsed(string partDescr) {
      string SQL = string.Format("SELECT CUT_CUTLISTS.CLID, (CUT_CUTLISTS.PARTNUM + ' REV' + CUT_CUTLISTS.REV) AS PARTNUM, CUT_CUTLISTS.DESCR, CUT_CUTLISTS.LENGTH, " +
          "CUT_CUTLISTS.WIDTH, CUT_CUTLISTS.HEIGHT, CUT_CUTLISTS.CDATE, CUT_CUTLISTS.CUSTID, CUT_CUTLISTS.SETUP_BY, CUT_CUTLISTS.STATE_BY, " +
          "CUT_CUTLISTS.DRAWING, CUT_CUTLIST_PARTS.QTY, CUT_CUTLISTS.STATEID FROM " +
          "(CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID) INNER JOIN " +
          "CUT_CUTLISTS ON CUT_CUTLIST_PARTS.CLID = CUT_CUTLISTS.CLID WHERE " +
          "(((CUT_PARTS.PARTNUM) Like '{0}'));", partDescr);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    private DataSet GetCustomers() {
      string SQL = "SELECT * FROM GEN_CUSTOMERS ORDER BY CUSTOMER;";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    private DataSet GetCustomers(bool current) {
      string SQL = string.Format("SELECT * FROM GEN_CUSTOMERS WHERE GEN_CUSTOMERS.CUSTACTIVE = {0} ORDER BY GEN_CUSTOMERS.CUSTOMER;", (current ? 1 : 0));
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    private DataSet GetStates() {
      string SQL = "SELECT * FROM CUT_STATES;";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public double GetEdgeThickness(int ID) {
      string SQL = string.Format("SELECT THICKNESS FROM CUT_EDGES WHERE EDGEID = {0}", ID.ToString());
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows)
            return dr.GetDouble(0);
          else
            return 0.0;
        }
      }
    }

    public int GetMaterialID(string description) {
      if (description == null)
        return 2929;

      string SQL = string.Format("SELECT MATID FROM CUT_MATERIALS WHERE DESCR = '{0}'", description);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows)
            return dr.GetInt32(0);
          else
            return 2929;
        }
      }
    }

    public int GetOpID(string description) {
      if (description == string.Empty)
        return 0;

      string SQL = string.Format("SELECT OPID FROM CUT_OPS WHERE OPDESCR = '{0}'", description);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows)
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetEdgeID(string description) {
      if (description == string.Empty)
        return 0;

      string SQL = string.Format("SELECT EDGEID FROM CUT_EDGES WHERE DESCR = '{0}'", description);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows)
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetOpIDByName(string name) {
      if (name == string.Empty)
        return 0;

      string SQL = string.Format("SELECT OPID FROM CUT_OPS WHERE OPNAME Like '{0}'", name);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows)
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public List<string> GetOpDataByName(string name) {
      List<string> defaultList = new List<string> { "0", string.Empty, string.Empty, this.OpType.ToString() };
      if (name == string.Empty)
        return defaultList;

      string SQL = string.Format("SELECT OPID, OPNAME, OPDESCR, OPTYPE FROM CUT_OPS WHERE OPNAME Like '{0}' AND OPTYPE = {1}", name, OpType);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows) {
            List<string> x = new List<string>();
            x.Add(dr.GetString(0));
            x.Add(dr.GetString(1));
            x.Add(dr.GetString(2));
            x.Add(dr.GetString(3));
            return x;
          } else {
            return defaultList;
          }
        }
      }
    }

    public int GetOpTypeIDByName(string name) {
      if (name == string.Empty)
        return 1;

      string SQL = string.Format("SELECT TYPEID FROM CUT_PART_TYPES WHERE TYPEDESC Like '{0}'", name);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows) {
            int res = dr.GetInt32(0);
            return res;
          } else {
            return 1;
          }
        }
      }
    }

    public string GetOpTypeNameByID(int ID) {
      if (ID == 0)
        return "WOOD";

      string SQL = string.Format("SELECT TYPEDESC FROM CUT_PART_TYPES WHERE TYPEID = {0}", ID);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows) {
            string res = dr.GetString(0);
            return res;
          } else {
            return "WOOD";
          }
        }
      }
    }

    public string GetMaterialByID(string id) {
      if (id == string.Empty)
        return "TBD MATERIAL";

      int tp = 0;
      if (int.TryParse(id, out tp)) {

        string SQL = string.Format("SELECT DESCR FROM CUT_MATERIALS WHERE MATID = {0}", id);
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.HasRows)
              return dr.GetString(0);
            else
              return "TBD MATERIAL";
          }
        }
      }
      return "TBD MATERIAL";
    }

    public string GetEdgeByID(string id) {
      if (id == string.Empty)
        return string.Empty;

      int tp = 0;
      if (int.TryParse(id, out tp)) {
        string SQL = string.Format("SELECT DESCR FROM CUT_EDGES WHERE EDGEID = {0}", id);
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.HasRows)
              return dr.GetString(0);
            else
              return string.Empty;
          }
        }
      }
      return string.Empty;
    }

    public string GetOpByID(string id) {
      if (id == string.Empty)
        return string.Empty;

      int tp = 0;
      if (int.TryParse(id, out tp)) {
        string SQL = string.Format("SELECT OPDESCR FROM CUT_OPS WHERE OPID = {0}", id);
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.HasRows)
              return dr.GetString(0);
            else
              return string.Empty;
          }
        }
      }
      return string.Empty;
    }

    public string GetOpAbbreviationByID(string id) {
      if (id == string.Empty)
        return string.Empty;

      int tp = 0;
      if (int.TryParse(id, out tp)) {
        string SQL = string.Format("SELECT OPNAME FROM CUT_OPS WHERE OPID = {0}", id);
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.HasRows)
              return dr.GetString(0);
            else
              return string.Empty;
          }
        }
      }
      return string.Empty;
    }

    enum ECODataColumns {
      USER_FIRST,
      USER_LAST,
      CHANGES,
      STATUS,
      DESC,
      REVISION
    }

    public eco GetECOData(string ecoNumber) {
      eco e = new eco();
      int en;

      if (int.TryParse(ecoNumber, out en)) {
        string SQL = string.Empty;

        if (en > Properties.Settings.Default.LastLegacyECR) {
          SQL = string.Format("SELECT GEN_USERS.FIRST, GEN_USERS.LAST, ECR_MAIN.CHANGES, " +
              "ECR_STATUS.STATUS, ECR_MAIN.ERR_DESC, ECR_MAIN.REVISION FROM " +
              "(ECR_MAIN LEFT JOIN GEN_USERS ON ECR_MAIN.REQ_BY = GEN_USERS.UID) " +
              "LEFT JOIN ECR_STATUS ON ECR_MAIN.STATUS = ECR_STATUS.STAT_ID WHERE " +
              "(((ECR_MAIN.[ECR_NUM])={0}));", ecoNumber.Replace("'", "''"));
        } else {
          SQL = string.Format("SELECT Engineer, Engineer, Change, (IF (Holder = 'Completed')) AS STATUS, Change, Change FROM ECR_LEGACY WHERE (((ECR_LEGACY.ECRNum)='{0}'));", ecoNumber.Replace("'", "''"));
        }
        //conn.Open(); 
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            e.EcrNumber = int.Parse(ecoNumber);
            if (dr.HasRows) {
              e.Changes = ReturnString(dr, (int)ECODataColumns.CHANGES);
              e.ErrDescription = ReturnString(dr, (int)ECODataColumns.DESC);
              e.Revision = ReturnString(dr, (int)ECODataColumns.REVISION);
              e.Status = ReturnString(dr, (int)ECODataColumns.STATUS);
              e.RequestedBy = ReturnString(dr, (int)ECODataColumns.USER_FIRST) + " " + ReturnString(dr, (int)ECODataColumns.USER_FIRST);
              dr.Close();
            }
          }
        }
        return e;
      }

      return e;
    }

    public eco GetLegacyECOData(string ecoNumber) {
      eco e = new eco();
      // ECR_LEGACY.ECRNum is a string field.
      string SQL = string.Format("SELECT * FROM ECR_LEGACY WHERE (((ECR_LEGACY.ECRNum)='{0}'));", ecoNumber.Replace("'", "''"));

      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows) {
            e.EcrNumber = int.Parse(ecoNumber);
            e.Revision = ReturnString(dr, 5); // AffectedParts
            e.Changes = ReturnString(dr, 6); // Change
            string dString = ReturnString(dr, 4);
            if (dString != string.Empty) {
              string[] dateString = dString.Split('/', ' '); // DateCompleted
              System.DateTime dt = new DateTime(int.Parse(dateString[2]), int.Parse(dateString[0]), int.Parse(dateString[1]));
              e.Status = "Finished on " + dt.ToShortDateString();
            } else {
              dString = ReturnString(dr, 3);
              string[] dateString = dString.Split('/', ' '); // DateCompleted
              if (dateString.Length > 2) {
                System.DateTime dt = new DateTime(int.Parse(dateString[2]), int.Parse(dateString[0]), int.Parse(dateString[1]));
                e.Status = "Requested on " + dt.ToShortDateString();
              } else {
                e.Status = dString;
              }
            }
            e.RequestedBy = ReturnString(dr, 7); // FinishedBy
          }
        }
      }
      return e;
    }

    public DataSet GetAuthors() {
#if DEBUG
      DateTime start;
      DateTime end;
      start = DateTime.Now;
#endif
      string SQL = string.Format("SELECT GEN_USERS.* FROM GEN_USERS WHERE (((GEN_USERS.DEPT)={0}));", Properties.Settings.Default.UserDept);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
#if DEBUG
            end = DateTime.Now;
            System.Diagnostics.Debug.Print("*** AUTH ***<<< " + (end - start).ToString() + " >>>");
#endif
            return ds;
          }
        }
      }
    }

    public string GetAuthorUserName(string initial) {
      if (initial == string.Empty)
        return string.Empty;

      string SQL = string.Format("SELECT username FROM GEN_USERS WHERE INITIAL LIKE '{0}%';", initial);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {

          if (dr.HasRows) {
            string x = dr.GetString(0);
            dr.Close();
            return x;
          } else {
            return string.Empty;
          }
        }
      }
    }

    public int ReturnHash(SwProperties p) {
      if (p.PartName == string.Empty)
        return 0;

      string SQL = string.Format("SELECT CUT_PARTS.HASH FROM CUT_PARTS WHERE (((CUT_PARTS.PARTNUM)='{0}'));", p.PartName.Replace("'", "\""));
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    int GetCurrentAuthor() {
      string SQL = string.Format("SELECT UID FROM GEN_USERS WHERE USERNAME = '{0}';", Environment.UserName.Replace("'", "\""));
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int UpdateCutlist(string itemNo, string drawing, ushort rev, string descr, ushort custid, double l, double w, double h, ushort qty, ushort state,
      Dictionary<string, Part> prts) {
      swTableType.swTableType tt = new swTableType.swTableType();
      Dictionary<string, Part> prt = tt.GetParts();
      int currentAuthor = GetCurrentAuthor();
      int affected = 0;
      if (ENABLE_DB_WRITE) {
        string SQL = string.Empty;

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          SQL = string.Format("UPDATE CUT_CUTLISTS SET CUT_CUTLISTS.DRAWING = '{0}', CUT_CUTLISTS.CUSTID = {1}, CUT_CUTLISTS.CDATE = {2}, CUT_CUTLISTS.DESCR = '{3}', " +
           "CUT_CUTLISTS.LENGTH = {4}, CUT_CUTLISTS.WIDTH = {5}, CUT_CUTLISTS.HEIGHT = {6}, CUT_CUTLISTS.STATE_BY = {7}, CUT_CUTLISTS.STATEID = {8} " +
           "WHERE (((CUT_CUTLISTS.PARTNUM)='{9}') AND ((CUT_CUTLISTS.REV)='{10}'));", drawing.Replace("'", "\""), custid, DateTime.Now, descr.Replace("'", "\""), l, w, h,
           currentAuthor, state, itemNo.Replace("'", "\""), rev);

          affected = comm.ExecuteNonQuery();
        }
        if (affected < 1) {
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            SQL = string.Format("INSERT INTO CUT_CUTLISTS (PARTNUM, REV, DRAWING, CUSTID, CDATE, DESCR, LENGTH, WIDTH, HEIGHT, SETUP_BY, STATE_BY, STATEID) VALUES " +
              "('{0}', {1}, '{2}', {3}, '{4}', '{5}', {6}, {7}, {8}, {9}, {10}, {11});", itemNo.Replace("'", "\""), rev, drawing.Replace("'", "\""), custid,
              DateTime.Now, descr.Replace("'", "\""), l, w, h, currentAuthor, currentAuthor, Properties.Settings.Default.DefaultState);

            affected = comm.ExecuteNonQuery();
          }

          if (affected == 1) {
            foreach (KeyValuePair<string, Part> item in prts) {
              UpdatePart(item);
              UpdateCutlistPart(itemNo, item, qty);
            }
          }
        }
      }
      return affected;
    }

    public int UpdateCutlistPart(string cl, KeyValuePair<string, Part> kpprt, int qty) {
      int affected = 0;
      if (ENABLE_DB_WRITE) {
        string prt = kpprt.Key;
        Part p = kpprt.Value;
        string SQL = string.Format("UPDATE (CUT_CUTLIST_PARTS INNER JOIN CUT_CUTLISTS ON CUT_CUTLIST_PARTS.CLID = CUT_CUTLISTS.CLID) " +
          "INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID " +
          "SET CUT_CUTLIST_PARTS.MATID = {0}, CUT_CUTLIST_PARTS.EDGEID_LF = {1}, CUT_CUTLIST_PARTS.EDGEID_LB = {2}, CUT_CUTLIST_PARTS.EDGEID_WR = {3}, " +
          "CUT_CUTLIST_PARTS.EDGEID_WL = {4}, CUT_CUTLIST_PARTS.QTY = {5} " +
          "WHERE (((CUT_CUTLISTS.PARTNUM)='{6}') AND ((CUT_PARTS.PARTNUM)='{7}'));", p.MaterialID, p.EdgeFrontID, p.EdgeBackID, p.EdgeRightID, p.EdgeLeftID,
          p.Qty, cl, prt);

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) { affected = comm.ExecuteNonQuery(); }
        if (affected < 1) {
          // Seems like this should work every time given what went before.
          throw new Exception(string.Format("Couldn't execute this:\n{0}", SQL));
        }
      }
      return affected;
    }

    //private int InsertIntoCutlist(int clid, int partid, Part p, int qty) {
    //  int affected = 0;
    //  if (ENABLE_DB_WRITE) {
    //    string SQL;
    //    SQL = string.Format("INSERT INTO CUT_CUTLIST_PARTS (CLID, PARTID, MATID, EDGEID_LF, EDGEID_LB, EDGEID_WR, EDGEID_WL, QTY) VALUES " +
    //      "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", clid, partid, p.MaterialID, p.EdgeFrontID, p.EdgeBackID, p.EdgeRightID, p.EdgeLeftID, qty);
    //    using (OdbcCommand comm = new OdbcCommand(SQL, conn)) { affected = comm.ExecuteNonQuery(); }
    //  }
    //  return affected;
    //}

    /// <summary>
    /// Gets and/or creates a part.
    /// </summary>
    /// <returns>PartID</returns>
    private int UpdatePart(KeyValuePair<string, Part> kp) {
      int affected = 0;
      if (ENABLE_DB_WRITE) {
        Part p = kp.Value;
        string SQL;
        SQL = string.Format("UPDATE CUT_PARTS SET CUT_PARTS.DESCR = '{1}', CUT_PARTS.FIN_L = {2}, CUT_PARTS.FIN_W = {3}, CUT_PARTS.THICKNESS = {4}, " +
          "CUT_PARTS.CNC1 = '{5}', CUT_PARTS.CNC2 = '{6}', CUT_PARTS.BLANKQTY = {7}, CUT_PARTS.OVER_L = {8}, CUT_PARTS.OVER_W = {9}, CUT_PARTS.OP1ID = {10}, " +
          "CUT_PARTS.OP2ID = {11}, CUT_PARTS.OP3ID = {12}, CUT_PARTS.OP4ID = {13}, CUT_PARTS.OP5ID = {14}, CUT_PARTS.COMMENT = '{15}', CUT_PARTS.UPDATE_CNC = {16}, " +
          "CUT_PARTS.TYPE = {17}, WHERE (((CUT_PARTS.PARTNUM)='{0}') AND ((CUT_PARTS.HASH)='{18}'));", kp.Key, p.Description,
          p.Length, p.Width, p.Thickness, p.CNC1, p.CNC2, p.BlankQty, p.OverL, p.OverW, p.get_OpID(0), p.get_OpID(1), p.get_OpID(2), p.get_OpID(3), p.get_OpID(4),
          p.Comment, (p.UpdateCNC ? 1 : 0), p.DepartmentID, p.Hash);

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) { affected = comm.ExecuteNonQuery(); }

        object hashres = GetAnything("HASH", "CUT_PARTS", string.Format("PARTNUM = '{0}", kp.Key));
        if (affected < 1 && (int.Parse(hashres.ToString()) == p.Hash)) {
          SQL = string.Format("INSERT INTO CUT_PARTS (PARTNUM, DESCR, FIN_L, FIN_W, THICKNESS, CNC1, CNC2, BLANKQTY, OVER_L, " +
            "OVER_W, OP1ID, OP2ID, OP3ID, OP4ID, OP5ID, COMMENT, UPDATE_CNC, TYPE, HASH) VALUES " +
            "({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18})", kp.Key, p.Description,
            p.Length, p.Width, p.Thickness, p.CNC1, p.CNC2, p.BlankQty, p.OverL, p.OverW, p.get_OpID(0), p.get_OpID(1), p.get_OpID(2), p.get_OpID(3), p.get_OpID(4),
            p.Comment, (p.UpdateCNC ? 1 : 0), p.DepartmentID);
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) { affected = comm.ExecuteNonQuery(); }
        }
        return GetPartID(kp.Key);
      }
      return GetPartID(kp.Key);
    }

    public int GetPartID(string prt) {
      string SQL = string.Format("SELECT CUT_PARTS.PARTID FROM CUT_PARTS WHERE (((CUT_PARTS.PARTNUM)='{0}'))", prt.Replace("'", "\""));
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetCutlistID(string item) {
      string SQL = string.Format("SELECT CUT_CLID FROM CUT_CUTLISTS WHERE (((CUT_CUTLISTS.PARTNUM)='{0}'))", item.Replace("'", "\""));
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public object GetAnything(string field, string table, string filter) {
      string SQL = SQL = string.Format("SELECT {1}.{0} FROM  {1} WHERE {2}", field, table, filter);
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.HasRows && !dr.IsDBNull(0))
            return dr.GetValue(0);
          else
            return 0;
        }
      }
    }

    public int MakeOriginal(SwProperties p) {
      int rowsAffected = -1;

      if (ENABLE_DB_WRITE) {
        string SQL = string.Format("UPDATE CUT_PARTS SET CUT_PARTS.HASH = {1} " +
            "WHERE (((CUT_PARTS.PARTNUM)='{0}'));", p.PartName.Replace("'", "\""), p.Hash);

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          try {
            rowsAffected = comm.ExecuteNonQuery();
          } catch (InvalidOperationException ioe) {
            throw ioe;
          }
        }
      }

      return rowsAffected;
    }

    private void parse(string s, out double d) {
      double dtp = 0.0f;
      if (double.TryParse(s, out dtp))
        d = dtp;
      else
        d = 0.0f;
    }

    private void parse(string s, out int i) {
      int itp = 0;
      if (int.TryParse(s, out itp))
        i = itp;
      else
        i = 0;
    }

    public int UpdateParts(SwProperties p) {
      int rowsAffected = -1;
      if (ENABLE_DB_WRITE && p.Primary) {
        string dscr = p.GetProperty("Description").Value.Replace("'", "\"");

        double finL = 0.0f;
        parse(p.GetProperty("LENGTH").ResValue, out finL);

        double finW = 0.0f;
        parse(p.GetProperty("WIDTH").ResValue, out finW);

        double thkn = 0.0f;
        parse(p.GetProperty("THICKNESS").ResValue, out thkn);

        double ovrL = 0.0f;
        parse(p.GetProperty("OVERL").Value, out ovrL);

        double ovrW = 0.0f;
        parse(p.GetProperty("OVERW").Value, out ovrW);

        string cnc1 = p.GetProperty("CNC1").Value.Replace("'", "\"");
        string cnc2 = p.GetProperty("CNC2").Value.Replace("'", "\"");
        string blnk = p.GetProperty("BLANK QTY").Value.Replace("'", "\"");
        string cmnt = p.GetProperty("COMMENT").Value.Replace("'", "\"");
        string updt = p.GetProperty("UPDATE CNC").ID.Replace("'", "\"");


        int Op1 = 0;
        parse(p.GetProperty("OP1ID").ID, out Op1);
        int Op2 = 0;
        parse(p.GetProperty("OP2ID").ID, out Op1);
        int Op3 = 0;
        parse(p.GetProperty("OP3ID").ID, out Op1);
        int Op4 = 0;
        parse(p.GetProperty("OP4ID").ID, out Op1);
        int Op5 = 0;
        parse(p.GetProperty("OP5ID").ID, out Op1);

        string SQL = string.Format("UPDATE CUT_PARTS SET DESCR = '{0}', FIN_L = {1}, FIN_W = {2}, THICKNESS = {3}, CNC1 = '{4}', CNC2 = '{5}', " +
            "BLANKQTY = {6}, OVER_L = {7}, OVER_W = {8}, OP1ID = {9}, OP2ID = {10}, OP3ID = {11}, OP4ID = {12}, OP5ID = {13}, COMMENT = '{14}', " +
            "UPDATE_CNC = {15}, TYPE = {16} WHERE PARTNUM = '{17}'", dscr, finL, finW, thkn, cnc1, cnc2,
            blnk, ovrL, ovrW, Op1, Op2, Op3, Op4, Op5, cmnt,
            updt, OpType.ToString(), p.PartName);

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          try {
            rowsAffected = comm.ExecuteNonQuery();
          } catch (InvalidOperationException ioe) {
            throw ioe;
          }
        }
      }
      return rowsAffected;
    }

    //public int UpdateCutlistParts(SwProperties p) {
    //  int rowsAffected = -1;
    //  string cmid = p.GetProperty("MATID").ID.Replace("'", "\"");
    //  string efid = p.GetProperty("EFID").ID.Replace("'", "\"");
    //  string ebid = p.GetProperty("EBID").ID.Replace("'", "\"");
    //  string elid = p.GetProperty("ELID").ID.Replace("'", "\"");
    //  string erid = p.GetProperty("ERID").ID.Replace("'", "\"");

    //  if (ENABLE_DB_WRITE && p.Primary && p.CutlistID != null && p.CutlistQuantity != null) {
    //    string SQL = string.Format("UPDATE CUT_CUTLIST_PARTS " +
    //        "SET CUT_CUTLIST_PARTS.MATID = {0}, " +
    //        "CUT_CUTLIST_PARTS.EDGEID_LF = {1}, " +
    //        "CUT_CUTLIST_PARTS.EDGEID_LB = {2}, " +
    //        "CUT_CUTLIST_PARTS.EDGEID_WR = {3}, " +
    //        "CUT_CUTLIST_PARTS.EDGEID_WL = {4}, " +
    //        "CUT_CUTLIST_PARTS.QTY = {5} " +
    //        "FROM CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID " +
    //        "WHERE (((CUT_CUTLIST_PARTS.CLID)={6}) AND ((CUT_PARTS.PARTNUM)='{7}'));",
    //        cmid, efid, ebid, erid, elid, p.CutlistQuantity, p.CutlistID, p.PartName);

    //    using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
    //      try {
    //        rowsAffected = comm.ExecuteNonQuery();
    //      } catch (InvalidOperationException ioe) {
    //        throw ioe;
    //      }
    //    }
    //  }

    //  return rowsAffected;
    //}

    public int UpdateMachinePrograms() {
      return -1;
    }

    private string ReturnString(OdbcDataReader dr, int i) {
      if (dr.HasRows) {
        if (dr.IsDBNull(i)) {
          return string.Empty;
        } else {
          string returnString = dr.GetValue(i).ToString();
          return returnString;
        }
      }
      return string.Empty;
    }

    private DataSet _states;

    public DataSet States {
      get {
        if (_states != null && _states.Tables[0].Rows.Count > 0) {
          return _states;
        } else {
          _states = GetStates();
          return _states;
        }
      }

      private set {
        _states = value;
      }
    }

    private DataSet _customers;

    public DataSet Customers {
      get {
        if (_customers != null && _customers.Tables[0].Rows.Count > 0) {
          return _customers;
        } else {
          if (Properties.Settings.Default.OnlyCurrentCustomers)
            _customers = GetCustomers(true);
          else
            _customers = GetCustomers();
          return _customers;
        }
      }

      private set {
        _customers = value;
      }
    }
    private DataSet _opTypes;

    public DataSet OpTypes {
      get {
        if (_opTypes != null) {
          return _opTypes.Copy();
        } else {
          _opTypes = GetOpTypes();
          return _opTypes;
        }
      }
      private set { _opTypes = value; }
    }

    private DataSet _materials;

    public DataSet Materials {
      get { return GetMaterials(); }
      private set { _materials = value; }
    }

    private DataSet _edges;

    public DataSet Edges {
      get {
        if (_edges != null) {
          return _edges.Copy();
        } else {
          _edges = GetEdges();
          return _edges;
        }
      }
      private set { _edges = value; }
    }

    private int _opType;

    public int OpType {
      get { return _opType; }
      set {
        if (value > 2)
          _opType = 1;
        else
          _opType = value;
      }
    }


    private DataSet _Ops;

    public DataSet Ops {
      get {
        if ((this._Ops != null) && (this._Ops.Tables[0].Rows[0]["OPTYPE"].ToString() == this.OpType.ToString())) {
          return _Ops.Copy();
        }

        this._Ops = GetOps();
        return this._Ops = _Ops.Copy();
      }
      private set { _Ops = value; }
    }

  }
}
