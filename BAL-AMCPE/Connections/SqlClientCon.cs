using System;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace BAL_AMCPE.Connections
{
    public class SqlCon : IDisposable
    {
        private SqlConnection _con;
        private SqlTransaction _trans;
        private String _connectionString = WebConfigurationManager.AppSettings["GMEEDEVEntities"];
        private String _amcConnectionString = WebConfigurationManager.AppSettings["AMCEntities"];

        public SqlCon(String pSqlConnectionString)
        {
            _connectionString = pSqlConnectionString;
            _con = new SqlConnection(pSqlConnectionString);
        }
        public SqlCon()
        {
            _con = new SqlConnection(_amcConnectionString);
        }
        public SqlConnection ConnectionObject
        {
            get
            {
                return _con;
            }
            set
            {
                _con = value;
            }
        }
        public String ConnectionString
        {
            get
            {
                return _con.ConnectionString;
            }
        }
        public SqlTransaction TransactionObject
        {
            get
            {
                return _trans;
            }
        }
        public void Open()
        {
            try
            {
                if (_con.State == ConnectionState.Closed)
                {
                    _con.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Close()
        {
            try
            {
                if (_con.State == ConnectionState.Open)
                {
                    _con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BeginTransaction()
        {
            try
            {
                if (_con.State == ConnectionState.Closed)
                {
                    _con.Open();
                }
                _trans = _con.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CommitTransaction()
        {
            try
            {
                _trans.Commit();
                _con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RollBackTransaction()
        {
            try
            {
                _trans.Rollback();
                _con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            try
            {
                GC.SuppressFinalize(this);
                this.Dispose(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual void Dispose(bool pDisposable)
        {
            try
            {
                if (pDisposable)
                {
                    if (this._con != null)
                        this._con.Dispose();
                    if (this._trans != null)
                        this._trans.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
