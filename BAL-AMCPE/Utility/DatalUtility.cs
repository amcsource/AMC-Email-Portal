using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using BAL_AMCPE.Connections;

namespace BAL_AMCPE.Utils
{
    public static class DbUtility
    {
        #region "For SqlClient with optional transaction"
        public static DataTable GetDataTable(String pCommandText, SqlCon pSqlCon, SqlParameter[] pSqlParameters, System.Data.CommandType pCommandType, Boolean pTransactionEnabled)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                da = new SqlDataAdapter(pCommandText, pSqlCon.ConnectionObject);
                da.SelectCommand.CommandType = pCommandType;
                if ((pSqlParameters == null) == false)
                    if (pSqlParameters.Length > 0)
                        da.SelectCommand.Parameters.AddRange(pSqlParameters);
                if (pTransactionEnabled == false)
                    pSqlCon.Open();
                else
                    da.SelectCommand.Transaction = pSqlCon.TransactionObject;
                da.Fill(ds, "tbl");
                return (DataTable)ds.Tables["tbl"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds.Tables.Clear();
                ds.Dispose();
                da.Dispose();
                if (pTransactionEnabled == false)
                    pSqlCon.Close();
            }
        }
        public static object ExecuteScaller(String pCommandText, SqlCon pSqlCon, SqlParameter[] pSqlParameters, System.Data.CommandType pCommandType, Boolean pTransactionEnabled)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand(pCommandText, pSqlCon.ConnectionObject);
                cmd.CommandType = pCommandType;
                if ((pSqlParameters == null) == false)
                    if (pSqlParameters.Length > 0)
                        cmd.Parameters.AddRange(pSqlParameters);
                if (pTransactionEnabled == false)
                    pSqlCon.Open();
                else
                    cmd.Transaction = pSqlCon.TransactionObject;
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                if (pTransactionEnabled == false)
                    pSqlCon.Close();
            }
        }
        public static void ExecuteNonQuery(String pCommandText, SqlCon pSqlCon, SqlParameter[] pSqlParameters, System.Data.CommandType pCommandType, Boolean pTransactionEnabled)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand(pCommandText, pSqlCon.ConnectionObject);
                cmd.CommandType = pCommandType;
                if ((pSqlParameters == null) == false)
                    if (pSqlParameters.Length > 0)
                        cmd.Parameters.AddRange(pSqlParameters);
                if (pTransactionEnabled == false)
                    pSqlCon.Open();
                else
                    cmd.Transaction = pSqlCon.TransactionObject;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                if (pTransactionEnabled == false)
                    pSqlCon.Close();
            }
        }
        public static SqlDataReader ExecuteReader(String pCommandText, SqlCon pSqlCon, SqlParameter[] pSqlParameters, System.Data.CommandType pCommandType, Boolean pTransactionEnabled)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = new SqlCommand(pCommandText, pSqlCon.ConnectionObject);
                cmd.CommandType = pCommandType;
                if ((pSqlParameters == null) == false)
                    if (pSqlParameters.Length > 0)
                        cmd.Parameters.AddRange(pSqlParameters);
                if (pTransactionEnabled == true)
                {
                    cmd.Transaction = pSqlCon.TransactionObject;
                    return cmd.ExecuteReader();
                }
                else
                {
                    pSqlCon.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                if (pTransactionEnabled == false)
                    pSqlCon.Close();
            }
        }
        #endregion
    }
}
