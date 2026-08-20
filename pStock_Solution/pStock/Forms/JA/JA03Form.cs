using pStock.Common;
using pStock.Data;

namespace pStock.Forms.JA;

/// <summary>
/// 원본 JA03.pas / JA03.dfm (TfrmJA03) 이식 — 일자별 집계작업(입고/보관/출고/수금 일계표 + 미수금 누적).
/// 원본 SG1(StringAlignGrid) 15개 컬럼 구조를 DataGridView로 재구성.
/// 컬럼: 일자, 입고금액/부가세1~3, 보관금액/부가세합계, 출고금액/부가세1~3, 합계, 합계/10, 수금액, 미수금잔액.
/// 거래처 검색 팝업(BA00C)은 아직 변환되지 않아 거래처코드 직접입력만 지원한다.
/// </summary>
public partial class JA03Form : Form
{
    public JA03Form()
    {
        InitializeComponent();
    }

    private void JA03Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void ShiftMonth(int delta)
    {
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var d))
            d = DateTime.Now;
        edtMonth.Text = d.AddMonths(delta).ToString("yyyy-MM");
        ReloadList();
    }

    /// <summary>원본 edtCvcodKeyDown.</summary>
    private void EdtCvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            using var dlg = new Common.CvcodLookupForm(cvguFilter: "2");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCvcod.Text = dlg.SelectedCode;
                dspName.Text = dlg.SelectedName;
            }
            else
            {
                dspName.Clear();
            }
            ReloadList();
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD AND CVGU IN('2','3')");
        q.ParamByName("CVCOD").AsString = edtCvcod.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edtCvcod.Text = q.FieldByName("CVCOD").AsString;
            dspName.Text = q.FieldByName("CVNAM").AsString;
        }
        ReloadList();
    }

    /// <summary>원본 prcDBopen + prcMonthSum + prcYearSum + prcCalMisu.</summary>
    private void ReloadList()
    {
        grid.Rows.Clear();
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var monthDate)) return;

        var cvcodFilter = string.IsNullOrWhiteSpace(edtCvcod.Text) ? "" : $" AND CVCOD = '{edtCvcod.Text.Trim()}'";
        var cvcodFilterA = string.IsNullOrWhiteSpace(edtCvcod.Text) ? "" : $" AND A.CVCOD = '{edtCvcod.Text.Trim()}'";

        double carryBalance = GetPriorBalance(monthDate);
        grid.Rows.Add("[이월]", "", "", "", "", "", "", "", "", "", "", "", "", "", PublicLib.MoneyToStr((long)carryBalance));

        using var q = new DbQuery();
        q.Add("SELECT IDATE, 'I' GUBN, SUM(CAST(IQTY AS FLOAT)) QTY, SUM(CAST(IAMT AS FLOAT)) AMT,");
        q.Add("       SUM(CAST(JAMT1 AS FLOAT)) J1, SUM(CAST(JAMT2 AS FLOAT)) J2, SUM(CAST(JAMT3 AS FLOAT)) J3");
        q.Add("  FROM IPGOF");
        q.Add($" WHERE IDATE LIKE @YYMM{cvcodFilter}");
        q.Add(" GROUP BY IDATE");
        q.Add("UNION ALL");
        q.Add("SELECT TDATE, 'B', SUM(CAST(IOQTY AS FLOAT)), SUM(CAST(OAMT AS FLOAT)),");
        q.Add("       SUM(CAST(JAMT1 AS FLOAT)) J1, SUM(CAST(JAMT2 AS FLOAT)) J2, SUM(CAST(JAMT3 AS FLOAT)) J3");
        q.Add("  FROM IPCHF");
        q.Add($" WHERE TDATE LIKE @YYMM AND GUBN1='1'{cvcodFilter}");
        q.Add(" GROUP BY TDATE");
        q.Add("UNION ALL");
        q.Add("SELECT TDATE, 'O', SUM(CAST(TRQTY AS FLOAT)), SUM(CAST(TRAMT AS FLOAT)),");
        q.Add("       SUM(CAST(JAMT1 AS FLOAT)) J1, SUM(CAST(JAMT2 AS FLOAT)) J2, SUM(CAST(JAMT3 AS FLOAT)) J3");
        q.Add("  FROM SALE_M A1");
        q.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add($" WHERE TDATE LIKE @YYMM{cvcodFilterA}");
        q.Add(" GROUP BY TDATE");
        q.ParamByName("YYMM").AsString = edtMonth.Text.Trim() + "%";
        q.Open();

        var byDate = new SortedDictionary<string, (double iamt, double ij1, double ij2, double ij3,
            double bamt, double bvat, double oamt, double oj1, double oj2, double oj3)>();

        if (!q.IsEmpty)
        {
            q.First();
            while (!q.Eof)
            {
                var date = q.FieldByName("IDATE").AsString;
                var gubn = q.FieldByName("GUBN").AsString;
                var amt = q.FieldByName("AMT").AsFloat;
                var j1 = q.FieldByName("J1").AsFloat;
                var j2 = q.FieldByName("J2").AsFloat;
                var j3 = q.FieldByName("J3").AsFloat;

                if (!byDate.ContainsKey(date)) byDate[date] = default;
                var row = byDate[date];
                if (gubn == "I") row = row with { iamt = amt, ij1 = j1, ij2 = j2, ij3 = j3 };
                else if (gubn == "B") row = row with { bamt = amt, bvat = j1 + j2 + j3 };
                else if (gubn == "O") row = row with { oamt = amt, oj1 = j1, oj2 = j2, oj3 = j3 };
                byDate[date] = row;

                q.Next();
            }
        }

        double monthSumIamt = 0, monthSumJ1 = 0, monthSumJ2 = 0, monthSumJ3 = 0, monthSumBamt = 0, monthSumBvat = 0,
               monthSumOamt = 0, monthSumOj1 = 0, monthSumOj2 = 0, monthSumOj3 = 0, monthSumSugm = 0;
        double running = carryBalance;

        foreach (var (date, v) in byDate)
        {
            double sum = v.iamt + v.ij1 + v.ij2 + v.ij3 + v.bamt + v.bvat + v.oamt + v.oj1 + v.oj2 + v.oj3;
            double sugm = GetCollectionAmount(date);
            running += sum + Math.Truncate(sum / 10) - sugm;

            monthSumIamt += v.iamt; monthSumJ1 += v.ij1; monthSumJ2 += v.ij2; monthSumJ3 += v.ij3;
            monthSumBamt += v.bamt; monthSumBvat += v.bvat;
            monthSumOamt += v.oamt; monthSumOj1 += v.oj1; monthSumOj2 += v.oj2; monthSumOj3 += v.oj3;
            monthSumSugm += sugm;

            grid.Rows.Add(
                date.Length >= 8 ? date[8..] : date,
                PublicLib.MoneyToStr((long)v.iamt), PublicLib.MoneyToStr((long)v.ij1),
                PublicLib.MoneyToStr((long)v.ij2), PublicLib.MoneyToStr((long)v.ij3),
                PublicLib.MoneyToStr((long)v.bamt), PublicLib.MoneyToStr((long)v.bvat),
                PublicLib.MoneyToStr((long)v.oamt), PublicLib.MoneyToStr((long)v.oj1),
                PublicLib.MoneyToStr((long)v.oj2), PublicLib.MoneyToStr((long)v.oj3),
                PublicLib.MoneyToStr((long)sum), PublicLib.MoneyToStr((long)Math.Truncate(sum / 10)),
                PublicLib.MoneyToStr((long)sugm), PublicLib.MoneyToStr((long)running));
        }

        var monthSum = monthSumIamt + monthSumJ1 + monthSumJ2 + monthSumJ3 + monthSumBamt + monthSumBvat +
                       monthSumOamt + monthSumOj1 + monthSumOj2 + monthSumOj3;
        grid.Rows.Add("[월계]",
            PublicLib.MoneyToStr((long)monthSumIamt), PublicLib.MoneyToStr((long)monthSumJ1),
            PublicLib.MoneyToStr((long)monthSumJ2), PublicLib.MoneyToStr((long)monthSumJ3),
            PublicLib.MoneyToStr((long)monthSumBamt), PublicLib.MoneyToStr((long)monthSumBvat),
            PublicLib.MoneyToStr((long)monthSumOamt), PublicLib.MoneyToStr((long)monthSumOj1),
            PublicLib.MoneyToStr((long)monthSumOj2), PublicLib.MoneyToStr((long)monthSumOj3),
            PublicLib.MoneyToStr((long)monthSum), PublicLib.MoneyToStr((long)Math.Truncate(monthSum / 10)),
            PublicLib.MoneyToStr((long)monthSumSugm), "");

        AddYearSumRow(monthDate, cvcodFilter, cvcodFilterA);
    }

    /// <summary>원본 prcPriorAmtGet.</summary>
    private double GetPriorBalance(DateTime monthDate)
    {
        using var q = new DbQuery();
        q.Add("SELECT SUM(CAST(OAMT01+OVAT01 AS FLOAT)) I01,SUM(CAST(OAMT02+OVAT02 AS FLOAT)) I02,SUM(CAST(OAMT03+OVAT03 AS FLOAT)) I03,SUM(CAST(OAMT04+OVAT04 AS FLOAT)) I04,");
        q.Add("       SUM(CAST(OAMT05+OVAT05 AS FLOAT)) I05,SUM(CAST(OAMT06+OVAT06 AS FLOAT)) I06,SUM(CAST(OAMT07+OVAT07 AS FLOAT)) I07,SUM(CAST(OAMT08+OVAT08 AS FLOAT)) I08,");
        q.Add("       SUM(CAST(OAMT09+OVAT09 AS FLOAT)) I09,SUM(CAST(OAMT10+OVAT10 AS FLOAT)) I10,SUM(CAST(OAMT11+OVAT11 AS FLOAT)) I11,SUM(CAST(OAMT12+OVAT12 AS FLOAT)) I12,");
        q.Add("       SUM(CAST(SAMT01 AS FLOAT)) O01,SUM(CAST(SAMT02 AS FLOAT)) O02,SUM(CAST(SAMT03 AS FLOAT)) O03,SUM(CAST(SAMT04 AS FLOAT)) O04,");
        q.Add("       SUM(CAST(SAMT05 AS FLOAT)) O05,SUM(CAST(SAMT06 AS FLOAT)) O06,SUM(CAST(SAMT07 AS FLOAT)) O07,SUM(CAST(SAMT08 AS FLOAT)) O08,");
        q.Add("       SUM(CAST(SAMT09 AS FLOAT)) O09,SUM(CAST(SAMT10 AS FLOAT)) O10,SUM(CAST(SAMT11 AS FLOAT)) O11,SUM(CAST(SAMT12 AS FLOAT)) O12,");
        q.Add("       SUM(CAST(BAMT AS FLOAT)) BAMT");
        q.Add("  FROM MISUF A,CVMAST B");
        q.Add(" WHERE MYEAR = @YEAR AND A.CVCOD=B.CVCOD");
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text)) q.Add($" AND A.CVCOD='{edtCvcod.Text.Trim()}'");
        q.ParamByName("YEAR").AsString = monthDate.Year.ToString();
        q.Open();

        if (q.IsEmpty) return 0;

        double amt = q.FieldByName("BAMT").AsFloat;
        for (int i = 1; i < monthDate.Month; i++)
        {
            var idx = i.ToString("00");
            amt += q.FieldByName($"I{idx}").AsFloat;
            amt -= q.FieldByName($"O{idx}").AsFloat;
        }
        return amt;
    }

    /// <summary>원본 prcDBopen 내 SUGMF 조회(해당 일자 수금액).</summary>
    private double GetCollectionAmount(string date)
    {
        using var q = new DbQuery();
        q.Add($"SELECT SUM(CAST(ARAMT AS FLOAT)) AS AMT FROM SUGMF WHERE ARDAT='{date}'");
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text)) q.Add($" AND CVCOD='{edtCvcod.Text.Trim()}'");
        q.Open();
        return q.IsEmpty || q.FieldByName("AMT").IsNull ? 0 : q.FieldByName("AMT").AsFloat;
    }

    /// <summary>원본 prcYearSum.</summary>
    private void AddYearSumRow(DateTime monthDate, string cvcodFilter, string cvcodFilterA)
    {
        using var q = new DbQuery();
        q.Add("SELECT 'I' AS GUBN,SUM(CAST(IQTY AS FLOAT)) QTY,SUM(CAST(IAMT AS FLOAT)) AMT,");
        q.Add("       SUM(CAST(JAMT1 AS FLOAT)) J1,SUM(CAST(JAMT2 AS FLOAT)) J2,SUM(CAST(JAMT3 AS FLOAT)) J3 FROM IPGOF");
        q.Add($" WHERE IDATE BETWEEN @DATE1 AND @DATE2{cvcodFilter}");
        q.Add(" UNION ALL");
        q.Add("SELECT 'B',SUM(CAST(IOQTY AS FLOAT)) QTY,SUM(CAST(OAMT AS FLOAT)) AMT,");
        q.Add("       SUM(CAST(JAMT1 AS FLOAT)) J1,SUM(CAST(JAMT2 AS FLOAT)) J2,SUM(CAST(JAMT3 AS FLOAT)) J3 FROM IPCHF");
        q.Add($" WHERE TDATE BETWEEN @DATE1 AND @DATE2 AND GUBN1='1'{cvcodFilter}");
        q.Add(" UNION ALL");
        q.Add("SELECT 'O',SUM(CAST(TRQTY AS FLOAT)) QTY,SUM(CAST(TRAMT AS FLOAT)) AMT,");
        q.Add("       SUM(CAST(JAMT1 AS FLOAT)) J1,SUM(CAST(JAMT2 AS FLOAT)) J2,SUM(CAST(JAMT3 AS FLOAT)) J3");
        q.Add("  FROM SALE_M A1");
        q.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add($" WHERE TDATE BETWEEN @DATE1 AND @DATE2{cvcodFilterA}");
        q.ParamByName("DATE1").AsString = $"{monthDate.Year}-01-01";
        q.ParamByName("DATE2").AsString = edtMonth.Text.Trim() + "-31";
        q.Open();

        double iamt = 0, j1 = 0, j2 = 0, j3 = 0, bamt = 0, bvat = 0, oamt = 0, oj1 = 0, oj2 = 0, oj3 = 0;
        if (!q.IsEmpty)
        {
            q.First();
            while (!q.Eof)
            {
                var gubn = q.FieldByName("GUBN").AsString;
                if (gubn == "I") { iamt = q.FieldByName("AMT").AsFloat; j1 = q.FieldByName("J1").AsFloat; j2 = q.FieldByName("J2").AsFloat; j3 = q.FieldByName("J3").AsFloat; }
                else if (gubn == "B") { bamt = q.FieldByName("AMT").AsFloat; bvat = q.FieldByName("J1").AsFloat + q.FieldByName("J2").AsFloat + q.FieldByName("J3").AsFloat; }
                else if (gubn == "O") { oamt = q.FieldByName("AMT").AsFloat; oj1 = q.FieldByName("J1").AsFloat; oj2 = q.FieldByName("J2").AsFloat; oj3 = q.FieldByName("J3").AsFloat; }
                q.Next();
            }
        }

        double sum = iamt + j1 + j2 + j3 + bamt + bvat + oamt + oj1 + oj2 + oj3;

        using var qs = new DbQuery();
        qs.Add("SELECT SUM(CAST(ARAMT AS FLOAT)) AS AMT FROM SUGMF WHERE ARDAT BETWEEN @DATE1 AND @DATE2");
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text)) qs.Add($" AND CVCOD='{edtCvcod.Text.Trim()}'");
        qs.ParamByName("DATE1").AsString = $"{monthDate.Year}-01-01";
        qs.ParamByName("DATE2").AsString = edtMonth.Text.Trim() + "-31";
        qs.Open();
        double sugm = qs.IsEmpty || qs.FieldByName("AMT").IsNull ? 0 : qs.FieldByName("AMT").AsFloat;

        grid.Rows.Add("[누계]",
            PublicLib.MoneyToStr((long)iamt), PublicLib.MoneyToStr((long)j1),
            PublicLib.MoneyToStr((long)j2), PublicLib.MoneyToStr((long)j3),
            PublicLib.MoneyToStr((long)bamt), PublicLib.MoneyToStr((long)bvat),
            PublicLib.MoneyToStr((long)oamt), PublicLib.MoneyToStr((long)oj1),
            PublicLib.MoneyToStr((long)oj2), PublicLib.MoneyToStr((long)oj3),
            PublicLib.MoneyToStr((long)sum), PublicLib.MoneyToStr((long)Math.Truncate(sum / 10)),
            PublicLib.MoneyToStr((long)sugm), "");
    }

    private void EdtMonth_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) ReloadList();
    }

    private void BtnMonthDown_Click(object? sender, EventArgs e)
    {
        ShiftMonth(-1); ReloadList();
    }

    private void BtnMonthUp_Click(object? sender, EventArgs e)
    {
        ShiftMonth(1); ReloadList();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        this.edtCvcod.Clear(); this.dspName.Clear(); this.grid.Rows.Clear(); this.edtCvcod.Focus();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        ReloadList();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void JA03Form_Load(object? sender, EventArgs e)
    {
        this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); ReloadList();
    }
}
