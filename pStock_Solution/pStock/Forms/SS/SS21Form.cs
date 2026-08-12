using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS21.pas / SS21.dfm (TfrmSS21) 이식 — 입고 관리(IPGOF 테이블) 목록/검색/삭제.
///
/// 참고: 원본은 신규/수정 시 별도 입력 다이얼로그(SS21A)를 띄우며, 그 안에서
/// 재고파일(ITEMBL)·미수금파일(MISUF)·보관 자동전표(IPCHF)까지 함께 갱신하는
/// 복잡한 트랜잭션 로직을 수행한다. 그 입력 다이얼로그는 별도 변환 단계에서
/// 다룰 예정이라, 이 화면에서는 목록 조회/검색/삭제(→ 재고·미수 파일 원복 포함)까지만
/// 지원하고 신규/수정 버튼은 안내 메시지로 대체해 두었다. 삭제 로직(prcDBdel)은
/// 재고/미수 파일 갱신까지 원본 그대로 이식했다(간략화된 UPDATE 방식 사용).
/// </summary>
public partial class SS21Form : Form
{
    public SS21Form()
    {
        InitializeComponent();
    }

    private void BtnExcel_Click(object? sender, EventArgs e)
    {
        pStock.Common.ExcelExporter.Export(this.grid, "입고관리");
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        pStock.Common.GridPrinter.Print(this.grid, "입고관리");
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

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        Search();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void SS21Form_Load(object? sender, EventArgs e)
    {
        this.dtpDate1.Value = DateTime.Now; this.dtpDate2.Value = DateTime.Now; Search();
    }

    private void SS21Form_KeyDown(object? sender, KeyEventArgs e)
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
        q.Add("SELECT A.*,(JAMT1+JAMT2+JAMT3) JAMT,B.CVNAM,C.ITDSC,C.ISPEC,C.BCOST,C.OCOST");
        q.Add("  FROM IPGOF A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD");
        q.Add("  LEFT OUTER JOIN ITEMAS C ON A.ITNBR=C.ITNBR");
        q.Add(" WHERE IDATE BETWEEN @DATE1 AND @DATE2");
        q.Add("   AND A.CVCOD LIKE @CVCOD");
        q.Add(" ORDER BY IDATE,ISEQ");
        q.ParamByName("CVCOD").AsString = edtCvcd1.Text.Trim() + "%";
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
            ["IDATE"] = "입고일자", ["ISEQ"] = "순번", ["IGUBN"] = "구분", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
            ["ITNBR"] = "품번", ["ITDSC"] = "품명", ["ISPEC"] = "규격", ["DANWI"] = "단위",
            ["IQTY"] = "입고수량", ["OQTY"] = "출고중수량", ["IDAN"] = "단가", ["IAMT"] = "금액",
            ["JAMT"] = "부가세", ["BCOST"] = "기준단가", ["OCOST"] = "출고단가", ["HOUSE"] = "저장위치",
            ["YDATE"] = "만기일", ["IBIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in grid.Columns)
            if (col.Name is "TPRO" or "MDATE" or "JAMT1" or "JAMT2" or "JAMT3") col.Visible = false;
    }

    /// <summary>원본 prcDbSum.</summary>
    private void UpdateSummary()
    {
        using var q = new DbQuery();
        q.Add("SELECT SUM(IQTY) IQTY,SUM(IAMT) IAMT,SUM(JAMT1+JAMT2+JAMT3) JAMT FROM IPGOF");
        q.Add(" WHERE IDATE BETWEEN @DATE1 AND @DATE2");
        q.Add("   AND CVCOD LIKE @CVCOD");
        q.ParamByName("CVCOD").AsString = edtCvcd1.Text.Trim() + "%";
        q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
        q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
        q.Open();

        if (q.IsEmpty)
        {
            lblAmt2.Text = "0"; lblAmt3.Text = "0";
        }
        else
        {
            lblAmt2.Text = PublicLib.MoneyToStr((long)q.FieldByName("IAMT").AsFloat);
            lblAmt3.Text = PublicLib.MoneyToStr((long)q.FieldByName("JAMT").AsFloat);
        }
    }

    private void OpenEntry(bool isNew)
    {
        using var dlg = new SS21AForm();

        if (isNew)
        {
            dlg.Job = "I";
        }
        else
        {
            if (grid.CurrentRow?.DataBoundItem is not DataRowView row) return;

            dlg.Job = "U";
            SetPrivateFieldsFromRow(dlg, row);
        }

        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }

    /// <summary>원본 DBGrid1DblClick: 선택 행 값을 입력창에 채워 넣는다.</summary>
    private void SetPrivateFieldsFromRow(SS21AForm dlg, DataRowView row)
    {
        dlg.Text = "입고 수정";
        dlg.OriginalHouse = row["HOUSE"].ToString() ?? "";
        dlg.OriginalQty = Convert.ToInt32(row["IQTY"]);
        dlg.OriginalAmt = Convert.ToInt32(row["IAMT"]) +
            (row.Row.Table.Columns.Contains("JAMT1") ? Convert.ToInt32(row["JAMT1"]) : 0) +
            (row.Row.Table.Columns.Contains("JAMT2") ? Convert.ToInt32(row["JAMT2"]) : 0) +
            (row.Row.Table.Columns.Contains("JAMT3") ? Convert.ToInt32(row["JAMT3"]) : 0);
        dlg.LoadForEdit(row);
    }

    /// <summary>원본 prcDBdel: 입고 삭제 + 재고파일/미수파일 원복 + 보관 자동전표 삭제.</summary>
    private void Delete()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) return;

