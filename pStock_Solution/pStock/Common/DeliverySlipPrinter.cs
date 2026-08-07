using System.Drawing.Printing;
using pStock.Data;

namespace pStock.Common;

/// <summary>
/// 원본 ss020u11.pas (Tss020F11, QuickReport) 이식 — 출고 확인증 인쇄.
/// 원본은 한 페이지에 동일 내용을 2부(위/아래) 인쇄해 절취선으로 나눠 쓰는 이중 서식이었으나,
/// 여기서는 같은 내용을 한 페이지에 위아래로 2회 반복 출력해 원본과 동일한 용도(회사 보관용 +
/// 거래처 전달용)로 쓸 수 있게 했다. 8줄 단위로 자동 페이지 넘김하는 것도 원본과 동일하다.
/// </summary>
public static class DeliverySlipPrinter
{
    private const int LinesPerPage = 8;

    public static void Print(string salNo)
    {
        using var q = new DbQuery();
        q.Add("SELECT A1.*, A2.*, B1.CVNAM, B1.OWNAM, B1.TELNO,");
        q.Add("       B3.LNNAM, B3.LNTEL, (B3.ADDR1+' '+B3.ADDR2) LNADR,");
        q.Add("       B2.ITDSC, B2.ISPEC, B2.ITWGT");
        q.Add("  FROM SALE_M A1");
        q.Add("  LEFT OUTER JOIN SALE_D A2 ON A1.SALNO=A2.SALNO");
        q.Add("  LEFT OUTER JOIN CVMAST B1 ON A1.CVCOD=B1.CVCOD");
        q.Add("  LEFT OUTER JOIN REACH B3 ON A1.LNCOD=B3.LNCOD");
        q.Add("  LEFT OUTER JOIN ITEMAS B2 ON A2.ITCOD=B2.ITNBR");
        q.Add(" WHERE A1.SALNO=@SALNO");
        q.Add(" ORDER BY A2.SEQNO");
        q.ParamByName("SALNO").AsString = salNo;
        q.Open();

        if (q.IsEmpty)
        {
            MessageBox.Show(AppMessages.IP0003, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var header = new
        {
            Cvnam = q.FieldByName("CVNAM").AsString,
            Lnnam = q.FieldByName("LNNAM").AsString,
            Lnadr = q.FieldByName("LNADR").AsString,
            Mbigo = q.FieldByName("MBIGO").AsString,
            Tdate = q.FieldByName("TDATE").AsString,
        };

        var lines = new List<(string seq, string item, string qty, string wgt, string trwgt, string bigo)>();
        int seq = 1;
        double sumQty = 0, sumWgt = 0;
        q.First();
        while (!q.Eof)
        {
            var qty = q.FieldByName("TRQTY").AsFloat;
            var trwgt = q.FieldByName("TRWGT").AsFloat;
            lines.Add((
                seq.ToString(),
                $"{q.FieldByName("ITDSC").AsString} {q.FieldByName("ISPEC").AsString}".Trim(),
                PublicLib.MoneyToStr((long)qty),
                PublicLib.MoneyToStr((long)q.FieldByName("ITWGT").AsFloat),
                PublicLib.MoneyToStr((long)trwgt),
                q.FieldByName("DBIGO").AsString));
            sumQty += qty;
            sumWgt += trwgt;
            seq++;
            q.Next();
        }

        int totalPages = Math.Max(1, (int)Math.Ceiling(lines.Count / (double)LinesPerPage));
        int page = 0;

        using var doc = new PrintDocument();
        doc.DefaultPageSettings.Landscape = false;

        doc.PrintPage += (_, e) =>
        {
            var g = e.Graphics!;
            var bounds = e.MarginBounds;
            float halfHeight = bounds.Height / 2f;

            DrawCopy(g, new RectangleF(bounds.Left, bounds.Top, bounds.Width, halfHeight - 10),
                header.Cvnam, header.Lnnam, header.Lnadr, header.Mbigo, header.Tdate,
                lines, page, totalPages, sumQty, sumWgt, "보관용");
            DrawCopy(g, new RectangleF(bounds.Left, bounds.Top + halfHeight + 10, bounds.Width, halfHeight - 10),
                header.Cvnam, header.Lnnam, header.Lnadr, header.Mbigo, header.Tdate,
                lines, page, totalPages, sumQty, sumWgt, "거래처용");

            page++;
            e.HasMorePages = page < totalPages;
        };

        using var preview = new PrintPreviewDialog
        {
            Document = doc,
            Width = 900,
            Height = 750,
            StartPosition = FormStartPosition.CenterScreen,
        };
        preview.ShowDialog();
    }

    private static void DrawCopy(Graphics g, RectangleF area, string cvnam, string lnnam, string lnadr,
        string mbigo, string tdate, List<(string seq, string item, string qty, string wgt, string trwgt, string bigo)> lines,
        int page, int totalPages, double sumQty, double sumWgt, string copyLabel)
    {
        var titleFont = new Font("맑은 고딕", 16, FontStyle.Bold);
        var labelFont = new Font("맑은 고딕", 9);
        var headerFont = new Font("맑은 고딕", 9, FontStyle.Bold);

        float y = area.Top;
        g.DrawString("출 고 확 인 증", titleFont, Brushes.Black, area.Left + area.Width / 2 - 80, y);
        g.DrawString($"({copyLabel})", labelFont, Brushes.Gray, area.Right - 60, y + 4);
        y += 30;

        var yy = tdate.Length >= 4 ? tdate[..4] : "";
        var mm = tdate.Length >= 7 ? tdate.Substring(5, 2) : "";
        var dd = tdate.Length >= 10 ? tdate.Substring(8, 2) : "";
        g.DrawString($"일자: {yy}년 {mm}월 {dd}일    수신: {cvnam}    착지처: {lnnam} ({lnadr})",
            headerFont, Brushes.Black, area.Left, y);
        y += 16;
        if (!string.IsNullOrWhiteSpace(mbigo))
        {
            g.DrawString("비고: " + mbigo, labelFont, Brushes.Black, area.Left, y);
            y += 16;
        }

        // 테이블 헤더
        float[] colW = { 40, area.Width - 40 - 80 - 80 - 80 - 150, 80, 80, 80, 150 };
        string[] headers = { "순번", "품명", "수량", "단위중량", "총중량", "비고" };
        float x = area.Left;
        float rowH = 18;
        for (int c = 0; c < headers.Length; c++)
        {
            g.DrawRectangle(Pens.Black, x, y, colW[c], rowH);
            g.DrawString(headers[c], headerFont, Brushes.Black, x + 2, y + 1);
            x += colW[c];
        }
        y += rowH;

        int start = page * LinesPerPage;
        int end = Math.Min(start + LinesPerPage, lines.Count);
        for (int i = start; i < start + LinesPerPage; i++)
        {
            x = area.Left;
            var vals = i < end
                ? new[] { lines[i].seq, lines[i].item, lines[i].qty, lines[i].wgt, lines[i].trwgt, lines[i].bigo }
                : new[] { "", "", "", "", "", "" };
            for (int c = 0; c < vals.Length; c++)
            {
                g.DrawRectangle(Pens.LightGray, x, y, colW[c], rowH);
                g.DrawString(vals[c], labelFont, Brushes.Black, x + 2, y + 1);
                x += colW[c];
            }
            y += rowH;
        }

        g.DrawString($"합계 수량: {PublicLib.MoneyToStr((long)sumQty)}   총중량: {PublicLib.MoneyToStr((long)sumWgt)}   " +
            $"Page {page + 1}/{totalPages}", labelFont, Brushes.Black, area.Left, y + 4);
    }
}
