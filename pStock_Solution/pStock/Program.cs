using pStock.Forms;

namespace pStock;

internal static class Program
{
    // 원본 dpr: CreateFileMapping(...,'창고관리') 로 중복 실행 방지
    private const string SingleInstanceName = "Global\\pStock_ChangGoGwanli_Mutex";

    [STAThread]
    private static void Main()
    {
        using var mutex = new Mutex(true, SingleInstanceName, out bool createdNew);
        if (!createdNew)
        {
            MessageBox.Show(
                "이 프로그램이 이미 실행중입니다.!\r\n\r\n실행되고 있는 프로그램을 닫고 다시 하십시오.!",
                "중복실행오류!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Application.ThreadException += (_, e) => MainForm.HandleAppException(e.Exception);

        // 원본: frm_login := Tfrm_login.Create(Application); frm_login.ShowModal;
        //       if frm_login.Cnt > 2 then Halt;
        using var login = new LoginForm();
        login.ShowDialog();
        if (login.Cnt > 2) return;

        Application.Run(new MainForm());
    }
}
