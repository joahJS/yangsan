using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS33.pas / SS33.dfm (TfrmSS33) 이식 — 계산서 관리(TAXF 테이블) 목록/검색/삭제.
/// 신규/수정 입력창(SS33B)과 자동발행(SS33A)은 이후 단계에서 연결 예정.
/// </summary>
public partial class SS33Form : Form
{
    public SS33Form()
    {
        InitializeComponent();
    }

    private void SS33Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 prcDBopen + qryListAfterOpen(qrySum/qrySum1).</summary>
    private void Search()
    {
        bool isTab1 = tabs.SelectedTab == tab1;

        using var q = new DbQuery();
        q.Add("SELECT A.*,CVNAM FROM TAXF A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD");
        q.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
        if (isTab1)
        {
            q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
            q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
            q.Add("ORDER BY TDATE,SEQNO");
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
            {
                q.Add(" AND A.CVCOD BETWEEN @FCVCOD AND @TCVCOD");
                q.ParamByName("FCVCOD").AsString = edtCvcd1.Text.Trim();
                q.ParamByName("TCVCOD").AsString = edtCvcd2.Text.Trim();
            }
            q.ParamByName("DATE1").AsString = dtpDate3.Value.ToString("yyyy-MM-dd");
            q.ParamByName("DATE2").AsString = dtpDate4.Value.ToString("yyyy-MM-dd");
            q.Add("ORDER BY A.CVCOD,TDATE,SEQNO");
        }
        q.Open();
        gridList.DataSource = q.Table;
        ApplyListHeaders();

        gridSum.Rows.Clear();
        if (isTab1)
        {
            using var qs = new DbQuery();
            qs.Add("SELECT TDATE,COUNT(*) CNT,SUM(TAMT) SAMT FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            qs.Add(" GROUP BY TDATE");
            qs.Add(" UNION");
            qs.Add("SELECT '합      계',COUNT(*),SUM(TAMT) FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            qs.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
            qs.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
            qs.Open();
            FillSum(qs, isDateKey: true);
        }
        else
        {
            using var qs = new DbQuery();
            qs.Add("SELECT CVCOD,COUNT(*) CNT,SUM(TAMT) SAMT FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
                qs.Add(" AND CVCOD BETWEEN @FCVCOD AND @TCVCOD");
            qs.Add(" GROUP BY CVCOD");
            qs.Add(" UNION");
            qs.Add("SELECT '합계',COUNT(*),SUM(TAMT) FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
                qs.Add(" AND CVCOD BETWEEN @FCVCOD AND @TCVCOD");
            qs.ParamByName("DATE1").AsString = dtpDate3.Value.ToString("yyyy-MM-dd");
            qs.ParamByName("DATE2").AsString = dtpDate4.Value.ToString("yyyy-MM-dd");
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
            {
                qs.ParamByName("FCVCOD").AsString = edtCvcd1.Text.Trim();
                qs.ParamByName("TCVCOD").AsString = edtCvcd2.Text.Trim();
            }
            qs.Open();
            FillSum(qs, isDateKey: false);
        }
    }

    private void FillSum(DbQuery qs, bool isDateKey)
    {
        if (qs.IsEmpty) return;
        qs.First();
        while (!qs.Eof)
        {
            var key = isDateKey ? qs.FieldByName("TDATE").AsString : qs.FieldByName("CVCOD").AsString;
            gridSum.Rows.Add(key, qs.FieldByName("CNT").AsString, PublicLib.MoneyToStr((long)qs.FieldByName("SAMT").AsFloat));
            qs.Next();
        }
    }

    private void ApplyListHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["TDATE"] = "발행일자", ["SEQNO"] = "순번", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
            ["TAMT"] = "공급가액", ["TVAT"] = "부가세", ["TGUBN"] = "구분", ["TBIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (gridList.Columns[field] != null) gridList.Columns[field]!.HeaderText = caption;

        // TAXF는 라인아이템 상세 컬럼(MMDD1~4/ITNBR1~4 등)이 매우 많아 요약 목록에서는
        // 위에서 매핑한 주요 컬럼만 보여주고 나머지는 숨긴다.
        foreach (DataGridViewColumn col in gridList.Columns)
            if (!map.ContainsKey(col.Name)) col.Visible = false;
    }

    private void OpenEntry(bool isNew)
    {
        using var dlg = new SS33BForm();
        if (!isNew)
        {
            if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;
            dlg.Text = "계산서 수정";
            dlg.LoadForEdit(row);
        }
        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }

    /// <summary>원본 prcSubDel.</summary>
    private void Delete()
    {
        if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;
        var tdate = row["TDATE"].ToString() ?? "";
        var seqno = Convert.ToInt32(row["SEQNO"]);

        if (!PublicLib.ConfirmDelete($"발행일자: {tdate}\r\n순번: {seqno}")) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM TAXF WHERE TDATE=@TDATE AND SEQNO=@SEQNO");
            q.ParamByName("TDATE").AsString = tdate;
            q.ParamByName("SEQNO").AsInteger = seqno;
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생..\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Search();
    }
}
