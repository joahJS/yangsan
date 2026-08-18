using pStock.Common;
using pStock.Data;

namespace pStock.Forms;

/// <summary>
/// 원본 login.pas / login.dfm (Tfrm_login) 이식.
/// PASSWD 테이블에서 사용자를 조회하고, 비밀번호/사용기간을 검사한다.
/// 3회 오류 시 프로그램을 종료한다 (원본 pStock.dpr: frm_login.Cnt > 2 then Halt).
/// </summary>
public partial class LoginForm : Form
{
    private string _vPass = string.Empty;
    private string _vSdat = string.Empty;
    private string _vEdat = string.Empty;

    /// <summary>원본 frm_login.Cnt : 로그인 실패 횟수. 3 이상이면 dpr에서 Halt.</summary>
    public int Cnt { get; private set; }

    public LoginForm()
    {
        InitializeComponent();
    }

    private void LoginForm_Load(object? sender, EventArgs e)
    {
        Cnt = 0;
        try
        {
            // 원본: DB_MD.dm_NK.DBNK.Connected := true;
            _ = AppDb.Connection;
        }
        catch (Exception ex)
        {
            MessageBox.Show(AppMessages.MIP0006 + "\r\n\r\n" + ex.Message, "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        var savedUserId = LoginPreferences.LoadSavedUserId();
        if (!string.IsNullOrEmpty(savedUserId))
        {
            edtCode.Text = savedUserId;
            chkRemember.Checked = true;
        }

        EdtCode_Leave(this, EventArgs.Empty);
    }

    /// <summary>원본 edtCodeExit: 입력한 코드로 PASSWD 조회.</summary>
    private void EdtCode_Leave(object? sender, EventArgs e)
    {
        UserContext.UsrInfo.Clear();
        if (string.IsNullOrWhiteSpace(edtCode.Text)) return;

        try
        {
            using var q = new DbQuery();
            q.Add("SELECT * FROM PASSWD WHERE USRID = @CODE");
            q.ParamByName("CODE").AsString = edtCode.Text.Trim();
            q.Open();

            if (q.IsEmpty)
            {
                _vPass = string.Empty;
                _vSdat = string.Empty;
                _vEdat = string.Empty;
            }
            else
            {
                _vPass = q.FieldByName("PPASS").AsString;
                _vSdat = q.FieldByName("PSDAT").AsString;
                _vEdat = q.FieldByName("PEDAT").AsString;

                UserContext.UsrInfo.Add(q.FieldByName("USRID").AsString);
                UserContext.UsrInfo.Add(q.FieldByName("PNAME").AsString);

                UserContext.Current.Code = q.FieldByName("USRID").AsString;
                UserContext.Current.Name = q.FieldByName("PNAME").AsString;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>원본 btnOkClick.</summary>
    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (edtNo.Text.Trim() == _vPass.Trim())
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            if (string.CompareOrdinal(_vSdat, today) <= 0 && string.CompareOrdinal(today, _vEdat) < 0)
            {
                if (chkRemember.Checked)
                    LoginPreferences.SaveUserId(edtCode.Text.Trim());
                else
                    LoginPreferences.ClearSavedUserId();

                Close();
                return;
            }

            MessageBox.Show("작업기간이 지나서 실행불가입니다.", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        else
        {
            MessageBox.Show("아이디 또는 비밀번호가 일치하지 않습니다.", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        Cnt++;

        if (Cnt > 2)
        {
            MessageBox.Show("3번의 입력된 비밀번호가 틀렸습니다. 프로그램을 종료합니다.", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            btnCancel.PerformClick();
            return;
        }

        edtCode.Focus();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Cnt = 3;
        Close();
    }

    private void LoginForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        // 원본 prcCloseData(self): 폼에 붙은 쿼리/테이블 컴포넌트를 닫던 처리.
        // DbQuery는 using으로 개별 정리되므로 여기서는 별도 처리가 필요 없다.
    }

    private void LoginForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            btnCancel.PerformClick();
        }
    }
}
