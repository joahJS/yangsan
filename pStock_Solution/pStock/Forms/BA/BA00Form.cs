using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

/// <summary>
/// 원본 BA00.pas / BA00.dfm (TfrmBA00) 이식 — 사업장 마스터(SAUPJANGF 테이블).
/// 회사 전체 설정을 담는 단일 레코드 화면(SCODE='100001' 고정).
/// </summary>
public class BA00Form : Form
{
    private const string CompanyCode = "100001";

    private readonly TextBox edtSang = new();   // 상호
    private readonly TextBox edtName = new();   // 대표자명
    private readonly TextBox edtSa1 = new();    // 사업자번호 앞3
    private readonly TextBox edtSa2 = new();    // 사업자번호 중2
    private readonly TextBox edtSa3 = new();    // 사업자번호 뒤5
    private readonly TextBox edtNo1 = new();    // 법인번호 앞6
    private readonly TextBox edtNo2 = new();    // 법인번호 뒤7
    private readonly TextBox edtUptae = new();  // 업태
    private readonly TextBox edtJong = new();   // 종목
    private readonly TextBox edtPost1 = new();  // 우편번호 앞3
    private readonly TextBox edtPost2 = new();  // 우편번호 뒤3
    private readonly TextBox edtAddr = new();   // 주소
    private readonly TextBox edtDDD = new();    // 지역번호
    private readonly TextBox edtTel = new();
    private readonly TextBox edtFax = new();
    private readonly TextBox edtBigo = new();

    private readonly Button btnSave = new() { Text = "저장(F2)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    public BA00Form()
    {
        Text = "사업장 마스터";
        Width = 660;
        KeyPreview = true;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;

        BuildLayout();

        Load += (_, _) => LoadCompanyInfo();
        KeyDown += BA00Form_KeyDown;
    }

    private void BuildLayout()
    {
        edtBigo.Multiline = true;
        edtBigo.Height = 60;

        var editPanel = new Panel { Dock = DockStyle.Fill };
        var grid = new GridLayout(editPanel, 10, 10, slotWidth: 290, labelWidth: 80, rowHeight: 30, slotsPerRow: 2);

        grid.Add("상호", edtSang);
        grid.Add("대표자", edtName);

        grid.Add("사업자번호", edtSa1, editWidth: 50);
        edtSa1.MaxLength = 3;
        edtSa2.Left = edtSa1.Right + 5; edtSa2.Top = edtSa1.Top; edtSa2.Width = 40; edtSa2.MaxLength = 2;
        edtSa3.Left = edtSa2.Right + 5; edtSa3.Top = edtSa1.Top; edtSa3.Width = 60; edtSa3.MaxLength = 5;
        editPanel.Controls.AddRange(new Control[] { edtSa2, edtSa3 });

        grid.Add("법인번호", edtNo1, editWidth: 80);
        edtNo1.MaxLength = 6;
        edtNo2.Left = edtNo1.Right + 5; edtNo2.Top = edtNo1.Top; edtNo2.Width = 80; edtNo2.MaxLength = 7;
        editPanel.Controls.Add(edtNo2);

        grid.Add("업태", edtUptae);
        grid.Add("종목", edtJong);

        grid.Add("우편번호", edtPost1, editWidth: 50);
        edtPost1.MaxLength = 3;
        edtPost2.Left = edtPost1.Right + 5; edtPost2.Top = edtPost1.Top; edtPost2.Width = 50; edtPost2.MaxLength = 3;
        var btnPost = new Button { Text = "검색", Left = edtPost2.Right + 5, Top = edtPost1.Top - 2, Width = 60 };
        btnPost.Click += (_, _) => LookupPostalCode();
        editPanel.Controls.AddRange(new Control[] { edtPost2, btnPost });

        grid.Add("지역번호", edtDDD);

        grid.Add("전화번호", edtTel);
        grid.Add("팩스번호", edtFax);

        grid.NewRow();
        grid.Add("주소", edtAddr, span: 2);
        edtAddr.DoubleClick += (_, _) => LookupPostalCode();

        grid.NewRow();
        grid.Add("비고", edtBigo, span: 2);

        // 비고가 Multiline(높이 60)이라 grid.Bottom()이 계산하는 표준 행 높이(rowHeight=30)보다
        // 실제로 더 아래까지 차지한다. 그 값 그대로 버튼 Y좌표를 잡으면 버튼이 비고 입력란에
        // 가려지므로, 비고의 실제 Bottom을 기준으로 버튼 위치를 잡는다.
        int y = edtBigo.Bottom + 20;
        btnSave.Left = 220; btnSave.Top = y; btnSave.Width = 100; btnSave.Height = 30;
        btnClose.Left = 330; btnClose.Top = y; btnClose.Width = 100; btnClose.Height = 30;
        editPanel.Controls.AddRange(new Control[] { btnSave, btnClose });

        Controls.Add(editPanel);

        // 버튼 아래로 남는 여백 없이 딱 맞게 창 높이를 잡는다(가로폭은 기존 그대로 유지).
        ClientSize = new Size(ClientSize.Width, y + btnSave.Height + 20);

        btnSave.Click += (_, _) => Save();
        btnClose.Click += (_, _) => Close();
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
