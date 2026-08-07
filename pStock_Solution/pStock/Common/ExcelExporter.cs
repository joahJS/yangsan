using ClosedXML.Excel;

namespace pStock.Common;

/// <summary>
/// 원본 PublicLib.pas의 gp_SendToExcel(RealGrid 전용)을 대체하는 범용 Excel 내보내기.
/// 화면에 보이는 DataGridView 그대로(현재 정렬/숨김 컬럼 제외)를 xlsx로 저장한다.
/// </summary>
public static class ExcelExporter
{
    /// <summary>
    /// DataGridView의 화면에 보이는 컬럼/행을 그대로 xlsx로 저장한다.
    /// 저장 성공 시 탐색기로 열지 물어본다.
    /// </summary>
    public static void Export(DataGridView grid, string sheetName = "Sheet1", string? suggestedFileName = null)
    {
        if (grid.Rows.Count == 0)
        {
            MessageBox.Show(AppMessages.IP0003, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Filter = "Excel 파일 (*.xlsx)|*.xlsx",
            FileName = suggestedFileName ?? (sheetName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss")),
        };
        if (sfd.ShowDialog() != DialogResult.OK) return;

        try
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(string.IsNullOrWhiteSpace(sheetName) ? "Sheet1" : sheetName);

            var visibleCols = grid.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            for (int c = 0; c < visibleCols.Count; c++)
                ws.Cell(1, c + 1).Value = visibleCols[c].HeaderText;
            ws.Row(1).Style.Font.Bold = true;

            int r = 2;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                for (int c = 0; c < visibleCols.Count; c++)
                {
                    var value = row.Cells[visibleCols[c].Index].Value;
                    var cell = ws.Cell(r, c + 1);
                    if (value == null || value == DBNull.Value) { cell.Value = ""; continue; }

                    if (value is IConvertible conv && (value is int or long or short or byte or double or float or decimal))
                        cell.Value = Convert.ToDouble(conv);
                    else if (value is DateTime dt)
                        cell.Value = dt;
                    else
                        cell.Value = value.ToString();
                }
                r++;
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(sfd.FileName);

            if (MessageBox.Show("엑셀 파일로 저장되었습니다.\r\n지금 여시겠습니까?", "확인",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("엑셀 저장중 오류가 발생했습니다.\r\n" + ex.Message, "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
