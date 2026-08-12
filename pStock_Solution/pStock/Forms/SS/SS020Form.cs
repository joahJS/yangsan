using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 ss020u00.pas (TSS020F00) 이식 — 출고 관리(SALE_M/SALE_D) 목록/상세/삭제.
/// 신규 등록(SS020F01)은 이후 단계에서 연결 예정.
/// </summary>
public partial class SS020Form : Form
{
    private static readonly string[] SearchFields =
        { "A1.SALNO", "A1.CVCOD", "CVNAM", "A1.PLNCD", "A1.LNCOD", "LNNAM", "B2.ADDR1", "MBIGO" };

    public SS020Form()
    {
        InitializeComponent();
    }

    private void OpenEditForSelected()
    {
        if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;
        var salno = row["SALNO"].ToString() ?? "";
        if (string.IsNullOrEmpty(salno)) return;

        using var dlg = new SS020F01Form();
        dlg.LoadForEdit(salno);
        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }

    private void BtnExcel_Click(object? sender, EventArgs e)
    {
        pStock.Common.ExcelExporter.Export(this.gridList, "출고관리");
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        pStock.Common.GridPrinter.Print(this.gridList, "출고관리");
    }

    private void GridList_SelectionChanged(object? sender, EventArgs e)
    {
        LoadDetail();
    }

