using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
namespace SQL
{
    /// <summary>
    /// Summary description for SQL_Helper
    /// </summary>
    public class SQL_Helper
    {
        private static string CNN_STRING;
        private static int COMMAND_TIMEOUT;
        static SQL_Helper()
        {
            CNN_STRING = ConfigurationManager.ConnectionStrings["ConnSQL"].ConnectionString;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["COMMAND_TIMEOUT"]))
            {
                COMMAND_TIMEOUT = Convert.ToInt32(ConfigurationManager.AppSettings["COMMAND_TIMEOUT"]);
            }
            else
            {
                COMMAND_TIMEOUT = 120;
            }
        }
        public static SqlConnection GetDB_Connection()
        {
            return new SqlConnection(CNN_STRING);
        }
        public static SqlConnection GetDB_Connection(string strCnn)
        {
            return new SqlConnection(strCnn);
        }
        public static SqlCommand GetDB_Command(string strCmd)
        {
            SqlCommand cmd = new SqlCommand(strCmd, GetDB_Connection());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = COMMAND_TIMEOUT;
            return cmd;
        }
        public static SqlCommand GetDB_Command(string strCmd, string strCnn)
        {
            SqlCommand cmd = new SqlCommand(strCmd, GetDB_Connection(strCnn));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = COMMAND_TIMEOUT;
            return cmd;
        }
        public static SqlCommand GetDB_Command(string strCmd, DbConnection cnn)
        {
            SqlCommand cmd = new SqlCommand(strCmd, (SqlConnection)cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = COMMAND_TIMEOUT;
            return cmd;
        }
        public static SqlCommand GetDB_Command(string strCmd, DbConnection cnn, DbTransaction tran)
        {
            SqlCommand cmd = new SqlCommand(strCmd, (SqlConnection)cnn, (SqlTransaction)tran);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = COMMAND_TIMEOUT;
            return cmd;
        }
        public static SqlCommand GetDB_CommandNoConnection(string strCmd)
        {
            SqlCommand cmd = new SqlCommand(strCmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = COMMAND_TIMEOUT;
            return cmd;
        }
        public static SqlParameter Get_OutputParam(string param_name, SqlDbType param_type)
        {
            SqlParameter param = new SqlParameter(param_name, param_type);
            param.Direction = ParameterDirection.Output;
            return param;
        }
        public static SqlParameter Get_OutputParam(string param_name, SqlDbType param_type, int param_size)
        {
            SqlParameter param = new SqlParameter(param_name, param_type);
            param.Direction = ParameterDirection.Output;
            param.Size = param_size;
            return param;
        }
        public static SqlParameter Get_Param(string param_name, object param_value)
        {
            return new SqlParameter(param_name, param_value);
        }
        public static DataTable GetDataTable(string strCmd)
        {
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(CNN_STRING))
            {
                using (SqlCommand cmd = new SqlCommand(strCmd))
                {
                    using (DbDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            dt = new DataTable();
                            cmd.Connection = conn;
                            cmd.CommandTimeout = COMMAND_TIMEOUT;
                            conn.Open();
                            adp.Fill(dt);
                            cmd.Connection.Close();
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            if (conn.State == ConnectionState.Open)
                                conn.Close();
                            throw ex;
                        }
                    }
                }
            }
            return dt;
        }
        public static DataTable GetDataTable(DbCommand cmd)
        {
            DataTable dt = null;
            using (SqlDataAdapter adp = new SqlDataAdapter((SqlCommand)cmd))
            {
                try
                {
                    dt = new DataTable();
                    cmd.CommandTimeout = COMMAND_TIMEOUT;
                    cmd.Connection.Open();
                    adp.Fill(dt);
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                    throw ex;
                }
            }
            return dt;
        }
        public static DataSet GetDataSet(DbCommand cmd)
        {
            DataSet ds = null;
            using (SqlDataAdapter adp = new SqlDataAdapter((SqlCommand)cmd))
            {
                try
                {
                    ds = new DataSet();
                    cmd.CommandTimeout = COMMAND_TIMEOUT;
                    cmd.Connection.Open();
                    adp.Fill(ds);
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                    throw ex;
                }
            }
            return ds;
        }
        public enum FieldType
        {
            DATE, MONEY, INT, DOUBLE, STRING
        }
        public static object FieldToValue(object val, FieldType DBType)
        {
            object result = null;
            if (val != DBNull.Value)
            {
                switch (DBType)
                {
                    case FieldType.STRING:
                        result = val.ToString();
                        break;
                    case FieldType.INT:
                        result = Convert.ToInt32(val);
                        break;
                    case FieldType.DOUBLE:
                        result = Convert.ToDouble(val);                        
                        break;
                }
            }
            return result;
        }
        public static string FieldToString(object val, FieldType DBType)
        {
            string strVal = string.Empty;
            if (val != null && val != DBNull.Value)
            {
                switch (DBType)
                {
                    case FieldType.DATE:
                        strVal = Convert.ToDateTime(val).ToString("MM/dd/yyyy");
                        break;
                    case FieldType.MONEY:
                        strVal = Convert.ToDouble(val).ToString("#,##0.00");
                        break;
                }

            }
            return strVal;
        }

        public static string GetErrorMessage(SqlDataReader reader)
        {
            string err_msg = string.Empty;
            if (reader != null)
            {
                while (reader.Read())
                {
                    if (string.Equals("ERROR_MSG", reader.GetName(0), StringComparison.OrdinalIgnoreCase))
                    {
                        err_msg = reader[0].ToString();
                    }
                }
            }
            return err_msg;
        }
        #region PROPERTIES
        #endregion
    }
}