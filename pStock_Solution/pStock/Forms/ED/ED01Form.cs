using pStock.Data;

namespace pStock.Forms.ED;

/// <summary>
/// 원본 ED01.pas / ED01.dfm (TfrmED01) 이식 — 월마감 작업.
/// 해당 월의 입고/보관/출고/수금 자료를 ITEMBL(재고)/MISUF(미수금) 파일에 반영(마감)한다.
/// 진행률 게이지는 단순 ProgressBar로 대체했다.
/// </summary>
public class ED01Form : Form
{
    private readonly TextBox edtYear = new();
    private readonly ComboBox cboMonth = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly CheckBox chk1 = new() { Text = "입고 마감" };
    private readonly CheckBox chk2 = new() { Text = "보관 마감" };
    private readonly CheckBox chk3 = new() { Text = "출고 마감" };
    private readonly CheckBox chk4 = new() { Text = "수금 마감" };
    private readonly CheckBox chk5 = new() { Text = "임시자료 정리" };
    private readonly ProgressBar progress = new();
    private readonly Button btnOk = new() { Text = "마감실행(F2)" };
    private readonly Button btnCancel = new() { Text = "취소(Esc)" };

    public ED01Form()
    {
        Text = "월마감 작업";
        Width = 450;
        Height = 400;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) =>
        {
            edtYear.Text = DateTime.Now.Year.ToString();
            cboMonth.SelectedItem = DateTime.Now.Month.ToString();
        };
        KeyDown += ED01Form_KeyDown;
    }

    private void BuildLayout()
    {
        for (int i = 1; i <= 12; i++) cboMonth.Items.Add(i.ToString());

        var lblYear = new Label { Text = "년도", Left = 20, Top = 20, AutoSize = true };
        edtYear.Left = 70; edtYear.Top = 16; edtYear.Width = 60;
        var lblMonth = new Label { Text = "월", Left = 150, Top = 20, AutoSize = true };
        cboMonth.Left = 180; cboMonth.Top = 16; cboMonth.Width = 60;

        int y = 60;
        chk1.Left = 20; chk1.Top = y; chk1.AutoSize = true; y += 26;
        chk2.Left = 20; chk2.Top = y; chk2.AutoSize = true; y += 26;
        chk3.Left = 20; chk3.Top = y; chk3.AutoSize = true; y += 26;
        chk4.Left = 20; chk4.Top = y; chk4.AutoSize = true; y += 26;
        chk5.Left = 20; chk5.Top = y; chk5.AutoSize = true; y += 40;
        foreach (var c in new[] { chk1, chk2, chk3, chk4, chk5 }) c.Checked = true;

        progress.Left = 20; progress.Top = y; progress.Width = 390; y += 40;

        btnOk.Left = 100; btnOk.Top = y; btnOk.Width = 100;
        btnCancel.Left = 220; btnCancel.Top = y; btnCancel.Width = 100;

        Controls.AddRange(new Control[]
        {
            lblYear, edtYear, lblMonth, cboMonth,
            chk1, chk2, chk3, chk4, chk5, progress, btnOk, btnCancel
        });

        btnOk.Click += (_, _) => RunClose();
        btnCancel.Click += (_, _) => Close();
    }

    private void ED01Form_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F2) btnOk.PerformClick();
        else if (e.KeyCode == Keys.Escape) Close();
    }

    /// <summary>원본 btnOkClick.</summary>
    private void RunClose()
    {
        var year = edtYear.Text.Trim();
        var month = (cboMonth.SelectedIndex + 1).ToString("00");

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            if (!InitJob(year, month))
            {
                AppDb.Rollback();
                MessageBox.Show("월 마감 초기화 작업중 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (chk1.Checked && !IpgoJob(year, month))
            {
                AppDb.Rollback();
                MessageBox.Show("월 입고업무 마감시 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chk2.Checked && !SaveJob(year, month))
            {
                AppDb.Rollback();
                MessageBox.Show("월 보관업무 마감시 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chk3.Checked && !ChulJob(year, month))
            {
                AppDb.Rollback();
                MessageBox.Show("월 출고업무 마감시 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chk4.Checked && !SugmJob(year, month))
            {
                AppDb.Rollback();
                MessageBox.Show("수금 마감시 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chk5.Checked && !DeleteJob())
            {
                AppDb.Rollback();
                MessageBox.Show("자료정리 작업중 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("마감작업중 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show($"{year}년 {int.Parse(month)}월 마감작업이 완료되었습니다.", "확인",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    /// <summary>원본 fncInitJob: 당월 입출고/매출/수금 컬럼 0으로 초기화.</summary>
    private static bool InitJob(string year, string month)
    {
        try
        {
            using (var q = new DbQuery())
            {
                q.Add($"UPDATE ITEMBL SET I1QT{month}=0, O1QT{month}=0 WHERE IYEAR=@YEAR");
                q.ParamByName("YEAR").AsString = year;
                q.ExecSQL();
            }
            using (var q = new DbQuery())
            {
                q.Add($"UPDATE MISUF SET OAMT{month}=0, OVAT{month}=0, SAMT{month}=0 WHERE MYEAR=@YEAR");
                q.ParamByName("YEAR").AsString = year;
                q.ExecSQL();
            }
            return true;
        }
        catch { return false; }
    }

    /// <summary>원본 fncIpgojob: 당월 입고를 재고/미수파일에 반영.</summary>
    private static bool IpgoJob(string year, string month)
    {
        using var q = new DbQuery();
        q.Add("SELECT HOUSE,CVCOD,ITNBR,SUM(IQTY) IQTY,");
        q.Add("       SUM(IAMT+JAMT1+JAMT2+JAMT3) IAMT FROM IPGOF");
        q.Add(" WHERE IDATE LIKE @MONTH AND IGUBN='1'");
        q.Add(" GROUP BY HOUSE,CVCOD,ITNBR");
        q.ParamByName("MONTH").AsString = $"{year}-{month}%";
        q.Open();
        if (q.IsEmpty) return true;

        q.First();
        while (!q.Eof)
        {
            if (!LedgerUpdates.ItemblUpdate(year, month, q.FieldByName("ITNBR").AsString,
                    q.FieldByName("HOUSE").AsString, q.FieldByName("IQTY").AsInteger, 0, 0)) return false;
            if (!LedgerUpdates.MisuUpdate(year, month, q.FieldByName("CVCOD").AsString, 1,
                    q.FieldByName("IAMT").AsInteger, 0, 0)) return false;
            q.Next();
        }
        return true;
    }

    /// <summary>원본 fncSaveJob: 당월 보관료를 미수파일에 반영.</summary>
    private static bool SaveJob(string year, string month)
    {
        using var q = new DbQuery();
        q.Add("SELECT CVCOD,SUM(OAMT+JAMT1+JAMT2+JAMT3) TAMT FROM IPCHF");
        q.Add(" WHERE TDATE LIKE @MONTH AND GUBN1='1'");
        q.Add(" GROUP BY CVCOD");
        q.ParamByName("MONTH").AsString = $"{year}-{month}%";
        q.Open();
        if (q.IsEmpty) return true;

        q.First();
        while (!q.Eof)
        {
            if (!LedgerUpdates.MisuUpdate(year, month, q.FieldByName("CVCOD").AsString, 1,
                    q.FieldByName("TAMT").AsInteger, 0, 0)) return false;
            q.Next();
        }
        return true;
    }

    /// <summary>원본 fncChulJob: 당월 출고를 미수/재고파일에 반영.</summary>
    private static bool ChulJob(string year, string month)
    {
        using (var q = new DbQuery())
        {
            q.Add("SELECT CVCOD, SUM(TAMT) TAMT");
            q.Add("  FROM SALE_M A1");
            q.Add("  LEFT OUTER JOIN");
            q.Add("      (SELECT SALNO, SUM(TRAMT+JAMT1+JAMT2+JAMT3) TAMT");
            q.Add("         FROM SALE_D");
            q.Add("        GROUP BY SALNO) A2 ON A1.SALNO=A2.SALNO");
            q.Add(" WHERE TDATE LIKE @MONTH");
            q.Add(" GROUP BY CVCOD");
            q.ParamByName("MONTH").AsString = $"{year}-{month}%";
            q.Open();
            if (!q.IsEmpty)
            {
                q.First();
                while (!q.Eof)
                {
                    if (!LedgerUpdates.MisuUpdate(year, month, q.FieldByName("CVCOD").AsString, 1,
                            q.FieldByName("TAMT").AsInteger, 0, 0)) return false;
                    q.Next();
                }
            }
        }

        using (var q = new DbQuery())
        {
            q.Add("SELECT HOUSE, ITCOD, SUM(TRQTY) TQTY");
            q.Add("  FROM SALE_M A1");
            q.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
            q.Add(" WHERE TDATE LIKE @MONTH");
            q.Add(" GROUP BY HOUSE, ITCOD");
            q.ParamByName("MONTH").AsString = $"{year}-{month}%";
            q.Open();
            if (!q.IsEmpty)
            {
                q.First();
                while (!q.Eof)
                {
                    if (!LedgerUpdates.ItemblUpdate(year, month, q.FieldByName("ITCOD").AsString,
                            q.FieldByName("HOUSE").AsString, 0, q.FieldByName("TQTY").AsInteger, 0)) return false;
                    q.Next();
                }
            }
        }

        return true;
    }

    /// <summary>원본 fncSugmjob: 당월 수금을 미수파일에 반영.</summary>
    private static bool SugmJob(string year, string month)
    {
        using var q = new DbQuery();
        q.Add("SELECT CVCOD,SUM(ARAMT) IAMT FROM SUGMF");
        q.Add(" WHERE ARDAT LIKE @MONTH");
        q.Add(" GROUP BY CVCOD");
        q.ParamByName("MONTH").AsString = $"{year}-{month}%";
        q.Open();
        if (q.IsEmpty) return true;

        q.First();
        while (!q.Eof)
        {
            if (!LedgerUpdates.MisuUpdate(year, month, q.FieldByName("CVCOD").AsString, 2,
                    0, q.FieldByName("IAMT").AsInteger, 0)) return false;
            q.Next();
        }
        return true;
    }

    /// <summary>원본 fncDeletejob: 임시 작업파일(WORK02F) 정리.</summary>
    private static bool DeleteJob()
    {
        try
        {
            using var q = new DbQuery();
            q.Add("DELETE FROM WORK02F");
            q.ExecSQL();
            return true;
        }
        catch { return false; }
    }
}
