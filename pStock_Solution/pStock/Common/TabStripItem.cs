namespace pStock.Common;

/// <summary>
/// MainForm 상단 탭 스트립에 들어가는 탭 하나(제목 + 닫기 ×). 실제 MDI 대신 formHostPanel
/// 위에 TopLevel=false로 올려 붙인 화면들을 브라우저 탭처럼 전환/닫기 위해 쓴다.
/// </summary>
public sealed class TabStripItem
{
    private static readonly Color ActiveFill = Color.White;
    private static readonly Color InactiveFill = Color.FromArgb(229, 231, 235);
    private static readonly Color ActiveText = Color.FromArgb(31, 41, 55);
    private static readonly Color InactiveText = Color.FromArgb(107, 114, 128);

    public Panel Panel { get; }
    public event EventHandler? Activate;
    public event EventHandler? CloseRequested;

    private readonly Label _label;
    private readonly Label _closeButton;

    public TabStripItem(string caption)
    {
        var font = new Font("맑은 고딕", 9.5F);
        using (var measureBmp = new Bitmap(1, 1))
        using (var g = Graphics.FromImage(measureBmp))
        {
            var textSize = g.MeasureString(caption, font);
            int labelWidth = (int)Math.Ceiling(textSize.Width) + 4;

            _label = new Label
            {
                Text = caption,
                Font = font,
                ForeColor = InactiveText,
                AutoSize = false,
                Width = labelWidth,
                Height = 20,
                Location = new Point(12, 9),
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Margin = new Padding(0),
            };
        }

        _closeButton = new Label
        {
            Text = "×",
            Font = new Font("맑은 고딕", 10F, FontStyle.Bold),
            ForeColor = InactiveText,
            AutoSize = false,
            Width = 18,
            Height = 18,
            Location = new Point(_label.Right + 6, 8),
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            Margin = new Padding(0),
        };

        Panel = new Panel
        {
            Width = _closeButton.Right + 10,
            Height = 34,
            BackColor = InactiveFill,
            Margin = new Padding(0, 6, 4, 0),
        };
        Panel.Controls.Add(_label);
        Panel.Controls.Add(_closeButton);

        _label.Click += (_, _) => Activate?.Invoke(this, EventArgs.Empty);
        Panel.Click += (_, _) => Activate?.Invoke(this, EventArgs.Empty);
        _closeButton.Click += (_, _) => CloseRequested?.Invoke(this, EventArgs.Empty);
        _closeButton.MouseEnter += (_, _) => _closeButton.ForeColor = Color.FromArgb(220, 53, 69);
        _closeButton.MouseLeave += (_, _) => _closeButton.ForeColor = Panel.BackColor == ActiveFill ? ActiveText : InactiveText;
    }

    public void SetActive(bool active)
    {
        Panel.BackColor = active ? ActiveFill : InactiveFill;
        _label.ForeColor = active ? ActiveText : InactiveText;
        _closeButton.ForeColor = active ? ActiveText : InactiveText;
    }
}
