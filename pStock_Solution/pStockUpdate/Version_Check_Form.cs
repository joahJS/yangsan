using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using DevExpress.XtraSplashScreen;
using System.Runtime.InteropServices;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;

namespace pStockUpdate
{
    public partial class Version_Check_Form : DevExpress.XtraEditors.XtraForm
    {
        public Version_Check_Form()
        {
            InitializeComponent();
        }

        public string PROCEDURE_ID = "usp_SYS_VersionCheck";
        public string BIG_CATE = "VERSION";
        public string INIT_NAME = "version";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        class iniUtil
        {
            private string iniPath;
            public iniUtil(string path)
            {
                this.iniPath = path;  //INI 파일 위치를 생성할때 인자로 넘겨 받음
            }

            public String GetIniValue(String Section, String Key)
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
                return temp.ToString();
            }

            // INI 값을 셋팅
            public void SetIniValue(String Section, String Key, String Value)
            {
                WritePrivateProfileString(Section, Key, Value, iniPath);
            }
        }

        private void Version_Check_Form_Load(object sender, EventArgs e)
        {
            string internal_IP = GetInternalIP();
            string myIp = new WebClient().DownloadString("http://ipinfo.io/ip").Trim();
            if (string.IsNullOrEmpty(myIp)) //null 또는 빈값일때 Get Internal IP를 가져오게 한다.
            {
                myIp = GetInternalIP();
            }

            DBConn.sqlConnection = "server = 121.66.17.30,16433; uid = pineit; pwd = pineit0401; database = CHANG01_TEMP";
            

            SplashScreenManager.ShowForm(typeof(UpdateScreen), true, true);
            SplashScreenManager.CloseForm();
            DBConn.dbCon = DBConn.DbConn();

            string filePath = Application.StartupPath + @"\version.ini";
            Lb_VerChkMsg.Text = "최신버전 업데이트 체크를 진행하겠습니다.";

            iniUtil ini = new iniUtil(filePath);
            string CurVer = ini.GetIniValue(BIG_CATE, INIT_NAME);      // 현재 버전 담을 변수
            string VersionId = "";                                     // 최신 버전 담을 변수

            // 최신 버전 정보를 얻기 위한 프로시저 접근
            Dictionary<string, string> dicParams = new Dictionary<string, string>();
            dicParams.Add("CMD", "LAST_VERSION");

            // dicParams의 최신 버전을 체크하고, 더 높은 버전이 있다면 dt 반환
            DataTable dt = DBConn.GetDataTable(DBConn.dbCon, this.PROCEDURE_ID, dicParams);

            if (dt.Rows.Count > 0)
            {
                VersionId = dt.Rows[0]["VERSION_ID"]?.ToString();
            }

            if (!CheckVersion(filePath))
            {
                Lb_VerChkMsg.Text = "현재 버전은 최신 버전이 아닙니다.\n업데이트 버튼을 눌러 업데이트를 진행하세요.";
                Lb_VerInfo.Text = "현재 버전 : " + CurVer + "   →   최신 버전 : " + VersionId;
            }
            else
            {
                Lb_VerChkMsg.Text = "현재 버전은 최신버전입니다.\n로그인 화면으로 이동합니다. 잠시만 기다려주세요...";
                Lb_VerInfo.Text = "현재 버전 : " + VersionId + " ver.";
                Bt_Update.Visible = false;
                Bt_Close.Visible = false;
                DelaySystem(2000);
                CloseUpdate();
            }
        }

