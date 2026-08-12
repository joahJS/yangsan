namespace pStock.Forms;

/// <summary>
/// 아직 C#으로 변환되지 않은 업무 화면(BA/JA/SS/ED 등)을 위한 임시 MDI 자식 폼.
/// FormRegistry에 실제 폼이 등록되기 전까지 이 폼이 대신 열린다.
/// </summary>
public partial class PlaceholderForm : Form
{
    public PlaceholderForm(string caption)
    {
        InitializeComponent();

        Text = caption;
        lblMessage.Text = $"'{caption}' 화면은 아직 C#으로 변환되지 않았습니다.\r\n다음 단계에서 이어서 작업합니다.";
    }
}
