using System.Windows.Forms;

namespace pStock.Forms.Common;

/// <summary>
/// 입력 다이얼로그(출고 등록, 입고 등록 등)에서 공용으로 쓰는 가로형 격자 레이아웃 헬퍼.
///
/// 기존에는 폼마다 y 좌표를 직접 계산하며 세로로만 쌓다 보니(AddRow 패턴), 화면이 길어져
/// 세로 스크롤이 생기고, 라벨 길이가 제각각이라 입력란 위치가 들쭉날쭉했다.
/// 이 헬퍼는 "슬롯" 단위(라벨+입력란+여백)로 필드를 배치하고, 한 줄에 slotsPerRow개가
/// 차면 자동으로 다음 줄로 넘어간다. 같은 화면 안에서는 라벨 폭이 고정이라 항상 같은
/// 위치에서 입력란이 시작하고, 여러 필드를 한 줄에 나열해 세로 길이를 줄일 수 있다.
/// </summary>
public sealed class GridLayout
{
    private readonly Control _parent;
    private readonly int _startX;
    private readonly int _startY;
    private readonly int _slotWidth;
    private readonly int _labelWidth;
    private readonly int _rowHeight;
    private readonly int _slotsPerRow;
    private int _col;
    private int _row;

    /// <param name="parent">컨트롤을 추가할 부모(Panel 등)</param>
    /// <param name="startX">시작 X좌표</param>
    /// <param name="startY">시작 Y좌표</param>
    /// <param name="slotWidth">슬롯 하나의 전체 폭(라벨+입력란+여백 포함)</param>
    /// <param name="labelWidth">슬롯 안에서 라벨이 차지하는 폭(이 폭만큼 뒤에서 입력란 시작)</param>
    /// <param name="rowHeight">줄 간격</param>
    /// <param name="slotsPerRow">한 줄에 들어갈 최대 슬롯 수</param>
    public GridLayout(Control parent, int startX, int startY, int slotWidth, int labelWidth, int rowHeight, int slotsPerRow)
    {
        _parent = parent; _startX = startX; _startY = startY;
        _slotWidth = slotWidth; _labelWidth = labelWidth; _rowHeight = rowHeight; _slotsPerRow = slotsPerRow;
    }

    /// <summary>라벨 + 입력 컨트롤을 한 슬롯(또는 span개 슬롯)에 배치한다.</summary>
    /// <param name="span">이 필드가 차지할 슬롯 수(넓은 입력란이 필요할 때 2 이상 지정)</param>
    /// <param name="editWidth">입력란 폭을 직접 지정(생략하면 슬롯 폭에서 라벨 폭을 뺀 값 사용)</param>
    public Control Add(string caption, Control edit, int span = 1, int? editWidth = null)
    {
        if (_col + span > _slotsPerRow) NewRow();
        int x = _startX + _col * _slotWidth;
        int y = _startY + _row * _rowHeight;

        var lbl = new Label { Text = caption, Left = x, Top = y + 3, AutoSize = true };
        _parent.Controls.Add(lbl);

        edit.Left = x + _labelWidth;
        edit.Top = y;
        edit.Width = editWidth ?? (span * _slotWidth - _labelWidth - 12);
        _parent.Controls.Add(edit);

        _col += span;
        return edit;
    }

    /// <summary>체크박스처럼 자체 Text를 표시하는 컨트롤을 라벨 없이 그대로 배치한다.</summary>
    public Control AddRaw(Control control, int span = 1, int? width = null)
    {
        if (_col + span > _slotsPerRow) NewRow();
        int x = _startX + _col * _slotWidth;
        int y = _startY + _row * _rowHeight;
        control.Left = x;
        control.Top = y + 2;
        if (width.HasValue) control.Width = width.Value;
        _parent.Controls.Add(control);
        _col += span;
        return control;
    }

    /// <summary>강제로 다음 줄로 넘어간다(현재 줄에 아무것도 없으면 무시).</summary>
    public void NewRow()
    {
        if (_col == 0) return;
        _col = 0;
        _row++;
    }

    /// <summary>지금까지 배치된 가장 아래쪽 Y좌표 + 여백. 패널/폼 높이 계산에 사용.</summary>
    public int Bottom(int extra = 10) => _startY + (_row + (_col > 0 ? 1 : 0)) * _rowHeight + extra;
}
