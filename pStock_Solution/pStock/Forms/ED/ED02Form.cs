using pStock.Common;
using pStock.Data;

namespace pStock.Forms.ED;

/// <summary>
/// 원본 ED02.pas / ED02.dfm (TfrmED02) 이식 — 년마감 작업.
/// 재고(ITEMBL)와 미수금(MISUF)을 다음 해로 이월한다.
/// </summary>
public class ED02Form : Form
{
    private readonly TextBox edtYear = new();
    private readonly Button btnDown = new() { Text = "◀" };
    private readonly Button btnUp = new() { Text = "▶" };
    private readonly Button btnOk = new() { Text = "마감실행(F2)" };
    private readonly Button btnCancel = new() { Text = "취소(Esc)" };

    public ED02Form()
    {
        Text = "년마감 작업";
        Width = 400;
        Height = 220;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => edtYear.Text = DateTime.Now.Year.ToString();
        KeyDown += ED02Form_KeyDown;
    }

    private void BuildLayout()
    {
        var lblYear = new Label { Text = "마감년도", Left = 20, Top = 25, AutoSize = true };
        edtYear.Left = 100; edtYear.Top = 20; edtYear.Width = 60;
        btnDown.Left = 165; btnDown.Top = 19; btnDown.Width = 30;
        btnUp.Left = 200; btnUp.Top = 19; btnUp.Width = 30;

        var lblHint = new Label
        {
            Text = "선택한 년도의 재고/미수금을 다음 해로 이월합니다.",
            Left = 20, Top = 60, AutoSize = true
        };

        btnOk.Left = 100; btnOk.Top = 130; btnOk.Width = 100;
        btnCancel.Left = 210; btnCancel.Top = 130; btnCancel.Width = 100;

        Controls.AddRange(new Control[] { lblYear, edtYear, btnDown, btnUp, lblHint, btnOk, btnCancel });

        btnDown.Click += (_, _) => edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) - 1).ToString();
        btnUp.Click += (_, _) => edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) + 1).ToString();
        btnOk.Click += (_, _) => RunClose();
        btnCancel.Click += (_, _) => Close();
    }

    private void ED02Form_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F2) btnOk.PerformClick();
        else if (e.KeyCode == Keys.Escape) Close();
    }

    private void RunClose()
    {
        if (MessageBox.Show("년이월 작업을 진행합니다.\r\n다시 한번 확인해 주십시오.", "확인",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        var year = edtYear.Text.Trim();
        try
        {
            AppDb.BeginTransaction();
            if (!YearJob(year))
            {
                AppDb.Rollback();
                MessageBox.Show("년마감시 에러발생..", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("년마감시 에러발생..\r\n" + ex.Message, "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show($"{year}년 마감작업이 완료되었습니다.", "확인",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        Close();
    }

    /// <summary>원본 fncYearJob: 재고이월 + 미수금이월.</summary>
    private static bool YearJob(string year)
    {
        var nextYear = (int.Parse(year) + 1).ToString();

        try
        {
            // 1. 재고이월
            using (var q = new DbQuery())
            {
                q.Add("DELETE FROM ITEMBL WHERE IYEAR = @YEAR");
                q.ParamByName("YEAR").AsString = nextYear;
                q.ExecSQL();
            }

            using (var q = new DbQuery())
            {
                q.Add("SELECT IYEAR,A.ITNBR,HOUSE,");
                q.Add("       (BBALQ+I1QT01+I1QT02+I1QT03+I1QT04+I1QT05+I1QT06");
                q.Add("             +I1QT07+I1QT08+I1QT09+I1QT10+I1QT11+I1QT12");
                q.Add("             -IOQT01-IOQT02-IOQT03-IOQT04-IOQT05-IOQT06");
                q.Add("             -IOQT07-IOQT08-IOQT09-IOQT10-IOQT11-IOQT12");
                q.Add("             -O1QT01-O1QT02-O1QT03-O1QT04-O1QT05-O1QT06");
                q.Add("             -O1QT07-O1QT08-O1QT09-O1QT10-O1QT11-O1QT12) JQTY");
                q.Add("  FROM ITEMBL A");
                q.Add("  LEFT OUTER JOIN ITEMAS B ON A.ITNBR=B.ITNBR");
                q.Add(" WHERE IYEAR=@YEAR");
                q.ParamByName("YEAR").AsString = year;
                q.Open();

                if (!q.IsEmpty)
                {
                    q.First();
                    while (!q.Eof)
                    {
                        var jqty = q.FieldByName("JQTY").AsInteger;
                        if (jqty != 0)
                        {
                            using var ins = new DbQuery();
                            ins.Add("INSERT INTO ITEMBL(");
                            ins.Add("     I1QT01,I1QT02,I1QT03,I1QT04,I1QT05,I1QT06,I1QT07,I1QT08,I1QT09,I1QT10,I1QT11,I1QT12,");
                            ins.Add("     IOQT01,IOQT02,IOQT03,IOQT04,IOQT05,IOQT06,IOQT07,IOQT08,IOQT09,IOQT10,IOQT11,IOQT12,");
                            ins.Add("     O1QT01,O1QT02,O1QT03,O1QT04,O1QT05,O1QT06,O1QT07,O1QT08,O1QT09,O1QT10,O1QT11,O1QT12,");
                            ins.Add("     IYEAR,ITNBR,HOUSE,BBALQ,IDATE,ODATE,MDATE)");
                            ins.Add("    VALUES(0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,");
                            ins.Add("       @YEAR,@ITNBR,@HOUSE,@BQTY,'','',@MDATE)");
                            ins.ParamByName("YEAR").AsString = nextYear;
                            ins.ParamByName("ITNBR").AsString = q.FieldByName("ITNBR").AsString;
                            ins.ParamByName("HOUSE").AsString = q.FieldByName("HOUSE").AsString;
                            ins.ParamByName("BQTY").AsInteger = jqty;
                            ins.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                            ins.ExecSQL();
                        }
                        q.Next();
                    }
                }
            }

            // 2. 미수금이월
            using (var q = new DbQuery())
            {
                q.Add("DELETE FROM MISUF WHERE MYEAR = @YEAR");
                q.ParamByName("YEAR").AsString = nextYear;
                q.ExecSQL();
            }

            using (var q = new DbQuery())
            {
                q.Add("SELECT MYEAR,CVCOD,");
                q.Add("       (BAMT+OAMT01+OAMT02+OAMT03+OAMT04+OAMT05+OAMT06");
                q.Add("            +OAMT07+OAMT08+OAMT09+OAMT10+OAMT11+OAMT12");
                q.Add("            +OVAT01+OVAT02+OVAT03+OVAT04+OVAT05+OVAT06");
                q.Add("            +OVAT07+OVAT08+OVAT09+OVAT10+OVAT11+OVAT12");
                q.Add("            -SAMT01-SAMT02-SAMT03-SAMT04-SAMT05-SAMT06");
                q.Add("            -SAMT07-SAMT08-SAMT09-SAMT10-SAMT11-SAMT12) SAMT");
                q.Add("  FROM MISUF WHERE MYEAR = @YEAR");
                q.ParamByName("YEAR").AsString = year;
                q.Open();

                if (!q.IsEmpty)
                {
                    q.First();
                    while (!q.Eof)
                    {
                        var samt = q.FieldByName("SAMT").AsInteger;
                        if (samt != 0)
                        {
                            using var ins = new DbQuery();
                            ins.Add("INSERT INTO MISUF (OAMT01,OAMT02,OAMT03,OAMT04,OAMT05,OAMT06,OAMT07,OAMT08,OAMT09,OAMT10,OAMT11,OAMT12,");
                            ins.Add("       OVAT01,OVAT02,OVAT03,OVAT04,OVAT05,OVAT06,OVAT07,OVAT08,OVAT09,OVAT10,OVAT11,OVAT12,");
                            ins.Add("       SAMT01,SAMT02,SAMT03,SAMT04,SAMT05,SAMT06,SAMT07,SAMT08,SAMT09,SAMT10,SAMT11,SAMT12,");
                            ins.Add("       MYEAR,CVCOD,BAMT,MDATE)");
                            ins.Add("      VALUES(0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,");
                            ins.Add("             0,0,0,0,0,0,0,0,0,0,0,0, @YEAR,@CVCOD,@BAMT,@MDATE)");
                            ins.ParamByName("YEAR").AsString = nextYear;
                            ins.ParamByName("CVCOD").AsString = q.FieldByName("CVCOD").AsString;
                            ins.ParamByName("BAMT").AsInteger = samt;
                            ins.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
                            ins.ExecSQL();
                        }
                        q.Next();
                    }
                }
            }

            return true;
        }
        catch { return false; }
    }
}
