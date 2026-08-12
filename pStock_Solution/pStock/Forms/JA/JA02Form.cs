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
public partial class JA02Form : Form
{
    public JA02Form()
    {
        InitializeComponent();
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

    private void EdtYear_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) LoadYear();
    }

    private void BtnYearDown_Click(object? sender, EventArgs e)
    {
        this.edtYear.Text = (PublicLib.StrToIntSafe(this.edtYear.Text) - 1).ToString(); LoadYear();
    }

    private void BtnYearUp_Click(object? sender, EventArgs e)
    {
        this.edtYear.Text = (PublicLib.StrToIntSafe(this.edtYear.Text) + 1).ToString(); LoadYear();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        ClearEdit();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        LoadYear();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void JA02Form_Load(object? sender, EventArgs e)
    {
        ResetHouseList(); ClearEdit();
    }
}
