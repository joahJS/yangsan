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
        BuildDynamicLayout();
        ClearForm();
    }

    /// <summary>
    /// 원본 라인아이템 4건 컨트롤 생성 + BuildLayout() — GridLayout 헬퍼와 col1/y 같은
    /// 지역변수로 좌표를 동적으로 계산하기 때문에 WinForms 디자이너가 InitializeComponent()
    /// 안에서는 처리하지 못해 이 메서드로 분리했다.
    /// </summary>
    private void BuildDynamicLayout()
    {
        this.edtMmdd[0] = new TextBox();
        this.edtItnbr[0] = new TextBox();
        this.edtItdsc[0] = new TextBox { ReadOnly = true };
        this.edtQty[0] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[0] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[0] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[0]);
        PublicLib.MakeTypingFriendly(this.edtCost[0]);
        PublicLib.MakeTypingFriendly(this.edtAmt[0]);
        this.edtMmdd[1] = new TextBox();
        this.edtItnbr[1] = new TextBox();
        this.edtItdsc[1] = new TextBox { ReadOnly = true };
        this.edtQty[1] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[1] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[1] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[1]);
        PublicLib.MakeTypingFriendly(this.edtCost[1]);
        PublicLib.MakeTypingFriendly(this.edtAmt[1]);
        this.edtMmdd[2] = new TextBox();
        this.edtItnbr[2] = new TextBox();
        this.edtItdsc[2] = new TextBox { ReadOnly = true };
        this.edtQty[2] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[2] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[2] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[2]);
        PublicLib.MakeTypingFriendly(this.edtCost[2]);
        PublicLib.MakeTypingFriendly(this.edtAmt[2]);
        this.edtMmdd[3] = new TextBox();
        this.edtItnbr[3] = new TextBox();
        this.edtItdsc[3] = new TextBox { ReadOnly = true };
        this.edtQty[3] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[3] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[3] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[3]);
        PublicLib.MakeTypingFriendly(this.edtCost[3]);
        PublicLib.MakeTypingFriendly(this.edtAmt[3]);

        int col1 = 20;
        var grid = new GridLayout(this, col1, 15, slotWidth: 260, labelWidth: 85, rowHeight: 30, slotsPerRow: 3);
        grid.Add("발행일자", this.edtTDate);
        grid.Add("전표번호", this.edtNo);
        grid.NewRow();
        grid.Add("거래처코드", this.edtCvcod);
        this._tip.SetToolTip(this.edtCvcod, "Enter 키를 누르면 거래처를 검색합니다.");
        this.edtCvcod.KeyDown += this.EdtCvcod_KeyDown;
        grid.Add("거래처명", this.dspCvnam);
        grid.AddRaw(this.rdo1, width: 60);
        grid.AddRaw(this.rdo2, width: 60);
        int y = grid.Bottom();

        var headerY = y;
        this.lblHdr = new Label[6];
        this.lblHdr[0] = new Label { Text = "일자", Left = col1, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[0]);
        this.lblHdr[1] = new Label { Text = "품번", Left = col1 + 70, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[1]);
        this.lblHdr[2] = new Label { Text = "품명", Left = col1 + 200, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[2]);
        this.lblHdr[3] = new Label { Text = "수량", Left = col1 + 380, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[3]);
        this.lblHdr[4] = new Label { Text = "단가", Left = col1 + 460, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[4]);
        this.lblHdr[5] = new Label { Text = "금액", Left = col1 + 550, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[5]);
        y += 20;

        // 행 0
        this.edtMmdd[0].Left = col1; this.edtMmdd[0].Top = y; this.edtMmdd[0].Width = 60;
        this.edtItnbr[0].Left = col1 + 70; this.edtItnbr[0].Top = y; this.edtItnbr[0].Width = 120;
        this.edtItnbr[0].KeyDown += (_, e) => this.EdtItnbr_KeyDown(0, e);
        this._tip.SetToolTip(this.edtItnbr[0], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[0].Left = col1 + 200; this.edtItdsc[0].Top = y; this.edtItdsc[0].Width = 170;
        this.edtQty[0].Left = col1 + 380; this.edtQty[0].Top = y; this.edtQty[0].Width = 70;
        this.edtQty[0].ValueChanged += (_, _) => this.RecalcRow(0);
        this.edtCost[0].Left = col1 + 460; this.edtCost[0].Top = y; this.edtCost[0].Width = 80;
        this.edtCost[0].ValueChanged += (_, _) => this.RecalcRow(0);
        this.edtAmt[0].Left = col1 + 550; this.edtAmt[0].Top = y; this.edtAmt[0].Width = 100;
        this.edtAmt[0].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[0], this.edtItnbr[0], this.edtItdsc[0], this.edtQty[0], this.edtCost[0], this.edtAmt[0] });
        y += 30;

        // 행 1
        this.edtMmdd[1].Left = col1; this.edtMmdd[1].Top = y; this.edtMmdd[1].Width = 60;
        this.edtItnbr[1].Left = col1 + 70; this.edtItnbr[1].Top = y; this.edtItnbr[1].Width = 120;
        this.edtItnbr[1].KeyDown += (_, e) => this.EdtItnbr_KeyDown(1, e);
        this._tip.SetToolTip(this.edtItnbr[1], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[1].Left = col1 + 200; this.edtItdsc[1].Top = y; this.edtItdsc[1].Width = 170;
        this.edtQty[1].Left = col1 + 380; this.edtQty[1].Top = y; this.edtQty[1].Width = 70;
        this.edtQty[1].ValueChanged += (_, _) => this.RecalcRow(1);
        this.edtCost[1].Left = col1 + 460; this.edtCost[1].Top = y; this.edtCost[1].Width = 80;
        this.edtCost[1].ValueChanged += (_, _) => this.RecalcRow(1);
        this.edtAmt[1].Left = col1 + 550; this.edtAmt[1].Top = y; this.edtAmt[1].Width = 100;
        this.edtAmt[1].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[1], this.edtItnbr[1], this.edtItdsc[1], this.edtQty[1], this.edtCost[1], this.edtAmt[1] });
        y += 30;

        // 행 2
        this.edtMmdd[2].Left = col1; this.edtMmdd[2].Top = y; this.edtMmdd[2].Width = 60;
        this.edtItnbr[2].Left = col1 + 70; this.edtItnbr[2].Top = y; this.edtItnbr[2].Width = 120;
        this.edtItnbr[2].KeyDown += (_, e) => this.EdtItnbr_KeyDown(2, e);
        this._tip.SetToolTip(this.edtItnbr[2], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[2].Left = col1 + 200; this.edtItdsc[2].Top = y; this.edtItdsc[2].Width = 170;
        this.edtQty[2].Left = col1 + 380; this.edtQty[2].Top = y; this.edtQty[2].Width = 70;
        this.edtQty[2].ValueChanged += (_, _) => this.RecalcRow(2);
        this.edtCost[2].Left = col1 + 460; this.edtCost[2].Top = y; this.edtCost[2].Width = 80;
        this.edtCost[2].ValueChanged += (_, _) => this.RecalcRow(2);
        this.edtAmt[2].Left = col1 + 550; this.edtAmt[2].Top = y; this.edtAmt[2].Width = 100;
        this.edtAmt[2].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[2], this.edtItnbr[2], this.edtItdsc[2], this.edtQty[2], this.edtCost[2], this.edtAmt[2] });
        y += 30;

        // 행 3
        this.edtMmdd[3].Left = col1; this.edtMmdd[3].Top = y; this.edtMmdd[3].Width = 60;
        this.edtItnbr[3].Left = col1 + 70; this.edtItnbr[3].Top = y; this.edtItnbr[3].Width = 120;
        this.edtItnbr[3].KeyDown += (_, e) => this.EdtItnbr_KeyDown(3, e);
        this._tip.SetToolTip(this.edtItnbr[3], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[3].Left = col1 + 200; this.edtItdsc[3].Top = y; this.edtItdsc[3].Width = 170;
        this.edtQty[3].Left = col1 + 380; this.edtQty[3].Top = y; this.edtQty[3].Width = 70;
        this.edtQty[3].ValueChanged += (_, _) => this.RecalcRow(3);
        this.edtCost[3].Left = col1 + 460; this.edtCost[3].Top = y; this.edtCost[3].Width = 80;
        this.edtCost[3].ValueChanged += (_, _) => this.RecalcRow(3);
        this.edtAmt[3].Left = col1 + 550; this.edtAmt[3].Top = y; this.edtAmt[3].Width = 100;
        this.edtAmt[3].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[3], this.edtItnbr[3], this.edtItdsc[3], this.edtQty[3], this.edtCost[3], this.edtAmt[3] });
        y += 30;

        this.AddRow(this, "비고", this.edtBigo, col1, ref y, 400, labelWidth: 85);

        this.lblAmtCap = new Label { Text = "합계금액:", Left = col1, Top = y + 5, AutoSize = true };
        this.lblAmt.Left = col1 + 90; this.lblAmt.Top = y + 5;
        this.lblVatCap = new Label { Text = "부가세:", Left = col1 + 250, Top = y + 5, AutoSize = true };
        this.lblVat.Left = col1 + 330; this.lblVat.Top = y + 5;
        this.Controls.AddRange(new Control[] { this.lblAmtCap, this.lblAmt, this.lblVatCap, this.lblVat });
        y += 35;

        this.btnAdd.Left = 150; this.btnAdd.Top = y; this.btnAdd.Width = 130;
        this.btnOne.Left = 290; this.btnOne.Top = y; this.btnOne.Width = 100;
        this.btnClose.Left = 400; this.btnClose.Top = y; this.btnClose.Width = 100;
        this.Controls.AddRange(new Control[] { this.btnAdd, this.btnOne, this.btnClose });

        this.btnAdd.Click += (_, _) => { if (this.SaveEntry()) { this.Saved = true; this.ClearForm(); this.edtCvcod.Focus(); } };
        this.btnOne.Click += (_, _) => { if (this.SaveEntry()) { this.Saved = true; this.Close(); } };
        this.btnClose.Click += (_, _) => this.Close();

        this.ClientSize = new Size(this.ClientSize.Width, y + 45);
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
