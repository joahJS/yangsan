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

    // Group: "STOCK"=재고관리, "FLOW"=재고수불, "SALES"=매출관리, "CLOSE"=마감관리,
    //        "BASE"=기초관리, "SYS"=시스템관리 (사이드바 카테고리와 1:1로 대응)
    public static readonly List<Entry> Entries = new()
    {
        // ---- STOCK: 재고관리 --------------------------------------------------------------
        new(51, "재고관리",          "STOCK", false, () => new JA.JA01Form()),
        new(52, "재고관리-년간",     "STOCK", false, () => new JA.JA02Form()),
        new(53, "재고관리-품목",     "STOCK", false, () => new JA.JA04Form()),
        new(40, "운송현황",          "STOCK", false, () => new JA.JA05Form()),
        new(62, "일자별 집계작업",   "STOCK", false, () => new JA.JA03Form()),

        // ---- FLOW: 재고수불 (입출고/보관) --------------------------------------------------
        new(21, "입고 관리",         "FLOW", false, () => new SS.SS21Form()),
        new(31, "출고 관리",         "FLOW", false, () => new SS.SS020Form()),
        new(22, "보관료 관리",       "FLOW", false, () => new SS.SS23Form()),

        // ---- SALES: 매출관리 --------------------------------------------------------------
        new(32, "수금 관리",         "SALES", false, () => new SS.SS32Form()),
        new(61, "계산서 관리",       "SALES", false, () => new SS.SS33Form()),
        new(36, "미수금 조회",       "SALES", false, () => new SS.SS34Form()),
        new(38, "거래처원장 조회",   "SALES", false, () => new SS.SS35Form()),
        new(39, "품목원장 조회",     "SALES", false, () => new SS.SS36Form()),

        // ---- CLOSE: 마감관리 --------------------------------------------------------------
        new(71, "월마감 작업",       "CLOSE", true,  () => new ED.ED01Form()),
        new(72, "년마감 작업",       "CLOSE", true,  () => new ED.ED02Form()),
        new(73, "기초잔액 보수",     "CLOSE", false, () => new ED.ED03Form()),

        // ---- BASE: 기초관리 (공통코드/거래처/품목 등 마스터 데이터) -------------------------
        new(11, "공통코드 마스터",   "BASE", false, () => new BA.BA02Form()),
        new(12, "거래처 마스터",     "BASE", false, () => new BA.BA01Form()),
        new(13, "품목,단가 마스터",  "BASE", false, () => new BA.BA03Form()),
        new(14, "착지처 마스터",     "BASE", false, () => new BA.BA04Form()),
        new(15, "사업장 마스터",     "BASE", true,  () => new BA.BA00Form()),

        // ---- SYS: 시스템관리 --------------------------------------------------------------
        new(17, "버전관리",          "SYS", false, () => new BA.VersionListForm()),
        new(16, "패스워드 변경",     "SYS", true,  () => new BA.BA00EForm()),
    };

    public static Entry? Find(int tag) => Entries.FirstOrDefault(e => e.Tag == tag);
}
