using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace pStock.Common;

/// <summary>
/// 원본 PublicLib.pas 이식. 화면 전반에서 재사용되는 공용 유틸리티 함수 모음.
///
/// * Grid/Excel/리포트 관련 함수(gp_SendToExcel, gp_GridToExcel, gp_RealGridSort,
///   gp_DBGridSort, gp_ResizeDbGrid, gp_InitReport 등)는 원본이 RealGrid/QuickReport 같은
///   델파이 전용 서드파티 컴포넌트에 강하게 결합돼 있어, 실제 BA/JA/SS 화면을
///   DataGridView 기반으로 재설계할 때 해당 화면 코드와 함께 다시 구현합니다.
/// * 코드조회 팝업(gp_LoadFormXXX / gp_CallNameXXX, cm000~cm005 계열)은 검색 SQL 자체는
///   재사용 가능하므로 Data/Lookups.cs 로 옮겨서 BA 단계에서 함께 정리합니다.
/// </summary>
public static class PublicLib
{
    public const string CRLF1 = "\r\n";
    public const string CRLF2 = "\r\n\r\n";
    public const char QT1 = '\'';

    public const int DataInput = 1;
    public const int DataUpdate = 2;
    public const int DataDelete = 3;

    public static readonly string[] Yoil = { "일", "월", "화", "수", "목", "금", "토" };

    // ------------------------------------------------------------------
    // 문자열 <-> 숫자
    // ------------------------------------------------------------------

    /// <summary>gf_StrToInt: 빈 문자열/오류 시 0 반환.</summary>
    public static int StrToIntSafe(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return 0;
        return int.TryParse(s.Trim(), out var v) ? v : 0;
    }

    /// <summary>StrToInteger와 동일 (원본에 중복 정의되어 있던 함수, 동작 동일하여 통합).</summary>
    public static int StrToInteger(string s) => StrToIntSafe(s);

    /// <summary>gF_StrToFloat: 콤마(,)를 제거하고 실수로 변환. 오류 시 0.</summary>
    public static double StrToFloatSafe(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;
        var cleaned = s.Replace(",", "");
        return double.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0;
    }

    /// <summary>gf_DelChar: 문자열에서 특정 문자를 모두 제거.</summary>
    public static string DelChar(string source, string ch)
    {
        return string.IsNullOrEmpty(source) ? source : source.Replace(ch, "").Trim();
    }

    /// <summary>fncStrToMoney: 콤마 포함 숫자 문자열 -> int. (원본은 오류 시 예외를 그대로 던짐)</summary>
    public static int StrToMoney(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;
        return int.Parse(s.Replace(",", ""));
    }

    /// <summary>fncStrToCurr: 콤마 포함 숫자 문자열 -> decimal(Currency). 오류 시 0.</summary>
    public static decimal StrToCurrSafe(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0m;
        var cleaned = s.Replace(",", "");
        return decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : 0m;
    }

    /// <summary>fncMoneyToStr: 정수 -> 3자리 콤마 문자열.</summary>
    public static string MoneyToStr(long value) => value.ToString("#,0", CultureInfo.InvariantCulture);

    /// <summary>fncCurrToStr: Currency -> 3자리 콤마 문자열(소수부 유지).</summary>
    public static string CurrToStr(decimal value)
    {
        var s = value.ToString("#,0.####################", CultureInfo.InvariantCulture);
        return string.IsNullOrWhiteSpace(s) ? "0" : s;
    }

    /// <summary>
    /// fncfloatToStr(I, J, K): 정수부 J자리, 소수부 K자리로 맞춘 문자열('정수부.소수부').
    /// 원본은 정수부가 J자리보다 짧으면 앞을 0으로 채우고, 길면 뒤 J자리만 취한다.
    /// </summary>
    public static string FloatToStrFixed(double value, int intDigits, int fracDigits)
    {
        var a = value.ToString("F" + fracDigits, CultureInfo.InvariantCulture);
        var dotIdx = a.IndexOf('.');
        string intPart = dotIdx >= 0 ? a[..dotIdx] : a;
        string fracPart = dotIdx >= 0 ? a[(dotIdx + 1)..] : new string('0', fracDigits);

        intPart = intPart.Length > intDigits
            ? intPart[^intDigits..]
            : intPart.PadLeft(intDigits, '0');

        fracPart = fracPart.Length > fracDigits
            ? fracPart[..fracDigits]
            : fracPart.PadRight(fracDigits, '0');

        return $"{intPart}.{fracPart}";
    }

    /// <summary>fncSetStr: 문자열을 Cnt 길이만큼 공백으로 채움 (고정폭 출력용).</summary>
    public static string SetStr(string s, int cnt)
    {
        s ??= string.Empty;
        return s.Trim().Length < cnt ? s.PadRight(cnt) : s[..Math.Min(s.Length, 5)];
    }

