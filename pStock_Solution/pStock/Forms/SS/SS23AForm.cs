using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS23A.pas / SS23A.dfm (TfrmSS23A) 이식 — 보관료 등록/수정(IPCHF, GUBN1='1').
/// 품번 입력 시 현재고(JQTY)를 계산해 보관수량/금액을 자동 채운다.
/// </summary>
public partial class SS23AForm : Form
{
    public string Job { get; set; } = "I";
    public int OriginalAmt { get; set; }
    public bool Saved { get; private set; }

    public SS23AForm()
    {
        InitializeComponent();
        BuildDynamicLayout();
        ClearForm();
    }

    /// <summary>
    /// 원본 BuildLayout() — GridLayout 헬퍼로 좌표를 동적으로 계산하기 때문에(지역변수 사용)
    /// WinForms 디자이너가 InitializeComponent() 안에서는 처리하지 못해 이 메서드로 분리했다.
    /// </summary>
    private void BuildDynamicLayout()
    {
        PublicLib.MakeTypingFriendly(this.edtOqty);
        PublicLib.MakeTypingFriendly(this.edtOcost);
        PublicLib.MakeTypingFriendly(this.edtOamt);
        PublicLib.MakeTypingFriendly(this.edtJamt1);
        PublicLib.MakeTypingFriendly(this.edtJamt2);
        PublicLib.MakeTypingFriendly(this.edtJamt3);

        this.pnlJob.Left = 20; this.pnlJob.Top = 10;
        this.Controls.Add(this.pnlJob);

        var layout = new GridLayout(this, 20, 45, slotWidth: 220, labelWidth: 75, rowHeight: 30, slotsPerRow: 3);

        layout.Add("산정일자", this.dtpDate);
        this.dtpDate.Format = DateTimePickerFormat.Short;
        this.dtpDate.ValueChanged += (_, _) => this.lblNo.Text = TermLabel(this.dtpDate.Value);
        layout.AddRaw(this.lblNo);
        layout.Add("전표번호", this.edtNo);

        layout.Add("거래처코드", this.dspCvcod);
        this._tip.SetToolTip(this.dspCvcod, "Enter 키를 누르면 거래처를 검색합니다.");
        this.dspCvcod.KeyDown += new KeyEventHandler(this.DspCvcod_KeyDown);
        layout.Add("거래처명", this.dspCvnam, span: 2);

        layout.Add("품번", this.edtItnbr);
        this._tip.SetToolTip(this.edtItnbr, "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItnbr.KeyDown += new KeyEventHandler(this.EdtItnbr_KeyDown);
        layout.Add("품명", this.dspItdsc, span: 2);

        layout.Add("단위", this.dspDanwi);
        layout.Add("보관수량", this.edtOqty);
        this.edtOqty.ValueChanged += (_, _) => RecalcAmt();
        layout.Add("보관단가", this.edtOcost);
        this.edtOcost.ValueChanged += (_, _) => RecalcAmt();

        layout.Add("보관금액", this.edtOamt);
        layout.Add("부가세1", this.edtJamt1);
        this.edtJamt1.ValueChanged += (_, _) => RecalcTotal();
        layout.Add("부가세2", this.edtJamt2);
        this.edtJamt2.ValueChanged += (_, _) => RecalcTotal();

        layout.Add("부가세3", this.edtJamt3);
        this.edtJamt3.ValueChanged += (_, _) => RecalcTotal();
        layout.Add("합계금액", this.dspTamt);

        layout.NewRow();
        layout.Add("비고", this.edtBigo, span: 3);

        int y = layout.Bottom(20);
        this.btnAdd.Left = 150; this.btnAdd.Top = y; this.btnAdd.Width = 120;
        this.btnOne.Left = 280; this.btnOne.Top = y; this.btnOne.Width = 100;
        this.btnClose.Left = 390; this.btnClose.Top = y; this.btnClose.Width = 100;
        this.Controls.AddRange(new Control[] { this.btnAdd, this.btnOne, this.btnClose });

        this.btnAdd.Click += (_, _) => { if (SaveEntry()) { Saved = true; ClearForm(); this.edtItnbr.Focus(); } };
        this.btnOne.Click += (_, _) => { if (SaveEntry()) { Saved = true; Close(); } };
        this.btnClose.Click += (_, _) => Close();

        this.ClientSize = new Size(this.ClientSize.Width, y + 45);
    }

    private void SS23AForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: ClearForm(); dtpDate.Focus(); break;
            case Keys.F2: if (Job == "I") btnAdd.PerformClick(); break;
            case Keys.F3: btnOne.PerformClick(); break;
            case Keys.Escape: Close(); break;
        }
    }

    /// <summary>원본 fncTermGet: 반월 기수 표시(간이 근사 — 1~15일=1기, 16일 이후=2기).</summary>
    private static string TermLabel(DateTime date) => (date.Day <= 15 ? "1" : "2") + " 기";

    public void ClearForm()
    {
        dspCvcod.Clear(); dspCvnam.Clear(); edtNo.Clear();
        edtItnbr.Clear(); dspItdsc.Clear(); dspDanwi.Clear();
        edtOqty.Value = 0; edtOcost.Value = 0; edtOamt.Value = 0;
        edtJamt1.Value = 0; edtJamt2.Value = 0; edtJamt3.Value = 0;
        dspTamt.Value = 0;
        edtBigo.Clear();
        lblNo.Text = TermLabel(dtpDate.Value);
        OriginalAmt = 0;
        Job = "I";
        pnlJob.Text = "전표등록중";
    }

    private void RecalcAmt()
    {
        edtOamt.Value = edtOqty.Value * edtOcost.Value;
        RecalcTotal();
    }

    private void RecalcTotal()
    {
        dspTamt.Value = edtOamt.Value + edtJamt1.Value + edtJamt2.Value + edtJamt3.Value;
    }

    /// <summary>거래처코드 칸에서 Enter 입력시 거래처 검색 팝업을 띄운다(품번 입력시 자동으로도 채워지지만,
    /// 거래처를 먼저 선택하고 싶은 경우를 위한 수동 선택 경로).</summary>
    private void DspCvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        using var dlg = new CvcodLookupForm(cvguFilter: "1");
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            dspCvcod.Text = dlg.SelectedCode;
            dspCvnam.Text = dlg.SelectedName;
        }
    }

    /// <summary>원본 edtItnbrKeyDown: 현재고(JQTY) 계산 포함 품목 조회.</summary>
    private void EdtItnbr_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtItnbr.Text))
        {
            using var dlg = new ItemLookupForm();
            if (dlg.ShowDialog(this) != DialogResult.OK) { ClearForm(); edtItnbr.Focus(); return; }
            edtItnbr.Text = dlg.SelectedItnbr;
        }

        LoadCurrentStockItem(edtItnbr.Text.Trim());
    }

    /// <summary>원본 qryWork 조회(ITEMBL 현재고 JQTY 계산 포함).</summary>
    private void LoadCurrentStockItem(string itnbr)
    {
        const string qtyExpr =
            "BBALQ+I1QT01+I1QT02+I1QT03+I1QT04+I1QT05+I1QT06+I1QT07+I1QT08+I1QT09+" +
            "I1QT10+I1QT11+I1QT12-O1QT01-O1QT02-O1QT03-O1QT04-O1QT05-O1QT06-O1QT07-" +
            "O1QT08-O1QT09-O1QT10-O1QT11-O1QT12-IOQT01-IOQT02-IOQT03-IOQT04-IOQT05-" +
            "IOQT06-IOQT07-IOQT08-IOQT09-IOQT10-IOQT11-IOQT12";

        using var q = new DbQuery();
        q.Add($"SELECT (ITDSC+' '+ISPEC) CODNAM,B.BCOST,C.CVCOD,C.CVNAM,A.ITNBR,B.DANWI,");
        q.Add($"        SUM({qtyExpr}) JQTY FROM ITEMBL A");
        q.Add("   LEFT OUTER JOIN ITEMAS B ON A.ITNBR=B.ITNBR");
        q.Add("   LEFT OUTER JOIN CVMAST C ON SUBSTRING(A.ITNBR,1,4)=C.CVCOD");
        q.Add("  WHERE IYEAR=@YEAR AND A.ITNBR=@ITNBR");
        q.Add($"    AND ({qtyExpr})<>0");
        q.Add("  GROUP BY ITDSC,ISPEC,B.BCOST,C.CVCOD,C.CVNAM,A.ITNBR,B.DANWI");
        q.ParamByName("YEAR").AsString = DateTime.Now.Year.ToString();
        q.ParamByName("ITNBR").AsString = itnbr;
        q.Open();

        if (q.IsEmpty)
        {
            MessageBox.Show("현재고가 있는 품목이 아닙니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ClearForm();
            edtItnbr.Focus();
            return;
        }

        edtItnbr.Text = q.FieldByName("ITNBR").AsString;
        dspItdsc.Text = q.FieldByName("CODNAM").AsString;
        dspDanwi.Text = q.FieldByName("DANWI").AsString;
        dspCvnam.Text = q.FieldByName("CVNAM").AsString;
        dspCvcod.Text = q.FieldByName("CVCOD").AsString;
        var jqty = q.FieldByName("JQTY").AsFloat;
        var bcost = q.FieldByName("BCOST").AsFloat;
        edtOqty.Value = (decimal)jqty;
        edtOcost.Value = (decimal)bcost;
        edtOamt.Value = (decimal)Math.Truncate(jqty * bcost);
        RecalcTotal();
    }

    private bool ErrCheck()
    {
        if (string.IsNullOrWhiteSpace(edtItnbr.Text))
        {
            MessageBox.Show("품목코드는 필수항목입니다.\r\n반드시 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtItnbr.Focus();
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
                maxQ.Add("SELECT MAX(SEQNO) AS MAXSEQ FROM IPCHF WHERE TDATE=@TDATE AND GUBN1='1' AND SEQNO>5000");
                maxQ.ParamByName("TDATE").AsString = dateStr;
                maxQ.Open();
                seqNo = maxQ.IsEmpty || maxQ.FieldByName("MAXSEQ").IsNull ? 5001 : maxQ.FieldByName("MAXSEQ").AsInteger + 1;

                q.Add("INSERT INTO IPCHF (TDATE,SEQNO,GUBN1,CVCOD,ITNBR,DANWI,IOQTY,");
                q.Add("       ODAN,OAMT,JAMT1,JAMT2,JAMT3,");
                q.Add("       TPRO,HOUSE,TBIGO,KEYNO,MDATE)");
                q.Add("  VALUES(@TDATE,@SEQNO,@GUBN1,@CVCOD,@ITNBR,@DANWI,@IOQTY,");
                q.Add("       @ODAN,@OAMT,@JAMT1,@JAMT2,@JAMT3,");
                q.Add("       @TPRO,@HOUSE,@TBIGO,@KEYNO,@MDATE)");
            }
            else
            {
                seqNo = PublicLib.StrToMoney(edtNo.Text);
                q.Add("UPDATE IPCHF SET GUBN1=@GUBN1,CVCOD=@CVCOD,ITNBR=@ITNBR,");
                q.Add("    DANWI=@DANWI,IOQTY=@IOQTY,ODAN=@ODAN,OAMT=@OAMT,");
                q.Add("    JAMT1=@JAMT1,JAMT2=@JAMT2,JAMT3=@JAMT3,TPRO=@TPRO,");
                q.Add("    HOUSE=@HOUSE,TBIGO=@TBIGO,KEYNO=@KEYNO,MDATE=@MDATE");
                q.Add(" WHERE TDATE=@TDATE AND SEQNO=@SEQNO");
            }

            q.ParamByName("TDATE").AsString = dateStr;
            q.ParamByName("SEQNO").AsInteger = seqNo;
            q.ParamByName("GUBN1").AsString = "1";
            q.ParamByName("CVCOD").AsString = dspCvcod.Text.Trim();
            q.ParamByName("ITNBR").AsString = edtItnbr.Text.Trim();
            q.ParamByName("DANWI").AsString = dspDanwi.Text.Trim();
            q.ParamByName("IOQTY").AsInteger = (int)edtOqty.Value;
            q.ParamByName("ODAN").AsInteger = (int)edtOcost.Value;
            q.ParamByName("OAMT").AsInteger = (int)edtOamt.Value;
            q.ParamByName("JAMT1").AsInteger = (int)edtJamt1.Value;
            q.ParamByName("JAMT2").AsInteger = (int)edtJamt2.Value;
            q.ParamByName("JAMT3").AsInteger = (int)edtJamt3.Value;
            q.ParamByName("TPRO").AsInteger = PublicLib.StrToMoney(dtpDate.Value.ToString("yyyyMM") + (dtpDate.Value.Day <= 15 ? "1" : "2"));
            q.ParamByName("HOUSE").AsString = "";
            q.ParamByName("TBIGO").AsString = edtBigo.Text.Trim();
            q.ParamByName("KEYNO").AsString = "";
            q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
            q.ExecSQL();

            var cvPrefix = edtItnbr.Text.Length >= 4 ? edtItnbr.Text[..4] : edtItnbr.Text;
            var totalAmt = (int)edtOamt.Value + (int)edtJamt1.Value + (int)edtJamt2.Value + (int)edtJamt3.Value;
            if (!LedgerUpdates.MisuUpdate(year, month, cvPrefix, 1, totalAmt - OriginalAmt, 0, 0))
            {
                AppDb.Rollback();
                MessageBox.Show("미수파일 쓰는중 에러발생(+)..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("보관전표 보수시 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    /// <summary>원본 SS23.DBGrid1DblClick: 목록 선택 행 값을 편집 폼에 채운다.</summary>
    public void LoadForEdit(DataRowView row)
    {
        dtpDate.Value = Convert.ToDateTime(row["TDATE"]);
        edtNo.Text = row["SEQNO"].ToString();
        dspCvcod.Text = row["CVCOD"].ToString();
        dspCvnam.Text = row.Row.Table.Columns.Contains("CVNAM") ? row["CVNAM"].ToString() : "";
        edtItnbr.Text = row["ITNBR"].ToString();
        dspItdsc.Text = (row.Row.Table.Columns.Contains("ITDSC") ? row["ITDSC"].ToString() : "") + " " +
                        (row.Row.Table.Columns.Contains("ISPEC") ? row["ISPEC"].ToString() : "");
        dspDanwi.Text = row["DANWI"].ToString();
        edtOqty.Value = Convert.ToDecimal(row["IOQTY"]);
        edtOcost.Value = Convert.ToDecimal(row["ODAN"]);
        edtOamt.Value = Convert.ToDecimal(row["OAMT"]);
        edtJamt1.Value = row.Row.Table.Columns.Contains("JAMT1") ? Convert.ToDecimal(row["JAMT1"]) : 0;
        edtJamt2.Value = row.Row.Table.Columns.Contains("JAMT2") ? Convert.ToDecimal(row["JAMT2"]) : 0;
        edtJamt3.Value = row.Row.Table.Columns.Contains("JAMT3") ? Convert.ToDecimal(row["JAMT3"]) : 0;
        RecalcTotal();
        edtBigo.Text = row["TBIGO"].ToString();
        lblNo.Text = TermLabel(dtpDate.Value);

        OriginalAmt = (int)edtOamt.Value + (int)edtJamt1.Value + (int)edtJamt2.Value + (int)edtJamt3.Value;
        pnlJob.Text = "전표수정중";
        Job = "U";
        dtpDate.Enabled = false;
        edtItnbr.Enabled = false;
        btnAdd.Enabled = false;
    }
}
