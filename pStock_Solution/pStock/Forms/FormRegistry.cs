namespace pStock.Forms;

/// <summary>
/// 원본 Main.pas의 prcFormShow(Inx: Integer) 케이스문을 이식.
/// 메뉴/버튼 Tag 값으로 어떤 화면을 열지 결정한다.
/// 아직 변환되지 않은 화면은 Factory가 PlaceholderForm을 반환한다.
/// BA/JA/SS 화면을 실제로 변환할 때마다 아래 Factory만 교체하면 된다.
/// </summary>
public static class FormRegistry
{
    public record Entry(int Tag, string Caption, string Group, bool Modal, Func<Form> Factory);

    private static Form Placeholder(string caption) => new PlaceholderForm(caption);

    // Group: "BA" = 자료관리(구매/거래처), "JA" = 재고관리, "SS" = 매출/판매관리, "ED" = 마감작업
    public static readonly List<Entry> Entries = new()
    {
        // ---- BA: 자료관리 (구매/거래처) ------------------------------------------------
        new(11, "공통코드 마스터",   "BA", false, () => new BA.BA02Form()),
        new(12, "거래처 마스터",     "BA", false, () => new BA.BA01Form()),
        new(13, "품목,단가 마스터",  "BA", false, () => new BA.BA03Form()),
        new(14, "착지처 마스터",     "BA", false, () => new BA.BA04Form()),
        new(15, "사업장 마스터",     "BA", true,  () => new BA.BA00Form()),
        new(17, "버전관리",          "BA", false, () => new BA.VersionListForm()),
        new(16, "패스워드 변경",     "BA", true,  () => new BA.BA00EForm()),

        // ---- JA: 재고 관리 --------------------------------------------------------------
        new(51, "재고관리",          "JA", false, () => new JA.JA01Form()),
        new(52, "재고관리-년간",     "JA", false, () => new JA.JA02Form()),
        new(53, "재고관리-품목",     "JA", false, () => new JA.JA04Form()),
        new(40, "운송현황",          "JA", false, () => new JA.JA05Form()),
        new(62, "일자별 집계작업",   "JA", false, () => new JA.JA03Form()),

        // ---- SS: 매출/판매 관리 ----------------------------------------------------------
        new(21, "입고 관리",         "SS", false, () => new SS.SS21Form()),
        new(22, "보관료 관리",       "SS", false, () => new SS.SS23Form()),
        new(31, "출고 관리",         "SS", false, () => new SS.SS020Form()),
        new(32, "수금 관리",         "SS", false, () => new SS.SS32Form()),
        new(36, "미수금 조회",       "SS", false, () => new SS.SS34Form()),
        new(38, "거래처원장 조회",   "SS", false, () => new SS.SS35Form()),
        new(39, "품목원장 조회",     "SS", false, () => new SS.SS36Form()),
        new(61, "계산서 관리",       "SS", false, () => new SS.SS33Form()),

        // ---- ED: 마감 작업 ---------------------------------------------------------------
        new(71, "월마감 작업",       "ED", true,  () => new ED.ED01Form()),
        new(72, "년마감 작업",       "ED", true,  () => new ED.ED02Form()),
        new(73, "기초잔액 보수",     "ED", false, () => new ED.ED03Form()),
    };

    public static Entry? Find(int tag) => Entries.FirstOrDefault(e => e.Tag == tag);
}
