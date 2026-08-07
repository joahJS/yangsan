using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pStockUpdate
{
    public static class DBConn
    {
        public static SqlConnection dbCon;
        public static SqlTransaction dbTran;
        public static string sqlConnection = "server = 121.66.17.30,16433; uid = pineit; pwd = pineit0401; database = CHANG01_TEMP";

        //로컬호스트
        //public static string sqlConnection = "server = localhost; Database = meskda; Integrated Security = True;"; 

        public static SqlConnection DbConn()
        {
            string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "version_check_log.txt");
            try
            {
                string internal_IP = Client_IP;
                string external_IP = "";
                external_IP = Get_External_IP;

                sqlConnection = string.Format("server = 121.66.17.30,16433; uid = pineit; pwd = pineit0401; database = CHANG01_TEMP");
            }
            catch (Exception e)
            {
               File.WriteAllText(logPath, e.ToString());
               XtraMessageBox.Show(e.ToString());
            }

            try
            {
                dbCon = new SqlConnection(sqlConnection);
                dbCon.Open();
                return dbCon;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString());
                return null;
            }
        }

        #region [접속 정보]
        // 내부 IP
        public static string Client_IP
        {
            get
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ClientIP = string.Empty;
                for (int i = 0; i < host.AddressList.Length; i++)
                {
                    if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ClientIP = host.AddressList[i].ToString();
                    }
                }
                return ClientIP;
            }
        }
        // 외부 IP
        public static string Get_External_IP
        {
            get
            {
                try
                {
                    // CMD 외부 IP 받아오기 명령어 : nslookup myip.opendns.com resolver1.opendns.com
                    // 명령어를 한번에 할당하면 중간 공백때문에 실제 실행할 때, 뒤에 ? 가 붙어서 결과값이 다름
                    string command = "nslookup";
                    string arguments = "myip.opendns.com";
                    string resolver = "resolver1.opendns.com";

                    // CMD 명령 수행
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = $"{arguments} {resolver}";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    string result = process.StandardOutput.ReadToEnd();         // 명령 순수 결과값
                    string result_temp = result.Replace(" ", string.Empty);     // 결과값 공백 제거
                    string[] ip = result_temp.Trim().Split(':');                // IP를 구분하기 위해 : 로 구분 ([4] 항목이 외부 IP)
                    process.WaitForExit();
                    process.Close();

                    return ip[4];
                }
                catch (Exception)
                {
                    //XtraMessageBox.Show(ex.Message, "외부 IP 조회실패");
                    return string.Empty;
                }
            }
        }
        #endregion

        public static void DbDisConn(SqlConnection dbCon1)
        {
            try
            {
                dbCon1.Close();
                dbCon1.Dispose();
                dbCon1 = null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        public static DataTable GetDataTable(string strSql)
        {
            DataSet ds = new DataSet();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600;
                        //cmd.CommandText = strSql;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                if ((ds != null)) ds.Dispose();
            }
        }

        public static DataTable GetDataTable(SqlConnection dc, string strSql)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlDataAdapter adpt = new SqlDataAdapter(strSql, dc);

                adpt.SelectCommand.Transaction = dbTran;

                adpt.Fill(ds);

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                XtraMessageBox.Show(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                if ((ds != null)) ds.Dispose();
            }
        }

        public static DataTable GetDataTable(SqlConnection dc, string spName, Dictionary<string, string> paramsDic)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(spName, dc);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> item in paramsDic)
                {
                    string pname = String.Format("@{0}", item.Key);

                    sqlCmd.Parameters.Add(pname, SqlDbType.NVarChar);
                    sqlCmd.Parameters[pname].Value = item.Value;
                }

                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                XtraMessageBox.Show(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return null;
            }
        }

        public static DataTable GetDataTable(SqlConnection dc, string spName, Dictionary<string, object> paramsDic)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(spName, dc);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, object> item in paramsDic)
                {
                    string pname = String.Format("@{0}", item.Key);
                    sqlCmd.Parameters.AddWithValue(pname, item.Value);
                    //sqlCmd.Parameters.Add(pname, SqlDbType.NVarChar);
                    //sqlCmd.Parameters[pname].Value = item.Value;
                }

                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                XtraMessageBox.Show(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return null;
            }
        }

        //2021-06-15 Log 저장용
        public static DataTable GetDataTable_1(SqlConnection dc, string spName, Dictionary<string, string> paramsDic)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(spName, dc);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> item in paramsDic)
                {
                    string pname = String.Format("@{0}", item.Key);

                    sqlCmd.Parameters.Add(pname, SqlDbType.NVarChar);
                    sqlCmd.Parameters[pname].Value = item.Value;
                }

                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                XtraMessageBox.Show(sqlEx.Message);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DataTable GetDataTableByParam(string strSql, Dictionary<string, string> dicParams)
        {
            DataSet ds = new DataSet();

            try
            {
                using(SqlConnection conn = new SqlConnection(sqlConnection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = strSql;
                        cmd.Prepare();
                        foreach (KeyValuePair<string, string> param in dicParams)
                        {
                            cmd.Parameters.AddWithValue(string.Format("@{0}", param.Key), param.Value);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                XtraMessageBox.Show(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                if ((ds != null)) ds.Dispose();
            }
        }

        public static DataTable GetDataTableByProcedure(SqlConnection dc, string spName, string[] flds, string[] data)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand sqlCmd = new SqlCommand(spName, dc);

                sqlCmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < flds.Length; i++)
                {
                    string pname = String.Format("@{0}", flds[i]);

                    sqlCmd.Parameters.Add(pname, SqlDbType.NVarChar);
                    sqlCmd.Parameters[pname].Value = data[i];
                }

                SqlDataAdapter adp = new SqlDataAdapter(sqlCmd);
                adp.SelectCommand.Transaction = dbTran;
                adp.Fill(ds);

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                XtraMessageBox.Show(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                if ((ds != null)) ds.Dispose();
            }
        }
        public static DataTable GetDataTable(string spName, Dictionary<string, string> dicParams)
        {
            DataSet ds = new DataSet();

            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnection))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (KeyValuePair<string, string> item in dicParams)
                        {
                            string pname = String.Format("@{0}", item.Key);
                            cmd.Parameters.Add(pname, SqlDbType.NVarChar);
                            cmd.Parameters[pname].Value = item.Value;
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                    }
                }

                return ds.Tables[0];
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if ((ds != null)) ds.Dispose();
            }
        }

        public static int ExecuteNonQuery(string spName, Dictionary<string, string> paramsDic)
        {
            int cnt = 0;
            SqlConnection tcon = new SqlConnection(sqlConnection);

            try
            {

                SqlCommand sqlCmd = new SqlCommand(spName, tcon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> item in paramsDic)
                {
                    string pname = String.Format("@{0}", item.Key);

                    sqlCmd.Parameters.Add(pname, SqlDbType.NVarChar);
                    sqlCmd.Parameters[pname].Value = item.Value;
                }

                tcon.Open();
                cnt = sqlCmd.ExecuteNonQuery();
                tcon.Close();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            finally
            {
                if (tcon.State != ConnectionState.Closed)
                    tcon.Close();
            }

            return cnt;

        }

    }
}