        var idate = row["IDATE"].ToString() ?? "";
        var iseq = Convert.ToInt32(row["ISEQ"]);
        var itnbr = row["ITNBR"].ToString() ?? "";
        var house = row["HOUSE"].ToString() ?? "";
        var iqty = Convert.ToInt32(row["IQTY"]);
        var iamt = Convert.ToInt32(row["IAMT"]);
        var jamt1 = row.Row.Table.Columns.Contains("JAMT1") ? Convert.ToInt32(row["JAMT1"]) : 0;
        var jamt2 = row.Row.Table.Columns.Contains("JAMT2") ? Convert.ToInt32(row["JAMT2"]) : 0;
        var jamt3 = row.Row.Table.Columns.Contains("JAMT3") ? Convert.ToInt32(row["JAMT3"]) : 0;
        var bcost = row.Row.Table.Columns.Contains("BCOST") ? Convert.ToInt32(row["BCOST"]) : 0;

        if (!PublicLib.ConfirmDelete($"입고일자: {idate}\r\n순번: {iseq}")) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            q.Add("DELETE FROM IPGOF WHERE IDATE=@IDATE AND ISEQ=@ISEQ");
            q.ParamByName("IDATE").AsString = idate;
            q.ParamByName("ISEQ").AsInteger = iseq;
            q.ExecSQL();

            if (!UpdateItembl(idate[..4], idate.Substring(5, 2), itnbr, house, -iqty))
            {
                AppDb.Rollback();
                MessageBox.Show("재고파일에 쓰는중 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!UpdateMisu(idate[..4], idate.Substring(5, 2), itnbr.Length >= 4 ? itnbr[..4] : itnbr,
                    -(iamt + jamt1 + jamt2 + jamt3)))
            {
                AppDb.Rollback();
                MessageBox.Show("미수파일 쓰는중 에러발생(+)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var q2 = new DbQuery())
            {
                q2.Add("DELETE FROM IPCHF WHERE TDATE=@TDATE AND KEYNO=@KEYNO AND GUBN1='1'");
                q2.ParamByName("TDATE").AsString = idate;
                q2.ParamByName("KEYNO").AsString = idate + iseq.ToString("000");
                q2.ExecSQL();
            }

            if (!UpdateMisu(idate[..4], idate.Substring(5, 2), itnbr.Length >= 4 ? itnbr[..4] : itnbr,
                    (int)Math.Truncate(iqty * (double)bcost * -1)))
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
            MessageBox.Show("자료삭제시 에러발생.\r\n" + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Search();
    }

    /// <summary>원본 fncItemblUpdate(간략화: 해당 월 입고수량 I1QTnn 컬럼에 증감분 반영).</summary>
    private static bool UpdateItembl(string year, string month, string itnbr, string house, int deltaQty)
    {
        try
        {
            using var check = new DbQuery();
            check.Add("SELECT * FROM ITEMBL WHERE IYEAR=@YEAR AND ITNBR=@ITNBR AND HOUSE=@HOUSE");
            check.ParamByName("YEAR").AsString = year;
            check.ParamByName("ITNBR").AsString = itnbr;
            check.ParamByName("HOUSE").AsString = house;
            check.Open();
            if (check.IsEmpty) return true;

            using var upd = new DbQuery();
            upd.Add($"UPDATE ITEMBL SET I1QT{month} = I1QT{month} + @DELTA, MDATE=GETDATE()");
            upd.Add(" WHERE IYEAR=@YEAR AND ITNBR=@ITNBR AND HOUSE=@HOUSE");
            upd.ParamByName("DELTA").AsInteger = deltaQty;
            upd.ParamByName("YEAR").AsString = year;
            upd.ParamByName("ITNBR").AsString = itnbr;
            upd.ParamByName("HOUSE").AsString = house;
            upd.ExecSQL();
            return true;
        }
        catch { return false; }
    }

    /// <summary>원본 fncMisuUpdate(간략화: 해당 월 OAMTnn 컬럼에 증감분 반영).</summary>
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
