using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.JA;

/// <summary>
/// 원본 JA02.pas / JA02.dfm (TfrmJA02) 이식 — 재고관리-년간(품목 1건의 12개월 수불 현황).
/// 원본은 StringAlignGrid(SG1)에 1~6월/7~12월을 나란히 배치했지만, 여기서는
/// DataGridView에 1~12월을 세로로 나열하는 더 단순한 구조로 재구성했다.
///
/// 원본 qryList 기본 SQL은 JA01과 동일한 ITEMBL 조인(BaseSql)에
/// ITNBR/HOUSE 조건만 추가한 것이라, JA01Form.BaseSql과 같은 쿼리를 재사용한다.
/// 품목 검색 팝업(BA00D)은 아직 변환되지 않아 품번 직접입력만 지원한다.
/// </summary>
public class JA02Form : Form
{
    private readonly TextBox edtCode = new();   // 품번
    private readonly TextBox dspName = new() { ReadOnly = true };
    private readonly TextBox edtYear = new();
    private readonly ComboBox cboHouse = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly FastDataGridView grid = new();

    private readonly Button btnNew = new() { Text = "초기화(F1)" };
    private readonly Button btnSearch = new() { Text = "조회(F5)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };
    private readonly Button btnYearDown = new() { Text = "◀" };
    private readonly Button btnYearUp = new() { Text = "▶" };

    public JA02Form()
    {
        Text = "재고관리-년간";
        Width = 900;
        Height = 600;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { ResetHouseList(); ClearEdit(); };
        KeyDown += JA02Form_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnSearch, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnSearch, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lblCode = new Label { Text = "품번", Left = 5, Top = 12, AutoSize = true };
        edtCode.Left = 50; edtCode.Top = 8; edtCode.Width = 100;
        edtCode.KeyDown += EdtCode_KeyDown;
        dspName.Left = 160; dspName.Top = 8; dspName.Width = 250;

        var lblYear = new Label { Text = "년도", Left = 420, Top = 12, AutoSize = true };
        edtYear.Left = 460; edtYear.Top = 8; edtYear.Width = 60;
        edtYear.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) LoadYear(); };
        btnYearDown.Left = 525; btnYearDown.Top = 8; btnYearDown.Width = 30;
        btnYearUp.Left = 558; btnYearUp.Top = 8; btnYearUp.Width = 30;
        btnYearDown.Click += (_, _) => { edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) - 1).ToString(); LoadYear(); };
        btnYearUp.Click += (_, _) => { edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) + 1).ToString(); LoadYear(); };

        var lblHouse = new Label { Text = "저장위치", Left = 600, Top = 12, AutoSize = true };
        cboHouse.Left = 660; cboHouse.Top = 8; cboHouse.Width = 100;

        editPanel.Controls.AddRange(new Control[]
        {
            lblCode, edtCode, dspName, lblYear, edtYear, btnYearDown, btnYearUp, lblHouse, cboHouse
        });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.Columns.Add("MONTH", "월");
        grid.Columns.Add("BQTY", "기초재고");
        grid.Columns.Add("IQTY", "입고수량");
        grid.Columns.Add("OQTY", "출고수량");
        grid.Columns.Add("XQTY", "재고조정");
        grid.Columns.Add("JQTY", "재고");
        grid.Columns.Add("IAMT", "입고금액");
        grid.Columns.Add("SAMT", "출고금액");
        grid.Columns.Add("OAMT", "보관금액");
        grid.Columns.Add("TAMT", "누계금액");

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => ClearEdit();
        btnSearch.Click += (_, _) => LoadYear();
        btnClose.Click += (_, _) => Close();
    }

    private void JA02Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void ResetHouseList()
    {
        cboHouse.Items.Clear();
        cboHouse.Items.Add(" ");
        foreach (var (refno, _) in ReffpfCache.Get("CG")) cboHouse.Items.Add(refno);
        cboHouse.SelectedIndex = 0;
    }

    private void ClearEdit()
    {
        edtCode.Clear();
        dspName.Clear();
        edtYear.Text = DateTime.Now.Year.ToString();
        if (cboHouse.Items.Count > 0) cboHouse.SelectedIndex = 0;
        grid.Rows.Clear();
        edtCode.Focus();
    }

    /// <summary>원본 edtCodeKeyDown: 품번 Enter 시 품목 정보 조회.</summary>
    private void EdtCode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCode.Text))
        {
            using var dlg = new Common.ItemLookupForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCode.Text = dlg.SelectedItnbr;
                dspName.Text = $"{dlg.SelectedItdsc} {dlg.SelectedIspec}".Trim();
            }
            else
            {
                dspName.Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM ITEMAS WHERE ITNBR=@ITNBR");
        q.ParamByName("ITNBR").AsString = edtCode.Text.Trim();
        q.Open();
        dspName.Text = q.IsEmpty ? string.Empty : $"{q.FieldByName("ITDSC").AsString} {q.FieldByName("ISPEC").AsString}";
    }

    /// <summary>원본 btnSearchClick (prcDBopen + prcAmtSum).</summary>
    private void LoadYear()
    {
        if (string.IsNullOrWhiteSpace(edtCode.Text) || string.IsNullOrWhiteSpace(edtYear.Text)) return;

        using var q = new DbQuery();
        q.Add(pStock.Common.SqlFragments.ItemblBaseSql);
        q.Add($"AND A.ITNBR = '{edtCode.Text.Trim()}'");
        q.Add($"AND HOUSE = '{cboHouse.Text.Trim()}'");
        q.ParamByName("YEAR").AsString = edtYear.Text.Trim();
        q.Open();

        grid.Rows.Clear();

        if (q.IsEmpty)
        {
            for (int m = 1; m <= 12; m++) grid.Rows.Add(m + "월", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return;
        }

        double running = q.FieldByName("BBALQ").AsCurrency > 0 ? (double)q.FieldByName("BBALQ").AsCurrency : ToDbl(q, "BBALQ");
        for (int m = 1; m <= 12; m++)
        {
            var mm = m.ToString("00");
            double iqty = ToDbl(q, $"I1QT{mm}");
            double oqty = ToDbl(q, $"O1QT{mm}");
            double xqty = ToDbl(q, $"IOQT{mm}");
            double jqty = running + iqty - oqty - xqty;

            var (iamt, samt, oamt) = MonthlyAmounts(m);

            grid.Rows.Add(m + "월",
                PublicLib.MoneyToStr((long)running),
                PublicLib.MoneyToStr((long)iqty),
                PublicLib.MoneyToStr((long)oqty),
                PublicLib.MoneyToStr((long)xqty),
                PublicLib.MoneyToStr((long)jqty),
                PublicLib.MoneyToStr((long)iamt),
                PublicLib.MoneyToStr((long)samt),
                PublicLib.MoneyToStr((long)oamt),
                PublicLib.MoneyToStr((long)(iamt + samt + oamt)));

            running = jqty;
        }
    }

    private static double ToDbl(DbQuery q, string field) =>
        q.FieldByName(field).IsNull ? 0 : (double)q.FieldByName(field).AsCurrency;

    /// <summary>원본 prcAmtSum의 월별 금액 3종 조회(입고/출고/보관).</summary>
    private (double iamt, double samt, double oamt) MonthlyAmounts(int month)
    {
        var period = $"{edtYear.Text.Trim()}-{month:00}%";

        using (var q1 = new DbQuery())
        {
            q1.Add("SELECT SUM(IAMT) AS IAMT FROM IPGOF WHERE IDATE LIKE @IDATE AND IGUBN='1'");
            q1.Add($"AND ITNBR = '{edtCode.Text.Trim()}' AND HOUSE = '{cboHouse.Text.Trim()}'");
            q1.ParamByName("IDATE").AsString = period;
            q1.Open();
            double iamt = q1.IsEmpty || q1.FieldByName("IAMT").IsNull ? 0 : (double)q1.FieldByName("IAMT").AsCurrency;

            using var q2 = new DbQuery();
            q2.Add("SELECT SUM(TRAMT) AS SAMT FROM SALE_M A1");
            q2.Add("LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
            q2.Add("WHERE TDATE LIKE @TDATE");
            q2.Add($"AND ITCOD = '{edtCode.Text.Trim()}' AND HOUSE = '{cboHouse.Text.Trim()}'");
            q2.ParamByName("TDATE").AsString = period;
            q2.Open();
            double samt = q2.IsEmpty || q2.FieldByName("SAMT").IsNull ? 0 : (double)q2.FieldByName("SAMT").AsCurrency;

            using var q3 = new DbQuery();
            q3.Add("SELECT SUM(OAMT) AS OAMT FROM IPCHF WHERE TDATE LIKE @TDATE AND GUBN1='1'");
            q3.Add($"AND ITNBR = '{edtCode.Text.Trim()}' AND HOUSE = '{cboHouse.Text.Trim()}'");
            q3.ParamByName("TDATE").AsString = period;
            q3.Open();
            double oamt = q3.IsEmpty || q3.FieldByName("OAMT").IsNull ? 0 : (double)q3.FieldByName("OAMT").AsCurrency;

            return (iamt, samt, oamt);
        }
    }
}

