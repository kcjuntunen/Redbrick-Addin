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

    public enum ECODataColumns {
      USER_FIRST,
      USER_LAST,
      CHANGES,
      STATUS,
      DESC,
      REVISION
    }

    public enum WhereUsedRes {
      CLID,
      PARTNUM, //REV,
      DESCR,
      LENGTH, WIDTH, HEIGHT,
      CDATE,
      CUSTID,
      SETUP_BY, STATE_BY,
      DRAWING,
      QTY,
      STATEID
    }

    public enum CutlistDataFields {
      CLID,
      PARTNUM, REV,
      DRAWING,
      CUSTID,
      CDATE,
      DESCR,
      LENGTH, WIDTH, HEIGHT,
      SETUP_BY, STATE_BY,
      STATEID
    }

    public enum CutlistDataFieldsJoined {
      CLID,
      PARTNUM, // REV,
      DRAWING,
      CUSTID,
      CDATE,
      DESCR,
      LENGTH, WIDTH, HEIGHT,
      SETUP_BY, STATE_BY,
      STATEID
    }

    public enum OpFields {
      OPID,
      OPNAME,
      OPDESCR,
      OPTYPE
    }

    private DataSet GetMaterials() {
      if (_materials == null) {
        lock (threadLock) {
#if DEBUG
          DateTime start;
          DateTime end;
          start = DateTime.Now;
#endif
          string SQL = @"SELECT MATID,DESCR,COLOR FROM CUT_MATERIALS ORDER BY DESCR";
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
        string SQL = @"SELECT EDGEID,DESCR,COLOR,THICKNESS FROM CUT_EDGES ORDER BY DESCR";

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

    private DataSet GetOps(int optype) {
      lock (threadLock) {
        if (OpType != optype) {
          OpType = optype;
          string SQL = @"SELECT OPID, OPNAME, OPDESCR, OPTYPE FROM CUT_PART_TYPES "
              + @"INNER JOIN CUT_OPS ON CUT_PART_TYPES.TYPEID = CUT_OPS.OPTYPE WHERE CUT_PART_TYPES.TYPEID = ? ORDER BY OPDESCR";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            comm.Parameters.AddWithValue("@OpType", optype);
            using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
              using (DataSet ds = new DataSet()) {
                da.Fill(ds);
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
        string SQL = @"SELECT * FROM CUT_OPS WHERE OPTYPE = ? ORDER BY OPDESCR";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@OpType", OpType);
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
                Ops = ds;
              }
            }
          }
        }
        return this.Ops;
      }
    }

    public DataSet GetOpTypes() {
      lock (threadLock) {
        string SQL = @"SELECT * FROM CUT_PART_TYPES ORDER BY TYPEID";
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

    public List<string> GetCustomersForDrawing() {
      List<string> output = new List<string>();
      string SQL = @"SELECT CUSTOMER, CUSTNUM FROM GEN_CUSTOMERS WHERE GEN_CUSTOMERS.CUSTACTIVE = 1 ORDER BY CUSTOMER";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          string tmpstr = string.Empty;
          while (dr.Read()) {
            tmpstr = string.Format("{0} - {1}", dr.GetString(0), dr.GetString(1));
            output.Add(tmpstr);
          }
          return output;
        }
      }
    }

    private DataSet GetCustomers() {
      string SQL = @"SELECT * FROM GEN_CUSTOMERS ORDER BY CUSTOMER";
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
      string SQL = @"SELECT * FROM GEN_CUSTOMERS WHERE GEN_CUSTOMERS.CUSTACTIVE = ? ORDER BY GEN_CUSTOMERS.CUSTOMER";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@current", current);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public DataSet GetStates() {
      string SQL = @"SELECT * FROM CUT_STATES";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public DataSet GetWherePartUsed(int partID) {
      string SQL = @"SELECT CUT_CUTLISTS.CLID, (CUT_CUTLISTS.PARTNUM + ' REV' + CUT_CUTLISTS.REV) AS PARTNUM, CUT_CUTLISTS.DESCR, CUT_CUTLISTS.LENGTH, " +
          @"CUT_CUTLISTS.WIDTH, CUT_CUTLISTS.HEIGHT, CUT_CUTLISTS.CDATE, CUT_CUTLISTS.CUSTID, CUT_CUTLISTS.SETUP_BY, CUT_CUTLISTS.STATE_BY, " +
          @"CUT_CUTLISTS.DRAWING, CUT_CUTLIST_PARTS.QTY, CUT_CUTLISTS.STATEID FROM " +
          @"(CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID) INNER JOIN " +
          @"CUT_CUTLISTS ON CUT_CUTLIST_PARTS.CLID = CUT_CUTLISTS.CLID WHERE " +
          @"(((CUT_PARTS.PARTID)=?)) ORDER BY CUT_CUTLISTS.PARTNUM";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@PartID", partID);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public bool PartUsed(int partID) {
      string SQL = @"SELECT CUT_CUTLIST_PARTS.*, CUT_CUTLIST_PARTS.PARTID FROM CUT_CUTLIST_PARTS WHERE (((CUT_CUTLIST_PARTS.PARTID)=?))";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@PartID", partID);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds.Tables[0].Rows.Count > 0;
          }
        }
      }
    }

    public bool PartUsed(string part) {
      string SQL = @"SELECT CUT_CUTLIST_PARTS.*, CUT_PARTS.PARTID, CUT_PARTS.PARTNUM " +
        @"FROM CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID " +
        @"WHERE (((CUT_PARTS.PARTNUM)= ?))";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@Part", part);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds.Tables[0].Rows.Count > 0;
          }
        }
      }
    }

    public bool PartUsed(SwProperties part) {
      string SQL = @"SELECT CUT_CUTLIST_PARTS.*, CUT_PARTS.PARTID, CUT_PARTS.PARTNUM " +
        @"FROM CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID " +
        @"WHERE (((CUT_PARTS.PARTNUM)= ?))";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@Part", part.PartName);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds.Tables[0].Rows.Count > 0;
          }
        }
      }
    }

    public DataSet GetWherePartUsed(string partDescr) {
      string SQL = @"SELECT CUT_CUTLISTS.CLID, (CUT_CUTLISTS.PARTNUM + ' REV' + CUT_CUTLISTS.REV) AS PARTNUM, CUT_CUTLISTS.DESCR, CUT_CUTLISTS.LENGTH, " +
          @"CUT_CUTLISTS.WIDTH, CUT_CUTLISTS.HEIGHT, CUT_CUTLISTS.CDATE, CUT_CUTLISTS.CUSTID, CUT_CUTLISTS.SETUP_BY, CUT_CUTLISTS.STATE_BY, " +
          @"CUT_CUTLISTS.DRAWING, CUT_CUTLIST_PARTS.QTY, CUT_CUTLISTS.STATEID FROM " +
          @"(CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID) INNER JOIN " +
          @"CUT_CUTLISTS ON CUT_CUTLIST_PARTS.CLID = CUT_CUTLISTS.CLID WHERE " +
          @"(((CUT_PARTS.PARTNUM) Like ?))  ORDER BY CUT_CUTLISTS.PARTNUM";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@PartName", partDescr);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public DataTable GetWhereProgUsed(string prog) {
      string SQL = @"SELECT CUT_PARTS.PARTNUM, CUT_PARTS.DESCR, CUT_PARTS.FIN_L, CUT_PARTS.FIN_W, CUT_PARTS.THICKNESS, CUT_PARTS.CNC1, CUT_PARTS.CNC2, " +
        @"CUT_PARTS.BLANKQTY, CUT_PARTS.OVER_L, CUT_PARTS.OVER_W, CUT_OPS.OPDESCR, CUT_OPS_1.OPDESCR, CUT_OPS_2.OPDESCR, CUT_OPS_3.OPDESCR, CUT_OPS_4.OPDESCR, " +
        @"CUT_PARTS.COMMENT, CUT_PARTS.UPDATE_CNC FROM CUT_OPS AS CUT_OPS_4 " +
        @"RIGHT JOIN (CUT_OPS AS CUT_OPS_3 RIGHT JOIN (CUT_OPS AS CUT_OPS_2 RIGHT JOIN (CUT_OPS AS CUT_OPS_1 RIGHT JOIN " +
        @"(CUT_PARTS LEFT JOIN CUT_OPS ON CUT_PARTS.OP1ID = CUT_OPS.OPID) ON CUT_OPS_1.OPID = CUT_PARTS.OP2ID) ON CUT_OPS_2.OPID = CUT_PARTS.OP3ID) " +
        @"ON CUT_OPS_3.OPID = CUT_PARTS.OP4ID) ON CUT_OPS_4.OPID = CUT_PARTS.OP5ID WHERE (((CUT_PARTS.CNC1)=?)) OR (((CUT_PARTS.CNC2)=?)) ORDER BY CUT_PARTS.PARTNUM";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@proga", prog);
        comm.Parameters.AddWithValue("@progb", prog);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataTable dt = new DataTable()) {
            dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
            da.Fill(dt);
            return dt;
          }
        }
      }
    }

    public string GetStateByID(int ID) {
      string SQL = @"SELECT STATE FROM CUT_STATES WHERE ID = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@StateID", ID);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
            return dr.GetString(0);
          else
            return "UNSET";
        }
      }
    }

    public double GetEdgeThickness(int ID) {
      string SQL = @"SELECT THICKNESS FROM CUT_EDGES WHERE EDGEID = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@EdgeID", ID);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
            return dr.GetDouble(0);
          else
            return 0.0;
        }
      }
    }

    public int GetMaterialID(string description) {
      if (description == string.Empty)
        return Properties.Settings.Default.DefaultMaterial;

      string SQL = @"SELECT MATID FROM CUT_MATERIALS WHERE DESCR = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@description", description);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
            return dr.GetInt32(0);
          else
            return Properties.Settings.Default.DefaultMaterial;
        }
      }
    }

    public int GetOpID(string description) {
      if (description == string.Empty)
        return 0;

      string SQL = @"SELECT OPID FROM CUT_OPS WHERE OPDESCR = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@descr", description);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetEdgeID(string description) {
      if (description == string.Empty)
        return 0;

      string SQL = @"SELECT EDGEID FROM CUT_EDGES_XREF WHERE DESCR = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@descr", description);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetEdgeID2(string description) {
      if (description == string.Empty)
        return 0;

      string SQL = @"SELECT EDGEID FROM CUT_EDGES WHERE DESCR = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@descr", description);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetQtyByClIDAndPartID(int clid, int partid) {
      string SQL = @"SELECT QTY FROM CUT_CUTLIST_PARTS WHERE CLID = ? AND PARTID = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@cutlistID", clid);
        comm.Parameters.AddWithValue("@partID", partid);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
            return dr.GetInt32(0);
          else
            return 1;
        }
      }
    }

    public int GetOpIDByName(string name) {
      if (name == string.Empty)
        return 0;

      string SQL = @"SELECT OPID FROM CUT_OPS WHERE OPNAME Like ? AND OPTYPE = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@name", name);
        comm.Parameters.AddWithValue("@optype", OpType);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read())
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

      string SQL = @"SELECT OPID, OPNAME, OPDESCR, OPTYPE FROM CUT_OPS WHERE OPNAME Like ? AND OPTYPE = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@OpName", name);
        comm.Parameters.AddWithValue("@OpType", OpType);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read()) {
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

    public List<string> GetOpDataByID(string ID) {
      int tp = 0;
      List<string> defaultList = new List<string> { "0", string.Empty, string.Empty, this.OpType.ToString() };

      if ((ID != string.Empty) && (int.TryParse(ID, out tp))) {
        string SQL = @"SELECT OPID, OPNAME, OPDESCR, OPTYPE FROM CUT_OPS WHERE OPID = ?";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@OpId", tp);
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.Read()) {
              List<string> x = new List<string>();
              x.Add(dr.GetString(0));
              x.Add(dr.GetString(1));
              x.Add(dr.GetString(2));
              x.Add(dr.GetString(3));
              return x;
            } else {
            }
          }
        }
      }
      return defaultList;
    }

    public int GetOpTypeIDByName(string name) {
      if (name == string.Empty)
        return 1;

      string SQL = @"SELECT TYPEID FROM CUT_PART_TYPES WHERE TYPEDESC Like ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@Name", name);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read()) {
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
        return @"WOOD";

      string SQL = @"SELECT TYPEDESC FROM CUT_PART_TYPES WHERE TYPEID = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@ID", ID);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read()) {
            string res = dr.GetString(0);
            return res;
          } else {
            return @"WOOD";
          }
        }
      }
    }

    public string GetMaterialByID(string id) {
      if (id == string.Empty)
        return @"TBD MATERIAL";

      int tp = 0;
      if (int.TryParse(id, out tp)) {

        string SQL = @"SELECT DESCR FROM CUT_MATERIALS WHERE MATID = ?";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@id", id);
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.Read())
              return dr.GetString(0);
            else
              return @"TBD MATERIAL";
          }
        }
      }
      return @"TBD MATERIAL";
    }

    public string GetEdgeByID(string id) {
      if (id == string.Empty)
        return string.Empty;

      int tp = 0;
      if (int.TryParse(id, out tp)) {
        string SQL = @"SELECT DESCR FROM CUT_EDGES WHERE EDGEID = ?";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@id", id);
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.Read())
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
        string SQL = @"SELECT OPDESCR FROM CUT_OPS WHERE OPID = ?";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@OpID", id);
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.Read())
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
        string SQL = @"SELECT OPNAME FROM CUT_OPS WHERE OPID = ?";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@id", tp);
          using (OdbcDataReader dr = comm.ExecuteReader()) {
            if (dr.Read())
              return dr.GetString(0);
            else
              return string.Empty;
          }
        }
      }
      return string.Empty;
    }

    public int GetDrawingID(FileInfo drawingPath) {
      int drId = 0;
      string SQL = @"SELECT FileID FROM GEN_DRAWINGS WHERE FName LIKE ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@drawingPath", drawingPath.Name);
        using (OdbcDataReader dr = comm.ExecuteReader(CommandBehavior.SingleResult)) {
          if (dr.Read()) {
            drId = dr.GetInt32(0);
          }
        }
      }
      return drId;
    }

    public int GetDrawingID(string filename) {
      int drId = 0;
      string SQL = @"SELECT FileID FROM GEN_DRAWINGS WHERE FName LIKE ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@filename", filename);
        using (OdbcDataReader dr = comm.ExecuteReader(CommandBehavior.SingleResult)) {
          if (dr.Read()) {
            drId = dr.GetInt32(0);
          }
        }
      }
      return drId;
    }

    //public int ECRIsBogus(string econumber) {
    //  int en = 0;
    //  if (int.TryParse(econumber, out en)) {
    //    if (en > GetLastLegacyECR()) {
    //      string SQL = @""; 
    //    }
    //  }
    //}

    public int GetLastLegacyECR() {
      int max = 0;
      string SQL = @"SELECT MAX(ECRNum) FROM ECR_LEGACY;";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataReader dr = comm.ExecuteReader(CommandBehavior.SingleResult)) {
          if (dr.Read() && int.TryParse(dr.GetString(0), out max)) {
            return max;
          } else {
            return Properties.Settings.Default.LastLegacyECR;
          }
        }
      }
    }

    public eco GetECOData(string ecoNumber) {
      eco e = new eco();
      int en;

      if (int.TryParse(ecoNumber, out en)) {
        string SQL = string.Empty;

        if (en > GetLastLegacyECR()) {
          SQL = @"SELECT GEN_USERS.FIRST, GEN_USERS.LAST, ECR_MAIN.CHANGES, " +
              @"ECR_STATUS.STATUS, ECR_MAIN.ERR_DESC, ECR_MAIN.REVISION FROM " +
              @"(ECR_MAIN LEFT JOIN GEN_USERS ON ECR_MAIN.REQ_BY = GEN_USERS.UID) " +
              @"LEFT JOIN ECR_STATUS ON ECR_MAIN.STATUS = ECR_STATUS.STAT_ID WHERE " +
              @"(((ECR_MAIN.[ECR_NUM])=?))";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            comm.Parameters.AddWithValue("@ecrNo", en);
            using (OdbcDataReader dr = comm.ExecuteReader()) {
              e.EcrNumber = int.Parse(ecoNumber);
              if (dr.Read()) {
                if (!dr.IsDBNull((int)ECODataColumns.CHANGES))
                  e.Changes = dr.GetString((int)ECODataColumns.CHANGES);
                if (!dr.IsDBNull((int)ECODataColumns.DESC))
                  e.ErrDescription = dr.GetString((int)ECODataColumns.DESC);
                if (!dr.IsDBNull((int)ECODataColumns.REVISION))
                  e.Revision = dr.GetString((int)ECODataColumns.REVISION);
                if (!dr.IsDBNull((int)ECODataColumns.STATUS))
                  e.Status = dr.GetString((int)ECODataColumns.STATUS);
                if (!dr.IsDBNull((int)ECODataColumns.USER_FIRST) && !dr.IsDBNull((int)ECODataColumns.USER_LAST))
                  e.RequestedBy = dr.GetString((int)ECODataColumns.USER_FIRST) + " " + dr.GetString((int)ECODataColumns.USER_LAST);
                dr.Close();
              }
            }
          }
        } else {
          // ECR_LEGACY.ECRNum is a string field.
          SQL = @"SELECT * FROM ECR_LEGACY WHERE (((ECR_LEGACY.ECRNum)=?))";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            comm.Parameters.AddWithValue("@ecrNo", ecoNumber);
            using (OdbcDataReader dr = comm.ExecuteReader()) {
              e.EcrNumber = int.Parse(ecoNumber);
              if (dr.Read()) {
                if (!dr.IsDBNull(6))
                  e.Changes = dr.GetString(6);
                if (!dr.IsDBNull(4))
                  e.Status = @"Finished on " + dr.GetDate(4).ToString().Split(' ')[0];
                else if (!dr.IsDBNull(3))
                  e.Status = @"Requested on " + dr.GetDate(3).ToString().Split(' ')[0];
                if (!dr.IsDBNull(5))
                  e.Revision = dr.GetString(5);
                if (!dr.IsDBNull(7))
                  e.RequestedBy = dr.GetString(7);
              }
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
      string SQL = @"SELECT * FROM ECR_LEGACY WHERE (((ECR_LEGACY.ECRNum)=?))";

      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@ecoNumber", ecoNumber);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read()) {
            e.EcrNumber = int.Parse(ecoNumber);
            e.Revision = dr.GetString(5); // AffectedParts
            e.Changes = dr.GetString(6); // Change
            string dString = dr.GetDate(4).ToString();
            if (dString != string.Empty) {
              string[] dateString = dString.Split('/', ' '); // DateCompleted
              System.DateTime dt = new DateTime(int.Parse(dateString[2]), int.Parse(dateString[0]), int.Parse(dateString[1]));
              e.Status = @"Finished on " + dt.ToShortDateString();
            } else {
              dString = dr.GetDate(3).ToString();
              string[] dateString = dString.Split('/', ' '); // DateCompleted
              if (dateString.Length > 2) {
                System.DateTime dt = new DateTime(int.Parse(dateString[2]), int.Parse(dateString[0]), int.Parse(dateString[1]));
                e.Status = @"Requested on " + dt.ToShortDateString();
              } else {
                e.Status = dString;
              }
            }
            e.RequestedBy = dr.GetString(7); // FinishedBy
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
      string SQL = @"SELECT UID, USERNAME, (FIRST + ' ' + LAST) AS NAME, INITIAL " +
        "FROM GEN_USERS WHERE (((GEN_USERS.DEPT)=?)) ORDER BY LAST";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@dept", Properties.Settings.Default.UserDept);
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

      string SQL = @"SELECT username FROM GEN_USERS WHERE INITIAL LIKE ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@initial", initial);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read()) {
            string x = dr.GetString(0);
            dr.Close();
            return x;
          } else {
            return string.Empty;
          }
        }
      }
    }

    public string GetAuthorFullName(string initial) {
      if (initial == string.Empty)
        return string.Empty;

      string SQL = @"SELECT (FIRST + ' ' + LAST) AS NAME FROM GEN_USERS WHERE INITIAL LIKE ? AND DEPT = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@initial", initial + "%");
        comm.Parameters.AddWithValue("@dept", Properties.Settings.Default.UserDept);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read()) {
            string x = dr.GetString(0);
            dr.Close();
            return x;
          } else {
            return initial;
          }
        }
      }
    }

    public int ReturnHash(SwProperties p) {
      if (p.PartName == string.Empty)
        return 0;

      string SQL = @"SELECT CUT_PARTS.HASH FROM CUT_PARTS WHERE (((CUT_PARTS.PARTNUM)=?))";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@partnum", p.PartName);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetCurrentAuthor() {
      string SQL = @"SELECT UID FROM GEN_USERS WHERE USERNAME = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@uname", Environment.UserName);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public string GetCurrentAuthorInitial() {
      string SQL = @"SELECT INITIAL FROM GEN_USERS WHERE USERNAME = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@uname", Environment.UserName);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetString(0);
          else
            return string.Empty;
        }
      }
    }

    public string GetAuthorByID(int id) {
      string SQL = @"SELECT INITIAL FROM GEN_USERS WHERE UID = ?";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@uid", id);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetString(0);
          else
            return string.Empty;
        }
      }
    }

    public DataSet GetDepartments() {
      string SQL = @"SELECT DEPT_ID, DEPT_NAME, DEPT_PRIOR FROM GEN_DEPTS";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public int SetState(int cutlistid, int stateid) {
      int currentAuthor = GetCurrentAuthor();
      int affected = 0;
      if (ENABLE_DB_WRITE) {
        string SQL = @"UPDATE CUT_CUTLISTS SET STATEID = ?, STATE_BY = ? WHERE CLID = ?;";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@stateid", stateid);
          comm.Parameters.AddWithValue("@stateby", currentAuthor);
          comm.Parameters.AddWithValue("@clid", cutlistid);
          affected = comm.ExecuteNonQuery();
        }
      }
      return affected;
    }

    public int UpdateCutlist(string itemNo, string drawing, string rev, string descr, ushort custid, double l, double w, double h, ushort state,
      Dictionary<string, Part> prts) {
      int currentAuthor = GetCurrentAuthor();
      int affected = 0;
      if (ENABLE_DB_WRITE) {
        string SQL = string.Empty;
        SQL = @"UPDATE CUT_CUTLISTS SET DRAWING = ?, CUSTID = ?, CDATE = GETDATE(), DESCR = ?, " +
         @"LENGTH = ?, WIDTH = ?, HEIGHT = ?, STATE_BY = ?, STATEID = ? " +
         @"WHERE PARTNUM=? AND REV=?;";

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@drawing", FilterString(drawing));
          comm.Parameters.AddWithValue("@custid", Convert.ToInt32(custid));
          comm.Parameters.AddWithValue("@descr", FilterString(descr));
          comm.Parameters.AddWithValue("@l", l);
          comm.Parameters.AddWithValue("@w", w);
          comm.Parameters.AddWithValue("@h", h);
          comm.Parameters.AddWithValue("@stateby", Convert.ToInt32(currentAuthor));
          comm.Parameters.AddWithValue("@stateid", (state < 1 ? 1 : Convert.ToInt32(state)));
          comm.Parameters.AddWithValue("@partnum", FilterString(itemNo));
          comm.Parameters.AddWithValue("@rev", rev);
          affected = comm.ExecuteNonQuery();
        }
        if (affected < 1) {
          SQL = @"INSERT INTO CUT_CUTLISTS (PARTNUM, REV, DRAWING, CUSTID, CDATE, DESCR, LENGTH, WIDTH, HEIGHT, SETUP_BY, STATE_BY, STATEID) VALUES " +
            @"(?, ?, ?, ?, GETDATE(), ?, ?, ?, ?, ?, ?, ?);";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            comm.Parameters.AddWithValue("@partnum", FilterString(itemNo));
            comm.Parameters.AddWithValue("@rev", rev);
            comm.Parameters.AddWithValue("@drawing", FilterString(drawing));
            comm.Parameters.AddWithValue("@custid", Convert.ToInt32(custid));
            comm.Parameters.AddWithValue("@descr", FilterString(descr));
            comm.Parameters.AddWithValue("@l", l);
            comm.Parameters.AddWithValue("@w", w);
            comm.Parameters.AddWithValue("@h", h);
            comm.Parameters.AddWithValue("@setupby", currentAuthor);
            comm.Parameters.AddWithValue("@stateby", Convert.ToInt32(currentAuthor));
            comm.Parameters.AddWithValue("@stateid", Convert.ToInt32(Properties.Settings.Default.DefaultState));
            affected = comm.ExecuteNonQuery();
          }
        }
        if (affected == 1) {
          foreach (KeyValuePair<string, Part> item in prts) {
            int partID = UpdatePart(item);
            string itemHash = string.Format("{0:X}", item.Value.Hash);
            string recordHash = string.Format("{0:X}", GetHash(item.Key));
            if ((itemHash == recordHash) || (recordHash == "0")) {
              UpdateCutlistPart(GetCutlistID(itemNo, rev.ToString()), partID, item);
            }
          }
        }
      }
      return affected;
    }

    public int UpdateCutlistPart(int clid, int prtid, KeyValuePair<string, Part> kpprt) {
      int affected = 0;
      Properties.Settings.Default.CurrentCutlist = clid;
      if (ENABLE_DB_WRITE) {
        string prt = kpprt.Key;
        Part p = kpprt.Value;
        string SQL = @"UPDATE CUT_CUTLIST_PARTS " +
          @"SET MATID = ?, EDGEID_LF = ?, EDGEID_LB = ?, EDGEID_WR = ?, EDGEID_WL = ?, " +
          @"QTY = ? WHERE CLID=? AND PARTID=?";

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@matid", Convert.ToInt32(p.MaterialID));
          comm.Parameters.AddWithValue("@efid", Convert.ToInt32(p.EdgeFrontID));
          comm.Parameters.AddWithValue("@ebid", Convert.ToInt32(p.EdgeBackID));
          comm.Parameters.AddWithValue("@erid", Convert.ToInt32(p.EdgeRightID));
          comm.Parameters.AddWithValue("@elid", Convert.ToInt32(p.EdgeLeftID));
          comm.Parameters.AddWithValue("@qty", Convert.ToInt32(p.Qty));
          comm.Parameters.AddWithValue("@clid", clid);
          comm.Parameters.AddWithValue("@prtid", prtid);
          affected = comm.ExecuteNonQuery();
        }
        if (affected < 1) {

          SQL = @"INSERT INTO CUT_CUTLIST_PARTS (CLID, PARTID, MATID, EDGEID_LF, EDGEID_LB, EDGEID_WR, EDGEID_WL, QTY) " +
            @"VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            comm.Parameters.AddWithValue("@clid", clid);
            comm.Parameters.AddWithValue("@prtid", prtid);
            comm.Parameters.AddWithValue("@matid", Convert.ToInt32(p.MaterialID));
            comm.Parameters.AddWithValue("@efid", Convert.ToInt32(p.EdgeFrontID));
            comm.Parameters.AddWithValue("@ebid", Convert.ToInt32(p.EdgeBackID));
            comm.Parameters.AddWithValue("@erid", Convert.ToInt32(p.EdgeRightID));
            comm.Parameters.AddWithValue("@elid", Convert.ToInt32(p.EdgeLeftID));
            comm.Parameters.AddWithValue("@qty", Convert.ToInt32(p.Qty));
            affected = comm.ExecuteNonQuery();
          }
        }
      }
      return affected;
    }

    /// <summary>
    /// Remove a part from a cutlist.
    /// </summary>
    /// <param name="clid">Cutlist ID integer.</param>
    /// <param name="p">Property set.</param>
    /// <returns></returns>
    public int RemovePartFromCutlist(int clid, SwProperties p) {
      int affected = 0;
      int partid = GetPartID(p.PartName);
      if (ENABLE_DB_WRITE && (GetHash(p.PartName) == p.Hash)) {
        string SQL = "DELETE FROM CUT_CUTLIST_PARTS WHERE CLID = ? AND PARTID = ?";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@clid", clid);
          comm.Parameters.AddWithValue("@partid", partid);
          affected = comm.ExecuteNonQuery();

          if (affected > 0 && !PartUsed(p.PartName)) {
            RemovePart(p);
          }
        }
      }
      return affected;
    }

    public int RemovePart(SwProperties part) {
      int affected = 0;
      int partid = GetPartID(part.PartName);
      if (ENABLE_DB_WRITE && (GetHash(part.PartName) == part.Hash)) {
        string SQL = "DELETE FROM CUT_PARTS WHERE PARTNUM = ?";
        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@part", part.PartName);
          affected = comm.ExecuteNonQuery();
        }
      }
      return affected;
    }

    /// <summary>
    /// Gets and/or creates a part.
    /// </summary>
    /// <returns>PartID</returns>
    public int UpdatePart(KeyValuePair<string, Part> kp) {
      int affected = 0;
      if (ENABLE_DB_WRITE) {
        Part p = kp.Value;
        string SQL;
        string hash = string.Format("{0:X}", p.Hash);
        SQL = @"UPDATE CUT_PARTS SET DESCR = ?, FIN_L = ?, FIN_W = ?, THICKNESS = ?, CNC1 = ?, CNC2 = ?, BLANKQTY = ?, OVER_L = ?, OVER_W = ?, OP1ID = ?, " +
          @"OP2ID = ?, OP3ID = ?, OP4ID = ?, OP5ID = ?, COMMENT = ?, UPDATE_CNC = ?, TYPE = ? WHERE PARTNUM=? AND (HASH=? OR HASH IS NULL);";

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@descr", FilterString(p.Description));
          comm.Parameters.AddWithValue("@finl", Convert.ToDouble(p.Length));
          comm.Parameters.AddWithValue("@finw", Convert.ToDouble(p.Width));
          comm.Parameters.AddWithValue("@thk", Convert.ToDouble(p.Thickness));
          comm.Parameters.AddWithValue("@cnc1", FilterString(p.CNC1));
          comm.Parameters.AddWithValue("@cnc2", FilterString(p.CNC2));
          comm.Parameters.AddWithValue("@blnkqty", p.BlankQty < 1 ? 1 : Convert.ToInt32(p.BlankQty));
          comm.Parameters.AddWithValue("@ovrl", Convert.ToDouble(p.OverL));
          comm.Parameters.AddWithValue("@ovrw", Convert.ToDouble(p.OverW));

          for (ushort i = 0; i < 5; i++)
            comm.Parameters.AddWithValue(string.Format("@op{0}ip", i + 1), Convert.ToInt32(p.get_OpID(i)));

          comm.Parameters.AddWithValue("@comment", FilterString(p.Comment, Properties.Settings.Default.FlameWar));
          comm.Parameters.AddWithValue("@updCnc", (p.UpdateCNC ? 1 : 0));
          comm.Parameters.AddWithValue("@type", Convert.ToInt32(p.DepartmentID));
          comm.Parameters.AddWithValue("@prtNo", FilterString(p.PartNumber));
          comm.Parameters.AddWithValue("@hash", Convert.ToInt32(hash, 16));

          affected = comm.ExecuteNonQuery();
        }

        if (GetHash(p.PartNumber) == 0) {
          SQL = @"UPDATE CUT_PARTS SET HASH = ? WHERE PARTNUM = ?";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            comm.Parameters.AddWithValue("@hash", Convert.ToInt32(hash, 16));
            comm.Parameters.AddWithValue("@partnum", p.PartNumber);

            affected = comm.ExecuteNonQuery();
          }
        }

        if (affected < 1 && (GetPartID(kp.Key) < 1)) {
          SQL = @"INSERT INTO CUT_PARTS (PARTNUM, DESCR, FIN_L, FIN_W, THICKNESS, CNC1, CNC2, BLANKQTY, OVER_L, " +
            @"OVER_W, OP1ID, OP2ID, OP3ID, OP4ID, OP5ID, COMMENT, UPDATE_CNC, TYPE, HASH) VALUES " +
            @"(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
          using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
            comm.Parameters.AddWithValue("@prtNo", FilterString(p.PartNumber));
            comm.Parameters.AddWithValue("@descr", FilterString(p.Description));
            comm.Parameters.AddWithValue("@finl", Convert.ToDouble(p.Length));
            comm.Parameters.AddWithValue("@finw", Convert.ToDouble(p.Width));
            comm.Parameters.AddWithValue("@thk", Convert.ToDouble(p.Thickness));
            comm.Parameters.AddWithValue("@cnc1", FilterString(p.CNC1));
            comm.Parameters.AddWithValue("@cnc2", FilterString(p.CNC2));
            comm.Parameters.AddWithValue("@blnkqty", Convert.ToInt32(p.BlankQty));
            comm.Parameters.AddWithValue("@ovrl", Convert.ToDouble(p.OverL));
            comm.Parameters.AddWithValue("@ovrw", Convert.ToDouble(p.OverW));

            for (ushort i = 0; i < 5; i++)
              comm.Parameters.AddWithValue(string.Format("@op{0}ip", i + 1), Convert.ToInt32(p.get_OpID(i)));

            comm.Parameters.AddWithValue("@comment", FilterString(p.Comment, Properties.Settings.Default.FlameWar));
            comm.Parameters.AddWithValue("@updCnc", (p.UpdateCNC ? 1 : 0));
            comm.Parameters.AddWithValue("@type", Convert.ToInt32(p.DepartmentID));
            comm.Parameters.AddWithValue("@hash", Convert.ToInt32(hash, 16));
            affected = comm.ExecuteNonQuery();
          }
        }
        return GetPartID(kp.Key);
      }
      return GetPartID(kp.Key);
    }

    public int GetPartID(string prt) {
      string SQL = @"SELECT CUT_PARTS.PARTID FROM CUT_PARTS WHERE (((CUT_PARTS.PARTNUM)=?))";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@partnum", prt);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetCutlistID(string item) {
      string SQL = @"SELECT CLID FROM CUT_CUTLISTS WHERE (((CUT_CUTLISTS.PARTNUM)=?))";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@itemno", item);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetCutlistID(string item, string rev) {
      string SQL = @"SELECT CLID FROM CUT_CUTLISTS WHERE (((CUT_CUTLISTS.PARTNUM)=?)) AND (((CUT_CUTLISTS.REV)=?))";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@itemNo", item);
        comm.Parameters.AddWithValue("@rev", rev);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public int GetHash(string partNum) {
      using (OdbcCommand comm = new OdbcCommand(@"SELECT HASH FROM CUT_PARTS WHERE PARTNUM = ?", conn)) {
        comm.Parameters.AddWithValue("@prtNo", partNum);
        using (OdbcDataReader dr = comm.ExecuteReader()) {
          if (dr.Read() && !dr.IsDBNull(0))
            return dr.GetInt32(0);
          else
            return 0;
        }
      }
    }

    public DataSet GetCutlistData(string item, string rev) {
      string SQL = @"SELECT * FROM CUT_CUTLISTS WHERE (((CUT_CUTLISTS.PARTNUM)=?)) AND (((CUT_CUTLISTS.REV)=?));";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@item", item);
        comm.Parameters.AddWithValue("@rev", rev);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public DataSet GetCutlistData(int clid) {
      string SQL = @"SELECT * FROM CUT_CUTLISTS WHERE (((CUT_CUTLISTS.CLID)=?));";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@clid", clid);
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public DataSet GetCutlists() {
      string SQL = @"SELECT CUT_CUTLISTS.CLID, (CUT_CUTLISTS.PARTNUM + ' REV' + CUT_CUTLISTS.REV) AS PARTNUM, CUT_CUTLISTS.DRAWING, " +
        @"CUT_CUTLISTS.CUSTID, CUT_CUTLISTS.CDATE, CUT_CUTLISTS.DESCR, CUT_CUTLISTS.LENGTH, CUT_CUTLISTS.WIDTH, CUT_CUTLISTS.HEIGHT, " +
        @"CUT_CUTLISTS.SETUP_BY, CUT_CUTLISTS.STATE_BY, CUT_CUTLISTS.STATEID FROM CUT_CUTLISTS ORDER BY CUT_CUTLISTS.PARTNUM";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        using (OdbcDataAdapter da = new OdbcDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds;
          }
        }
      }
    }

    public int MakeOriginal(SwProperties p) {
      int rowsAffected = 0;

      if (ENABLE_DB_WRITE) {
        string SQL = @"UPDATE CUT_PARTS SET CUT_PARTS.HASH = ? WHERE (((CUT_PARTS.PARTNUM)=?));";

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@hash", p.Hash);
          comm.Parameters.AddWithValue("@partNo", p.PartName);
          try {
            rowsAffected = comm.ExecuteNonQuery();
          } catch (InvalidOperationException ioe) {
            throw ioe;
          }
        }
      }

      return rowsAffected;
    }

    public void InsertError(int errNo, string errmsg, string targetSite) {
      string SQL = "INSERT INTO GEN_ERRORS ERRDATE, ERRUSER, ERRNUM, ERRMSG, ERROBJ, ERRCHK, ERRAPP VALUES " +
        "(GETDATE(), ?, ?, ?, ?, ?, ?)";
      using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@ErrUser", GetCurrentAuthor());
        comm.Parameters.AddWithValue("@ErrNum", errNo);
        comm.Parameters.AddWithValue("@ErrMsg", errmsg);
        comm.Parameters.AddWithValue("@ErrObj", targetSite);
        comm.Parameters.AddWithValue("@ErrChk", -1);
        comm.Parameters.AddWithValue("@ErrApp", @"Property Editor");
        comm.ExecuteNonQuery();
      }
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

    public int UpdateParts(SwProperties p, int clid) {
      int rowsAffected = 0;
      if (ENABLE_DB_WRITE) {
        string SQL = @"UPDATE CUT_PARTS SET DESCR = ?, FIN_L = ?, FIN_W = ?, THICKNESS = ?, CNC1 = ?, CNC2 = ?, " +
            "BLANKQTY = ?, OVER_L = ?, OVER_W = ?, OP1ID = ?, OP2ID = ?, OP3ID = ?, OP4ID = ?, OP5ID = ?, COMMENT = ?, " +
            "UPDATE_CNC = ?, TYPE = ? WHERE PARTNUM = ? AND HASH = ?";

        using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
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
          parse(p.GetProperty("OP2ID").ID, out Op2);
          int Op3 = 0;
          parse(p.GetProperty("OP3ID").ID, out Op3);
          int Op4 = 0;
          parse(p.GetProperty("OP4ID").ID, out Op4);
          int Op5 = 0;
          parse(p.GetProperty("OP5ID").ID, out Op5);
          comm.Parameters.AddWithValue("@descr", dscr);
          comm.Parameters.AddWithValue("@finl", finL);
          comm.Parameters.AddWithValue("@finw", finW);
          comm.Parameters.AddWithValue("@thkn", thkn);
          comm.Parameters.AddWithValue("@cnc1", cnc1);
          comm.Parameters.AddWithValue("@cnc2", cnc2);
          comm.Parameters.AddWithValue("@blnk", blnk);
          comm.Parameters.AddWithValue("@ovrL", ovrL);
          comm.Parameters.AddWithValue("@ovrW", ovrW);
          comm.Parameters.AddWithValue("@op1", Op1);
          comm.Parameters.AddWithValue("@op2", Op2);
          comm.Parameters.AddWithValue("@op3", Op3);
          comm.Parameters.AddWithValue("@op4", Op4);
          comm.Parameters.AddWithValue("@op5", Op5);
          comm.Parameters.AddWithValue("@comment", cmnt);
          comm.Parameters.AddWithValue("@updtCnc", (updt == "1"));
          comm.Parameters.AddWithValue("@opType", OpType);
          comm.Parameters.AddWithValue("@prtName", p.PartName);
          comm.Parameters.AddWithValue("@hash", p.Hash);

          try {
            comm.ExecuteNonQuery();
          } catch (InvalidOperationException ioe) {
            throw ioe;
          }
        }
      }
      return rowsAffected;
    }

    static public Part MakePartFromPropertySet(SwProperties swp) {
      Part p = new Part();
      p.FileInformation = swp.PartFileInfo;
      p.PartNumber = swp.PartName;
      p.SetMaterialID(swp.GetProperty("MATID").ID);
      p.SetEdgeFrontID(swp.GetProperty("EFID").ID);
      p.SetEdgeBackID(swp.GetProperty("EBID").ID);
      p.SetEdgeLeftID(swp.GetProperty("ELID").ID);
      p.SetEdgeRightID(swp.GetProperty("ERID").ID);

      p.Description = swp.GetProperty("Description").Value;
      p.SetLength(swp.GetProperty("LENGTH").ResValue);
      p.SetWidth(swp.GetProperty("WIDTH").ResValue);
      p.SetThickness(swp.GetProperty("THICKNESS").ResValue);

      p.Comment = swp.GetProperty("COMMENT").Value;
      p.CNC1 = swp.GetProperty("CNC1").Value;
      p.CNC2 = swp.GetProperty("CNC2").Value;
      p.SetUpdateCNC(swp.GetProperty("UPDATE CNC").ID);

      p.SetOverL(swp.GetProperty("OVERL").Value);
      p.SetOverW(swp.GetProperty("OVERW").Value);
      p.SetBlankQty(swp.GetProperty("BLANK QTY").Value);
      p.SetDeptID(swp.GetProperty("DEPTID").ID);

      p.SetOpID(swp.GetProperty("OP1ID").ID, 0);
      p.SetOpID(swp.GetProperty("OP2ID").ID, 1);
      p.SetOpID(swp.GetProperty("OP3ID").ID, 2);
      p.SetOpID(swp.GetProperty("OP4ID").ID, 3);
      p.SetOpID(swp.GetProperty("OP5ID").ID, 4);

      int tp = 0;
      int.TryParse(swp.GetProperty("CRC32").Value, out tp);
      string hash = string.Format("{0:X}", tp);
      p.Hash = uint.Parse(hash, System.Globalization.NumberStyles.HexNumber);

      return p;
    }

    static public Part MakePartFromPropertySet(SwProperties swp, ushort q) {
      Part p = new Part();
      p.SetQuantity(q.ToString());

      p.FileInformation = swp.PartFileInfo;
      p.PartNumber = swp.PartName;
      p.SetMaterialID(swp.GetProperty("MATID").ID);
      p.SetEdgeFrontID(swp.GetProperty("EFID").ID);
      p.SetEdgeBackID(swp.GetProperty("EBID").ID);
      p.SetEdgeLeftID(swp.GetProperty("ELID").ID);
      p.SetEdgeRightID(swp.GetProperty("ERID").ID);

      p.Description = swp.GetProperty("Description").Value;
      p.SetLength(swp.GetProperty("LENGTH").ResValue);
      p.SetWidth(swp.GetProperty("WIDTH").ResValue);
      p.SetThickness(swp.GetProperty("THICKNESS").ResValue);

      p.Comment = swp.GetProperty("COMMENT").Value;
      p.CNC1 = swp.GetProperty("CNC1").Value;
      p.CNC2 = swp.GetProperty("CNC2").Value;
      p.SetUpdateCNC(swp.GetProperty("UPDATE CNC").ID);

      p.SetOverL(swp.GetProperty("OVERL").Value);
      p.SetOverW(swp.GetProperty("OVERW").Value);
      p.SetBlankQty(swp.GetProperty("BLANK QTY").Value);
      p.SetDeptID(swp.GetProperty("DEPTID").ID);

      p.SetOpID(swp.GetProperty("OP1ID").ID, 0);
      p.SetOpID(swp.GetProperty("OP2ID").ID, 1);
      p.SetOpID(swp.GetProperty("OP3ID").ID, 2);
      p.SetOpID(swp.GetProperty("OP4ID").ID, 3);
      p.SetOpID(swp.GetProperty("OP5ID").ID, 4);

      int tp = 0;
      int.TryParse(swp.GetProperty("CRC32").Value, out tp);
      string hash = string.Format("{0:X}", tp);
      p.Hash = uint.Parse(hash, System.Globalization.NumberStyles.HexNumber);

      return p;
    }

    static public void FilterTextForControl(System.Windows.Forms.Control c) {
      //if (c is System.Windows.Forms.TextBox) {
      //  System.Windows.Forms.TextBox d = (c as System.Windows.Forms.TextBox);
      //  int pos = d.SelectionStart;
      //  d.Text = FilterString(d.Text);
      //  d.SelectionStart = pos;
      //} else if (c is System.Windows.Forms.ComboBox) {
      //  System.Windows.Forms.ComboBox d = (c as System.Windows.Forms.ComboBox);
      //  int pos = d.SelectionStart;
      //  d.Text = FilterString(d.Text);
      //  d.SelectionStart = pos;
      //}
    }

    public static string FilterString(string raw) {
      string filtered = raw.ToUpper();
      char[,] chars = new char[,] {
                     {'\u0027', '\u2032'},
                     {'\u0022', '\u2033'},
                     {';', '\u037E'},
                     {'%', '\u066A'},
                     {'*', '\u2217'}};

      for (int j = 0; j < chars.GetLength(0); j++) {
        filtered = filtered.Replace(chars[j, 0], chars[j, 1]);
      }

      return filtered;
    }

    public static string FilterString(string raw, bool flame) {
      string filtered = string.Empty;

      if (flame) {
        filtered = raw.ToUpper();
      } else {
        filtered = raw;
      }
      char[,] chars = new char[,] {
                     {'\u0027', '\u2032'},
                     {'\u0022', '\u2033'},
                     {';', '\u037E'},
                     {'%', '\u066A'},
                     {'*', '\u2217'}};

      for (int j = 0; j < chars.GetLength(0); j++) {
        filtered = filtered.Replace(chars[j, 0], chars[j, 1]);
      }

      return filtered;
    }

    public int UpdateMachinePrograms() {
      return -1;
    }

    private string ReturnString(OdbcDataReader dr, int i) {
      if (dr.Read()) {
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
