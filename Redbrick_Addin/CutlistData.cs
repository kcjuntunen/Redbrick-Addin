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
                return 0;

            string SQL = string.Format("SELECT MATID FROM CUT_MATERIALS WHERE DESCR = '{0}'", description);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
                using (OdbcDataReader dr = comm.ExecuteReader()) {
                    if (dr.HasRows)
                        return dr.GetInt32(0);
                    else
                        return 0;
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

        public eco GetECOData(string ecoNumber) {
            eco e = new eco();
            string SQL = string.Format("SELECT GEN_USERS.FIRST, GEN_USERS.LAST, ECR_MAIN.CHANGES, " +
                "ECR_STATUS.STATUS, ECR_MAIN.ERR_DESC, ECR_MAIN.REVISION FROM " +
                "(ECR_MAIN LEFT JOIN GEN_USERS ON ECR_MAIN.REQ_BY = GEN_USERS.UID) " +
                "LEFT JOIN ECR_STATUS ON ECR_MAIN.STATUS = ECR_STATUS.STAT_ID WHERE " +
                "(((ECR_MAIN.[ECR_NUM])={0}));", ecoNumber);
            //conn.Open(); 
            using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
                using (OdbcDataReader dr = comm.ExecuteReader()) {
                    e.EcrNumber = int.Parse(ecoNumber);
                    if (dr.HasRows) {
                        e.Changes = ReturnString(dr, 2);
                        e.ErrDescription = ReturnString(dr, 4);
                        e.Revision = ReturnString(dr, 5);
                        e.Status = ReturnString(dr, 3);
                        e.RequestedBy = ReturnString(dr, 0) + " " + ReturnString(dr, 1);
                        dr.Close();
                    }
                }
            }
            return e;
        }

        public eco GetLegacyECOData(string ecoNumber) {
            eco e = new eco();
            // ECR_LEGACY.ECRNum is a string field.
            string SQL = string.Format("SELECT * FROM ECR_LEGACY WHERE (((ECR_LEGACY.ECRNum)='{0}'));", ecoNumber.Replace("'", "''")); 

            using (OdbcCommand comm = new OdbcCommand(SQL, conn)) {
                using (OdbcDataReader dr = comm.ExecuteReader()) {
                    if (dr.HasRows)	{
                        e.EcrNumber = int.Parse(ecoNumber);
                        e.Revision = ReturnString(dr, 5); // AffectedParts
                        e.Changes = ReturnString(dr, 6); // Change
                        string[] dateString = ReturnString(dr, 4).Split('/', ' '); // DateCompleted
                        System.DateTime dt = new DateTime(int.Parse(dateString[2]), int.Parse(dateString[0]), int.Parse(dateString[1]));
                        e.RequestedBy = ReturnString(dr, 7); // FinishedBy
                        e.Status = "Finished on " + dt.ToShortDateString();
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

        public int InsertIntoCutlist(SwProperties p) {
            int affectedRows = 0;
            if (ENABLE_DB_WRITE && p.Primary) {
                throw new NotImplementedException();
            }
            return affectedRows;
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

        public int UpdateCutlistParts(SwProperties p) {
            int rowsAffected = -1;
            string cmid = p.GetProperty("MATID").ID.Replace("'", "\"");
            string efid = p.GetProperty("EFID").ID.Replace("'", "\"");
            string ebid = p.GetProperty("EBID").ID.Replace("'", "\"");
            string elid = p.GetProperty("ELID").ID.Replace("'", "\"");
            string erid = p.GetProperty("ERID").ID.Replace("'", "\"");

            if (ENABLE_DB_WRITE && p.Primary && p.CutlistID != null && p.CutlistQuantity != null) {
                string SQL = string.Format("UPDATE CUT_CUTLIST_PARTS " +
                    "SET CUT_CUTLIST_PARTS.MATID = {0}, " +
                    "CUT_CUTLIST_PARTS.EDGEID_LF = {1}, " +
                    "CUT_CUTLIST_PARTS.EDGEID_LB = {2}, " +
                    "CUT_CUTLIST_PARTS.EDGEID_WR = {3}, " +
                    "CUT_CUTLIST_PARTS.EDGEID_WL = {4}, " +
                    "CUT_CUTLIST_PARTS.QTY = {5} " +
                    "FROM CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID " +
                    "WHERE (((CUT_CUTLIST_PARTS.CLID)={6}) AND ((CUT_PARTS.PARTNUM)='{7}'));",
                    cmid, efid, ebid, erid, elid, p.CutlistQuantity, p.CutlistID, p.PartName);

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
