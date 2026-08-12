using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS23.pas / SS23.dfm (TfrmSS23) 이식 — 보관료 관리(IPCHF, GUBN1='1') 목록/검색/삭제.
/// 신규/수정 입력창(SS23A)과 자동계산(SS23B)은 이후 단계에서 연결 예정.
/// </summary>
public partial class SS23Form : Form
{
    public SS23Form()
    {
        InitializeComponent();
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

    private void BtnExcel_Click(object? sender, EventArgs e)
    {
        pStock.Common.ExcelExporter.Export(this.grid, "보관료관리");
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        pStock.Common.GridPrinter.Print(this.grid, "보관료관리");
    }

    private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        OpenEntry(isNew: false);
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        OpenEntry(isNew: true);
    }

    private void BtnDel_Click(object? sender, EventArgs e)
    {
        Delete();
    }

    private void BtnCompute_Click(object? sender, EventArgs e)
    {
        using var dlg = new SS23BForm();
        dlg.ShowDialog(this);
        if (dlg.Executed) Search();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        Search();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void SS23Form_Load(object? sender, EventArgs e)
    {
        this.dtpDate1.Value = DateTime.Now; this.dtpDate2.Value = DateTime.Now; Search();
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
