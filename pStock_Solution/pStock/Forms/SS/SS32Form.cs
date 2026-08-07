using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS32.pas / SS32.dfm (TfrmSS32) 이식 — 수금 관리(SUGMF 테이블) 목록/일자별집계/삭제.
/// 신규/수정 입력창(SS32A)은 이후 단계에서 연결 예정.
/// </summary>
public class SS32Form : Form
{
    private readonly FastDataGridView gridList = new();
    private readonly FastDataGridView gridSum = new();
    private readonly DateTimePicker dtpDate1 = new();
    private readonly DateTimePicker dtpDate2 = new();
    private readonly Label lblAmt1 = new() { AutoSize = true };

    private readonly Button btnNew = new() { Text = "신규(F1)" };
    private readonly Button btnDel = new() { Text = "삭제(F4)" };
    private readonly Button btnSearch = new() { Text = "조회" };
    private readonly Button btnExcel = new() { Text = "엑셀저장" };
    private readonly Button btnPrint = new() { Text = "인쇄" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    public SS32Form()
    {
        Text = "수금 관리";
        Width = 1100;
        Height = 650;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { dtpDate1.Value = DateTime.Now; dtpDate2.Value = DateTime.Now; Search(); };
        KeyDown += SS32Form_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnDel, btnSearch, btnExcel, btnPrint, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnDel, btnSearch, btnExcel, btnPrint, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }
        btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(gridList, "수금관리");
        btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(gridList, "수금관리");

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lblDate = new Label { Text = "기간", Left = 5, Top = 12, AutoSize = true };
        dtpDate1.Left = 50; dtpDate1.Top = 8; dtpDate1.Width = 110; dtpDate1.Format = DateTimePickerFormat.Short;
        var lblTilde = new Label { Text = "~", Left = 165, Top = 12, AutoSize = true };
        dtpDate2.Left = 180; dtpDate2.Top = 8; dtpDate2.Width = 110; dtpDate2.Format = DateTimePickerFormat.Short;
        var lblAmtCap = new Label { Text = "수금합계:", Left = 320, Top = 12, AutoSize = true };
        lblAmt1.Left = 390; lblAmt1.Top = 12;
        editPanel.Controls.AddRange(new Control[] { lblDate, dtpDate1, lblTilde, dtpDate2, lblAmtCap, lblAmt1 });

        var split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 750 };
        gridList.Dock = DockStyle.Fill;
        gridList.ReadOnly = true;
        gridList.AllowUserToAddRows = false;
        gridList.CellDoubleClick += (_, _) => OpenEntry(isNew: false);

        gridSum.Dock = DockStyle.Fill;
        gridSum.ReadOnly = true;
        gridSum.AllowUserToAddRows = false;
        gridSum.Columns.Add("ARDAT", "일자");
        gridSum.Columns.Add("CNT", "건수");
        gridSum.Columns.Add("SAMT", "금액");

        split.Panel1.Controls.Add(gridList);
        split.Panel2.Controls.Add(gridSum);

        Controls.Add(split);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => OpenEntry(isNew: true);
        btnDel.Click += (_, _) => Delete();
        btnSearch.Click += (_, _) => Search();
        btnClose.Click += (_, _) => Close();
    }

    private void SS32Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F4: btnDel.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 btnSearchClick + qryListAfterOpen(qrySum) + prcDbSum.</summary>
    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT A.*,B.CVNAM FROM SUGMF A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD");
        q.Add(" WHERE ARDAT BETWEEN @DATE1 AND @DATE2");
        q.Add(" ORDER BY ARDAT,ARSEQ");
        q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        q.Open();
        gridList.DataSource = q.Table;
        ApplyListHeaders();

        using var qs = new DbQuery();
        qs.Add("SELECT ARDAT,COUNT(*) CNT,SUM(ARAMT) SAMT FROM SUGMF");
        qs.Add(" WHERE ARDAT BETWEEN @DATE1 AND @DATE2");
        qs.Add(" GROUP BY ARDAT");
        qs.Add(" UNION");
        qs.Add("SELECT '합      계',COUNT(*),SUM(ARAMT) FROM SUGMF");
        qs.Add(" WHERE ARDAT BETWEEN @DATE1 AND @DATE2");
        qs.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        qs.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        qs.Open();
        gridSum.Rows.Clear();
        if (!qs.IsEmpty)
        {
            qs.First();
            while (!qs.Eof)
            {
                gridSum.Rows.Add(qs.FieldByName("ARDAT").AsString,
                    qs.FieldByName("CNT").AsString, PublicLib.MoneyToStr((long)qs.FieldByName("SAMT").AsFloat));
                qs.Next();
            }
        }

        using var q2 = new DbQuery();
        q2.Add("SELECT SUM(ARAMT) SAMT FROM SUGMF");
        q2.Add(" WHERE ARDAT BETWEEN @DATE1 AND @DATE2");
        q2.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q2.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        q2.Open();
        lblAmt1.Text = q2.IsEmpty ? "0" : PublicLib.MoneyToStr((long)q2.FieldByName("SAMT").AsFloat);
    }

    private void ApplyListHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["ARDAT"] = "수금일자", ["ARSEQ"] = "순번", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
            ["ARGU"] = "구분", ["ARAMT"] = "수금액", ["ABIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (gridList.Columns[field] != null) gridList.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in gridList.Columns)
            if (col.Name == "MDATE") col.Visible = false;
    }

    private void OpenEntry(bool isNew)
    {
        using var dlg = new SS32AForm();
        if (!isNew)
        {
            if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;
            dlg.Text = "수금 수정";
            dlg.LoadForEdit(row);
        }
        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }

    /// <summary>원본 prcDBdel.</summary>
    private void Delete()
    {
        if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;

        var ardat = row["ARDAT"].ToString() ?? "";
        var arseq = Convert.ToInt32(row["ARSEQ"]);
        var cvcod = row["CVCOD"].ToString() ?? "";
        var aramt = Convert.ToInt32(row["ARAMT"]);

        if (!PublicLib.ConfirmDelete($"수금일자: {ardat}\r\n순번: {arseq}")) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM SUGMF WHERE ARDAT=@ARDAT AND ARSEQ=@ARSEQ");
            q.ParamByName("ARDAT").AsString = ardat;
            q.ParamByName("ARSEQ").AsInteger = arseq;
            q.ExecSQL();

            if (!UpdateMisu(ardat[..4], ardat.Substring(5, 2), cvcod, -aramt))
            {
                AppDb.Rollback();
                MessageBox.Show("미수금 수금정리작업중 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생.\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Search();
    }

    /// <summary>원본 fncMisuUpdate(2: 수금액 SAMTnn 컬럼).</summary>
    private static bool UpdateMisu(string year, string month, string cvcod, int deltaAmt)
    {
        try
        {
            using var check = new DbQuery();
            check.Add("SELECT * FROM MISUF WHERE MYEAR=@YEAR AND CVCOD=@CVCOD");
            check.ParamByName("YEAR").AsString = year;
            check.ParamByName("CVCOD").AsString = cvcod;
            check.Open();
            if (check.IsEmpty) return true;

            using var upd = new DbQuery();
            upd.Add($"UPDATE MISUF SET SAMT{month} = SAMT{month} + @DELTA, MDATE=GETDATE()");
            upd.Add(" WHERE MYEAR=@YEAR AND CVCOD=@CVCOD");
            upd.ParamByName("DELTA").AsInteger = deltaAmt;
            upd.ParamByName("YEAR").AsString = year;
            upd.ParamByName("CVCOD").AsString = cvcod;
            upd.ExecSQL();
            return true;
        }
        catch { return false; }
    }
}
