using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 ss020u01.pas (TSS020F01) 이식 — 출고 등록/수정(SALE_M/SALE_D).
/// 원본은 그리드 행 상태(신규/수정/삭제)를 개별 추적해 부분 반영하지만,
/// 여기서는 저장 시 해당 출고번호의 상세 내역을 전부 지우고 다시 쓰는 방식으로 단순화했다
/// (결과는 동일하지만 트랜잭션 안에서 원자적으로 처리되어 더 안전하다).
/// 재고/미수 반영은 저장 전후 상세 합계 차이만큼만 적용한다.
/// </summary>
public partial class SS020F01Form : Form
{
    /// <summary>원본 bIns: true=신규, false=수정.</summary>
    public bool IsInsert { get; set; } = true;
    public bool Saved { get; private set; }

    public SS020F01Form()
    {
        InitializeComponent();
        InitScreen();
    }

    private void SS020F01Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: InitScreen(); eCvcod.Focus(); break;
            case Keys.F2: bAdd.PerformClick(); break;
            case Keys.F3: bOne.PerformClick(); break;
            case Keys.F4: btnDelRow.PerformClick(); break;
            case Keys.Escape: Close(); break;
        }
    }

    /// <summary>원본 up_InitScreen.</summary>
    public void InitScreen()
    {
        IsInsert = true;
        eTdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        eNo.Clear(); chkAuto.Checked = true;
        eCvcod.Clear(); lCvnam.Clear(); lTelno.Clear();
        eLncod.Clear(); lLnnam.Clear(); lLnadr.Clear();
        ePlncd.Text = UserContext.Current.Name;
        eMbigo.Clear(); eSsamt.Value = 0;
        rgListS.Rows.Clear();
        RecalcTotal();
        bAdd.Enabled = true;
    }

    /// <summary>원본 e_CVCODKeyDown.</summary>
    private void ECvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(eCvcod.Text))
        {
            using var dlg = new CvcodLookupForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                eCvcod.Text = dlg.SelectedCode;
                lCvnam.Text = dlg.SelectedName;
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD");
        q.ParamByName("CVCOD").AsString = eCvcod.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            lCvnam.Text = q.FieldByName("CVNAM").AsString;
            lTelno.Text = q.FieldByName("TELNO").AsString;
        }
    }

    private void ELncod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(eLncod.Text))
        {
            using var dlg = new Common.LnLookupForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                eLncod.Text = dlg.SelectedCode;
                lLnnam.Text = dlg.SelectedName;
                lLnadr.Text = dlg.SelectedAddr;
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM REACH WHERE LNCOD=@LNCOD");
        q.ParamByName("LNCOD").AsString = eLncod.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            lLnnam.Text = q.FieldByName("LNNAM").AsString;
            lLnadr.Text = $"{q.FieldByName("ADDR1").AsString} {q.FieldByName("ADDR2").AsString}".Trim();
        }
    }

    /// <summary>원본 rgList_sEditButtonClick / up_InfoITEMAS: 그리드 행 더블클릭 시 품목 검색.</summary>
    private void RgListS_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var col = rgListS.Columns[e.ColumnIndex].Name;
        if (col != "ITCOD") return;

        using var dlg = new ItemLookupForm();
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        var row = rgListS.Rows[e.RowIndex];
        row.Cells["ITCOD"].Value = dlg.SelectedItnbr;
        row.Cells["ITNAM"].Value = dlg.SelectedItdsc;
        row.Cells["ISPEC"].Value = dlg.SelectedIspec;
        row.Cells["DANWI"].Value = dlg.SelectedDanwi;
        row.Cells["UCOST"].Value = dlg.SelectedOcost;
        if (row.Cells["TRQTY"].Value == null || Convert.ToDecimal(row.Cells["TRQTY"].Value) == 0)
            row.Cells["TRQTY"].Value = 1;
        if (string.IsNullOrEmpty(row.Cells["HOUSE"].Value?.ToString()))
            row.Cells["HOUSE"].Value = "";

        RecalcRowAndTotal();
    }

    /// <summary>원본 rgList_sCalcColumns + up_setQty.</summary>
    private void RecalcRowAndTotal()
    {
        foreach (DataGridViewRow row in rgListS.Rows)
        {
            if (row.IsNewRow) continue;
            var qty = ToDec(row.Cells["TRQTY"].Value);
            var cost = ToDec(row.Cells["UCOST"].Value);
            row.Cells["TRAMT"].Value = qty * cost;
        }
        RecalcTotal();
    }

    private void RecalcTotal()
    {
        decimal tamt = 0, jamt = 0;
        foreach (DataGridViewRow row in rgListS.Rows)
        {
            if (row.IsNewRow) continue;
            tamt += ToDec(row.Cells["TRAMT"].Value);
            jamt += ToDec(row.Cells["JAMT1"].Value) + ToDec(row.Cells["JAMT2"].Value) + ToDec(row.Cells["JAMT3"].Value);
        }
        dTamt.Value = tamt;
        dJamt.Value = jamt;
    }

    private static decimal ToDec(object? v) => v == null || v == DBNull.Value ? 0 : Convert.ToDecimal(v);

    private bool ErrCheck()
    {
        if (!DateTime.TryParse(eTdate.Text, out _))
        {
            MessageBox.Show("날짜 형식이 올바르지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            eTdate.Focus();
            return false;
        }
        if (string.IsNullOrWhiteSpace(eCvcod.Text))
        {
            MessageBox.Show("거래처코드 필수입력 항목이 빠졌습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            eCvcod.Focus();
            return false;
        }
        if (rgListS.Rows.Count == 0)
        {
            MessageBox.Show("상세내역을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        foreach (DataGridViewRow row in rgListS.Rows)
        {
            if (row.IsNewRow) continue;
            var itcod = row.Cells["ITCOD"].Value?.ToString() ?? "";
            if (itcod != "" && ToDec(row.Cells["TRQTY"].Value) == 0)
            {
                MessageBox.Show("수량 필수입력 항목이 빠졌습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        return true;
    }

    /// <summary>원본 uf_AddJob(간략화: 상세 전체 재작성 방식).</summary>
    private bool SaveEntry(out string salNo)
    {
        salNo = string.Empty;
        if (!ErrCheck()) return false;

        var tdate = DateTime.Parse(eTdate.Text).ToString("yyyy-MM-dd");
        var year = DateTime.Parse(eTdate.Text).Year.ToString();
        var month = DateTime.Parse(eTdate.Text).Month.ToString("00");

        try
        {
            AppDb.BeginTransaction();

            // 수정인 경우, 기존 상세 재고/미수 영향을 먼저 원복
            List<(string itcod, string house, decimal qty)> oldLines = new();
            decimal oldTotal = 0;
            if (!IsInsert)
            {
                using var oldQ = new DbQuery();
                oldQ.Add("SELECT * FROM SALE_D WHERE SALNO=@SALNO");
                oldQ.ParamByName("SALNO").AsString = eNo.Text.Trim();
                oldQ.Open();
                if (!oldQ.IsEmpty)
                {
                    oldQ.First();
                    while (!oldQ.Eof)
                    {
                        oldLines.Add((oldQ.FieldByName("ITCOD").AsString, oldQ.FieldByName("HOUSE").AsString, (decimal)oldQ.FieldByName("TRQTY").AsFloat));
                        oldTotal += (decimal)oldQ.FieldByName("TRAMT").AsFloat + (decimal)oldQ.FieldByName("JAMT1").AsFloat +
                                    (decimal)oldQ.FieldByName("JAMT2").AsFloat + (decimal)oldQ.FieldByName("JAMT3").AsFloat;
                        oldQ.Next();
                    }
                }
            }

            salNo = IsInsert && chkAuto.Checked ? GenerateAutoNo(tdate) : eNo.Text.Trim();

            using (var delD = new DbQuery())
            {
                delD.Add("DELETE FROM SALE_D WHERE SALNO=@SALNO");
                delD.ParamByName("SALNO").AsString = salNo;
                delD.ExecSQL();
            }

            int seq = 1;
            decimal newTotal = 0;
            var newLines = new List<(string itcod, string house, decimal qty)>();
            foreach (DataGridViewRow row in rgListS.Rows)
            {
                if (row.IsNewRow) continue;
                var itcod = row.Cells["ITCOD"].Value?.ToString() ?? "";
                if (itcod == "") continue;

                using var ins = new DbQuery();
                ins.Add("INSERT INTO SALE_D (SALNO,SEQNO,ITCOD,TRQTY,TRWGT,UCOST,TRAMT,JAMT1,JAMT2,JAMT3,TRPRO,HOUSE,DBIGO,USRID,MDATE)");
                ins.Add("VALUES(@SALNO,@SEQNO,@ITCOD,@TRQTY,0,@UCOST,@TRAMT,@JAMT1,@JAMT2,@JAMT3,0,@HOUSE,@DBIGO,@USRID,@MDATE)");
                ins.ParamByName("SALNO").AsString = salNo;
                ins.ParamByName("SEQNO").AsInteger = seq++;
                ins.ParamByName("ITCOD").AsString = itcod;
                var qty = ToDec(row.Cells["TRQTY"].Value);
                ins.ParamByName("TRQTY").AsCurrency = qty;
                ins.ParamByName("UCOST").AsCurrency = ToDec(row.Cells["UCOST"].Value);
                var amt = ToDec(row.Cells["TRAMT"].Value);
                ins.ParamByName("TRAMT").AsCurrency = amt;
                var j1 = ToDec(row.Cells["JAMT1"].Value);
                var j2 = ToDec(row.Cells["JAMT2"].Value);
                var j3 = ToDec(row.Cells["JAMT3"].Value);
                ins.ParamByName("JAMT1").AsCurrency = j1;
                ins.ParamByName("JAMT2").AsCurrency = j2;
                ins.ParamByName("JAMT3").AsCurrency = j3;
                var house = row.Cells["HOUSE"].Value?.ToString() ?? "";
                ins.ParamByName("HOUSE").AsString = house;
                ins.ParamByName("DBIGO").AsString = row.Cells["DBIGO"].Value?.ToString() ?? "";
                ins.ParamByName("USRID").AsString = UserContext.Current.Code;
                ins.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                ins.ExecSQL();

                newTotal += amt + j1 + j2 + j3;
                newLines.Add((itcod, house, qty));
            }

            using (var checkM = new DbQuery())
            {
                checkM.Add("SELECT * FROM SALE_M WHERE SALNO=@SALNO");
                checkM.ParamByName("SALNO").AsString = salNo;
                checkM.Open();

                using var qm = new DbQuery();
                if (checkM.IsEmpty)
                {
                    qm.Add("INSERT INTO SALE_M (SALNO,TDATE,JGUBN,CVCOD,SSAMT,LNCOD,MBIGO,PLNCD,REFNO,MDATE)");
                    qm.Add("VALUES(@SALNO,@TDATE,'2',@CVCOD,@SSAMT,@LNCOD,@MBIGO,@PLNCD,'',@MDATE)");
                }
                else
                {
                    qm.Add("UPDATE SALE_M SET TDATE=@TDATE,CVCOD=@CVCOD,SSAMT=@SSAMT,LNCOD=@LNCOD,MBIGO=@MBIGO,PLNCD=@PLNCD,MDATE=@MDATE");
                    qm.Add(" WHERE SALNO=@SALNO");
                }
                qm.ParamByName("SALNO").AsString = salNo;
                qm.ParamByName("TDATE").AsString = tdate;
                qm.ParamByName("CVCOD").AsString = eCvcod.Text.Trim();
                qm.ParamByName("SSAMT").AsCurrency = eSsamt.Value;
                qm.ParamByName("LNCOD").AsString = eLncod.Text.Trim();
                qm.ParamByName("MBIGO").AsString = eMbigo.Text.Trim();
                qm.ParamByName("PLNCD").AsString = ePlncd.Text.Trim();
                qm.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                qm.ExecSQL();
            }

            // 재고 원복(수정 시 기존분 취소) 후 신규분 반영
            foreach (var (itcod, house, qty) in oldLines)
                LedgerUpdates.ItemblUpdate(year, month, itcod, house, 0, -(int)qty, 0);
            foreach (var (itcod, house, qty) in newLines)
                LedgerUpdates.ItemblUpdate(year, month, itcod, house, 0, (int)qty, 0);

            var cvPrefix = eCvcod.Text.Length >= 4 ? eCvcod.Text[..4] : eCvcod.Text;
            LedgerUpdates.MisuUpdate(year, month, cvPrefix, 1, 0, 0, (int)(newTotal - oldTotal) * -1);

            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("출고 저장중 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 원본 PublicLib.gf_GetAutoNo('S', pDate) 그대로 이식:
    ///   sNo := pGu + Copy(FormatDateTime('YYYYMMDD', 날짜), 1, 6);  // 'S' + YYYYMM (일자는 아예 안 씀)
    /// 즉 'S'(1) + 연월 YYYYMM(6) + 순번(3) = 10자로, SALNO 컬럼 길이(10)에 정확히 맞는다.
    /// 이전에는 실수로 일자까지 포함해서(2자리 연도로 줄여도) 원본과 다른 채번 규칙이 되어 있었다.
    /// </summary>
    private static string GenerateAutoNo(string tdate)
    {
        using var q = new DbQuery();
        var dateDigits = tdate.Replace("-", ""); // YYYYMMDD
        var yyyymm = dateDigits.Length >= 6 ? dateDigits[..6] : dateDigits;
        var prefix = "S" + yyyymm;
        q.Add("SELECT MAX(SALNO) AS MAXNO FROM SALE_M WHERE SALNO LIKE @PFX");
        q.ParamByName("PFX").AsString = prefix + "%";
        q.Open();
        if (q.IsEmpty || q.FieldByName("MAXNO").IsNull) return prefix + "001";
        var max = q.FieldByName("MAXNO").AsString;
        var seq = max.Length >= prefix.Length + 3 ? PublicLib.StrToIntSafe(max[prefix.Length..]) + 1 : 1;
        return prefix + seq.ToString("000");
    }

    /// <summary>원본 up_ReadSALEF: 목록에서 선택한 출고 전표를 편집 폼에 채운다.</summary>
    public void LoadForEdit(string salNo)
    {
        using var q = new DbQuery();
        q.Add("SELECT A1.*, A2.*, B1.CVNAM, B1.OWNAM, B1.TELNO, B1.FAXNO,");
        q.Add("       B2.LNNAM, (B2.ADDR1+' '+B2.ADDR2) LNADR, B3.*");
        q.Add("  FROM SALE_M A1");
        q.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add("  LEFT OUTER JOIN CVMAST B1 ON A1.CVCOD=B1.CVCOD");
        q.Add("  LEFT OUTER JOIN REACH B2 ON A1.LNCOD=B2.LNCOD");
        q.Add("  LEFT OUTER JOIN ITEMAS B3 ON A2.ITCOD=B3.ITNBR");
        q.Add(" WHERE A1.SALNO=@SALNO");
        q.Add(" ORDER BY A2.SEQNO");
        q.ParamByName("SALNO").AsString = salNo;
        q.Open();

        rgListS.Rows.Clear();
        if (q.IsEmpty) return;

        IsInsert = false;
        eNo.Text = q.FieldByName("SALNO").AsString;
        eTdate.Text = q.FieldByName("TDATE").AsString;
        eCvcod.Text = q.FieldByName("CVCOD").AsString;
        lCvnam.Text = q.FieldByName("CVNAM").AsString;
        lTelno.Text = q.FieldByName("TELNO").AsString;
        eLncod.Text = q.FieldByName("LNCOD").AsString;
        lLnnam.Text = q.FieldByName("LNNAM").AsString;
        lLnadr.Text = q.FieldByName("LNADR").AsString;
        ePlncd.Text = q.FieldByName("PLNCD").AsString;
        eMbigo.Text = q.FieldByName("MBIGO").AsString;
        eSsamt.Value = (decimal)q.FieldByName("SSAMT").AsFloat;
        chkAuto.Checked = false;
        chkAuto.Enabled = false;
        eCvcod.Enabled = false;

        q.First();
        while (!q.Eof)
        {
            rgListS.Rows.Add(
                q.FieldByName("ITCOD").AsString, q.FieldByName("ITDSC").AsString, q.FieldByName("ISPEC").AsString,
                q.FieldByName("DANWI").AsString, q.FieldByName("HOUSE").AsString,
                q.FieldByName("TRQTY").AsFloat, q.FieldByName("UCOST").AsFloat, q.FieldByName("TRAMT").AsFloat,
                q.FieldByName("JAMT1").AsFloat, q.FieldByName("JAMT2").AsFloat, q.FieldByName("JAMT3").AsFloat,
                q.FieldByName("DBIGO").AsString);
            q.Next();
        }
        RecalcTotal();
    }
}
