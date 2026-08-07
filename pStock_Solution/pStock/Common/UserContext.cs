namespace pStock.Common;

/// <summary>원본 PublicLib.pas의 TUserInfo record 이식.</summary>
public class UserInfo
{
    public string Code { get; set; } = string.Empty;      // 사번/아이디
    public string Name { get; set; } = string.Empty;      // 성명
    public string UserType { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Depart { get; set; } = string.Empty;    // 부서
    public string Duty { get; set; } = string.Empty;      // 직급
    public string Sex { get; set; } = string.Empty;
    public string Gubun { get; set; } = string.Empty;
    public DateTime? ModDat { get; set; }
    public string Remark { get; set; } = string.Empty;
}

/// <summary>
/// 원본 PublicLib.pas의 전역 변수(UserInfo, gUsrInfo, gInfoList, DBName, TempDir 등)를
/// 이식한 애플리케이션 전역 컨텍스트. 프로그램 전체에서 로그인 사용자 정보를 공유한다.
/// </summary>
public static class UserContext
{
    /// <summary>원본 var UserInfo: TUserInfo (로그인 성공 사용자 정보)</summary>
    public static UserInfo Current { get; set; } = new();

    /// <summary>원본 gUsrInfo: TStringList (로그인 화면에서 조회한 [USRID, PNAME])</summary>
    public static List<string> UsrInfo { get; } = new();

    /// <summary>
    /// 원본 gInfoList: Array[0..15] of string.
    /// 코드 검색 팝업(cm000~cm005 계열)에서 선택 결과를 담아 돌려주는 공용 버퍼.
    /// gInfoList[15] = 'Y'/'N' 으로 선택 여부(usJob)를 표시하던 관례를 그대로 유지.
    /// </summary>
    public static string[] InfoList { get; } = new string[16];

    public static void ClearInfoList()
    {
        for (int i = 0; i < InfoList.Length; i++) InfoList[i] = string.Empty;
    }

    public static string TempDir { get; set; } = Path.GetTempPath();
}
