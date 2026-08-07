using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS23.pas / SS23.dfm (TfrmSS23) 이식 — 보관료 관리(IPCHF, GUBN1='1') 목록/검색/삭제.
/// 신규/수정 입력창(SS23A)과 자동계산(SS23B)은 이후 단계에서 연결 예정.
/// </summary>
public class SS23Form : Form
{
    private readonly FastDataGridView grid = new();
    private readonly DateTimePicker dtpDate1 = new();
    private readonly DateTimePicker dtpDate2 = new();
    private readonly Label lblAmt0 = new() { AutoSize = true };
    private readonly Label lblAmt1 = new() { AutoSize = true };

    private readonly Button btnNew = new() { Text = "신규(F1)" };
    private readonly Button btnDel = new() { Text = "삭제(F4)" };
    private readonly Button btnCompute = new() { Text = "자동계산" };
    private readonly Button btnSearch = new() { Text = "조회" };
    private readonly Button btnExcel = new() { Text = "엑셀저장" };
    private readonly Button btnPrint = new() { Text = "인쇄" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    public SS23Form()
    {
        Text = "보관료 관리";
        Width = 1100;
        Height = 650;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { dtpDate1.Value = DateTime.Now; dtpDate2.Value = DateTime.Now; Search(); };
        KeyDown += SS23Form_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnDel, btnCompute, btnSearch, btnExcel, btnPrint, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnDel, btnCompute, btnSearch, btnExcel, btnPrint, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }
        btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(grid, "보관료관리");
        btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(grid, "보관료관리");

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lblDate = new Label { Text = "기간", Left = 5, Top = 12, AutoSize = true };
        dtpDate1.Left = 50; dtpDate1.Top = 8; dtpDate1.Width = 110; dtpDate1.Format = DateTimePickerFormat.Short;
        var lblTilde = new Label { Text = "~", Left = 165, Top = 12, AutoSize = true };
        dtpDate2.Left = 180; dtpDate2.Top = 8; dtpDate2.Width = 110; dtpDate2.Format = DateTimePickerFormat.Short;
        var lblAmtCap0 = new Label { Text = "보관금액:", Left = 320, Top = 12, AutoSize = true };
        lblAmt0.Left = 390; lblAmt0.Top = 12;
        var lblAmtCap1 = new Label { Text = "부가세:", Left = 500, Top = 12, AutoSize = true };
        lblAmt1.Left = 560; lblAmt1.Top = 12;

        editPanel.Controls.AddRange(new Control[] { lblDate, dtpDate1, lblTilde, dtpDate2, lblAmtCap0, lblAmt0, lblAmtCap1, lblAmt1 });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.CellDoubleClick += (_, _) => OpenEntry(isNew: false);

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => OpenEntry(isNew: true);
        btnDel.Click += (_, _) => Delete();
        btnCompute.Click += (_, _) =>
        {
            using var dlg = new SS23BForm();
            dlg.ShowDialog(this);
            if (dlg.Executed) Search();
        };
        btnSearch.Click += (_, _) => Search();
        btnClose.Click += (_, _) => Close();
    }

    private void SS23Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F4: btnDel.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 btnSearchClick.</summary>
    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT A.*,(JAMT1+JAMT2+JAMT3) JAMT,B.CVNAM,C.ITDSC,C.ISPEC FROM IPCHF A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD");
        q.Add("  LEFT OUTER JOIN ITEMAS C ON A.ITNBR=C.ITNBR");
        q.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2 AND GUBN1 IN ('1')");
        q.Add(" ORDER BY TDATE,SEQNO");
        q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        q.Open();

        grid.DataSource = q.Table;
        ApplyGridHeaders();
        UpdateSummary();
    }

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["TDATE"] = "산정일자", ["SEQNO"] = "순번", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
            ["ITNBR"] = "품번", ["ITDSC"] = "품명", ["ISPEC"] = "규격", ["DANWI"] = "단위",
            ["IOQTY"] = "보관수량", ["ODAN"] = "단가", ["OAMT"] = "보관금액", ["JAMT"] = "부가세",
            ["HOUSE"] = "저장위치", ["TBIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in grid.Columns)
            if (col.Name is "GUBN1" or "JAMT1" or "JAMT2" or "JAMT3" or "TPRO" or "KEYNO" or "MDATE")
                col.Visible = false;
    }

    /// <summary>원본 prcDbSum.</summary>
    private void UpdateSummary()
    {
        using var q = new DbQuery();
        q.Add("SELECT SUM(OAMT) OAMT,SUM(JAMT1+JAMT2+JAMT3) JAMT FROM IPCHF");
        q.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2 AND GUBN1='1'");
        q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        q.Open();

        if (q.IsEmpty) { lblAmt0.Text = "0"; lblAmt1.Text = "0"; }
        else
        {
            lblAmt0.Text = PublicLib.MoneyToStr((long)q.FieldByName("OAMT").AsFloat);
            lblAmt1.Text = PublicLib.MoneyToStr((long)q.FieldByName("JAMT").AsFloat);
        }
    }

    private void OpenEntry(bool isNew)
    {
        using var dlg = new SS23AForm();
        if (!isNew)
        {
            if (grid.CurrentRow?.DataBoundItem is not DataRowView row) return;
            dlg.Text = "보관료 수정";
            dlg.LoadForEdit(row);
        }
        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }

    /// <summary>원본 prcDBdel: 입고에 의한 자동전표(TPRO&lt;&gt;0)는 삭제 불가.</summary>
    private void Delete()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) return;

        var keyno = row.Row.Table.Columns.Contains("KEYNO") ? row["KEYNO"].ToString() ?? "" : "";
        var tpro = row.Row.Table.Columns.Contains("TPRO") ? Convert.ToInt32(row["TPRO"]) : 0;
        if (!string.IsNullOrWhiteSpace(keyno) && tpro != 0)
        {
            MessageBox.Show("입고에 의한 자동전표이므로 삭제하실 수 없습니다.", "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var tdate = row["TDATE"].ToString() ?? "";
        var seqno = Convert.ToInt32(row["SEQNO"]);
        var itnbr = row["ITNBR"].ToString() ?? "";
        var oamt = Convert.ToInt32(row["OAMT"]);
        var jamt1 = row.Row.Table.Columns.Contains("JAMT1") ? Convert.ToInt32(row["JAMT1"]) : 0;
        var jamt2 = row.Row.Table.Columns.Contains("JAMT2") ? Convert.ToInt32(row["JAMT2"]) : 0;
        var jamt3 = row.Row.Table.Columns.Contains("JAMT3") ? Convert.ToInt32(row["JAMT3"]) : 0;

        if (MessageBox.Show("삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM IPCHF WHERE TDATE=@TDATE AND SEQNO=@SEQNO");
            q.ParamByName("TDATE").AsString = tdate;
            q.ParamByName("SEQNO").AsInteger = seqno;
            q.ExecSQL();

            if (!UpdateMisu(tdate[..4], tdate.Substring(5, 2), itnbr.Length >= 4 ? itnbr[..4] : itnbr,
                    -(oamt + jamt1 + jamt2 + jamt3)))
            {
                AppDb.Rollback();
                MessageBox.Show("미수파일 쓰는중 에러발생(+)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Search();
    }

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
            upd.Add($"UPDATE MISUF SET OAMT{month} = OAMT{month} + @DELTA, MDATE=GETDATE()");
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
