using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

/// <summary>
/// 원본 BA01.pas / BA01.dfm (TfrmBA01) 이식 — 거래처 마스터(CVMAST 테이블).
/// 원본 DFM 확인 SQL: qryList = SELECT * FROM CVMAST ORDER BY CVCOD
/// cboGu 항목: 1.매입처 / 2.매출처 / 3.공통 (CVGU 컬럼에 1/2/3으로 저장)
/// </summary>
public partial class BA01Form : Form
{
    private string _vSort = "Code";
    private DataTable? _listTable;

    public BA01Form()
    {
        InitializeComponent();
        BuildDynamicLayout();
    }

    /// <summary>
    /// 원본 BuildLayout() — GridLayout 헬퍼로 좌표를 동적으로 계산하기 때문에(지역변수 사용)
    /// WinForms 디자이너가 InitializeComponent() 안에서는 처리하지 못해 이 메서드로 분리했다.
    /// InitializeComponent() 호출 직후 생성자에서 실행되므로 동작은 이전과 동일하다.
    /// </summary>
    private void BuildDynamicLayout()
    {
        this.cboGu.Items.AddRange(new object[] { "1. 매입처", "2. 매출처", "3. 공통" });

        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.Add(this.btnNew);
        this.panelTop.Controls.Add(this.btnAdd);
        this.panelTop.Controls.Add(this.btnOne);
        this.panelTop.Controls.Add(this.btnDel);
        this.panelTop.Controls.Add(this.btnExcel);
        this.panelTop.Controls.Add(this.btnPrint);
        this.panelTop.Controls.Add(this.btnClose);
        this.panelTop.Controls.Add(this.lblDbCnt);
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnAdd.Left = 100; this.btnAdd.Top = 8; this.btnAdd.Width = 90;
        this.btnOne.Left = 195; this.btnOne.Top = 8; this.btnOne.Width = 90;
        this.btnDel.Left = 290; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnExcel.Left = 385; this.btnExcel.Top = 8; this.btnExcel.Width = 90;
        this.btnPrint.Left = 480; this.btnPrint.Top = 8; this.btnPrint.Width = 90;
        this.btnClose.Left = 575; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.lblDbCnt.Left = 690; this.lblDbCnt.Top = 14;
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.grid, "거래처마스터");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.grid, "거래처마스터");

        this.searchPanel.Dock = DockStyle.Top;
        this.searchPanel.Height = 35;
        this.lblWord.Text = "검색어"; this.lblWord.Left = 5; this.lblWord.Top = 10; this.lblWord.AutoSize = true;
        this.edtWord.Left = 60; this.edtWord.Top = 6; this.edtWord.Width = 200;
        this.edtWord.KeyUp += (_, _) => LocateInGrid(this.edtWord.Text);
        this.searchPanel.Controls.AddRange(new Control[] { this.lblWord, this.edtWord });

        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 230;
        this.editPanel.AutoScroll = true;
        var layout = new GridLayout(this.editPanel, 5, 5, slotWidth: 250, labelWidth: 85, rowHeight: 30, slotsPerRow: 3);

        layout.Add("구분", this.cboGu);
        layout.Add("코드", this.edtCode);
        layout.AddRaw(this.chkAuto, width: 100);

        layout.Add("거래처명", this.edtName);
        layout.Add("대표자", this.edtOwnam);

        layout.Add("사업자번호", this.edtSano);
        layout.Add("법인번호", this.edtCvno);

        layout.Add("업태", this.edtUptae);
        layout.Add("종목", this.edtJongk);

        layout.Add("우편번호", this.edtPost);
        layout.Add("주소", this.edtAddr, span: 2);

        layout.Add("지역번호", this.edtDDD);
        layout.Add("전화번호", this.edtTel);
        layout.Add("팩스번호", this.edtFax);

        layout.Add("시작일", this.edtSdate);
        layout.Add("종료일", this.edtEdate);

        layout.NewRow();
        layout.Add("비고", this.edtBigo, span: 3);

        this.editPanel.Height = layout.Bottom(15);

        this.edtPost.DoubleClick += (_, _) => LookupPostalCode();
        this.edtAddr.DoubleClick += (_, _) => LookupPostalCode();
        this.chkAuto.Click += (_, _) => CodeToggle(!this.chkAuto.Checked);

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += (_, _) => SyncEditFromGrid();
        this.grid.KeyDown += (_, e) => { if (e.KeyCode == Keys.Delete) this.btnDel.PerformClick(); };

        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.searchPanel);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) => { ClearEdit(); CodeToggle(!this.chkAuto.Checked); this.cboGu.Focus(); };
        this.btnAdd.Click += (_, _) => Save(isInsert: true);
        this.btnOne.Click += (_, _) => Save(isInsert: false);
        this.btnDel.Click += (_, _) => Delete();
        this.btnClose.Click += (_, _) => Close();
    }

    private void BA01Form_Load(object? sender, EventArgs e)
    {
        this._vSort = "Code";
        ReloadList();
    }

    private void BA01Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F2: btnAdd.PerformClick(); break;
            case Keys.F3: btnOne.PerformClick(); break;
            case Keys.F4: btnDel.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 prcDBopen.</summary>
    private void ReloadList()
    {
        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST");
        q.Add(_vSort == "Name" ? "ORDER BY CVNAM" : "ORDER BY CVCOD");
        q.Open();
        _listTable = q.Table;
        grid.DataSource = _listTable;
        ApplyGridHeaders();
        lblDbCnt.Text = "거래처 총 " + PublicLib.MoneyToStr(_listTable?.Rows.Count ?? 0) + " 건";
    }

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["CVCOD"] = "코드", ["CVGU"] = "구분", ["CVNAM"] = "거래처명", ["OWNAM"] = "대표자",
            ["SANO"] = "사업자번호", ["CVNO"] = "법인번호", ["UPTAE"] = "업태", ["JONGK"] = "종목",
            ["POSNO"] = "우편번호", ["ADDR"] = "주소", ["DDD"] = "지역번호", ["TELNO"] = "전화번호",
            ["FAXNO"] = "팩스번호", ["SDATE"] = "시작일", ["JDATE"] = "종료일", ["CBIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in grid.Columns)
            if (col.Name is "TAXGU" or "MDATE") col.Visible = false;
    }

    private void LocateInGrid(string word)
    {
        if (_listTable == null) return;
        var field = _vSort == "Name" ? "CVNAM" : "CVCOD";
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.Cells[field].Value?.ToString()?.StartsWith(word, StringComparison.OrdinalIgnoreCase) == true)
            {
                grid.CurrentCell = row.Cells[0];
                break;
            }
        }
    }

    private void SyncEditFromGrid()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) { ClearEdit(); return; }

        edtCode.Text = row["CVCOD"].ToString();
        edtName.Text = row["CVNAM"].ToString();
        var cvgu = row["CVGU"].ToString();
        var guIdx = string.IsNullOrEmpty(cvgu) ? -1 : PublicLib.StrToIntSafe(cvgu) - 1;
        cboGu.SelectedIndex = guIdx >= 0 && guIdx < cboGu.Items.Count ? guIdx : -1;
        edtOwnam.Text = row["OWNAM"].ToString();
        edtSano.Text = row["SANO"].ToString();
        edtCvno.Text = row["CVNO"].ToString();
        edtUptae.Text = row["UPTAE"].ToString();
        edtJongk.Text = row["JONGK"].ToString();
        edtPost.Text = row["POSNO"].ToString();
        edtDDD.Text = row["DDD"].ToString();
        edtTel.Text = row["TELNO"].ToString();
        edtFax.Text = row["FAXNO"].ToString();
        edtAddr.Text = row["ADDR"].ToString();
        edtEdate.Text = row["JDATE"].ToString();
        edtSdate.Text = row["SDATE"].ToString();
        edtBigo.Text = row["CBIGO"].ToString();

        cboGu.Enabled = false;
        chkAuto.Enabled = false;
        CodeToggle(false);
    }

    private void ClearEdit()
    {
        edtCode.Clear(); edtName.Clear(); edtOwnam.Clear();
        edtSano.Clear(); edtCvno.Clear(); edtJongk.Clear(); edtUptae.Clear();
        edtPost.Clear(); edtAddr.Clear(); edtDDD.Clear();
        edtTel.Clear(); edtFax.Clear();
        edtSdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        edtEdate.Clear(); edtBigo.Clear();
        cboGu.SelectedIndex = 0;
        cboGu.Enabled = true; chkAuto.Enabled = true;
        CodeToggle(!chkAuto.Checked);
    }

    private void CodeToggle(bool enabled) => PublicLib.CodeToggle(edtCode, enabled);

    /// <summary>원본 fncErrCheck.</summary>
    private bool ErrCheck(bool isInsert)
    {
        if (cboGu.SelectedIndex < 0)
        {
            MessageBox.Show("구분을 선택하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboGu.Focus();
            return false;
        }

        if (!chkAuto.Checked && string.IsNullOrWhiteSpace(edtCode.Text))
        {
            MessageBox.Show("자동채번이 아닌경우는 코드를 반드시 입력하세요.", "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtCode.Focus();
            return false;
        }

        // 중복코드 검사는 신규 등록시에만 수행한다(수정시 자기 코드가 이미 존재하는건 정상).
        if (isInsert && !chkAuto.Checked && !string.IsNullOrWhiteSpace(edtCode.Text))
        {
            using var q = new DbQuery();
            q.Add($"SELECT * FROM CVMAST WHERE CVCOD='{edtCode.Text.Trim()}'");
            q.Open();
            if (!q.IsEmpty)
            {
                MessageBox.Show($"코드: {edtCode.Text.Trim()}는 이미존재하는 코드입니다.\r\n" +
                    "자동채번을 하거나, 중복되지 않는 번호를 입력하세요.", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                edtCode.Focus();
                return false;
            }
        }

        if (string.IsNullOrWhiteSpace(edtName.Text))
        {
            MessageBox.Show("거래처명을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtName.Focus();
            return false;
        }

        return true;
    }

    /// <summary>원본 fncAutoNo: 구분별 다음 코드 자동채번.</summary>
    private string AutoNo()
    {
        using var q = new DbQuery();
        q.Add("SELECT MAX(CVCOD) AS MAXCODE FROM CVMAST WHERE CVGU=@CVGU");
        q.ParamByName("CVGU").AsString = cboGu.Text.Substring(0, 1);
        q.Open();
        if (q.IsEmpty || q.FieldByName("MAXCODE").IsNull)
            return cboGu.Text.Substring(0, 1) + "001";

        var max = q.FieldByName("MAXCODE").AsString;
        var seq = PublicLib.StrToMoney(max.Length > 3 ? max[1..4] : "0") + 1;
        return cboGu.Text.Substring(0, 1) + seq.ToString("000");
    }

    /// <summary>원본 prcAddJob.</summary>
    private void Save(bool isInsert)
    {
        if (!ErrCheck(isInsert)) return;

        var code = isInsert && chkAuto.Checked ? AutoNo() : edtCode.Text.Trim();
        edtCode.Text = code;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            if (isInsert)
            {
                q.Add("INSERT INTO CVMAST (CVCOD,CVGU,CVNAM,SANO,CVNO,OWNAM,");
                q.Add("       UPTAE,JONGK,POSNO,ADDR,DDD,TELNO,FAXNO,SDATE,");
                q.Add("       JDATE,TAXGU,CBIGO,MDATE)");
                q.Add("    VALUES(@CVCOD,@CVGU,@CVNAM,@SANO,@CVNO,@OWNAM,");
                q.Add("       @UPTAE,@JONGK,@POSNO,@ADDR,@DDD,@TELNO,@FAXNO,");
                q.Add("       @SDATE,@JDATE,@TAXGU,@CBIGO,@MDATE)");
            }
            else
            {
                q.Add("UPDATE CVMAST SET CVGU=@CVGU,CVNAM=@CVNAM,SANO=@SANO,");
                q.Add("       CVNO=@CVNO,OWNAM=@OWNAM,UPTAE=@UPTAE,JONGK=@JONGK,");
                q.Add("       POSNO=@POSNO,ADDR=@ADDR,DDD=@DDD,TELNO=@TELNO,");
                q.Add("       FAXNO=@FAXNO,SDATE=@SDATE,JDATE=@JDATE,TAXGU=@TAXGU,");
                q.Add("       CBIGO=@CBIGO,MDATE=@MDATE");
                q.Add(" WHERE CVCOD = @CVCOD");
            }

            q.ParamByName("CVCOD").AsString = code;
            q.ParamByName("CVGU").AsString = (cboGu.SelectedIndex + 1).ToString();
            q.ParamByName("CVNAM").AsString = edtName.Text.Trim();
            q.ParamByName("SANO").AsString = edtSano.Text.Trim();
            q.ParamByName("CVNO").AsString = edtCvno.Text.Trim();
            q.ParamByName("OWNAM").AsString = edtOwnam.Text.Trim();
            q.ParamByName("UPTAE").AsString = edtUptae.Text.Trim();
            q.ParamByName("JONGK").AsString = edtJongk.Text.Trim();
            q.ParamByName("POSNO").AsString = edtPost.Text.Trim();
            q.ParamByName("ADDR").AsString = edtAddr.Text.Trim();
            q.ParamByName("DDD").AsString = edtDDD.Text.Trim();
            q.ParamByName("TELNO").AsString = edtTel.Text.Trim();
            q.ParamByName("FAXNO").AsString = edtFax.Text.Trim();
            q.ParamByName("TAXGU").AsString = "0";
            q.ParamByName("CBIGO").AsString = edtBigo.Text.Trim();
            q.ParamByName("SDATE").AsString = edtSdate.Text.Trim();
            q.ParamByName("JDATE").AsString = edtEdate.Text.Trim();
            q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");

            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("거래처 자료 보수시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ReloadList();
    }

    /// <summary>원본 btnDelClick.</summary>
    private void Delete()
    {
        if (!PublicLib.ConfirmDelete("거래처코드: " + edtCode.Text)) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM CVMAST WHERE CVCOD = @CVCOD");
            q.ParamByName("CVCOD").AsString = edtCode.Text.Trim();
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ReloadList();
    }

    /// <summary>원본 up_InfoPOSTF / gp_LoadFormPOSTF / gp_CallNamePOSTF (우편번호 검색 팝업).</summary>
    private void LookupPostalCode()
    {
        using var dlg = new Common.PostalLookupForm();
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        edtDDD.Text = dlg.SelectedDDD;
        edtPost.Text = dlg.SelectedZip;
        edtAddr.Text = dlg.SelectedAddr;
        edtAddr.Focus();
    }
}