        private bool CheckVersion(string IniPath)
        {
            iniUtil ini = new iniUtil(IniPath);
            string Version = ini.GetIniValue(BIG_CATE, INIT_NAME);
            string UpdateYn = GetUpdateRemark(Version);
            if (!string.IsNullOrEmpty(UpdateYn))
            {
                if (UpdateYn.Equals("Y"))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        private string GetUpdateRemark(string VersionId)
        {
            Lb_VerChkMsg.Text = "업데이트 체크를 진행중입니다...";

            Dictionary<string, string> dicParams = new Dictionary<string, string>();

            dicParams.Add("CMD", "VER_CHECK");
            dicParams.Add("VERSION_ID", VersionId);

            // 현재 버전(sVersionId)과 DB의 최신 버전이 같으면 N, 다르면 Y 반환
            DataTable dtResult = DBConn.GetDataTable(DBConn.dbCon, this.PROCEDURE_ID, dicParams);
            if (dtResult.Rows.Count > 0)
            {
                return dtResult.Rows?[0]["UPDATE_YN"]?.ToString();
            }
            else
            {
                return null;
            }
        }

        private void DelaySystem(int MS)
        { /* 함수명 : DelaySystem * 1000ms = 1초 * 전달인자 : 얼마나 지연시킬것인가에 대한 변수 * */
            DateTime dtAfter = DateTime.Now;        // 현재시간 할당
            TimeSpan dtDuration = new TimeSpan(0, 0, 0, 0, MS);     // 시간 간격을 저장, 이 코드에서는 2000밀리초
            DateTime dtThis = dtAfter.Add(dtDuration);          // dtThis에 현재시간(dtAfter)에 2000밀리초를 합한 시간을 할당
            while (dtThis >= dtAfter)       // dtAfter(현재시간)이 dtThis(딜레이 시간)에 도달할 때까지 반복
            {
                System.Windows.Forms.Application.DoEvents();  //현재 시간 얻어 오기
                dtAfter = DateTime.Now;
            }
        }

        private void CloseUpdate()
        {
            string path = string.Format(@"{0}\{1}", Application.StartupPath, "pStock.exe");
            Application.Exit();
            Process.Start(path);
        }

        #region [ 업데이트 버튼 ]
        private void Bt_Update_Click(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(UpdateScreen), true, false);

            try
            {
                byte[] file = null;         // 파일이 Byte 타입으로 DB에 저장되어 있음
                Dictionary<string, string> dicParams = new Dictionary<string, string>();

                dicParams.Add("CMD", "LAST_VERSION");

                // dicParams의 최신 버전을 체크하고, 더 높은 버전이 있다면 높은 버전의 dt.Row 반환
                DataTable dt = DBConn.GetDataTable(DBConn.dbCon, this.PROCEDURE_ID, dicParams);
                if (dt.Rows.Count > 0)
                {
                    string VersionId = dt.Rows[0]["VERSION_ID"]?.ToString();
                    file = (byte[])dt.Rows[0]["FILE_NO"];       // FILE_NO에 해당하는 파일을 할당
                    UpdateFile(file, VersionId);                // 할당받은 파일로 업데이트
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                    Version_Check_Form_Load(null, null);
                }
                else         // DB zSYS_VERSION에 버전 정보가 없음
                {
                    throw new Exception("DB에 버전이 존재하지 않습니다.\r\n관리자에게 문의하세요.");
                }
            }
            catch (Exception ex)
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                XtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 받은 바이트를 그대로 pStock.exe에 덮어쓰던 방식에서, 배포 폴더 전체를 압축한 zip을
        /// 통째로 풀어 설치 폴더에 덮어쓰는 방식으로 변경했다. .NET 8은 exe(런처)와 dll(실제
        /// 코드)이 분리된 형태로 빌드되는데, exe만 갱신해서는 dll에 들어있는 실제 로직이
        /// 그대로 남아 "버전 정보는 최신인데 실제 프로그램은 이전 버전"이 되는 문제가 있었다.
        /// zip을 통째로 풀면 exe/dll/deps.json/runtimeconfig.json/참조 라이브러리가 전부
        /// 함께 갱신된다.
        /// </summary>
        private void UpdateFile(byte[] file, string VersionId)
        {
            string fileDir = Application.StartupPath;
            string tempZipPath = Path.Combine(Path.GetTempPath(), "pStock_update_" + Guid.NewGuid().ToString("N") + ".zip");

            try
            {
                File.WriteAllBytes(tempZipPath, file);

                using (var archive = System.IO.Compression.ZipFile.OpenRead(tempZipPath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        // 폴더 항목은 Name이 빈 문자열이므로 건너뛴다
                        if (string.IsNullOrEmpty(entry.Name)) continue;

                        string destPath = Path.Combine(fileDir, entry.FullName);
                        string destDir = Path.GetDirectoryName(destPath);
                        if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                            Directory.CreateDirectory(destDir);

                        entry.ExtractToFile(destPath, overwrite: true);
                    }
                }
            }
            finally
            {
                if (File.Exists(tempZipPath))
                {
                    try { File.Delete(tempZipPath); }
                    catch { /* 임시파일 삭제 실패는 무시 - 다음에 지워져도 무방 */ }
                }
            }

            IniSet(BIG_CATE, INIT_NAME, VersionId);        // 파일 업데이트 후 ini파일의 버전정보 갱신
        }

        private void IniSet(string sBicCate, string key, string var)
        {
            string filePath = Application.StartupPath + @"\version.ini";
            iniUtil ini = new iniUtil(filePath);
            ini.SetIniValue(sBicCate, key, var);
        }
        #endregion

        private void Bt_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string GetInternalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new Exception("IPv4 주소를 찾을 수 없습니다.");
        }
    }
}