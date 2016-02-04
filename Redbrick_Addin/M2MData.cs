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
    
    public int GetPartCount(string prtnum, string rev) {
      int count = 0;
      string SQL = @"SELECT COUNT(fpartno) FROM inmast WHERE fpartno = @prtno AND frev = @prtrv;";
      SqlCommand comm = ConstructCommand(SQL, prtnum, rev);
      count = GetInt(comm);
      return count;
    }

    public string GetProductClass(string prtnum, string rev) {
      string prodcl = string.Empty;
      bool purch = GetPurchased(prtnum, rev);
      string SQL = @"SELECT fprodcl FROM inmast WHERE fpartno = @prtno AND frev = @prtrv;";
      SqlCommand comm = ConstructCommand(SQL, prtnum, rev);
      prodcl = GetString(comm);
      return prodcl;
    }

    public bool GetPurchased(string prtnum, string rev) {
      bool purchased = false;
      string SQL = @"SELECT fcpurchase FROM inmast where fpartno = @prtno AND frev = @prtrv;";
      SqlCommand comm = ConstructCommand(SQL, prtnum, rev);
      purchased = GetBoolFromStringMatch(comm, "Y");
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