    /// <summary>fncStrToDate: 문자열에서 '-'와 '_' 제거.</summary>
    public static string StripDateSeparators(string s)
    {
        return string.IsNullOrEmpty(s) ? s : s.Replace("-", "").Replace("_", "").Trim();
    }

    // ------------------------------------------------------------------
    // 날짜
    // ------------------------------------------------------------------

    /// <summary>fncGetDate: 날짜의 년/월/일을 각각 문자열로 반환 (Inx: 1=년,2=월,3=일).</summary>
    public static string GetDatePart(DateTime date, int inx) => inx switch
    {
        1 => date.Year.ToString(),
        2 => date.Month.ToString(),
        3 => date.Day.ToString(),
        _ => string.Empty,
    };

    /// <summary>gf_IsDateCheck: 날짜 형식 유효성 검사 (실패 시 경고창 표시).</summary>
    public static bool IsValidDate(string s)
    {
        if (DateTime.TryParse(s, out _)) return true;
        MessageBox.Show("날짜 오류..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    /// <summary>fncTermCount: 반월(15일 기준) 단위로 두 날짜 사이의 기수(期數)를 계산.</summary>
    public static int TermCount(DateTime date1, DateTime date2)
    {
        int yearCnt = (date2.Year - date1.Year) * 12;
        int cnt = (date1.Day < 16 && date2.Day > 15) ? 2 : 1;
        cnt += (date2.Month + yearCnt - date1.Month) * 2;
        if (date1.Day > 16 && date2.Day < 15) cnt -= 1;
        return cnt;
    }

    // ------------------------------------------------------------------
    // 확인창 / 메시지
    // ------------------------------------------------------------------

    /// <summary>fncDelYN / gf_DelYN: "(메시지)\n\n위의 자료를 삭제할까요?" 확인창.</summary>
    public static bool ConfirmDelete(string message = "")
    {
        var msg = (string.IsNullOrWhiteSpace(message) ? "" : message)
                  + CRLF2 + "위의 자료를 삭제할까요?";
        return MessageBox.Show(msg, "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
               == DialogResult.Yes;
    }

    /// <summary>gf_CloseFormYN: "(메시지)\n\n프로그램을 종료하시겠습니까?" 확인창.</summary>
    public static bool ConfirmClose(string message = "")
    {
        var msg = (string.IsNullOrWhiteSpace(message) ? "" : message)
                  + CRLF2 + "프로그램을 종료하시겠습니까?";
        return MessageBox.Show(msg, "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
               == DialogResult.Yes;
    }

    // ------------------------------------------------------------------
    // 입력 컨트롤 색상 토글 (gp_EnterObj / gp_ExitObj / gp_CodeToggle)
    // 원본: 포커스를 받으면 연한 청록색($00BCDEDE), 벗어나면 기본 배경색.
    // ------------------------------------------------------------------

    public static readonly Color FocusColor = Color.FromArgb(0xDE, 0xDE, 0xBC); // BGR($00BCDEDE) -> RGB

    public static void OnEnterHighlight(Control c) => c.BackColor = FocusColor;
    public static void OnExitHighlight(Control c) => c.BackColor = SystemColors.Window;

    /// <summary>gp_CodeToggle: 컨트롤 활성/비활성과 동시에 색상도 맞춰준다.</summary>
    public static void CodeToggle(Control c, bool enabled)
    {
        c.Enabled = enabled;
        if (enabled) OnExitHighlight(c); else OnEnterHighlight(c);
    }

    // ------------------------------------------------------------------
    // 숫자만 입력 (prcOnly_Number)
    // ------------------------------------------------------------------

    /// <summary>TextBox의 KeyPress 핸들러에서 호출: 숫자/백스페이스/엔터/소수점/마이너스만 허용.</summary>
    public static void OnlyNumberKeyPress(KeyPressEventArgs e)
    {
        if (!(char.IsDigit(e.KeyChar) || e.KeyChar is (char)8 or (char)13 or '.' or '-'))
            e.Handled = true;
    }

    // ------------------------------------------------------------------
    // NumericUpDown 키보드 입력 편의 처리
    // ------------------------------------------------------------------

    /// <summary>
    /// NumericUpDown에 포커스가 들어오면(마우스 클릭/Tab 이동) 기존 값을 전체 선택 상태로
    /// 만들어, 사용자가 "0"을 따로 지우지 않고 바로 숫자를 입력해 덮어쓸 수 있게 한다.
    /// (별도 처리 없이도 키보드로 직접 입력하는 것 자체는 항상 가능하지만, 매번 값을 지우고
    /// 입력해야 해서 불편하다는 요청에 따라 추가.)
    /// </summary>
    public static void MakeTypingFriendly(NumericUpDown n)
    {
        n.Enter += (_, _) => n.BeginInvoke((MethodInvoker)(() => n.Select(0, n.Text.Length)));
    }
}
