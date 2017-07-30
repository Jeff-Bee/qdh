namespace Laplace.LiteCOS.DataBaseTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboCatalog = new System.Windows.Forms.ComboBox();
            this.btnLinkDb = new System.Windows.Forms.Button();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboProcedure = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.btnCreateAllProcedure = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboInitData = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnInitAllData = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboTable = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnCreateAllTable = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCreateDb = new System.Windows.Forms.Button();
            this.txtCatalog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboCatalog);
            this.groupBox1.Controls.Add(this.btnLinkDb);
            this.groupBox1.Controls.Add(this.txtPwd);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(727, 97);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接数据库";
            // 
            // cboCatalog
            // 
            this.cboCatalog.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCatalog.FormattingEnabled = true;
            this.cboCatalog.Location = new System.Drawing.Point(219, 62);
            this.cboCatalog.Name = "cboCatalog";
            this.cboCatalog.Size = new System.Drawing.Size(222, 20);
            this.cboCatalog.TabIndex = 5;
            this.cboCatalog.SelectedIndexChanged += new System.EventHandler(this.cboCatalog_SelectedIndexChanged);
            // 
            // btnLinkDb
            // 
            this.btnLinkDb.Location = new System.Drawing.Point(492, 18);
            this.btnLinkDb.Name = "btnLinkDb";
            this.btnLinkDb.Size = new System.Drawing.Size(107, 31);
            this.btnLinkDb.TabIndex = 4;
            this.btnLinkDb.Text = "连接数据库";
            this.btnLinkDb.UseVisualStyleBackColor = true;
            this.btnLinkDb.Click += new System.EventHandler(this.btnLinkDb_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(379, 23);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(107, 21);
            this.txtPwd.TabIndex = 3;
            this.txtPwd.Text = "qdh168";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "密码";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(264, 23);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(74, 21);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.Text = "qdh";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择数据库文件";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(217, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "用户名";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(75, 23);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(136, 21);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "www.litecms.cn,18433";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "SQL Server";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 97);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(727, 321);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(719, 295);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据库操作";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cboProcedure);
            this.groupBox5.Controls.Add(this.button5);
            this.groupBox5.Controls.Add(this.btnCreateAllProcedure);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(3, 183);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(713, 60);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "存储过程";
            // 
            // cboProcedure
            // 
            this.cboProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProcedure.FormattingEnabled = true;
            this.cboProcedure.Location = new System.Drawing.Point(90, 25);
            this.cboProcedure.Name = "cboProcedure";
            this.cboProcedure.Size = new System.Drawing.Size(236, 20);
            this.cboProcedure.TabIndex = 6;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(334, 23);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(114, 21);
            this.button5.TabIndex = 5;
            this.button5.Text = "创建选中存储过程";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // btnCreateAllProcedure
            // 
            this.btnCreateAllProcedure.Location = new System.Drawing.Point(454, 23);
            this.btnCreateAllProcedure.Name = "btnCreateAllProcedure";
            this.btnCreateAllProcedure.Size = new System.Drawing.Size(118, 21);
            this.btnCreateAllProcedure.TabIndex = 5;
            this.btnCreateAllProcedure.Text = "创建全部存储过程";
            this.btnCreateAllProcedure.UseVisualStyleBackColor = true;
            this.btnCreateAllProcedure.Click += new System.EventHandler(this.btnCreateAllProcedure_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "脚本列表";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboInitData);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.btnInitAllData);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 123);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(713, 60);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "初始化表记录";
            // 
            // cboInitData
            // 
            this.cboInitData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInitData.FormattingEnabled = true;
            this.cboInitData.Location = new System.Drawing.Point(90, 25);
            this.cboInitData.Name = "cboInitData";
            this.cboInitData.Size = new System.Drawing.Size(236, 20);
            this.cboInitData.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(334, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 21);
            this.button1.TabIndex = 5;
            this.button1.Text = "执行选中脚本";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnInitAllData
            // 
            this.btnInitAllData.Location = new System.Drawing.Point(454, 23);
            this.btnInitAllData.Name = "btnInitAllData";
            this.btnInitAllData.Size = new System.Drawing.Size(118, 21);
            this.btnInitAllData.TabIndex = 5;
            this.btnInitAllData.Text = "执行全部脚本";
            this.btnInitAllData.UseVisualStyleBackColor = true;
            this.btnInitAllData.Click += new System.EventHandler(this.btnInitAllData_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "脚本列表";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboTable);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.btnCreateAllTable);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 63);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(713, 60);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "表操作";
            // 
            // cboTable
            // 
            this.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTable.FormattingEnabled = true;
            this.cboTable.Location = new System.Drawing.Point(90, 25);
            this.cboTable.Name = "cboTable";
            this.cboTable.Size = new System.Drawing.Size(236, 20);
            this.cboTable.TabIndex = 6;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(334, 23);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(114, 21);
            this.button3.TabIndex = 5;
            this.button3.Text = "创建选中表";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnCreateAllTable
            // 
            this.btnCreateAllTable.Location = new System.Drawing.Point(454, 24);
            this.btnCreateAllTable.Name = "btnCreateAllTable";
            this.btnCreateAllTable.Size = new System.Drawing.Size(118, 21);
            this.btnCreateAllTable.TabIndex = 5;
            this.btnCreateAllTable.Text = "创建全部表";
            this.btnCreateAllTable.UseVisualStyleBackColor = true;
            this.btnCreateAllTable.Click += new System.EventHandler(this.btnCreateAllTable_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "脚本列表";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCreateDb);
            this.groupBox2.Controls.Add(this.txtCatalog);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(713, 60);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "创建新数据库";
            // 
            // btnCreateDb
            // 
            this.btnCreateDb.Location = new System.Drawing.Point(233, 24);
            this.btnCreateDb.Name = "btnCreateDb";
            this.btnCreateDb.Size = new System.Drawing.Size(93, 21);
            this.btnCreateDb.TabIndex = 5;
            this.btnCreateDb.Text = "一键创建";
            this.btnCreateDb.UseVisualStyleBackColor = true;
            this.btnCreateDb.Click += new System.EventHandler(this.btnCreateDb_Click);
            // 
            // txtCatalog
            // 
            this.txtCatalog.Location = new System.Drawing.Point(90, 24);
            this.txtCatalog.Name = "txtCatalog";
            this.txtCatalog.Size = new System.Drawing.Size(121, 21);
            this.txtCatalog.TabIndex = 3;
            this.txtCatalog.Text = "趣订货";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据库名称";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(719, 295);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtLog);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 418);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(727, 100);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "日志";
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(3, 17);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(721, 80);
            this.txtLog.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 518);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "趣订货.数据库工具";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboCatalog;
        private System.Windows.Forms.Button btnLinkDb;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cboProcedure;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnCreateAllProcedure;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboInitData;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnInitAllData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboTable;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnCreateAllTable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnCreateDb;
        private System.Windows.Forms.TextBox txtCatalog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtLog;
    }
}

