namespace pStock.Common;

/// <summary>
/// 버튼/라벨에 붙이는 작은 아이콘을 이미지 파일이나 외부 아이콘 폰트/SVG 라이브러리 없이
/// GDI+로 직접 그려서 만든다. 여러 화면에서 반복되는 "신규/저장/수정/삭제/조회/닫기/문서"
/// 같은 공용 아이콘을 한 곳에서 관리한다. InitializeComponent() 밖(생성자 이후)에서만
/// 호출해야 한다 — 디자이너는 InitializeComponent() 소스만 파싱하므로 이 코드가 여기 있어도
/// 디자인 타임 파싱에는 영향이 없고, 실행 시에는 정상적으로 이미지가 만들어져 반영된다.
/// </summary>
public static class IconFactory
{
    /// <summary>
    /// 모든 draw 메서드는 18x18 논리 캔버스 기준 좌표로 그려져 있다. 실제로는 4배 해상도로
    /// 그린 뒤(안티앨리어싱이 훨씬 매끈해짐) 호출부에서 버튼의 ImageSize를 <paramref name="size"/>로
    /// 작게 지정해 다운스케일하도록 한다 — 작은 아이콘을 그대로 그릴 때 생기는 흐림/계단현상을 줄인다.
    /// </summary>
    public static Bitmap Create(Action<Graphics, Color> draw, Color color, int size = 18)
    {
        const int logicalCanvas = 18;
        const int supersample = 4;
        int physical = size * supersample;
        var bmp = new Bitmap(physical, physical);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        g.ScaleTransform((float)physical / logicalCanvas, (float)physical / logicalCanvas);
        draw(g, color);
        return bmp;
    }

    public static void New(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.6f);
        g.DrawRectangle(pen, 2, 1, 10, 14);
        g.DrawLine(pen, 4, 8, 12, 8);
        g.DrawLine(pen, 8, 4, 8, 12);
    }

    public static void Save(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.6f);
        using var brush = new SolidBrush(c);
        g.DrawRectangle(pen, 2, 2, 14, 14);
        g.FillRectangle(brush, 5, 2, 6, 5);
    }

    public static void Edit(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.7f);
        g.DrawLine(pen, 3, 15, 12, 6);
        g.DrawLine(pen, 12, 6, 15, 3);
        g.DrawLine(pen, 15, 3, 17, 5);
        g.DrawLine(pen, 17, 5, 14, 8);
        g.DrawLine(pen, 14, 8, 5, 17);
        g.DrawLine(pen, 5, 17, 2, 18);
        g.DrawLine(pen, 2, 18, 3, 15);
    }

    public static void Delete(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.6f);
        g.DrawRectangle(pen, 3, 5, 12, 11);
        g.DrawLine(pen, 1, 5, 17, 5);
        g.DrawLine(pen, 7, 2, 11, 2);
    }

    public static void Search(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.8f);
        g.DrawEllipse(pen, 2, 2, 9, 9);
        g.DrawLine(pen, 10, 10, 16, 16);
    }

    public static void Close(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.8f);
        g.DrawLine(pen, 3, 3, 15, 15);
        g.DrawLine(pen, 15, 3, 3, 15);
    }

    public static void Minimize(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.8f);
        g.DrawLine(pen, 3, 14, 15, 14);
    }

    public static void Maximize(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.5f);
        g.DrawRectangle(pen, 3, 3, 12, 12);
    }

    public static void Restore(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.4f);
        g.DrawRectangle(pen, 5, 3, 10, 10);
        g.DrawLine(pen, 3, 6, 3, 15);
        g.DrawLine(pen, 3, 15, 12, 15);
        g.DrawLine(pen, 12, 15, 12, 13);
    }

    public static void Hierarchy(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.4f);
        g.DrawRectangle(pen, 6, 1, 6, 5);
        g.DrawRectangle(pen, 0, 12, 6, 5);
        g.DrawRectangle(pen, 12, 12, 6, 5);
        g.DrawLine(pen, 9, 6, 9, 9);
        g.DrawLine(pen, 3, 9, 15, 9);
        g.DrawLine(pen, 3, 9, 3, 12);
        g.DrawLine(pen, 15, 9, 15, 12);
    }

    public static void Document(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.5f);
        g.DrawRectangle(pen, 3, 2, 12, 15);
        g.DrawLine(pen, 6, 7, 14, 7);
        g.DrawLine(pen, 6, 10, 14, 10);
        g.DrawLine(pen, 6, 13, 11, 13);
    }
}
