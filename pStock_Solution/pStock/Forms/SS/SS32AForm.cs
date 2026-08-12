using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS32A.pas / SS32A.dfm (TfrmSS32A) 이식 — 수금 등록/수정(SUGMF).
/// </summary>
public partial class SS32AForm : Form
{
    public string Job { get; set; } = "I";
    public int OriginalAmt { get; set; }
    public bool Saved { get; private set; }

    public SS32AForm()
    {
        InitializeComponent();
    }

    private void SS32AForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: ClearForm(); edtCode.Focus(); break;
            case Keys.F2: if (Job == "I") btnAdd.PerformClick(); break;
            case Keys.F3: btnOne.PerformClick(); break;
            case Keys.Escape: Close(); break;
        }
    }

    private void ResetGuList()
    {
        cboGu.Items.Clear();
        cboGu.Items.Add(" ");
        foreach (var (_, retxf) in ReffpfCache.Get("SS")) cboGu.Items.Add(retxf);
        cboGu.SelectedIndex = cboGu.Items.Count > 1 ? 1 : 0;
    }

    public void ClearForm()
    {
        edtCode.Clear(); dspName.Clear();
        edtNo.Clear();
        if (cboGu.Items.Count > 1) cboGu.SelectedIndex = 1;
        edtAmt.Value = 0; edtBigo.Clear();
        OriginalAmt = 0;
        Job = "I";
        lblJob.Text = "수금등록중";
        lblMisu.Text = "0";
    }

    /// <summary>원본 edtCodeKeyDown.</summary>
    private void EdtCode_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCode.Text))
        {
            using var dlg = new CvcodLookupForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCode.Text = dlg.SelectedCode;
                dspName.Text = dlg.SelectedName;
            }
            else
            {
                dspName.Clear();
            }
        }
        else
        {
            using var q = new DbQuery();
            q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD");
            q.ParamByName("CVCOD").AsString = edtCode.Text.Trim();
            q.Open();
            if (!q.IsEmpty) dspName.Text = q.FieldByName("CVNAM").AsString;
        }

        LoadMisuBalance();
    }

    /// <summary>원본 prcMisuGet.</summary>
    private void LoadMisuBalance()
    {
        using var q = new DbQuery();
        q.Add("SELECT BAMT+OAMT01+OAMT02+OAMT03+OAMT04+OAMT05+OAMT06+OAMT07+");
        q.Add("       OAMT08+OAMT09+OAMT10+OAMT11+OAMT12+OVAT01+OVAT02+OVAT03+");
        q.Add("       OVAT04+OVAT05+OVAT06+OVAT07+OVAT08+OVAT09+OVAT10+OVAT11+");
        q.Add("       OVAT12-SAMT01-SAMT02-SAMT03-SAMT04-SAMT05-SAMT06-SAMT07-");
        q.Add("       SAMT08-SAMT09-SAMT10-SAMT11-SAMT12 AS BALANCE FROM MISUF");
        q.Add(" WHERE MYEAR=@YEAR AND CVCOD=@CVCOD");
        q.ParamByName("YEAR").AsString = dtpDate.Value.Year.ToString();
        q.ParamByName("CVCOD").AsString = edtCode.Text.Trim();
        q.Open();
        lblMisu.Text = q.IsEmpty || q.FieldByName("BALANCE").IsNull
            ? "0" : PublicLib.MoneyToStr((long)q.FieldByName("BALANCE").AsFloat);
    }

    private bool ErrCheck()
    {
        if (string.IsNullOrWhiteSpace(edtCode.Text))
        {
            MessageBox.Show("거래처코드는 필수항목입니다.\r\n반드시 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtCode.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(cboGu.Text))
        {
            MessageBox.Show("수금구분을 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboGu.Focus();
            return false;
        }
        if (edtAmt.Value == 0)
        {
            MessageBox.Show("수금액을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtAmt.Focus();
            return false;
        }
        return true;
    }

    private bool SaveEntry()
    {
        if (!ErrCheck()) return false;

        var dateStr = dtpDate.Value.ToString("yyyy-MM-dd");
        var year = dtpDate.Value.Year.ToString();
        var month = dtpDate.Value.Month.ToString("00");

        int seqNo;
        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            if (Job == "I")
            {
                using var maxQ = new DbQuery();
                maxQ.Add("SELECT MAX(ARSEQ) AS MAXSEQ FROM SUGMF WHERE ARDAT=@ARDAT");
                maxQ.ParamByName("ARDAT").AsString = dateStr;
                maxQ.Open();
                seqNo = maxQ.IsEmpty || maxQ.FieldByName("MAXSEQ").IsNull ? 1 : maxQ.FieldByName("MAXSEQ").AsInteger + 1;

                q.Add("INSERT INTO SUGMF (ARDAT,ARSEQ,CVCOD,ARGU,ARAMT,ABIGO,MDATE)");
                q.Add("  VALUES(@ARDAT,@ARSEQ,@CVCOD,@ARGU,@ARAMT,@ABIGO,@MDATE)");
            }
            else
            {
                seqNo = PublicLib.StrToMoney(edtNo.Text);
                q.Add("UPDATE SUGMF SET CVCOD=@CVCOD,ARGU=@ARGU,ARAMT=@ARAMT,");
                q.Add("    ABIGO=@ABIGO,MDATE=@MDATE");
                q.Add(" WHERE ARDAT=@ARDAT AND ARSEQ=@ARSEQ");
            }

            q.ParamByName("ARDAT").AsString = dateStr;
            q.ParamByName("ARSEQ").AsInteger = seqNo;
            q.ParamByName("CVCOD").AsString = edtCode.Text.Trim();
            q.ParamByName("ARGU").AsString = cboGu.Text.Trim();
            q.ParamByName("ARAMT").AsInteger = (int)edtAmt.Value;
            q.ParamByName("ABIGO").AsString = edtBigo.Text.Trim();
            q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
            q.ExecSQL();

            if (!LedgerUpdates.MisuUpdate(year, month, edtCode.Text.Trim(), 2, 0, (int)edtAmt.Value - OriginalAmt, 0))
            {
                AppDb.Rollback();
                MessageBox.Show("미수금 수금정리작업중 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("수금전표 보수시 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    /// <summary>원본 SS32.DBGrid1DblClick.</summary>
    public void LoadForEdit(DataRowView row)
    {
        dtpDate.Value = Convert.ToDateTime(row["ARDAT"]);
        edtNo.Text = row["ARSEQ"].ToString();
        edtCode.Text = row["CVCOD"].ToString();
        dspName.Text = row.Row.Table.Columns.Contains("CVNAM") ? row["CVNAM"].ToString() : "";
        if (cboGu.Items.Count == 0) ResetGuList();
        var idx = cboGu.Items.IndexOf((row["ARGU"].ToString() ?? "").Trim());
        cboGu.SelectedIndex = cboGu.Items.Count > 0 ? Math.Max(idx, 0) : -1;
        edtAmt.Value = Convert.ToDecimal(row["ARAMT"]);
        edtBigo.Text = row["ABIGO"].ToString();
        OriginalAmt = (int)edtAmt.Value;

        LoadMisuBalance();

        lblJob.Text = "수정중";
        Job = "U";
        btnAdd.Enabled = false;
    }
}
