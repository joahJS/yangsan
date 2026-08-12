using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

/// <summary>
/// 원본 BA00.pas / BA00.dfm (TfrmBA00) 이식 — 사업장 마스터(SAUPJANGF 테이블).
/// 회사 전체 설정을 담는 단일 레코드 화면(SCODE='100001' 고정).
/// </summary>
public partial class BA00Form : Form
{
    private const string CompanyCode = "100001";

    public BA00Form()
    {
        InitializeComponent();
    }

    private void BtnPost_Click(object? sender, EventArgs e)
    {
        LookupPostalCode();
    }

    private void EdtAddr_DoubleClick(object? sender, EventArgs e)
    {
        LookupPostalCode();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        Save();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void BA00Form_Load(object? sender, EventArgs e)
    {
        LoadCompanyInfo();
    }

    private void BA00Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: ClearEdit(); break;
            case Keys.F2: btnSave.PerformClick(); break;
            case Keys.Escape: Close(); break;
        }
    }

    /// <summary>원본 FormShow.</summary>
    private void LoadCompanyInfo()
    {
        using var q = new DbQuery();
        q.Add("SELECT * FROM SAUPJANGF WHERE SCODE = @SCODE");
        q.ParamByName("SCODE").AsString = CompanyCode;
        q.Open();

        if (q.RecordCount < 1) { ClearEdit(); return; }

        edtSang.Text = q.FieldByName("SSANG").AsString;
        edtName.Text = q.FieldByName("SNAME").AsString;
        var sano = q.FieldByName("SSANO").AsString;
        edtSa1.Text = Slice(sano, 1, 3);
        edtSa2.Text = Slice(sano, 5, 2);
        edtSa3.Text = Slice(sano, 8, 5);
        var bnno = q.FieldByName("SBNNO").AsString;
        edtNo1.Text = Slice(bnno, 1, 6);
        edtNo2.Text = Slice(bnno, 8, 7);
        edtUptae.Text = q.FieldByName("SUPTE").AsString;
        edtJong.Text = q.FieldByName("SJONG").AsString;
        var post = q.FieldByName("SPOST").AsString;
        edtPost1.Text = Slice(post, 1, 3);
        edtPost2.Text = Slice(post, 5, 3);
        edtAddr.Text = q.FieldByName("SADDR").AsString;
        edtDDD.Text = q.FieldByName("SDDD").AsString;
        edtTel.Text = q.FieldByName("STEL").AsString;
        edtFax.Text = q.FieldByName("SFAX").AsString;
        edtBigo.Text = q.FieldByName("SBIGO").AsString;
    }

    /// <summary>델파이 1-based Copy(s, start, len)와 동일한 동작.</summary>
    private static string Slice(string s, int start1Based, int len)
    {
        if (string.IsNullOrEmpty(s) || start1Based > s.Length) return string.Empty;
        var idx = start1Based - 1;
        var actualLen = Math.Min(len, s.Length - idx);
        return actualLen <= 0 ? string.Empty : s.Substring(idx, actualLen);
    }

    private void ClearEdit()
    {
        edtSang.Clear(); edtName.Clear(); edtSa1.Clear(); edtSa2.Clear();
        edtSa3.Clear(); edtNo1.Clear(); edtNo2.Clear(); edtUptae.Clear();
        edtJong.Clear(); edtPost1.Clear(); edtPost2.Clear(); edtAddr.Clear();
        edtDDD.Clear(); edtTel.Clear(); edtFax.Clear(); edtBigo.Clear();
        edtSang.Focus();
    }

    /// <summary>원본 fncErrCheck.</summary>
    private bool ErrCheck()
    {
        if (string.IsNullOrWhiteSpace(edtSang.Text) || string.IsNullOrWhiteSpace(edtName.Text))
        {
            MessageBox.Show("입력항목이 누락되었습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    /// <summary>원본 btnSaveClick.</summary>
    private void Save()
    {
        if (!ErrCheck()) return;

        using var check = new DbQuery();
        check.Add("SELECT * FROM SAUPJANGF WHERE SCODE=@SCODE");
        check.ParamByName("SCODE").AsString = CompanyCode;
        check.Open();
        bool isInsert = check.RecordCount < 1;

        var sano = (edtSa1.Text + edtSa2.Text + edtSa3.Text).Trim() == ""
            ? "" : $"{edtSa1.Text.Trim()}-{edtSa2.Text.Trim()}-{edtSa3.Text.Trim()}";
        var bnno = (edtNo1.Text + edtNo2.Text).Trim() == ""
            ? "" : $"{edtNo1.Text.Trim()}-{edtNo2.Text.Trim()}";
        var post = (edtPost1.Text + edtPost2.Text).Trim() == ""
            ? "" : $"{edtPost1.Text.Trim()}-{edtPost2.Text.Trim()}";

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            if (isInsert)
            {
                q.Add("INSERT INTO SAUPJANGF(SCODE,SSANG,SNAME,SSANO,SBNNO,SJONG,");
                q.Add("       SUPTE,SDDD,STEL,SFAX,SPOST,SADDR,SBIGO)");
                q.Add("  VALUES(@SCODE,@SSANG,@SNAME,@SSANO,@SBNNO,@SJONG,");
                q.Add("       @SUPTE,@SDDD,@STEL,@SFAX,@SPOST,@SADDR,@SBIGO)");
            }
            else
            {
                q.Add("UPDATE SAUPJANGF SET SSANG=@SSANG,SNAME=@SNAME,SSANO=@SSANO,");
                q.Add("       SBNNO=@SBNNO,SJONG=@SJONG,SUPTE=@SUPTE,SDDD=@SDDD,");
                q.Add("       STEL=@STEL,SFAX=@SFAX,SPOST=@SPOST,SADDR=@SADDR,");
                q.Add("       SBIGO=@SBIGO");
                q.Add(" WHERE SCODE=@SCODE");
            }
            q.ParamByName("SCODE").AsString = CompanyCode;
            q.ParamByName("SSANG").AsString = edtSang.Text.Trim();
            q.ParamByName("SNAME").AsString = edtName.Text.Trim();
            q.ParamByName("SSANO").AsString = sano;
            q.ParamByName("SBNNO").AsString = bnno;
            q.ParamByName("SJONG").AsString = edtJong.Text.Trim();
            q.ParamByName("SUPTE").AsString = edtUptae.Text.Trim();
            q.ParamByName("SPOST").AsString = post;
            q.ParamByName("SADDR").AsString = edtAddr.Text.Trim();
            q.ParamByName("SDDD").AsString = edtDDD.Text.Trim();
            q.ParamByName("STEL").AsString = edtTel.Text.Trim();
            q.ParamByName("SFAX").AsString = edtFax.Text.Trim();
            q.ParamByName("SBIGO").AsString = edtBigo.Text.Trim();
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 입/수정시 에러발생..\r\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Close();
    }

    /// <summary>원본 up_InfoPOSTF (우편번호 검색 팝업).</summary>
    private void LookupPostalCode()
    {
        using var dlg = new Common.PostalLookupForm();
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        edtPost1.Text = dlg.SelectedZip.Length >= 3 ? dlg.SelectedZip[..3] : dlg.SelectedZip;
        edtPost2.Text = dlg.SelectedZip.Length >= 6 ? dlg.SelectedZip[3..6] : "";
        edtAddr.Text = dlg.SelectedAddr;
        edtDDD.Text = dlg.SelectedDDD;
    }
}
