using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Laplace.LiteCOS.DataBaseTool
{
    public partial class MainForm : Form
    {
        private string ConnectionString = string.Empty;
        private string _errMsg = string.Empty;
        private SqlHelper sqlHelper = new SqlHelper();
        private readonly List<string>  _listTableSql = new List<string>();
        private readonly List<string> _listInitDataSql = new List<string>();
        private readonly List<string> _listProcedureSql = new List<string>();
        private const string DefaultDbName = "LiteCOS";


        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            #region--读取数据库连接配置--
            var myreg = Registry.LocalMachine.OpenSubKey("Software\\LiteCOS.数据库工具");
            if (myreg != null)
            {
                txtServer.Text = myreg.GetValue("DbServer").ToString();
                txtUserName.Text = myreg.GetValue("UserName").ToString();
                txtPwd.Text = myreg.GetValue("Password").ToString();
                myreg.Close();
            }
            #endregion-读取数据库连接配置-
            //加载本地脚本文件
            cboTable.Items.Clear();
            DirectoryInfo sqlFolder = new DirectoryInfo(Application.StartupPath + @"\\SQLFile\\Table\\");
            //遍历文件夹
            foreach (var file in sqlFolder.GetFiles())
            {
                _listTableSql.Add(file.FullName);
                cboTable.Items.Add(file.Name);
            }
            cboTable.SelectedIndex = 0;

            cboInitData.Items.Clear();
            sqlFolder = new DirectoryInfo(Application.StartupPath + @"\\SQLFile\\InitData\\");
            foreach (var file in sqlFolder.GetFiles())
            {
                _listInitDataSql.Add(file.FullName);
                cboInitData.Items.Add(file.Name);
            }
            cboInitData.SelectedIndex = 0;
            //存储过程
            cboProcedure.Items.Clear();
            sqlFolder = new DirectoryInfo(Application.StartupPath + @"\\SQLFile\\Procedure\\");
            foreach (var file in sqlFolder.GetFiles())
            {
                _listProcedureSql.Add(file.FullName);
                cboProcedure.Items.Add(file.Name);
            }
            cboProcedure.SelectedIndex = 0;

        }
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLinkDb_Click(object sender, EventArgs e)
        {
            InitDbConn(DefaultDbName);
        }
        private void btnCreateDb_Click(object sender, EventArgs e)
        {
            var dbName = txtCatalog.Text.Trim();
            if (string.IsNullOrEmpty(dbName))
            {
                return;
            }
            if (sqlHelper.Connection == null) InitDbConn("");
            if (sqlHelper.Connection == null)
            {
                return;
            }
            string strSQL = String.Format("USE [master] SELECT name FROM sys.databases WHERE name = N'{0}'"
                , dbName);
            var name = sqlHelper.GetStringValue(strSQL, out _errMsg);
            if (!string.IsNullOrEmpty(_errMsg))
            {
                MessageBox.Show(_errMsg);
                return;
            }
            if (!string.IsNullOrEmpty(name))
            {
                if (MessageBox.Show("数据库 " + dbName + " 已存在，继续执行新建数据库会覆盖原来的数据库，丢失原来的数据。" + Environment.NewLine +
                    "确定继续执行新建数据库吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }


            this.Cursor = Cursors.WaitCursor;
            var sqlString = "declare   @d   varchar(8000) "
                            + " set @d = ' '"
                            + " select @d = @d + '   kill   ' + cast(spid as varchar) + char(13)"
                            + " from master.sys.sysprocesses"
                            + " where dbid = db_id('"+ dbName +"')"
                            + "exec(@d)";
            sqlHelper.ExecuteCommand(sqlString,out _errMsg);
            var sql = LoadSqlFile(Application.StartupPath + @"\\SQLFile\\CreateDb.sql", DefaultDbName, dbName);
            if (!sqlHelper.ExecuteCommand(sql, out _errMsg))
            {
                AddLog(_errMsg);
                this.Cursor = Cursors.Default;
                return;
            }
            InitCatalog(dbName);
            if (!CreateAllTable())
            {
                MessageBox.Show("创建表失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            if (!InitAllData())
            {
                MessageBox.Show("初始化数据失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            if (!CreateAllProcedure())
            {
                MessageBox.Show("创建存储过程失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            
            this.Cursor = Cursors.Default;

            MessageBox.Show("数据库创建完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void AddLog(string log)
        {
            txtLog.AppendText(log);
        }

        private ArrayList LoadSqlFile(string file, string findtext = "", string ReplaceText = "")
        {
            ArrayList alSql = new ArrayList();
            using (StreamReader sr = new StreamReader(file, Encoding.Default))
            {
                
                string commandText = "";
                string varLine = "";
                while (sr.Peek() > -1)
                {
                    varLine = sr.ReadLine();
                    if (varLine == "")
                    {
                        continue;
                    }

                    if (varLine != "GO")
                    {
                        commandText += Environment.NewLine + varLine;
                        commandText += " ";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(findtext))
                            commandText = commandText.Replace(findtext, ReplaceText);
                        alSql.Add(commandText);
                        commandText = "";
                    }
                }

                if (commandText != "")
                {
                    if (!string.IsNullOrEmpty(findtext))
                        commandText = commandText.Replace(findtext, ReplaceText);
                    alSql.Add(commandText);
                }
            }
            return alSql;

        }
        public string Replace(string strSource, string strRe, string strTo)
        {
            string strSl, strRl;
            strSl = strSource.ToLower();
            strRl = strRe.ToLower();
            int start = strSl.IndexOf(strRl);
            if (start != -1)
            {
                strSource = strSource.Substring(0, start) + strTo
                + Replace(strSource.Substring(start + strRe.Length), strRe, strTo);
            }
            return strSource;
        }
        private void InitDbConn(string selectedDb)
        {
            sqlHelper.ConnectionString = "server=" + txtServer.Text + "; database=master; User ID=" 
                + txtUserName.Text + "; Password=" + txtPwd.Text + ";";

            try
            {
                if (sqlHelper.ConnectDb())
                {
                    //GetDataBaseFile("创建数据库ePipeMonitor.sql");
                    MessageBox.Show("MSSQL 数据库连接成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    #region--保存数据库连接配置--

                    try
                    {
                        var myreg = Registry.LocalMachine.CreateSubKey("Software\\LiteCOS.数据库工具");
                        if (myreg != null)
                        {
                            myreg.SetValue("DbServer", txtServer.Text.Trim());
                            myreg.SetValue("UserName", txtUserName.Text.Trim());
                            myreg.SetValue("Password", txtPwd.Text.Trim());
                            myreg.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex);
                    }
                    
                    #endregion-保存数据库连接配置-

                    //初始化数据库文件列表
                    InitCatalog(selectedDb);
                }
                else
                {
                    sqlHelper.ConnectionString = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// 初始化数据库文件列表
        /// </summary>
        /// <param name="selectedName"></param>
        private void InitCatalog(string selectedName = DefaultDbName)
        {
            cboCatalog.Items.Clear();
            foreach (var name in sqlHelper.GetStringList("SELECT [name] FROM master.dbo.SYSDATABASES", out _errMsg))
            {
                cboCatalog.Items.Add(name);
            }
            if (!string.IsNullOrEmpty(selectedName))
            {
                cboCatalog.SelectedItem = selectedName;
            }
            
        }
        /// <summary>
        /// 选中数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            var catalog = cboCatalog.SelectedItem.ToString();
            sqlHelper.ConnectionString =  string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                    txtServer.Text, catalog, txtUserName.Text, txtPwd.Text); ;

            try
            {
                if (sqlHelper.ConnectDb())
                {
                    //GetDataBaseFile("创建数据库ePipeMonitor.sql");
                    MessageBox.Show(String.Format("数据库{0}连接成功!", catalog), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(String.Format("数据库{0}连接失败!", catalog), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlHelper.ConnectionString = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCreateAllTable_Click(object sender, EventArgs e)
        {
            if (sqlHelper.Connection == null) InitDbConn(DefaultDbName);
            if (sqlHelper.Connection == null)
            {
                return;
            }
            if (CreateAllTable())
            {
                MessageBox.Show("创建全部表成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("创建全部表失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// 执行全部脚本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInitAllData_Click(object sender, EventArgs e)
        {
            if (sqlHelper.Connection == null) InitDbConn(DefaultDbName);
            if (sqlHelper.Connection == null)
            {
                return;
            }
            if (InitAllData())
            {
                MessageBox.Show("初始化全部数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("初始化全部数据失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        /// <summary>
        /// 创建全部存储过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateAllProcedure_Click(object sender, EventArgs e)
        {
            if (sqlHelper.Connection == null) InitDbConn(DefaultDbName);
            if (sqlHelper.Connection == null)
            {
                return;
            }
            if (CreateAllProcedure())
            {
                MessageBox.Show("创建全部存储过程成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("创建全部存储过程失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }
        private bool CreateAllTable()
        {
            return ExecuteSqlFiles(_listTableSql);
        }
        private bool InitAllData()
        {
            return ExecuteSqlFiles(_listInitDataSql);
        }
        private bool CreateAllProcedure()
        {
            return ExecuteSqlFiles(_listProcedureSql);
        }
        private bool ExecuteSqlFiles(List<string> listFile)
        {
            foreach (var file in listFile)
            {
                var sql = LoadSqlFile(file);
                if (!sqlHelper.ExecuteCommand(sql, out _errMsg))
                {
                    AddLog(_errMsg);
                    return false;
                }
            }
            return true;
        }

    }
}
