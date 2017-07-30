using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DapperExtensions;
using Dapper;
using System.Data;
using Laplace.Framework.Log;
using System.Collections;
namespace Laplace.Framework.Orm
{
    public static class SqlHelper
    {
        #region----
        #endregion--
        /// <summary>
        /// 插入实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Insert<T>(T parameter, string connectionString, out string errMsg) where T : class
        {
            errMsg = string.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    sqlConnection.Insert(parameter);
                    sqlConnection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError4Exception(ex);
                return false;
            }
        }
        /// <summary>
        /// 插入实体对象，返回自增主键值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int InsertWithReturnId<T>(T parameter, string connectionString) where T : class
        {
            string errMsg;
            return InsertWithReturnId(parameter, connectionString, out errMsg);
        }
        public static int InsertWithReturnId<T>(T parameter, string connectionString,out string errMsg) where T : class
        {
            errMsg=String.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var recordId = sqlConnection.Insert(parameter);
                    sqlConnection.Close();
                    return recordId;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return -1;
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Update<T>(T parameter, string connectionString, out string errMsg) where T : class
        {
            errMsg = string.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    sqlConnection.Update(parameter);
                    sqlConnection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                errMsg = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool Delete<T>(T parameter, string connectionString, out string errMsg) where T : class
        {
            errMsg = string.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    sqlConnection.Delete(parameter);
                    sqlConnection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                errMsg = ex.Message;
                return false;
            }

        }
        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool Delete<T>(PredicateGroup predicate, string connectionString,out string errMsg) where T : class
        {
            errMsg = String.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    sqlConnection.Delete<T>(predicate);
                    sqlConnection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                errMsg = ex.Message;
                return false;
            }
            
        }
        /// <summary>
        /// 返回所有实体对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IList<T> GetList<T>(string connectionString) where T : class
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var result = sqlConnection.GetList<T>();
                    sqlConnection.Close();
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                //errMsg = ex.Message;
                return null;
            }
            
        }
        /// <summary>
        /// 根据谓词条件返回满足查询条件的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IList<T> GetList<T>(PredicateGroup predicate, string connectionString) where T : class
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var result = sqlConnection.GetList<T>(predicate).ToList();
                    sqlConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                //errMsg = ex.Message;
                return null;
            }
            //Logger.LogInfo("SqlHelper=" + connectionString);
            
        }
        /// <summary>
        /// 调用分页存储过程
        /// http://www.lanhusoft.com/Article/130.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView<T> GetPageData<T>(PageCriteria criteria, string connectionString, out string errMsg) where T : class
        {
            errMsg = String.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    string proName = "proc_GetPageData";
                    p.Add("TableName", criteria.TableName);
                    p.Add("PrimaryKey", criteria.PrimaryKey);
                    p.Add("Fields", criteria.Fields);
                    p.Add("Condition", criteria.Condition);
                    p.Add("CurrentPage", criteria.CurrentPage);
                    p.Add("PageSize", criteria.PageSize);
                    p.Add("Sort", criteria.Sort);
                    p.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    sqlConnection.Open();
                    var pageData = new PageDataView<T>();
                    pageData.Items = sqlConnection.Query<T>(proName, p, commandType: CommandType.StoredProcedure).ToList();
                    sqlConnection.Close();
                    pageData.TotalNum = p.Get<int>("RecordCount");
                    pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
                    pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
                    return pageData;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                //errMsg = ex.Message;
                return null;
            }
            
        }
        /// <summary>
        /// 分页查询，返回DataTable
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="connectionString"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static PageDataView GetPageDataDataTable(PageCriteria criteria, string connectionString, out string errMsg) 
        {
            errMsg = String.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    var p = new DynamicParameters();
                    string proName = "proc_GetPageData";
                    p.Add("TableName", criteria.TableName);
                    p.Add("PrimaryKey", criteria.PrimaryKey);
                    p.Add("Fields", criteria.Fields);
                    p.Add("Condition", criteria.Condition);
                    p.Add("CurrentPage", criteria.CurrentPage);
                    p.Add("PageSize", criteria.PageSize);
                    p.Add("Sort", criteria.Sort);
                    p.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    sqlConnection.Open();
                 
                    var pageData = new PageDataView();
                    IDataReader reader = sqlConnection.ExecuteReader(proName, p, commandType: CommandType.StoredProcedure);
                    DataTable table = new DataTable();
                    table.Load(reader);
                    pageData.DataTable = table;
                    sqlConnection.Close();
                    pageData.TotalNum = p.Get<int>("RecordCount");
                    pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
                    pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
                    return pageData;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                //errMsg = ex.Message;
                return null;
            }

        }
        /// <summary>
        /// 返回满足条件的实体，若执行失败返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static T GetModel<T>(PredicateGroup predicate, string connectionString) where T : class
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var result = sqlConnection.GetList<T>(predicate).FirstOrDefault();
                    sqlConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                //errMsg = ex.Message;
                return null;
            }
            //Logger.LogInfo("SqlHelper=" + connectionString);
            
        }
        public static T GetModel<T>(PredicateGroup predicate, string connectionString,out string errMsg) where T : class
        {
            errMsg = string.Empty;
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var result = sqlConnection.GetList<T>(predicate).FirstOrDefault();
                    sqlConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                errMsg = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 返回满足查询条件的记录个数，若执行失败，返回-1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int GetCount<T>(PredicateGroup predicate, string connectionString) where T : class
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var result = sqlConnection.Count<T>(predicate);
                    sqlConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
                //errMsg = ex.Message;
                return -1;
            }
            
        }

  
        public static bool BulkCopy(DataTable dt, string connectionString)
        {
            if (dt == null)
            {
                return false;
            }
            var ret = false;
            var connection = new SqlConnection(connectionString);
            var bulkCopy = new SqlBulkCopy(connection);
            try
            {

                bulkCopy.DestinationTableName = dt.TableName;
                bulkCopy.BatchSize = dt.Rows.Count;
                connection.Open();
                if (dt.Rows.Count != 0)
                    bulkCopy.WriteToServer(dt);
                ret = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
                bulkCopy.Close();
            }
            return ret;
        }
        public static IEnumerable<T> QuerySP<T>(string storedProcedure, dynamic param = null,
            dynamic outParam = null, SqlTransaction transaction = null,
            bool buffered = true, int? commandTimeout = null, string connectionString = null) where T : class
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var output = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);
            return output;
        }
        //public static IEnumerable<T> ExecuteSP<T>(string storedProcedure, dynamic param = null,
        //    dynamic outParam = null, SqlTransaction transaction = null,
        //    bool buffered = true, int? commandTimeout = null, string connectionString = null) where T : class
        //{
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    var output = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);
        //    return output;
        //}

        public static void Execute(DynamicParameters p, string procedureName, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(procedureName, p, null, null, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool Execute(string sql, string connectionString)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    sqlConnection.Execute(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
                return false;
            }

        }
        /// <summary>
        /// 执行SQL语句，返回int，如果失败返回null
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int? ExecuteScalar2Int(string sql, string connectionString)
        {
            int intValue = int.MinValue;
            object ret = ExecuteScalar(sql, connectionString);

            if (ret != null && int.TryParse(ret.ToString(), out intValue))
            {
                return intValue;
            }
            else
            {
                return null;
            }
        }
        public static object ExecuteScalar(string sql, string connectionString)
        {
            object ret = null;
            try
            {

                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    ret = sqlConnection.ExecuteScalar(sql);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
            }
            return ret;
        }

        /// <summary>
        /// 执行sql语句,返回查询结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, string connectionString)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand cmd = sqlConnection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, "AppLogger");
                return null;
            }
        }
        private static void CombineParameters(ref dynamic param, dynamic outParam = null)
        {
            if (outParam != null)
            {
                if (param != null)
                {
                    param = new DynamicParameters(param);
                    ((DynamicParameters)param).AddDynamicParams(outParam);
                }
                else
                {
                    param = outParam;
                }
            }
        }

        private static int ConnectionTimeout { get; set; }

        private static int GetTimeout(int? commandTimeout = null)
        {
            if (commandTimeout.HasValue)
                return commandTimeout.Value;

            return ConnectionTimeout;
        }


        #region 执行返回DataReader的SQL命令

        /// <summary>
        /// 枚举常量指出数据库连接对象的所属，从而为GetReader提供恰当的CommandBehavior参数
        /// </summary>
        private enum SqlConnectionOwnership
        {
            /// <summary>数据库连接对象属于SqlHelper并由它来管理</summary>
            Internal,
            /// <summary>数据库连接对象属于外部调用并由外部调用来管理</summary>
            External
        }

        /// <summary>
        /// 创建SQL命令对象并绑定，使用恰当的CommandBehavior参数调用GetReader方法
        /// </summary>
        /// <remarks>
        /// 如果数据库连接对象是由SqlHelper创建打开的，那么将在DataReader对象关闭的时候一起关闭
        /// 如果数据库连接对象是由调用者提供的，那么将有调用者负责管理
        /// </remarks>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="transaction">事物对象(或者为null)</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名或者T-SQL命令）</param>
        /// <param name="commandParameters">参数数组</param>
        /// <param name="connectionOwnership">数据库连接对象所有者标记</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        private static SqlDataReader GetReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;

            // 创建命令对象并且绑定
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection, 30);

                // 创建阅读器
                SqlDataReader dataReader;

                // 使用恰当的CommandBehavior参数调用GetReader方法
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // 清除SQL命令对象的参数
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader can磘 set its values. 
                // When this happen, the parameters can磘 be used again in other command.
                // 注意:问题在于当reader被关闭时output类型的参数值是可以被引用的，
                // 所以如果参数数组被从SQL命令对象中分离掉了，SqlReader将不能再给它赋值
                // 这种情况一旦发生，参数数组将不能再被其他命令对象使用
                bool canClear = true;
                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }

        public static SqlDataReader GetReader(string connectionString, string commandText)
        {
            // 设定参数数组为null，调用重载方法
            return GetReader(connectionString, CommandType.Text, commandText, (SqlParameter[])null);
        }
        /// <summary>
        /// 在给定的数据库连接串下根据的类型、文本（存储过程名或者T-SQL命令）执行获取DataReader的SQL命令
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(connString, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名或者T-SQL命令）</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(string connectionString, CommandType commandType, string commandText)
        {
            // 设定参数数组为null，调用重载方法
            return GetReader(connectionString, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 在给定的数据库连接串下根据的类型、文本（存储过程名或者T-SQL命令）和参数数组执行获取DataReader的SQL命令
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名或者T-SQL命令）</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // 调用重载方法，事务参数为null，指定数据库对象由SqlHelper管理
                return GetReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                // 调用方法失败，关闭数据库对象
                if (connection != null) connection.Close();
                throw;
            }

        }

        /// <summary>
        /// 在指定的数据库连接串下根据的存储过程名称、值数组执行获取DataReader的SQL命令
        /// 此方法将先根据连接和存储过程名称获得参数数组（从数据库或者缓存），再将值数组分配到参数数组中
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(connString, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameterValues">值数组</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(string connectionString, string spName, params object[] parameterValues)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 值数组不为空
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return GetReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 调用没有参数数组为参数的重载方法
                return GetReader(connectionString, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 在给定的数据库连接下根据的类型、文本（存储过程名或者T-SQL命令）执行获取DataReader的SQL命令
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名或者T-SQL命令）</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // 设定参数数组为null，调用重载方法
            return GetReader(connection, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 在给定的数据库连接下根据的类型、文本（存储过程名或者T-SQL命令）和参数数组执行获取DataReader的SQL命令
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名或者T-SQL命令）</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            // 调用重载方法，事务参数为null，指定数据库对象由调用者管理
            return GetReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// 在指定的数据库连接下根据的存储过程名称、值数组执行获取DataReader的SQL命令
        /// 此方法将先根据连接和存储过程名称获得参数数组（从数据库或者缓存），再将值数组分配到参数数组中
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(conn, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameterValues">值数组</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 值数组不为空
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return GetReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 调用没有参数数组为参数的重载方法
                return GetReader(connection, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 在给定事物的数据库连接下根据的类型、文本（存储过程名或者T-SQL命令）执行获取DataReader的SQL命令 
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="transaction">事物对象</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名或者T-SQL命令）</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // 设定参数数组为null，调用重载方法
            return GetReader(transaction, commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 在给定事物的数据库连接下根据的类型、文本（存储过程名或者T-SQL命令）和参数数组执行获取DataReader的SQL命令 
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///   SqlDataReader dr = GetReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="transaction">事物对象</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名或者T-SQL命令）</param>
        /// <param name="commandParameters">参数数组</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            // 调用重载方法，指定数据库对象由调用者管理
            return GetReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        /// <summary>
        /// 在指定事物的数据库连接下根据的存储过程名称、值数组执行获取DataReader的SQL命令
        /// 此方法将先根据连接和存储过程名称获得参数数组（从数据库或者缓存），再将值数组分配到参数数组中
        /// </summary>
        /// <remarks>
        /// 举例： 
        ///  SqlDataReader dr = GetReader(trans, "GetOrders", 24, 36);
        /// </remarks>
        /// <param name="transaction">事物对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameterValues">值数组</param>
        /// <returns>返回保存执行结果的DataReader</returns>
        public static SqlDataReader GetReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 值数组不为空
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return GetReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                // 调用没有参数数组为参数的重载方法
                return GetReader(transaction, CommandType.StoredProcedure, spName);
            }
        }

        #endregion 执行返回DataReader的SQL命令
        #region 私有工具方法 构造函数

        //此类只提供静态方法，构造函数应当是私有的
        //private SqlHelper() { }

        /// <summary>
        /// 将参数数组绑定到SQL命令上 
        /// 输入类型的参数（InputOutput和Input）为null时转换为DBNull
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <param name="commandParameters">参数数组</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查参数类型是否是输入类型并且为null
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// 将dataRow的字段值分配到参数数组的各项上
        /// </summary>
        /// <param name="commandParameters">参数数组</param>
        /// <param name="dataRow">保存着存储过程参数值的DataRow</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // 没有数据时直接返回
                return;
            }

            int i = 0;
            // 设置参数值
            foreach (SqlParameter commandParameter in commandParameters)
            {
                // 检查参数名称
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new Exception(
                        string.Format(
                            "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                            i, commandParameter.ParameterName));
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary>
        /// 将值数组的各项分配到参数数组的各项上
        /// </summary>
        /// <param name="commandParameters">参数数组</param>
        /// <param name="parameterValues">值数组</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // 没有数据时直接返回
                return;
            }

            // 参数数组的长度和值数组的长度必须相等
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // 将值数组中的值分配到参数数组对应位置的参数中
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // 如果值数组兼容于IDbDataParameter类型，则分配值数组的Value属性
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary>
        /// 将数据库连接，事务处理，命令文本，命令类型，和命令参数绑定到将要执行的SQL命令上
        /// </summary>
        /// <param name="command">SQL命令</param>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandType">类型</param>
        /// <param name="commandText">文本（存储过程名称或者SQL语句）</param>
        /// <param name="commandParameters">命令参数</param>
        /// <param name="mustCloseConnection">在本方法内打开数据库连接时为true，否则为false</param>
        /// <param name="commandTimeout">CommandTimeout,默认30</param>
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText
            , SqlParameter[] commandParameters, out bool mustCloseConnection, int commandTimeout /*= 30*/)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // 将没有打开的数据库连接设为打开状态
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // 设置数据库连接
            command.Connection = connection;

            // 设置命令文本(存储过程名称或者SQL语句)
            command.CommandText = commandText;

            command.CommandTimeout = commandTimeout;
            // 设置事务处理(如果提供了事务)
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // 设置命令类型
            command.CommandType = commandType;

            // 设置命令参数(如果提供了命令参数)
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        #endregion 私有工具方法 构造函数
    }
    /// <summary>
    /// 存储过程参数数组缓存、获取类
    /// </summary>
    public sealed class SqlHelperParameterCache
    {
        #region 私有方法 变量 构造函数

        //此类只提供静态方法，构造函数应当是私有的
        private SqlHelperParameterCache() { }

        //在哈希表中静态保存的存储过程参数数组
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 根据给定的数据库连接和存储过程名查询数据库名称获取参数数组信息
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">返回</param>
        /// <returns>返回存储过程参数数组</returns>
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // 初始化数组中每个参数值为DBNull
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        /// <summary>
        /// 全部复制参数数组
        /// </summary>
        /// <param name="originalParameters">参数数组源</param>
        /// <returns>复制的参数数组</returns>
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion 私有方法 变量 构造函数

        #region 缓存方法

        /// <summary>
        /// 以数据库连接串和存储过程名称为键值将存储过程参数数组保存到缓存中(静态哈希表)
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数数组</param>
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// 根据数据库连接字符串和存储过程名从缓存中获得参数数组
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="commandText">存储过程名称</param>
        /// <returns>返回存储过程参数数组</returns>
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion 缓存方法

        #region 存储过程参数数组获取方法
        /// <summary>
        /// 根据数据库连接字符串和存储过程名称查找存储过程参数列表信息
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="spName">存储过程名称</param>
        /// <returns>返回存储过程参数数组</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// 根据数据库连接串创建连接后再根据存储过程名称在数据库查找存储过程参数列表信息
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">返回型参数（OUT/IN OUT）是否包含在返回结果中的标志</param>
        /// <returns>返回存储过程参数数组</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// 根据数据库连接和存储过程名称库查找存储过程参数列表信息
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="spName">存储过程名称</param>
        /// <returns>返回存储过程参数数组</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// 根据给定的数据库连接创建备份并在备份连接下根据存储过程名称在数据库查找存储过程参数列表信息
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">返回型参数（OUT/IN OUT）是否包含在返回结果中的标志</param>
        /// <returns>返回存储过程参数数组</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// 在指定数据库连接下根据存储过程名称在数据库查找存储过程参数列表信息
        /// 并且将此信息保存在缓存中以备后用
        /// </summary>
        /// <param name="connection">数据库连接对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">返回型参数（OUT/IN OUT）是否包含在返回结果中的标志</param>
        /// <returns>返回存储过程参数数组</returns>
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            // 用数据库连接字符串和一个标识（是否包含返回结果）作为哈希表键值
            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters;

            // 如果哈希表键值存在于缓存中，则从缓存中取得参数数组，否则将连接数据库查找相关信息
            cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;         //将结果缓存于哈希表中
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion 存储过程参数数组获取方法

    }


    /// <summary>
    /// 定义一个用来装载适合所有类的分页结果类
    /// PageDataView的Items一个泛型属性，所以可以适合所有的类，简洁而通用
    /// http://www.lanhusoft.com/Article/130.html
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataView<T>
    {
        public int TotalNum { get; set; }
        /// <summary>
        /// 转换为DataTable
        /// </summary>
        public DataTable DataTable { get; set; }
        //=> Items.ConvertToDataTable();

        public IList<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public int TotalPageCount { get; set; }
    }

    public class PageDataView
    {
        public int TotalNum { get; set; }
        /// <summary>
        /// 查询结果
        /// </summary>
        public DataTable DataTable { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPageCount { get; set; }
    }
    /// <summary>
    /// PageCriteria是一个封装查询条件相关信息的类
    /// http://www.lanhusoft.com/Article/130.html
    /// </summary>

    public class PageCriteria
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 查询字段
        /// </summary>
        public string Fields { get; set; }= "*";
        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey { get; set; } = "";

        /// <summary>
        /// 排序(SQL 语句)
        /// </summary>
        public string Sort { get; set; } = string.Empty;
        /// <summary>
        /// 查询条件(SQL 语句)
        /// </summary>
        public string Condition { get; set; } = string.Empty;

        /// <summary>
        /// 每页记录条数
        /// </summary>
        public int PageSize { get; set; } = 10;
        
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// 记录数
        /// </summary>
        public int RecordCount { get; set; } = 0;

        
        
    }
}
