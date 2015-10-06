using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Redbrick_Addin
{	
    public class CutlistData : IDisposable
    {
        private object threadLock = new object();

        private OdbcConnection conn;

        public OdbcConnection Connection
        {
            get { return conn; }
            private set { conn = value; }
        }
	
        public CutlistData()
        {
            conn = new OdbcConnection(Properties.Settings.Default.ConnectionString);
            this.OpType = 1;
            conn.Open();
        }

        public void Dispose()
        {
            conn.Close();
        }

        private DataSet GetMaterials()
        {
            if (this._materials == null)
            {
                lock (threadLock)
                {
#if DEBUG
                    DateTime start;
                    DateTime end;
                    start = DateTime.Now;
#endif
                    string SQL = "SELECT MATID,DESCR,COLOR FROM CUT_MATERIALS ORDER BY DESCR;";
                    using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                    {
                        using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                        {
                            using (DataSet ds = new DataSet())
                            {
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
            }
            else
	        {
		         return this._materials;
	        }
        }

        private DataSet GetEdges()
        {
            lock (threadLock)
            {
#if DEBUG
                DateTime start;
                DateTime end;
                start = DateTime.Now;
#endif
                string SQL = "SELECT EDGEID,DESCR,COLOR,THICKNESS FROM CUT_EDGES ORDER BY DESCR;";

                using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                {
                    using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                    {
                        using (DataSet ds = new DataSet())
                        {
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

        public DataSet GetOps(int opType)
        {
            lock (threadLock)
            {
                if (this.OpType == opType)
                {
                    this.OpType = opType;
                    string SQL = string.Format("SELECT OPID, OPNAME, OPDESCR, OPTYPE FROM CUT_PART_TYPES "
                        + "INNER JOIN CUT_OPS ON CUT_PART_TYPES.TYPEID = CUT_OPS.OPTYPE WHERE CUT_PART_TYPES.TYPEID = {0} ORDER BY OPDESCR", opType);
                    using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                    {
                        using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                        {
                            using (DataSet ds = new DataSet())
                            {
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
                }
                else
                {
                    return this.Ops;
                }
            }
        }

        public DataSet GetOps()
        {
            lock (threadLock)
            {
                string SQL =string.Format("SELECT * FROM CUT_OPS WHERE OPTYPE = {0} ORDER BY OPDESCR", this.OpType);
                using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                {
                    using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                    {
                        using (DataSet ds = new DataSet())
                        {

                            try
                            {
                                da.Fill(ds);
                            }
                            catch (StackOverflowException sofe)
                            {
                                throw sofe;
                            }
                            finally
                            {
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

        public DataSet GetOpTypes()
        {
            lock (threadLock)
            {
                string SQL = "SELECT * FROM CUT_PART_TYPES ORDER BY TYPEID";
                using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                {
                    using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            da.Fill(ds);
                            this.OpTypes = ds;
                            return this.OpTypes;
                        }
                    }
                }
            }
        }

        public DataSet GetWherePartUsed(int partID)
        {
            string SQL = string.Format("SELECT CUT_CUTLISTS.CLID, CUT_CUTLISTS.PARTNUM, CUT_CUTLISTS.REV, CUT_CUTLISTS.DESCR FROM " +
                "(CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID) INNER JOIN " +
                "CUT_CUTLISTS ON CUT_CUTLIST_PARTS.CLID = CUT_CUTLISTS.CLID WHERE " +
                "(((CUT_PARTS.PARTID)={0}));", partID);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                {
                    using (DataSet ds = new DataSet())
                    {
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        public DataSet GetWherePartUsed(string partDescr)
        {
            string SQL = string.Format("SELECT CUT_CUTLISTS.CLID, CUT_CUTLISTS.PARTNUM, CUT_CUTLISTS.REV, CUT_CUTLISTS.DESCR FROM " +
                "(CUT_CUTLIST_PARTS INNER JOIN CUT_PARTS ON CUT_CUTLIST_PARTS.PARTID = CUT_PARTS.PARTID) INNER JOIN " +
                "CUT_CUTLISTS ON CUT_CUTLIST_PARTS.CLID = CUT_CUTLISTS.CLID WHERE " +
                "(((CUT_PARTS.PARTNUM) Like '{0}'));", partDescr);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                {
                    using (DataSet ds = new DataSet())
                    {
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        public double GetEdgeThickness(int ID)
        {
            string SQL = string.Format("SELECT THICKNESS FROM CUT_EDGES WHERE EDGEID = {0}", ID.ToString());
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    if (dr.HasRows)
                        return dr.GetDouble(0);
                    else
                        return 0.0;
                }
            }
        }

        public int GetMaterialID(string description)
        {
            if (description == null)
                return 0;

            string SQL = string.Format("SELECT MATID FROM CUT_MATERIALS WHERE DESCR = '{0}'", description);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    if (dr.HasRows)
                        return dr.GetInt32(0);
                    else
                        return 0;
                }
            }
        }

        public int GetOpID(string description)
        {
            if (description == string.Empty)
                return 0;

            string SQL = string.Format("SELECT OPID FROM CUT_OPS WHERE OPDESCR = '{0}'", description);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    if (dr.HasRows)
                        return dr.GetInt32(0);
                    else
                        return 0;
                }
            }
        }

        public int GetEdgeID(string description)
        {
            if (description == string.Empty)
                return 0;

            string SQL = string.Format("SELECT EDGEID FROM CUT_EDGES WHERE DESCR = '{0}'", description);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    if (dr.HasRows)
                        return dr.GetInt32(0);
                    else
                        return 0;
                }
            }
        }

        public int GetOpIDByName(string name)
        {
            if (name == string.Empty)
                return 0;

            string SQL = string.Format("SELECT OPID FROM CUT_OPS WHERE OPNAME Like '{0}'", name);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    if (dr.HasRows)
                        return dr.GetInt32(0);
                    else
                        return 0;
                }
            }
        }

        public List<string> GetOpDataByName(string name)
        {
            if (name == string.Empty)
                return null;

            string SQL = string.Format("SELECT OPID, OPNAME, OPDESCR, OPTYPE FROM CUT_OPS WHERE OPNAME Like '{0}' AND OPTYPE = {1}", name, this.OpType);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        List<string> x = new List<string>();
                        x.Add(dr.GetString(0));
                        x.Add(dr.GetString(1));
                        x.Add(dr.GetString(2));
                        x.Add(dr.GetString(3));
                        return x;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public int GetOpTypeIDByName(string name)
        {
            if (name == string.Empty)
                return 1;

            string SQL = string.Format("SELECT TYPEID FROM CUT_PART_TYPES WHERE TYPEDESC Like '{0}'", name);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        int res = dr.GetInt32(0);
                        return res;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }

        public string GetMaterialByID(string id)
        {
            if (id == string.Empty)
                return "TBD MATERIAL";

            int tp = 0;
            if (int.TryParse(id, out tp))
            {

                string SQL = string.Format("SELECT DESCR FROM CUT_MATERIALS WHERE MATID = {0}", id);
                using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                {
                    using (OdbcDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                            return dr.GetString(0);
                        else
                            return "TBD MATERIAL";
                    }
                }
            }
            return "TBD MATERIAL";
        }

        public string GetEdgeByID(string id)
        {
            if (id == string.Empty)
                return string.Empty;

            int tp = 0;
            if (int.TryParse(id, out tp))
            {
                string SQL = string.Format("SELECT DESCR FROM CUT_EDGES WHERE EDGEID = {0}", id);
                using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                {
                    using (OdbcDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                            return dr.GetString(0);
                        else
                            return string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        public string GetOpByID(string id)
        {
            if (id == string.Empty)
                return string.Empty;

            int tp = 0;
            if (int.TryParse(id, out tp))
            {
                string SQL = string.Format("SELECT OPDESCR FROM CUT_OPS WHERE OPID = {0}", id);
                using (OdbcCommand comm = new OdbcCommand(SQL, conn))
                {
                    using (OdbcDataReader dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                            return dr.GetString(0);
                        else
                            return string.Empty;
                    }
                }
            }
            return string.Empty;
        }

        public eco GetECOData(string ecoNumber)
        {
            eco e = new eco();
            string SQL = string.Format("SELECT GEN_USERS.FIRST, GEN_USERS.LAST, ECR_MAIN.CHANGES, " +
                "ECR_STATUS.STATUS, ECR_MAIN.ERR_DESC, ECR_MAIN.REVISION FROM " +
                "(ECR_MAIN LEFT JOIN GEN_USERS ON ECR_MAIN.REQ_BY = GEN_USERS.UID) " + 
                "LEFT JOIN ECR_STATUS ON ECR_MAIN.STATUS = ECR_STATUS.STAT_ID WHERE " +
                "(((ECR_MAIN.[ECR_NUM])={0}));", ecoNumber);
            //conn.Open(); 
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {
                    e.EcrNumber = int.Parse(ecoNumber);
                    if (dr.HasRows)
                    {
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

        public DataSet GetAuthors()
        {
#if DEBUG
            DateTime start;
            DateTime end;
            start = DateTime.Now;
#endif
            string SQL = string.Format("SELECT GEN_USERS.* FROM GEN_USERS WHERE (((GEN_USERS.DEPT)={0}));", Properties.Settings.Default.UserDept);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(comm))
                {
                    using (DataSet ds = new DataSet())
                    {
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

        public string GetAuthorUserName(string initial)
        {
            if (initial == string.Empty)
                return string.Empty;

            string SQL = string.Format("SELECT username FROM GEN_USERS WHERE INITIAL LIKE '{0}%';", initial);
            using (OdbcCommand comm = new OdbcCommand(SQL, conn))
            {
                using (OdbcDataReader dr = comm.ExecuteReader())
                {

                    if (dr.HasRows)
                    {
                        string x = dr.GetString(0);
                        dr.Close();
                        return x;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        private string ReturnString(OdbcDataReader dr, int i)
        {
            if (dr.HasRows)
            {
                if (dr.IsDBNull(i))
                {
                    return string.Empty;
                }
                else
                {
                    string returnString = dr.GetValue(i).ToString();
                    return returnString;
                }
            }
            return string.Empty;
        }

        private DataSet _opTypes;

        public DataSet OpTypes
        {
            get
            {
                if (this._opTypes != null)
                {
                    return this._opTypes.Copy();
                }
                else
                {
                    this._opTypes = this.GetOpTypes();
                    return this._opTypes;
                }
            }
            private set { _opTypes = value; }
        }

        private DataSet _materials;

        public DataSet Materials
        {
            get { return this.GetMaterials(); }
            private set { _materials = value; }
        }

        private DataSet _edges;

        public DataSet Edges
        {
            get
            {
                if (this._edges != null)
                {
                    return this._edges.Copy();
                }
                else
                {
                    this._edges = this.GetEdges();
                    return this._edges;
                }
            }
            private set { _edges = value; }
        }

        private int _opType;

        public int OpType
        {
            get { return _opType; }
            set 
            {
                if (value > 2)
                    _opType = 1;
                else
                    _opType = value;
            }
        }


        private DataSet _Ops;

        public DataSet Ops
        {
            get
            {
                if ((this._Ops != null) && (this._Ops.Tables[0].Rows[0]["OPTYPE"].ToString() == this.OpType.ToString()))
                {
                    return this._Ops.Copy();
                }

                this._Ops = this.GetOps();
                return this._Ops = this._Ops.Copy();
            }
            private set { _Ops = value; }
        }
	
    }
}
