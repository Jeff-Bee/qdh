using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Laplace.LiteCOS.DataBaseTool
{
    public class SqlHelper
    {
        public SqlConnection Connection { get;private set; }
        public string ConnectionString;
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public bool ConnectDb()
        {
            Connection = new SqlConnection(ConnectionString);
            try
            {
                Connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
        public DataTable GetDataTable(string sqlStr,out string errMsg)
        {
            errMsg = String.Empty;
            DataSet dsDrop = new DataSet();

            SqlDataAdapter adpDrop = default(SqlDataAdapter);
            adpDrop = new SqlDataAdapter();

            try
            {

                SqlCommand cmdGet = new SqlCommand(sqlStr, new SqlConnection(ConnectionString));
                adpDrop.SelectCommand = cmdGet;

                adpDrop.Fill(dsDrop);
                adpDrop.Dispose();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return (dsDrop != null && dsDrop.Tables.Count > 0 ? dsDrop.Tables[0] : null);
        }
        public List<string> GetStringList(string sqlStr,out string errMsg)
        {
            var ret= new List<string>();
            var dt = GetDataTable(sqlStr, out errMsg);
            if (!string.IsNullOrEmpty(errMsg) || dt == null)
            {
                return ret;
            }
            ret.AddRange(from DataRow row in dt.Rows select row[0].ToString());
            return ret;
        }
        /// <summary>
        /// 根据SQL语句返回 字符串
        /// </summary>
        public string GetStringValue(string sqlStr,out string errMsg)
        {
            errMsg = String.Empty;
            string R = "";
            SqlCommand cmdGet = new SqlCommand(sqlStr, Connection);
            try
            {
                if ((cmdGet.ExecuteScalar() == null))
                {
                    R = "";
                }
                else
                {
                    R = cmdGet.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            cmdGet.Dispose();
            //Conn.Close()
            return R;
        }

        public bool ExecuteCommand(string sql,out string errMsg)
        {
            errMsg = String.Empty;
            // SqlTransaction varTrans = Connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;

            // command.Transaction = varTrans;
            try
            {
                command.CommandTimeout = 3600;
                command.CommandText = sql;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        public bool ExecuteCommand(ArrayList varSqlList, out string errMsg)
        {
            errMsg = String.Empty;
            SqlCommand command = new SqlCommand();
            command.Connection = Connection;
            var errSql = string.Empty;
            try
            {
                foreach (string sql in varSqlList)
                {
                    errSql = sql;
                    if (!string.IsNullOrEmpty(sql))
                    {
                        command.CommandText = sql;
                        Debug.WriteLine(command.CommandText);
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("执行SQL语句失败:\r\n====================\r\n{0}\r\n==================\r\n", errSql);
                errMsg += string.Format("错误提示:{0}\r\n", ex.Message);
                return false;
            }
        }
    }
}
