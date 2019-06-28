using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using NPOI.SS.UserModel;
using System.Configuration;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.Reflection;
using System.Threading;
using FastReport;
using FastReport.Data;

namespace TKWORKDELIVERY
{
    public partial class frmWORKDELIVERY : Form
    {
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlComm = new SqlCommand();
        string connectionString;
        StringBuilder sbSql = new StringBuilder();
        StringBuilder sbSqlQuery = new StringBuilder();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder();
        SqlDataAdapter adapter2 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder2 = new SqlCommandBuilder();
        SqlDataAdapter adapter3 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder3 = new SqlCommandBuilder();
        SqlDataAdapter adapter4 = new SqlDataAdapter();
        SqlCommandBuilder sqlCmdBuilder4 = new SqlCommandBuilder();
        SqlTransaction tran;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();
        DataSet ds4 = new DataSet();
        DataTable dt = new DataTable();
        string tablename = null;
        string EDITID;
        int result;
        Thread TD;

        string STATUS = null;
        string NO;
        string OLDBUYNO;
        string CHECKYN = "N";

        public frmWORKDELIVERY()
        {
            InitializeComponent();
            comboBox2load();
            comboBox3load();
        }

        #region FUNCTION

        public void comboBox2load()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dberp"].ConnectionString;
            sqlConn = new SqlConnection(connectionString);
            StringBuilder Sequel = new StringBuilder();
            Sequel.AppendFormat(@"SELECT [ID],[NAME],[EMAIL] FROM [TKWORKDELIVERY].[dbo].[EMP] ORDER BY  [ID]");
            SqlDataAdapter da = new SqlDataAdapter(Sequel.ToString(), sqlConn);
            DataTable dt = new DataTable();
            sqlConn.Open();

            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            da.Fill(dt);
            comboBox2.DataSource = dt.DefaultView;
            comboBox2.ValueMember = "ID";
            comboBox2.DisplayMember = "NAME";
            sqlConn.Close();


        }

        public void comboBox3load()
        {
            connectionString = ConfigurationManager.ConnectionStrings["dberp"].ConnectionString;
            sqlConn = new SqlConnection(connectionString);
            StringBuilder Sequel = new StringBuilder();
            Sequel.AppendFormat(@"SELECT [ID],[NAME],[EMAIL] FROM [TKWORKDELIVERY].[dbo].[EMP] ORDER BY  [ID]");
            SqlDataAdapter da = new SqlDataAdapter(Sequel.ToString(), sqlConn);
            DataTable dt = new DataTable();
            sqlConn.Open();

            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            da.Fill(dt);
            comboBox3.DataSource = dt.DefaultView;
            comboBox3.ValueMember = "ID";
            comboBox3.DisplayMember = "NAME";
            sqlConn.Close();


        }
        public void Search()
        {
            ds.Clear();


            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["dberp"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                sbSql.Clear();
                sbSqlQuery.Clear();

                sbSql.AppendFormat(@" SELECT [NO] AS '編號',CONVERT(NVARCHAR,[DATES],111) AS '日期',[CREATEOR] AS '交辨人',[SENDTO] AS '被交辨人',[MESSAGE] AS '交辨內容',[REPLY] AS '回覆',[STATUS] AS '結案碼',[CREATEORID] AS '交辨ID',[ID] ");
                sbSql.AppendFormat(@" FROM [TKWORKDELIVERY].[dbo].[WORKDELIVERY] ");
                sbSql.AppendFormat(@" WHERE CONVERT(NVARCHAR,[DATES],112)>='{0}' AND CONVERT(NVARCHAR,[DATES],112)<='{1}' ",dateTimePicker1.Value.ToString("yyyyMMdd"), dateTimePicker2.Value.ToString("yyyyMMdd"));
                sbSql.AppendFormat(@" AND [STATUS]='{0}' ",comboBox1.Text.ToString());
                sbSql.AppendFormat(@"  ");
                sbSql.AppendFormat(@"  ");

                adapter = new SqlDataAdapter(@"" + sbSql, sqlConn);

                sqlCmdBuilder = new SqlCommandBuilder(adapter);
                sqlConn.Open();
                ds.Clear();
                adapter.Fill(ds, "TEMPds1");
                sqlConn.Close();


                if (ds.Tables["TEMPds1"].Rows.Count == 0)
                {
                    dataGridView1.DataSource = null;
                }
                else
                {
                    if (ds.Tables["TEMPds1"].Rows.Count >= 1)
                    {
                        dataGridView1.DataSource = ds.Tables["TEMPds1"];
                        dataGridView1.AutoResizeColumns();


                    }

                }

            }
            catch
            {

            }
            finally
            {

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int rowindex = dataGridView1.CurrentRow.Index;
                if (rowindex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[rowindex];

                    dateTimePicker3.Value = Convert.ToDateTime(row.Cells["日期"].Value.ToString());

                    comboBox2.Text = row.Cells["交辨人"].Value.ToString();
                    comboBox3.Text = row.Cells["被交辨人"].Value.ToString();
                    comboBox4.Text = row.Cells["結案碼"].Value.ToString();

                    textBox1.Text = row.Cells["編號"].Value.ToString();                   
                    textBox2.Text = row.Cells["交辨內容"].Value.ToString();
                    textBox3.Text = row.Cells["回覆"].Value.ToString();
                    textBox4.Text = row.Cells["交辨ID"].Value.ToString();
                    textBoxID.Text = row.Cells["ID"].Value.ToString();

                }
                else
                {
                    textBox1.Text = null;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox4.Text = null;
                    textBoxID.Text = null;

                }
            }
        }

