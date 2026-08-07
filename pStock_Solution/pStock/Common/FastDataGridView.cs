using System.Reflection;

namespace pStock.Common;

/// <summary>
/// 표준 DataGridView는 기본적으로 더블버퍼링이 꺼져있고, DoubleBuffered 프로퍼티가
/// Control 기반클래스에 protected로 선언되어 있어 외부(폼 코드)에서 켤 방법이 없다.
/// 그 결과 행이 많은 그리드를 채울 때 위에서 아래로 한 행씩 그려지는 것처럼 보이는
/// 플리커(깜빡임) 현상이 발생한다. 이 서브클래스는 생성자에서 리플렉션으로 그 protected
/// 프로퍼티를 켜서 문제를 해결한다.
/// 사용법은 기존 DataGridView와 완전히 동일하며, 선언부의 타입만
/// DataGridView -> FastDataGridView 로 바꾸면 된다.
/// </summary>
public class FastDataGridView : DataGridView
{
    public FastDataGridView()
    {
        typeof(Control)
            .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(this, true, null);

        // 셀 하나만 선택 색상이 적용되던 기본 동작 대신, 셀을 클릭해도 그 행 전체가
        // 선택 색상으로 표시되도록 함. 각 화면에서 개별 설정할 필요 없이 전체 그리드에 일괄 적용됨.
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    }
}
