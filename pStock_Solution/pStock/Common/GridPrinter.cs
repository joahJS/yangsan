using System.Drawing.Printing;

namespace pStock.Common;

/// <summary>
/// 원본 QuickReport 기반 리스트 인쇄(SS21_1P, SS23_1P, SS32_1P, SS34_1P, SS35_1P,
/// SS36_1P, BA01_1P, BA03P 등)를 대체하는 범용 표 인쇄 도구.
/// DataGridView에 보이는 컬럼/행을 그대로 페이지 넘김하며 인쇄한다(가로 인쇄 기본).
/// </summary>
public static class GridPrinter
{
    public static void Print(DataGridView grid, string reportTitle)
    {
        if (grid.Rows.Count == 0)
        {
            MessageBox.Show(AppMessages.WU0010, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var cols = grid.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        var rows = grid.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();

        using var doc = new PrintDocument();
        doc.DefaultPageSettings.Landscape = true;
        int rowIndex = 0;
        int pageNo = 0;

        doc.PrintPage += (_, e) =>
        {
            pageNo++;
            var g = e.Graphics!;
            var bounds = e.MarginBounds;
            var titleFont = new Font("맑은 고딕", 14, FontStyle.Bold);
            var headerFont = new Font("맑은 고딕", 9, FontStyle.Bold);
            var cellFont = new Font("맑은 고딕", 9);

            float y = bounds.Top;
            g.DrawString(reportTitle, titleFont, Brushes.Black, bounds.Left, y);
            y += titleFont.GetHeight(g) + 4;
            g.DrawString(DateTime.Now.ToString("yyyy-MM-dd") + $"   {pageNo} 페이지", cellFont, Brushes.Gray, bounds.Left, y);
            y += cellFont.GetHeight(g) + 8;

            float colWidth = bounds.Width / Math.Max(cols.Count, 1);
            float rowHeight = cellFont.GetHeight(g) + 6;

            // 헤더
            float x = bounds.Left;
            foreach (var c in cols)
            {
                g.DrawRectangle(Pens.Black, x, y, colWidth, rowHeight);
                g.DrawString(c.HeaderText, headerFont, Brushes.Black, x + 2, y + 2);
                x += colWidth;
            }
            y += rowHeight;

            while (rowIndex < rows.Count)
            {
                if (y + rowHeight > bounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                x = bounds.Left;
                var row = rows[rowIndex];
                foreach (var c in cols)
                {
                    var text = row.Cells[c.Index].Value?.ToString() ?? "";
                    g.DrawRectangle(Pens.LightGray, x, y, colWidth, rowHeight);
                    g.DrawString(text, cellFont, Brushes.Black, x + 2, y + 2);
                    x += colWidth;
                }
                y += rowHeight;
                rowIndex++;
            }

            e.HasMorePages = false;
        };

        using var preview = new PrintPreviewDialog
        {
            Document = doc,
            Width = 1000,
            Height = 700,
            StartPosition = FormStartPosition.CenterScreen,
        };
        preview.ShowDialog();
    }
}