        public void SETSTATUS()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBoxID.Text = null;
            //textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;

        }
        public void SETSTATUS2()
        {
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;

        }
        public void SETSTAUSFIANL()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
        }

        public string GETNO()
        {

            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["dberp"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                StringBuilder sbSql = new StringBuilder();
                sbSql.Clear();
                sbSqlQuery.Clear();
                ds4.Clear();

                sbSql.Clear();

                sbSql.AppendFormat(@"  SELECT ISNULL(MAX([NO]),'000000000000') AS NO");
                sbSql.AppendFormat(@"  FROM [TKWORKDELIVERY].[dbo].[WORKDELIVERY] ");
                sbSql.AppendFormat(@"  WHERE [NO] LIKE '{0}%'", dateTimePicker3.Value.ToString("yyyyMMdd"));
                sbSql.AppendFormat(@"  ");
                sbSql.AppendFormat(@"  ");

                adapter4 = new SqlDataAdapter(@"" + sbSql, sqlConn);

                sqlCmdBuilder4 = new SqlCommandBuilder(adapter4);
                sqlConn.Open();
                ds4.Clear();
                adapter4.Fill(ds4, "TEMPds4");
                sqlConn.Close();


                if (ds4.Tables["TEMPds4"].Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    if (ds4.Tables["TEMPds4"].Rows.Count >= 1)
                    {
                        NO = SETNO(ds4.Tables["TEMPds4"].Rows[0]["NO"].ToString());
                        return NO;

                    }
                    return null;
                }

            }
            catch
            {
                return null;
            }
            finally
            {
                sqlConn.Close();
            }
        }

        public string SETNO(string NO)
        {
            if (NO.Equals("000000000000"))
            {
                return dateTimePicker3.Value.ToString("yyyyMMdd") + "0001";
            }

            else
            {
                int serno = Convert.ToInt16(NO.Substring(8, 4));
                serno = serno + 1;
                string temp = serno.ToString();
                temp = temp.PadLeft(4, '0');
                return dateTimePicker3.Value.ToString("yyyyMMdd") + temp.ToString();
            }
        }
        public void UPDATE()
        {
            try
            {

                //add ZWAREWHOUSEPURTH
                connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                sqlConn.Close();
                sqlConn.Open();
                tran = sqlConn.BeginTransaction();

                sbSql.AppendFormat(" UPDATE   [TKWORKDELIVERY].[dbo].[WORKDELIVERY]");
                sbSql.AppendFormat(" SET [NO]='{0}',[DATES]='{1}',[CREATEOR]='{2}',[SENDTO]='{3}',[MESSAGE]='{4}',[REPLY]='{5}',[STATUS]='{6}',[CREATEORID]='{7}'", textBox1.Text, dateTimePicker3.Value.ToString("yyyyMMdd"), comboBox2.Text, comboBox3.Text, textBox2.Text, textBox3.Text, comboBox4.Text, textBox4.Text);
                sbSql.AppendFormat(" WHERE  [ID]='{0}'",textBoxID.Text);
                sbSql.AppendFormat(" ");
                sbSql.AppendFormat(" ");
                sbSql.AppendFormat(" ");

                cmd.Connection = sqlConn;
                cmd.CommandTimeout = 60;
                cmd.CommandText = sbSql.ToString();
                cmd.Transaction = tran;
                result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    tran.Rollback();    //交易取消
                }
                else
                {
                    tran.Commit();      //執行交易  


                }
            }
            catch
            {

            }

            finally
            {
                sqlConn.Close();
            }
        }

        public void ADD()
        {
            try
            {
                textBox1.Text = GETNO();
                //add ZWAREWHOUSEPURTH
                connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                sqlConn.Close();
                sqlConn.Open();
                tran = sqlConn.BeginTransaction();

                sbSql.Clear();
                sbSql.AppendFormat(" INSERT INTO [TKWORKDELIVERY].[dbo].[WORKDELIVERY]");
                sbSql.AppendFormat(" ( [NO],[DATES],[CREATEOR],[SENDTO],[MESSAGE],[REPLY],[STATUS],[CREATEORID])");
                sbSql.AppendFormat(" VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", textBox1.Text, dateTimePicker3.Value.ToString("yyyyMMdd"), comboBox2.Text, comboBox3.Text, textBox2.Text, textBox3.Text, comboBox4.Text, textBox4.Text);
                sbSql.AppendFormat(" ");
                sbSql.AppendFormat(" ");


                cmd.Connection = sqlConn;
                cmd.CommandTimeout = 60;
                cmd.CommandText = sbSql.ToString();
                cmd.Transaction = tran;
                result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    tran.Rollback();    //交易取消
                }
                else
                {
                    tran.Commit();      //執行交易  


                }
            }
            catch
            {

            }

            finally
            {
                sqlConn.Close();
            }
        }
        public void DEL()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
                sqlConn = new SqlConnection(connectionString);

                sqlConn.Close();
                sqlConn.Open();
                tran = sqlConn.BeginTransaction();

                sbSql.Clear();

                sbSql.AppendFormat(" DELETE [TKWORKDELIVERY].[dbo].[WORKDELIVERY]");
                sbSql.AppendFormat(" WHERE  [ID]='{0}'", textBoxID.Text);
                sbSql.AppendFormat(" ");
                sbSql.AppendFormat(" ");

                cmd.Connection = sqlConn;
                cmd.CommandTimeout = 60;
                cmd.CommandText = sbSql.ToString();
                cmd.Transaction = tran;
                result = cmd.ExecuteNonQuery();

                if (result == 0)
                {
                    tran.Rollback();    //交易取消
                }
                else
                {
                    tran.Commit();      //執行交易  
                }

            }
            catch
            {

            }

            finally
            {
                sqlConn.Close();
            }

        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox4.Text = null;
            if (!string.IsNullOrEmpty(comboBox2.SelectedValue.ToString()) && !comboBox2.SelectedValue.ToString().Equals("System.Data.DataRowView"))
            {
                textBox4.Text = comboBox2.SelectedValue.ToString();
            }
        }

        public void SETFASTREPORT()
        {

            string SQL;
            Report report1 = new Report();
            report1.Load(@"REPORT\工作交付.frx");

            report1.Dictionary.Connections[0].ConnectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            //report1.Dictionary.Connections[0].ConnectionString = "server=192.168.1.105;database=TKPUR;uid=sa;pwd=dsc";

            TableDataSource Table = report1.GetDataSource("Table") as TableDataSource;
            SQL = SETFASETSQL();
            Table.SelectCommand = SQL;
            report1.Preview = previewControl1;
            report1.Show();

        }

        public string SETFASETSQL()
        {
            StringBuilder FASTSQL = new StringBuilder();

            FASTSQL.AppendFormat(@" SELECT [NO] AS '編號',CONVERT(NVARCHAR,[DATES],111) AS '日期',[CREATEOR] AS '交辨人',[SENDTO] AS '被交辨人',[MESSAGE] AS '交辨內容',[REPLY] AS '回覆',[STATUS] AS '結案碼',[CREATEORID] AS '交辨ID',[ID] ");
            FASTSQL.AppendFormat(@" FROM [TKWORKDELIVERY].[dbo].[WORKDELIVERY] ");
            FASTSQL.AppendFormat(@" WHERE CONVERT(NVARCHAR,[DATES],112)>='{0}' AND CONVERT(NVARCHAR,[DATES],112)<='{1}' ", dateTimePicker1.Value.ToString("yyyyMMdd"), dateTimePicker2.Value.ToString("yyyyMMdd"));
            FASTSQL.AppendFormat(@" AND [STATUS]='{0}' ", comboBox1.Text.ToString());
            FASTSQL.AppendFormat(@"  ");

            return FASTSQL.ToString();
        }


        #endregion

        #region BUTTON

        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            STATUS = "ADD";
            SETSTATUS();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            STATUS = "EDIT";

            SETSTATUS2();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (STATUS.Equals("EDIT"))
            {
                UPDATE();
            }
            else if (STATUS.Equals("ADD"))
            {
                ADD();
            }

            STATUS = null;

            SETSTAUSFIANL();

            Search();
            MessageBox.Show("完成");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            STATUS = null;
            string message = " 要刪除了?";

            DialogResult dialogResult = MessageBox.Show(message.ToString(), "要刪除了?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                DEL();

            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

            Search();
            MessageBox.Show("完成");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            SETFASTREPORT();
        }


        #endregion


    }
}
