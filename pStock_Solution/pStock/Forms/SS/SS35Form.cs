using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS35.pas / SS35.dfm (TfrmSS35) 이식 — 거래처원장 조회.
/// Tab1: 거래처별 원장(입고/보관/출고/수금 통합 + 이월잔액부터 시작하는 누적 잔액).
/// Tab2: 품목별 입출고 내역(단순 목록).
/// </summary>
public partial class SS35Form : Form
{
    public SS35Form()
    {
        InitializeComponent();
    }

    private void SS35Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void BtnExcel_Click(object? sender, EventArgs e) => pStock.Common.ExcelExporter.Export(
        this.tabs.SelectedTab == this.tab1 ? this.grid1 : this.grid2,
        this.tabs.SelectedTab == this.tab1 ? "거래처별원장" : "품목별입출고");

    private void BtnPrint_Click(object? sender, EventArgs e) => pStock.Common.GridPrinter.Print(
        this.tabs.SelectedTab == this.tab1 ? this.grid1 : this.grid2,
        this.tabs.SelectedTab == this.tab1 ? "거래처별원장" : "품목별입출고");

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        this.edtCvcod.Clear(); this.dspName.Clear(); this.edtCd2.Clear(); this.dspNm2.Clear();
        this.grid1.DataSource = null; this.grid2.DataSource = null;
        this.lblAmt.Text = "0"; this.lblQty1.Text = "0"; this.lblQty2.Text = "0";
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        if (this.tabs.SelectedTab == this.tab1) this.LoadCustLedger(); else this.LoadItemIO();
    }

    private void BtnClose_Click(object? sender, EventArgs e) => this.Close();

    private void SS35Form_Load(object? sender, EventArgs e)
    {
        var now = DateTime.Now;
        this.dtpDate1.Value = new DateTime(now.Year, now.Month, 1);
        this.dtpDate2.Value = now;
    }

    private void EdtCvcod_KeyDown(object? sender, KeyEventArgs e) => LookupCvcod(e, edtCvcod, dspName);
    private void EdtCd2_KeyDown(object? sender, KeyEventArgs e) => LookupCvcod(e, edtCd2, dspNm2);

    private void LookupCvcod(KeyEventArgs e, TextBox edt, TextBox dsp)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edt.Text))
        {
            using var dlg = new Common.CvcodLookupForm(cvguFilter: "2");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edt.Text = dlg.SelectedCode;
                dsp.Text = dlg.SelectedName;
            }
            else
            {
                dsp.Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD AND CVGU IN('2','3')");
        q.ParamByName("CVCOD").AsString = edt.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edt.Text = q.FieldByName("CVCOD").AsString;
            dsp.Text = q.FieldByName("CVNAM").AsString;
        }
    }

    /// <summary>원본 prcPriorAmtGet: 조회 시작일 기준 이월잔액.</summary>
    private double GetPriorBalance()
    {
        var year = dtpDate1.Value.Year;
        var month = dtpDate1.Value.Month;
        var day = dtpDate1.Value.Day;

        using var q = new DbQuery();
        q.Add("SELECT SUM(OAMT01+OVAT01) I01,SUM(OAMT02+OVAT02) I02,SUM(OAMT03+OVAT03) I03,SUM(OAMT04+OVAT04) I04,");
        q.Add("       SUM(OAMT05+OVAT05) I05,SUM(OAMT06+OVAT06) I06,SUM(OAMT07+OVAT07) I07,SUM(OAMT08+OVAT08) I08,");
        q.Add("       SUM(OAMT09+OVAT09) I09,SUM(OAMT10+OVAT10) I10,SUM(OAMT11+OVAT11) I11,SUM(OAMT12+OVAT12) I12,");
        q.Add("       SUM(SAMT01) O01,SUM(SAMT02) O02,SUM(SAMT03) O03,SUM(SAMT04) O04,");
        q.Add("       SUM(SAMT05) O05,SUM(SAMT06) O06,SUM(SAMT07) O07,SUM(SAMT08) O08,");
        q.Add("       SUM(SAMT09) O09,SUM(SAMT10) O10,SUM(SAMT11) O11,SUM(SAMT12) O12,");
        q.Add("       SUM(BAMT) BAMT");
        q.Add("  FROM MISUF A,CVMAST B");
        q.Add(" WHERE MYEAR = @YEAR AND A.CVCOD=B.CVCOD");
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text)) q.Add("AND A.CVCOD=@CUST");
        q.ParamByName("YEAR").AsString = year.ToString();
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text)) q.ParamByName("CUST").AsString = edtCvcod.Text.Trim();
        q.Open();
        if (q.IsEmpty) return 0;

        double amt = q.FieldByName("BAMT").AsFloat;
        for (int i = 1; i < month; i++)
        {
            var idx = i.ToString("00");
            amt += q.FieldByName($"I{idx}").AsFloat - q.FieldByName($"O{idx}").AsFloat;
        }

        if (day != 1)
        {
            var cvcodFilter = string.IsNullOrWhiteSpace(edtCvcod.Text) ? "" : $" AND CVCOD = '{edtCvcod.Text.Trim()}'";
            using var q2 = new DbQuery();
            q2.Add("SELECT SUM(IAMT) AS TOTAL FROM (");
            q2.Add($"SELECT SUM(IAMT+JAMT1+JAMT2+JAMT3) IAMT FROM IPGOF WHERE IDATE BETWEEN @FDATE AND @TDATE{cvcodFilter}");
            q2.Add(" UNION");
            q2.Add($"SELECT SUM(ARAMT)*-1 FROM SUGMF WHERE ARDAT BETWEEN @FDATE AND @TDATE{cvcodFilter}");
            q2.Add(" UNION");
            q2.Add("SELECT SUM(TRAMT+JAMT1+JAMT2+JAMT3) FROM SALE_M A1");
            q2.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
            q2.Add($" WHERE TDATE BETWEEN @FDATE AND @TDATE{cvcodFilter}) AA");
            q2.ParamByName("FDATE").AsString = new DateTime(year, month, 1).ToString("yyyy-MM-dd");
            q2.ParamByName("TDATE").AsString = dtpDate1.Value.AddDays(-1).ToString("yyyy-MM-dd");
            q2.Open();
            amt += q2.IsEmpty || q2.FieldByName("TOTAL").IsNull ? 0 : q2.FieldByName("TOTAL").AsFloat;
        }

        return amt;
    }

    /// <summary>원본 prcCustDisp.</summary>
    private void LoadCustLedger()
    {
        double running = GetPriorBalance();

        grid1.Columns.Clear();
        grid1.Columns.Add("DATE", "일자");
        grid1.Columns.Add("GUBN", "구분");
        grid1.Columns.Add("ITEM", "품목/거래처코드");
        grid1.Columns.Add("CVNAM", "거래처명");
        grid1.Columns.Add("QTY", "수량");
        grid1.Columns.Add("IAMT", "입고금액");
        grid1.Columns.Add("BAMT", "보관금액");
        grid1.Columns.Add("OAMT", "출고금액");
        grid1.Columns.Add("SUMAMT", "합계(VAT포함)");
        grid1.Columns.Add("SUGM", "수금액");
        grid1.Columns.Add("BALANCE", "잔액");
        grid1.Rows.Clear();
        grid1.Rows.Add("", "이월금액", "", "", "", "", "", "", "", "", PublicLib.MoneyToStr((long)running));

        var where = string.IsNullOrWhiteSpace(edtCvcod.Text) ? "" : $" WHERE A1.CVCOD='{edtCvcod.Text.Trim()}'";

        using var q = new DbQuery();
        q.Add("SELECT A1.*, CVNAM, ITDSC, ISPEC");
        q.Add("  FROM");
        q.Add(" (SELECT IDATE, CVCOD, 'I' AS GUBN, ITNBR, SUM(IQTY) IQTY,");
        q.Add("         SUM(IAMT) IAMT, SUM(JAMT1+JAMT2+JAMT3) JAMT");
        q.Add("    FROM IPGOF");
        q.Add("   WHERE IDATE BETWEEN @DATE1 AND @DATE2 AND IGUBN='1'");
        q.Add("   GROUP BY IDATE,CVCOD,ITNBR");
        q.Add("  UNION ALL");
        q.Add("  SELECT TDATE,CVCOD,'B',ITNBR,SUM(IOQTY) OQTY,SUM(OAMT) BAMT,");
        q.Add("         SUM(JAMT1+JAMT2+JAMT3) JAMT FROM IPCHF");
        q.Add("   WHERE TDATE BETWEEN @DATE1 AND @DATE2 AND GUBN1='1'");
        q.Add("   GROUP BY TDATE,CVCOD,ITNBR");
        q.Add("  UNION ALL");
        q.Add("  SELECT TDATE,CVCOD,'O',ITCOD, SUM(TRQTY), SUM(TRAMT),");
        q.Add("         SUM(JAMT1+JAMT2+JAMT3) JAMT");
        q.Add("    FROM SALE_M A1");
        q.Add("    LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add("   WHERE TDATE BETWEEN @DATE1 AND @DATE2");
        q.Add("   GROUP BY TDATE, CVCOD, ITCOD");
        q.Add("  UNION ALL");
        q.Add("  SELECT ARDAT, CVCOD,'Z', '', 0, SUM(ARAMT),0 FROM SUGMF");
        q.Add("   WHERE ARDAT BETWEEN @DATE1 AND @DATE2");
        q.Add("   GROUP BY ARDAT, CVCOD )A1");
        q.Add("  LEFT OUTER JOIN ITEMAS B1 ON A1.ITNBR=B1.ITNBR");
        q.Add("  LEFT OUTER JOIN CVMAST B2 ON A1.CVCOD=B2.CVCOD" + where);
        q.Add(" ORDER BY IDATE, GUBN, ITNBR");
        q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        q.Open();

        double sumI = 0, sumB = 0, sumO = 0, sumTotal = 0, sumVat = 0, sumSugm = 0;

        if (!q.IsEmpty)
        {
            q.First();
            while (!q.Eof)
            {
                var gubn = q.FieldByName("GUBN").AsString;
                var iamt = q.FieldByName("IAMT").AsFloat;
                var jamt = q.FieldByName("JAMT").AsFloat;
                var date = q.FieldByName("IDATE").AsString;
                var itnbr = q.FieldByName("ITNBR").AsString;
                var itdsc = q.FieldByName("ITDSC").AsString;
                var ispec = q.FieldByName("ISPEC").AsString;
                var cvnam = q.FieldByName("CVNAM").AsString;
                var cvcod = q.FieldByName("CVCOD").AsString;
                var qty = q.FieldByName("IQTY").AsFloat;

                string gubnLabel; string itemCol = $"{itnbr} {itdsc} {ispec}".Trim();
                double iCol = 0, bCol = 0, oCol = 0, sugmCol = 0;

                if (gubn == "Z")
                {
                    gubnLabel = "수금"; itemCol = cvcod; sugmCol = iamt; sumSugm += iamt;
                }
                else if (gubn == "I") { gubnLabel = "입고"; iCol = iamt + jamt; sumI += iamt + jamt; }
                else if (gubn == "B") { gubnLabel = "보관"; bCol = iamt + jamt; sumB += iamt + jamt; }
                else { gubnLabel = "출고"; oCol = iamt + jamt; sumO += iamt + jamt; }

                double sum = iamt + jamt;
                double vat = Math.Truncate(sum / 10);

                if (gubn != "Z")
                {
                    running += Math.Truncate(sum * 1.1);
                    sumTotal += sum;
                    sumVat += vat;
                }
                else
                {
                    running -= iamt;
                }

                grid1.Rows.Add(date, gubnLabel, itemCol, cvnam,
                    gubn == "Z" ? "" : PublicLib.MoneyToStr((long)qty),
                    gubn == "I" ? PublicLib.MoneyToStr((long)iCol) : "",
                    gubn == "B" ? PublicLib.MoneyToStr((long)bCol) : "",
                    gubn == "O" ? PublicLib.MoneyToStr((long)oCol) : "",
                    gubn == "Z" ? "" : $"{PublicLib.MoneyToStr((long)sum)} / {PublicLib.MoneyToStr((long)vat)}",
                    gubn == "Z" ? PublicLib.MoneyToStr((long)sugmCol) : "",
                    PublicLib.MoneyToStr((long)running));

                q.Next();
            }
        }

        grid1.Rows.Add("", "", "합  계", "", "",
            PublicLib.MoneyToStr((long)sumI), PublicLib.MoneyToStr((long)sumB), PublicLib.MoneyToStr((long)sumO),
            $"{PublicLib.MoneyToStr((long)sumTotal)} / {PublicLib.MoneyToStr((long)sumVat)}",
            PublicLib.MoneyToStr((long)sumSugm), "");

        lblAmt.Text = PublicLib.MoneyToStr((long)running);
    }

    /// <summary>원본 prcItemIOdsp.</summary>
    private void LoadItemIO()
    {
        using var q = new DbQuery();
        q.Add("SELECT A1.*, CVNAM, B2.*");
        q.Add("  FROM");
        q.Add(" (SELECT TDATE, CVCOD, ITCOD, '출고' AS GUBN,");
        q.Add("         SEQNO, HOUSE, TRQTY IOQTY, DBIGO TBIGO");
        q.Add("    FROM SALE_M A1");
        q.Add("    LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add("   WHERE TDATE BETWEEN @DATE1 AND @DATE2");
        q.Add("  UNION");
        q.Add("  SELECT IDATE, CVCOD, ITNBR, '입고', ISEQ,HOUSE,IQTY,IBIGO");
        q.Add("    FROM IPGOF");
        q.Add("   WHERE IDATE BETWEEN @DATE1 AND @DATE2 AND IGUBN='1') A1");
        q.Add("  LEFT OUTER JOIN CVMAST B1 ON A1.CVCOD=B1.CVCOD");
        q.Add("  LEFT OUTER JOIN ITEMAS B2 ON A1.ITCOD=B2.ITNBR");
        q.Add(" WHERE A1.CVCOD LIKE @CVCOD");
        q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        q.ParamByName("CVCOD").AsString = edtCd2.Text.Trim() + "%";
        q.Open();

        grid2.Columns.Clear();
        grid2.Columns.Add("TDATE", "일자");
        grid2.Columns.Add("SEQNO", "순번");
        grid2.Columns.Add("GUBN", "구분");
        grid2.Columns.Add("ITDSC", "품명");
        grid2.Columns.Add("ITNBR", "품번");
        grid2.Columns.Add("HOUSE", "저장위치");
        grid2.Columns.Add("IQTY", "입고수량");
        grid2.Columns.Add("OQTY", "출고수량");
        grid2.Columns.Add("CVNAM", "거래처명");
        grid2.Columns.Add("TBIGO", "비고");
        grid2.Rows.Clear();

        double iQty = 0, oQty = 0;
        if (!q.IsEmpty)
        {
            q.First();
            while (!q.Eof)
            {
                var gubn = q.FieldByName("GUBN").AsString;
                var qty = q.FieldByName("IOQTY").AsFloat;
                var iCol = ""; var oCol = "";
                if (gubn == "입고") { iCol = PublicLib.MoneyToStr((long)qty); iQty += qty; }
                else { oCol = PublicLib.MoneyToStr((long)qty); oQty += qty; }

                grid2.Rows.Add(
                    q.FieldByName("TDATE").AsString, q.FieldByName("SEQNO").AsString, gubn,
                    $"{q.FieldByName("ITDSC").AsString} {q.FieldByName("ISPEC").AsString}".Trim(),
                    q.FieldByName("ITNBR").AsString, q.FieldByName("HOUSE").AsString,
                    iCol, oCol, q.FieldByName("CVNAM").AsString, q.FieldByName("TBIGO").AsString);

                q.Next();
            }
        }
        grid2.Rows.Add("", "", "", "합  계", "", "", PublicLib.MoneyToStr((long)iQty), PublicLib.MoneyToStr((long)oQty), "", "");

        lblQty1.Text = PublicLib.MoneyToStr((long)iQty);
        lblQty2.Text = PublicLib.MoneyToStr((long)oQty);
    }
}
