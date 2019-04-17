using System;
using System.Data.SqlClient;

namespace patient.demography.helpers
{
    public class dbcontext
    {
        private bool _isDisposed;
        private SqlConnection _mainConnection;
        private SqlTransaction _mainTransaction;
        private bool newconnection = false;
        private bool newtransaction = false;

        public dbcontext(string Connectionstring)
        {
            newconnection = true;
            initclass(Connectionstring);
        }

        public dbcontext(SqlConnection Connection)
        {
            newconnection = false;
            initclass(Connection);
        }

        public SqlConnection Connection
        {
            get { return _mainConnection; }
        }

        public const string GetIdentity = " SELECT CAST(SCOPE_IDENTITY() as int); ";

        public string PagingStatement
        {
            get { return " OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY"; }
        }



        public SqlTransaction Transaction
        {
            get { return _mainTransaction; }
        }

        public void BeginTransaction(SqlTransaction transaction)
        {
            newtransaction = false;
            _mainTransaction = transaction;
            return;
        }

        public void BeginTransaction()
        {
            newtransaction = true;
            _mainTransaction = _mainConnection.BeginTransaction();
            return;
        }

        public void Commit()
        {
            _mainTransaction.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool bIsDisposing)
        {
            if (!_isDisposed)
            {
                if (bIsDisposing && newtransaction)
                {
                    if (_mainTransaction != null)
                    {
                        _mainTransaction.Dispose();
                        _mainTransaction = null;
                    }
                }

                if (bIsDisposing && newconnection)
                {
                    if (_mainConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _mainConnection.Close();
                    }

                    _mainConnection.Dispose();
                    _mainConnection = null;
                }
            }
            _isDisposed = true;
        }

        private void initclass(string ConnectionString)
        {
            try
            {
                if (ConnectionString.IsTrulyEmpty())
                {
                    throw new ArgumentNullException("Connection string not found");
                }
                _mainConnection = new SqlConnection(ConnectionString);
                _mainConnection.Open();

                _isDisposed = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void initclass(SqlConnection Connection)
        {
            try
            {
                _mainConnection = Connection;
                //_mainConnection.Open();

                _isDisposed = false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
