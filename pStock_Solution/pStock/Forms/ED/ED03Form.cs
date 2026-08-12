using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.ED;

/// <summary>
/// 원본 ED03.pas / ED03.dfm (TfrmED03) 이식 — 기초잔액 보수(MISUF.BAMT 이월잔액 등록/수정/삭제).
/// 거래처 검색 팝업(BA00C)은 아직 변환되지 않아 거래처코드 직접입력만 지원한다.
/// </summary>
public partial class ED03Form : Form
{
    public ED03Form()
    {
        InitializeComponent();
    }

    private void EdtYear_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) Search();
    }

    private void BtnYearDown_Click(object? sender, EventArgs e)
    {
        edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) - 1).ToString();
        Search();
    }

    private void BtnYearUp_Click(object? sender, EventArgs e)
    {
        edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) + 1).ToString();
        Search();
    }

    private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        SyncEditFromGrid();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        ClearEdit();
        edtCode.Focus();
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        Save();
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

    private void ED03Form_Load(object? sender, EventArgs e)
    {
        this.edtYear.Text = DateTime.Now.Year.ToString();
        Search();
    }

    private void ED03Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F2: btnAdd.PerformClick(); break;
            case Keys.F4: btnDel.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 prcDBopen.</summary>
    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT CVNAM,OWNAM,TELNO,FAXNO,A.* FROM MISUF A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD");
        q.Add(" WHERE MYEAR=@MYEAR AND BAMT<>0");
        q.Add("ORDER BY CVCOD");
        q.ParamByName("MYEAR").AsString = edtYear.Text.Trim();
        q.Open();

        grid.DataSource = q.Table;
        var map = new Dictionary<string, string>
        {
            ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명", ["OWNAM"] = "대표자",
            ["TELNO"] = "전화번호", ["FAXNO"] = "팩스번호", ["BAMT"] = "기초잔액",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;
    }

    private void SyncEditFromGrid()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) { ClearEdit(); return; }
        edtCode.Text = row["CVCOD"].ToString();
        edtCvnam.Text = row["CVNAM"].ToString();
        edtOwnam.Text = row["OWNAM"].ToString();
        edtBamt.Value = Convert.ToDecimal(row["BAMT"]);
    }

    private void ClearEdit()
    {
        edtCode.Clear(); edtCvnam.Clear(); edtOwnam.Clear(); edtBamt.Value = 0;
    }

    /// <summary>원본 edtCodeKeyDown.</summary>
    private void EdtCode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCode.Text))
        {
            using var dlg = new Common.CvcodLookupForm(cvguFilter: "2");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCode.Text = dlg.SelectedCode;
                edtCvnam.Text = dlg.SelectedName;
            }
            else
            {
                edtCvnam.Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD AND CVGU='2'");
        q.ParamByName("CVCOD").AsString = edtCode.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edtCode.Text = q.FieldByName("CVCOD").AsString;
            edtCvnam.Text = q.FieldByName("CVNAM").AsString;
        }
        else
        {
            edtCvnam.Clear();
        }
    }

    /// <summary>원본 fncErrCheck.</summary>
    private bool ErrCheck()
    {
        using var q = new DbQuery();
        q.Add($"SELECT * FROM CVMAST WHERE CVCOD='{edtCode.Text.Trim()}'");
        q.Open();
        if (q.IsEmpty)
        {
            MessageBox.Show($"코드: {edtCode.Text.Trim()}는 존재하지 않는 거래처코드입니다.\r\n" +
                "거래처코드를 확인하여 주십시오.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtCode.Focus();
            return false;
        }
        return true;
    }

    /// <summary>원본 fncAddJob: MISUF에 없으면 신규 생성 후 BAMT 갱신.</summary>
    private void Save()
    {
        if (!ErrCheck()) return;

        var year = edtYear.Text.Trim();
        var cvcod = edtCode.Text.Trim();

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            using (var check = new DbQuery())
            {
                check.Add("SELECT * FROM MISUF WHERE MYEAR=@YEAR AND CVCOD=@CVCOD");
                check.ParamByName("YEAR").AsString = year;
                check.ParamByName("CVCOD").AsString = cvcod;
                check.Open();

                if (check.IsEmpty)
                {
                    using var ins = new DbQuery();
                    ins.Add("INSERT INTO MISUF (OAMT01,OAMT02,OAMT03,OAMT04,OAMT05,OAMT06,OAMT07,OAMT08,OAMT09,OAMT10,OAMT11,OAMT12,");
                    ins.Add("       OVAT01,OVAT02,OVAT03,OVAT04,OVAT05,OVAT06,OVAT07,OVAT08,OVAT09,OVAT10,OVAT11,OVAT12,");
                    ins.Add("       SAMT01,SAMT02,SAMT03,SAMT04,SAMT05,SAMT06,SAMT07,SAMT08,SAMT09,SAMT10,SAMT11,SAMT12,");
                    ins.Add("       MYEAR,CVCOD,BAMT,MDATE)");
                    ins.Add("      VALUES(0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,");
                    ins.Add("             0,0,0,0,0,0,0,0,0,0,0,0, @YEAR,@CVCOD,0,@MDATE)");
                    ins.ParamByName("YEAR").AsString = year;
                    ins.ParamByName("CVCOD").AsString = cvcod;
                    ins.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                    ins.ExecSQL();
                }
            }

            q.Add("UPDATE MISUF SET BAMT=@BAMT,MDATE=@MDATE");
            q.Add(" WHERE MYEAR=@MYEAR AND CVCOD=@CVCOD");
            q.ParamByName("MYEAR").AsString = year;
            q.ParamByName("CVCOD").AsString = cvcod;
            q.ParamByName("BAMT").AsInteger = (int)edtBamt.Value;
            q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
            q.ExecSQL();

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("미수 이월금액 저장시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Search();
    }

    /// <summary>원본 btnDelClick: 해당년도 매출/수금 내역이 있으면 삭제 불가.</summary>
    private void Delete()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) return;
        var cvcod = row["CVCOD"].ToString() ?? "";
        var year = edtYear.Text.Trim();

        using (var check = new DbQuery())
        {
            check.Add("SELECT OAMT01+OAMT02+OAMT03+OAMT04+OAMT05+OAMT06+OAMT07+OAMT09+OAMT10+");
            check.Add("       OAMT11+OAMT12+SAMT01+SAMT02+SAMT03+SAMT04+SAMT05+SAMT06+SAMT07+");
            check.Add("       SAMT08+SAMT09+SAMT10+SAMT11+SAMT12 AS TOTAL FROM MISUF");
            check.Add(" WHERE MYEAR=@MYEAR AND CVCOD=@CVCOD");
            check.ParamByName("MYEAR").AsString = year;
            check.ParamByName("CVCOD").AsString = cvcod;
            check.Open();
            if (!check.IsEmpty && check.FieldByName("TOTAL").AsInteger > 0)
            {
                MessageBox.Show("해당년도에 매출/수금내역이 있으므로 삭제하실 수 없습니다.", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        if (!PublicLib.ConfirmDelete("거래처코드: " + cvcod)) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM MISUF WHERE MYEAR=@MYEAR AND CVCOD=@CVCOD");
            q.ParamByName("MYEAR").AsString = year;
            q.ParamByName("CVCOD").AsString = cvcod;
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("거래처 미수잔액 삭제시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Search();
    }
}
