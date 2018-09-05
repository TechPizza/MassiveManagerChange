using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataLayer : IDisposable
    {
        private SqlConnection _cn = new SqlConnection();

        public DataLayer()
        {
            _cn.ConnectionString = ConfigurationManager.ConnectionStrings["TentilabConnection"].ConnectionString;

            OpenConnection();
        }

        public void OpenConnection()
        {
            if (_cn.State != ConnectionState.Open)
                _cn.Open();
        }

        public void CloseConnection()
        {
            if (_cn.State != ConnectionState.Closed)
            {
                _cn.Close();
            }

        }

        public void Dispose()
        {
            CloseConnection();
        }

        public void CopyInActual()
        {
            SqlCommand _cmd = new SqlCommand("dbo.CopyInActual", _cn);
            _cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                _cmd.ExecuteNonQuery(); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _cmd = null;
            }
        }

        public void CalculateDelta()
        {
            SqlCommand _cmd = new SqlCommand("dbo.CalculateDelta", _cn);
            _cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                _cmd.ExecuteNonQuery(); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _cmd = null;
            }
        }

        public DataTable GetDeltaChanges()
        {
            SqlCommand _cmd = new SqlCommand("dbo.GetDeltaChanges", _cn);
            _cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader _dr = null;
            DataTable dt = new DataTable();
            try
            {
                _dr = _cmd.ExecuteReader();
                if (_dr.HasRows)
                    dt.Load(_dr);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dr.Close();
                _cmd = null;
            }

        }
    }
}