    private void GridList_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        OpenEditForSelected();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        using var dlg = new SS020F01Form();
        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }

    private void BtnDel_Click(object? sender, EventArgs e)
    {
        DeleteMaster();
    }

    private void BtnCut_Click(object? sender, EventArgs e)
    {
        DeleteDetail();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        Search();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void SS020Form_Load(object? sender, EventArgs e)
    {
        this.eDate2.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.eDate1.Text = this.eDate2.Text;
        this.cSrcd.SelectedIndex = 2;
        Search();
    }

    private void SS020Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F4: btnDel.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 up_Srch1.</summary>
    private void Search()
    {
        var where = $" WHERE A1.TDATE BETWEEN '{eDate1.Text.Trim()}' AND '{eDate2.Text.Trim()}'";
        if (!string.IsNullOrWhiteSpace(eSrwd.Text))
            where += $" AND {SearchFields[cSrcd.SelectedIndex]} LIKE '%{eSrwd.Text.Trim()}%'";

        using var q = new DbQuery();
        q.Add("SELECT A1.*, B1.CVNAM, B1.OWNAM, B1.TELNO, B1.FAXNO,");
        q.Add("       B2.LNNAM, (B2.ADDR1+' '+B2.ADDR2) LNADR, LNTEL,");
        q.Add("      (SELECT COUNT(*) FROM SALE_D A2 WHERE A1.SALNO=A2.SALNO) AS OCNT,");
        q.Add("      (SELECT SUM(A2.TRAMT) FROM SALE_D A2 WHERE A1.SALNO=A2.SALNO) AS OAMT,");
        q.Add("      (SELECT SUM(A2.JAMT1+A2.JAMT2+A2.JAMT3) FROM SALE_D A2 WHERE A1.SALNO=A2.SALNO) AS JAMT");
        q.Add("  FROM SALE_M A1");
        q.Add("  LEFT OUTER JOIN CVMAST B1 ON A1.CVCOD=B1.CVCOD");
        q.Add("  LEFT OUTER JOIN REACH B2 ON A1.LNCOD=B2.LNCOD" + where);
        q.Add(" ORDER BY A1.TDATE, A1.SALNO");
        q.Open();

        gridList.DataSource = q.Table;
        ApplyListHeaders();
        gridDetail.DataSource = null;
    }

    private void ApplyListHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["SALNO"] = "출고번호", ["TDATE"] = "출고일자", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
            ["OWNAM"] = "대표자", ["TELNO"] = "전화", ["FAXNO"] = "팩스", ["OCNT"] = "건수", ["OAMT"] = "금액", ["JAMT"] = "부가세",
            ["SSAMT"] = "운송비", ["LNCOD"] = "착지처코드", ["LNNAM"] = "착지처명", ["LNADR"] = "주소", ["LNTEL"] = "착지처전화",
            ["PLNCD"] = "담당자", ["MBIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (gridList.Columns[field] != null) gridList.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in gridList.Columns)
            if (col.Name is "JGUBN" or "REFNO" or "MDATE") col.Visible = false;
    }

    /// <summary>원본 up_SrchDetl1.</summary>
    private void LoadDetail()
    {
        if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) { gridDetail.DataSource = null; return; }
        var salno = row["SALNO"].ToString() ?? "";

        using var q = new DbQuery();
        q.Add("SELECT A1.*, B1.*, C1.PNAME");
        q.Add("  FROM SALE_D A1");
        q.Add("  LEFT OUTER JOIN ITEMAS B1 ON A1.ITCOD=B1.ITNBR");
        q.Add("  LEFT OUTER JOIN PASSWD C1 ON A1.USRID=C1.USRID");
        q.Add(" WHERE A1.SALNO=@SALNO");
        q.Add(" ORDER BY SEQNO");
        q.ParamByName("SALNO").AsString = salno;
        q.Open();

        gridDetail.DataSource = q.Table;
        var map = new Dictionary<string, string>
        {
            ["SEQNO"] = "순번", ["ITCOD"] = "품번", ["ITDSC"] = "품명", ["ISPEC"] = "규격", ["DANWI"] = "단위",
            ["TRQTY"] = "수량", ["TRWGT"] = "중량", ["UCOST"] = "단가", ["TRAMT"] = "금액",
            ["JAMT1"] = "부가세1", ["JAMT2"] = "부가세2", ["JAMT3"] = "부가세3", ["HOUSE"] = "저장위치",
            ["DBIGO"] = "비고", ["PNAME"] = "담당자",
            ["ITWGT"] = "단위중량", ["ICOST"] = "입고단가", ["BCOST"] = "기준단가", ["OCOST"] = "출고단가", ["SAVLOC"] = "품목저장위치",
        };
        foreach (var (field, caption) in map)
            if (gridDetail.Columns[field] != null) gridDetail.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in gridDetail.Columns)
            if (col.Name is "SALNO" or "TRPRO" or "USRID" or "MDATE" or "MDATE1" or "YESNO" or "IBIGO")
                col.Visible = false;

        UpdateSubTotal();
    }

    private void UpdateSubTotal()
    {
        lblCnt.Text = gridList.SelectedRows.Count.ToString();
        double amt = 0;
        if (gridList.CurrentRow?.DataBoundItem is DataRowView row)
        {
            amt = ToDouble(row["OAMT"]) + ToDouble(row["JAMT"]);
        }
        lblAmt.Text = PublicLib.MoneyToStr((long)amt);
    }

    private static double ToDouble(object v) => v == null || v == DBNull.Value ? 0 : Convert.ToDouble(v);

    /// <summary>원본 up_Del1: 출고 전표 전체 삭제(재고 원복 포함).</summary>
    private void DeleteMaster()
    {
        if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;
        var salno = row["SALNO"].ToString() ?? "";

        if (!PublicLib.ConfirmDelete("출고번호: " + salno)) return;

        using var detailQ = new DbQuery();
        detailQ.Add("SELECT * FROM SALE_D WHERE SALNO=@SALNO");
        detailQ.ParamByName("SALNO").AsString = salno;
        detailQ.Open();

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            if (!detailQ.IsEmpty)
            {
                var tdate = row["TDATE"].ToString() ?? "";
                detailQ.First();
                while (!detailQ.Eof)
                {
                    var itcod = detailQ.FieldByName("ITCOD").AsString;
                    var house = detailQ.FieldByName("HOUSE").AsString;
                    var qty = detailQ.FieldByName("TRQTY").AsInteger;
                    if (!UpdateItembl(tdate[..4], tdate.Substring(5, 2), itcod, house, "O1QT", -qty))
                    {
                        AppDb.Rollback();
                        MessageBox.Show("년간재고파일 처리중 오류가 발생하였습니다.", "경고",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    detailQ.Next();
                }
            }

            q.Add("DELETE FROM SALE_D WHERE SALNO = @SALNO;");
            q.Add("DELETE FROM SALE_M WHERE SALNO = @SALNO;");
            q.ParamByName("SALNO").AsString = salno;
            q.ExecSQL();

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("출고자료 삭제중 오류가 발생하였습니다.\r\n" + ex.Message, "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Search();
    }

    /// <summary>원본 btnCutClick: 출고 상세 1건 삭제(재고/미수 원복 포함).</summary>
    private void DeleteDetail()
    {
        if (gridList.CurrentRow?.DataBoundItem is not DataRowView masterRow) return;
        if (gridDetail.CurrentRow?.DataBoundItem is not DataRowView detailRow) return;

        var salno = masterRow["SALNO"].ToString() ?? "";
        var tdate = masterRow["TDATE"].ToString() ?? "";
        var cvcod = masterRow["CVCOD"].ToString() ?? "";
        var seqno = detailRow["SEQNO"].ToString() ?? "";
        var itcod = detailRow["ITCOD"].ToString() ?? "";
        var house = detailRow["HOUSE"].ToString() ?? "";
        var qty = Convert.ToInt32(detailRow["TRQTY"]);
        var amt = ToDouble(detailRow["TRAMT"]) + ToDouble(detailRow["JAMT1"]) + ToDouble(detailRow["JAMT2"]) + ToDouble(detailRow["JAMT3"]);

        if (!PublicLib.ConfirmDelete($"출고번호: {salno}\r\n순번: {seqno}")) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            if (!UpdateMisu(tdate[..4], tdate.Substring(5, 2), cvcod, (int)Math.Truncate(amt * -1)))
            {
                AppDb.Rollback();
                MessageBox.Show("년간미수잔액 처리중 오류가 발생하였습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!UpdateItembl(tdate[..4], tdate.Substring(5, 2), itcod, house, "O1QT", -qty))
            {
                AppDb.Rollback();
                MessageBox.Show("년간재고파일 처리중 오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            q.Add("DELETE FROM SALE_D WHERE SALNO=@SALNO AND SEQNO=@SEQNO");
            q.ParamByName("SALNO").AsString = salno;
            q.ParamByName("SEQNO").AsInteger = Convert.ToInt32(seqno);
            q.ExecSQL();

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("출고상세자료 삭제중 오류가 발생하였습니다.\r\n" + ex.Message, "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        LoadDetail();
    }

    private static bool UpdateItembl(string year, string month, string itnbr, string house, string colPrefix, int deltaQty)
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
            upd.Add($"UPDATE ITEMBL SET {colPrefix}{month} = {colPrefix}{month} + @DELTA, MDATE=GETDATE()");
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
