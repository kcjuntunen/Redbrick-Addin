using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Redbrick_Addin {
  public class M2MData : IDisposable {
    private SqlConnection conn;

    public M2MData() {
      conn = new SqlConnection(Properties.Settings.Default.M2MConnection);
      try {
        conn.Open();
      } catch (Exception e) {
        throw new Exception(@"Couldn't connect to M2M.", e);
      }
    }

    public void Dispose() {
      conn.Close();
    }

    public bool Connected() {
      if (conn.State == ConnectionState.Open)
        return true;
      else
        return false;
    }

    public int GetPartCount(string prtnum, string rev) {
      int count = 0;
      string SQL = @"SELECT COUNT(fpartno) FROM inmast WHERE fpartno = @prtno AND frev = @prtrv;";
      if (Connected()) {
        using (SqlCommand comm = new SqlCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@prtno", prtnum);
          comm.Parameters.AddWithValue("@prtrv", rev);
          using (SqlDataReader dr = comm.ExecuteReader()) {
            if (dr.Read()) {
              count = dr.GetInt32(0);
            }
          }
        }
      }
      return count;
    }

    public string GetProductClass(string prtnum, string rev) {
      string prodcl = string.Empty;
      string SQL = @"SELECT fprodcl FROM inmast WHERE fpartno = @prtno AND frev = @prtrv;";
      if (Connected()) {
        using (SqlCommand comm = new SqlCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@prtno", prtnum);
          comm.Parameters.AddWithValue("@prtrv", rev);
          using (SqlDataReader dr = comm.ExecuteReader()) {
            if (dr.Read()) {
              prodcl = dr.GetString(0);
            }
          }
        }
      }
      return prodcl;
    }

    public bool GetPurchased(string prtnum, string rev) {
      bool purchased = false;
      string SQL = @"SELECT fcpurchase FROM inmast WHERE fpartno = @prtno AND frev = @prtrv;";
      if (Connected()) {
        using (SqlCommand comm = new SqlCommand(SQL, conn)) {
          comm.Parameters.AddWithValue("@prtno", prtnum);
          comm.Parameters.AddWithValue("@prtrv", rev);
          using (SqlDataReader d = comm.ExecuteReader()) {
            if (d.Read()) {
              purchased = d.GetString(0) == "Y" ? true : false;
            }
          }
        }
      }
      return purchased;
    }

    public int GetPartType(string prtno, string prtrv) {
      int parttype = 0;

      switch (GetProductClass(prtno, prtrv)) {
        case "01":
          if (prtno.StartsWith("Z"))
            parttype = 1;
          else
            parttype = 3;
          break;
        case "02":
          parttype = 3;
          break;
        case "03":
          if (prtno.StartsWith("Z"))
            parttype = 1;
          else
            parttype = 3;
          break;
        case "04":
          if (GetPurchased(prtno, prtrv))
            parttype = 4;
          else
            parttype = 3;
          break;
        case "09":
          if (GetPurchased(prtno, prtrv))
            parttype = 2;
          else
            parttype = 1;
          break;
        case "10":
          parttype = 4;
          break;
        default:
          parttype = 7;
          break;
      }
      return parttype;
    }

    public DataTable GetJobsDue() {
      string SQL = @"SELECT jomast.fjobno, jomast.fstatus, jomast.fpartno, jomast.fpartrev, jodrtg.foperqty, jomast.fddue_date,  jodrtg.fpro_id " +
        @"FROM jomast INNER JOIN jodrtg ON jomast.fjobno = jodrtg.fjobno " +
        //@"WHERE (jomast.fstatus NOT LIKE @crit) " +
        @"ORDER BY jomast.fjobno;";
      using (SqlCommand comm = new SqlCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@crit", "C%");
        using (SqlDataAdapter da = new SqlDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds.Tables[0];
          }
        }
      }
    }

    public DataTable GetJobData(string partno, string partrev) {
      string SQL = @"SELECT jomast.fjobno, jomast.fstatus, jomast.fpartno, jomast.fpartrev, jodrtg.foperqty, jomast.fddue_date " +
        @"FROM jomast INNER JOIN jodrtg ON jomast.fjobno = jodrtg.fjobno " +
        //"WHERE (jomast.fstatus NOT LIKE @crit) AND ((jomast.fpartno)= @partno ) AND ((jomast.fpartrev)= @partrev)) " +
        "WHERE ((jomast.fpartno)= @partno ) AND ((jomast.fpartrev)= @partrev)) " +
        @"ORDER BY jomast.fjobno;";
      using (SqlCommand comm = new SqlCommand(SQL, conn)) {
        comm.Parameters.AddWithValue("@crit", "C%");
        comm.Parameters.AddWithValue("@partno", partno);
        comm.Parameters.AddWithValue("@partrev", partrev);
        using (SqlDataAdapter da = new SqlDataAdapter(comm)) {
          using (DataSet ds = new DataSet()) {
            da.Fill(ds);
            return ds.Tables[0];
          }
        }
      }
    }

    // --------------------------------------------------------------------------
    private SqlCommand ConstructCommand(string SQL, string prtno, string prtrv) {
      SqlCommand comm = new SqlCommand(SQL, conn);
      comm.Parameters.AddWithValue("@prtno", prtno);
      comm.Parameters.AddWithValue("@prtrv", prtrv);
      return comm;
    }

    private int GetInt(SqlCommand comm) {
      int intval = 0;
      using (SqlDataReader dr = comm.ExecuteReader(CommandBehavior.SingleResult)) {
        if (dr.Read()) {
          intval = dr.GetInt32(0);
        }
      }
      return intval;
    }

    private string GetString(SqlCommand comm) {
      string stringval = string.Empty;
      using (SqlDataReader dr = comm.ExecuteReader(CommandBehavior.SingleResult)) {
        if (dr.Read()) {
          stringval = dr.GetString(0);
        }
      }
      return stringval;
    }

    private bool GetBoolFromStringMatch(SqlCommand comm, string expect) {
      bool res = false;
      using (SqlDataReader dr = comm.ExecuteReader(CommandBehavior.SingleResult)) {
        if (dr.Read()) {
          res = dr.GetString(0) == expect ? true : false;
        }
      }
      return res;
    }
  }
}
