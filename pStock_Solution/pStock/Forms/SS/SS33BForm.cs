using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS33B.pas / SS33B.dfm (TfrmSS33B) 이식 — 계산서 등록/수정(TAXF).
/// 하나의 계산서에 품목 4건(MMDD/품번/품명/수량/단가/금액)까지 기재 가능한 원본 구조를 그대로 유지.
/// </summary>
public partial class SS33BForm : Form
{
    public string Job { get; set; } = "I";
    public bool Saved { get; private set; }

    public SS33BForm()
    {
        InitializeComponent();
        ClearForm();
    }

    private void AddRow(Control parent, string caption, Control edit, int left, ref int y, int width, bool sameRow = false, int? labelWidth = null)
    {
        var lbl = new Label { Text = caption, Left = left, Top = y + 3, AutoSize = true };
        parent.Controls.Add(lbl);
        edit.Left = labelWidth.HasValue ? left + labelWidth.Value : lbl.Right + 10;
        edit.Top = y;
        edit.Width = width;
        parent.Controls.Add(edit);
        if (!sameRow) y += 28;
    }

    private void SS33BForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: ClearForm(); edtCvcod.Focus(); break;
            case Keys.F2: btnAdd.PerformClick(); break;
            case Keys.F3: btnOne.PerformClick(); break;
            case Keys.Escape: Close(); break;
        }
    }

    public void ClearForm()
    {
        edtTDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        edtNo.Clear();
        edtCvcod.Clear(); dspCvnam.Clear();
        for (int i = 0; i < 4; i++)
        {
            edtMmdd[i].Clear(); edtItnbr[i].Clear(); edtItdsc[i].Clear();
            edtQty[i].Value = 0; edtCost[i].Value = 0; edtAmt[i].Value = 0;
        }
        edtBigo.Clear();
        rdo2.Checked = true;
        RecalcTotal();
        Job = "I";
    }

    private void RecalcRow(int i)
    {
        edtAmt[i].Value = edtQty[i].Value * edtCost[i].Value;
    }

    private void RecalcTotal()
    {
        decimal sum = edtAmt[0].Value + edtAmt[1].Value + edtAmt[2].Value + edtAmt[3].Value;
        lblAmt.Text = sum.ToString("#,0");
        lblVat.Text = Math.Truncate(sum / 10).ToString("#,0");
    }

    /// <summary>원본 edtCvcodKeyDown.</summary>
    private void EdtCvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            using var dlg = new CvcodLookupForm(cvguFilter: "2");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCvcod.Text = dlg.SelectedCode;
                dspCvnam.Text = dlg.SelectedName;
            }
            else
            {
                dspCvnam.Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD");
        q.ParamByName("CVCOD").AsString = edtCvcod.Text.Trim();
        q.Open();
        if (!q.IsEmpty) dspCvnam.Text = q.FieldByName("CVNAM").AsString;
    }

    /// <summary>원본 edtITNBR1KeyDown (4개 라인 공용).</summary>
    private void EdtItnbr_KeyDown(int i, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtItnbr[i].Text))
        {
            using var dlg = new ItemLookupForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtItnbr[i].Text = dlg.SelectedItnbr;
                edtItdsc[i].Text = $"{dlg.SelectedItdsc} {dlg.SelectedIspec}".Trim();
                edtCost[i].Value = dlg.SelectedOcost;
            }
            else
            {
                edtItdsc[i].Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM ITEMAS WHERE ITNBR=@ITNBR");
        q.ParamByName("ITNBR").AsString = edtItnbr[i].Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edtItdsc[i].Text = $"{q.FieldByName("ITDSC").AsString} {q.FieldByName("ISPEC").AsString}".Trim();
            edtCost[i].Value = (decimal)q.FieldByName("OCOST").AsFloat;
        }
    }

    private bool ErrCheck()
    {
        if (string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            MessageBox.Show("거래처코드는 필수항목입니다.\r\n반드시 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtCvcod.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(edtItdsc[0].Text))
        {
            MessageBox.Show("첫번째 품목을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtItnbr[0].Focus();
            return false;
        }
        if (edtAmt[0].Value == 0)
        {
            MessageBox.Show("첫번째 금액을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtAmt[0].Focus();
            return false;
        }
        return true;
    }

    private bool SaveEntry()
    {
        if (!ErrCheck()) return false;

        var tdateStr = edtTDate.Text.Trim();

        int seqNo;
        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();

            if (Job == "I")
            {
                using var maxQ = new DbQuery();
                maxQ.Add("SELECT MAX(SEQNO) AS MAXSEQ FROM TAXF WHERE TDATE=@TDATE");
                maxQ.ParamByName("TDATE").AsString = tdateStr;
                maxQ.Open();
                seqNo = maxQ.IsEmpty || maxQ.FieldByName("MAXSEQ").IsNull ? 1 : maxQ.FieldByName("MAXSEQ").AsInteger + 1;

                q.Add("INSERT INTO TAXF (TDATE,SEQNO,CVCOD,TAMT,TVAT,");
                q.Add("     MMDD1,ITNBR1,ITDSC1,TQTY1,TDANGA1,TAMT1,TVAT1,");
                q.Add("     MMDD2,ITNBR2,ITDSC2,TQTY2,TDANGA2,TAMT2,TVAT2,");
                q.Add("     MMDD3,ITNBR3,ITDSC3,TQTY3,TDANGA3,TAMT3,TVAT3,");
                q.Add("     MMDD4,ITNBR4,ITDSC4,TQTY4,TDANGA4,TAMT4,TVAT4,");
                q.Add("     TGUBN,TBIGO,TNO1,TNO2,MDATE)");
                q.Add("  VALUES(@TDATE,@SEQNO,@CVCOD,@TAMT,@TVAT,");
                q.Add("     @MMDD1,@ITNBR1,@ITDSC1,@TQTY1,@TDANGA1,@TAMT1,@TVAT1,");
                q.Add("     @MMDD2,@ITNBR2,@ITDSC2,@TQTY2,@TDANGA2,@TAMT2,@TVAT2,");
                q.Add("     @MMDD3,@ITNBR3,@ITDSC3,@TQTY3,@TDANGA3,@TAMT3,@TVAT3,");
                q.Add("     @MMDD4,@ITNBR4,@ITDSC4,@TQTY4,@TDANGA4,@TAMT4,@TVAT4,");
                q.Add("     @TGUBN,@TBIGO,@TNO1,@TNO2,@MDATE)");
            }
            else
            {
                seqNo = PublicLib.StrToMoney(edtNo.Text);
                q.Add("UPDATE TAXF SET CVCOD=@CVCOD,TAMT=@TAMT,TVAT=@TVAT,");
                q.Add("     MMDD1=@MMDD1,ITNBR1=@ITNBR1,ITDSC1=@ITDSC1,TQTY1=@TQTY1,TDANGA1=@TDANGA1,TAMT1=@TAMT1,TVAT1=@TVAT1,");
                q.Add("     MMDD2=@MMDD2,ITNBR2=@ITNBR2,ITDSC2=@ITDSC2,TQTY2=@TQTY2,TDANGA2=@TDANGA2,TAMT2=@TAMT2,TVAT2=@TVAT2,");
                q.Add("     MMDD3=@MMDD3,ITNBR3=@ITNBR3,ITDSC3=@ITDSC3,TQTY3=@TQTY3,TDANGA3=@TDANGA3,TAMT3=@TAMT3,TVAT3=@TVAT3,");
                q.Add("     MMDD4=@MMDD4,ITNBR4=@ITNBR4,ITDSC4=@ITDSC4,TQTY4=@TQTY4,TDANGA4=@TDANGA4,TAMT4=@TAMT4,TVAT4=@TVAT4,");
                q.Add("     TGUBN=@TGUBN,TBIGO=@TBIGO,TNO1=@TNO1,TNO2=@TNO2,MDATE=@MDATE");
                q.Add(" WHERE TDATE=@TDATE AND SEQNO=@SEQNO");
            }

            q.ParamByName("TDATE").AsString = tdateStr;
            q.ParamByName("SEQNO").AsInteger = seqNo;
            q.ParamByName("CVCOD").AsString = edtCvcod.Text.Trim();
            var total = edtAmt[0].Value + edtAmt[1].Value + edtAmt[2].Value + edtAmt[3].Value;
            q.ParamByName("TAMT").AsCurrency = total;
            q.ParamByName("TVAT").AsCurrency = Math.Truncate(total / 10);
            for (int i = 0; i < 4; i++)
            {
                var n = i + 1;
                q.ParamByName($"MMDD{n}").AsString = edtMmdd[i].Text.Trim();
                q.ParamByName($"ITNBR{n}").AsString = edtItnbr[i].Text.Trim();
                q.ParamByName($"ITDSC{n}").AsString = edtItdsc[i].Text.Trim();
                q.ParamByName($"TQTY{n}").AsCurrency = edtQty[i].Value;
                q.ParamByName($"TDANGA{n}").AsCurrency = edtCost[i].Value;
                q.ParamByName($"TAMT{n}").AsCurrency = edtAmt[i].Value;
                q.ParamByName($"TVAT{n}").AsCurrency = Math.Truncate(edtAmt[i].Value / 10);
            }
            q.ParamByName("TGUBN").AsString = rdo1.Checked ? "영수" : "청구";
            q.ParamByName("TBIGO").AsString = edtBigo.Text.Trim();
            q.ParamByName("TNO1").AsString = "A";
            q.ParamByName("TNO2").AsString = "";
            q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
            q.ExecSQL();

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 입/수정시 에러발생...\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    /// <summary>원본 SS33.prcSubUpt: 목록에서 선택한 행 값을 편집 폼에 채운다.</summary>
    public void LoadForEdit(DataRowView row)
    {
        edtTDate.Text = row["TDATE"].ToString();
        edtNo.Text = row["SEQNO"].ToString();
        edtCvcod.Text = row["CVCOD"].ToString();
        dspCvnam.Text = row.Row.Table.Columns.Contains("CVNAM") ? row["CVNAM"].ToString() : "";
        for (int i = 0; i < 4; i++)
        {
            var n = i + 1;
            edtMmdd[i].Text = row[$"MMDD{n}"].ToString();
            edtItnbr[i].Text = row[$"ITNBR{n}"].ToString();
            edtItdsc[i].Text = row[$"ITDSC{n}"].ToString();
            edtQty[i].Value = Convert.ToDecimal(row[$"TQTY{n}"]);
            edtCost[i].Value = Convert.ToDecimal(row[$"TDANGA{n}"]);
            edtAmt[i].Value = Convert.ToDecimal(row[$"TAMT{n}"]);
        }
        edtBigo.Text = row["TBIGO"].ToString();
        rdo1.Checked = row["TGUBN"].ToString() == "영수";
        rdo2.Checked = !rdo1.Checked;
        RecalcTotal();
        Job = "U";
    }
}
