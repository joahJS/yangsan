namespace pStock.Common;

/// <summary>
/// 로그인 화면의 "접속정보 기억하기" 체크박스 상태를 로컬 파일에 저장/복원한다.
/// 보안을 위해 사용자 코드(아이디)만 저장하고 비밀번호는 저장하지 않는다.
/// </summary>
public static class LoginPreferences
{
    private static string FilePath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pStock", "login.cfg");

    public static string? LoadSavedUserId()
    {
        try
        {
            return File.Exists(FilePath) ? File.ReadAllText(FilePath).Trim() : null;
        }
        catch
        {
            return null;
        }
    }

    public static void SaveUserId(string userId)
    {
        try
        {
            var dir = Path.GetDirectoryName(FilePath)!;
            Directory.CreateDirectory(dir);
            File.WriteAllText(FilePath, userId);
        }
        catch
        {
            // 로컬 파일 저장이 실패해도(권한 문제 등) 로그인 자체는 계속 진행한다 — 치명적이지 않음.
        }
    }

    public static void ClearSavedUserId()
    {
        try
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
        }
        catch
        {
        }
    }
}
