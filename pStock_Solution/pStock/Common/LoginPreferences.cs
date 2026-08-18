using System.Security.Cryptography;
using System.Text;

namespace pStock.Common;

/// <summary>
/// 로그인 화면의 "접속정보 기억하기" 체크박스 상태를 로컬 파일에 저장/복원한다.
/// 아이디는 평문으로, 비밀번호는 Windows DPAPI(CurrentUser 범위)로 암호화해서 저장한다 —
/// 암호화된 값은 저장한 Windows 계정에서만 복호화할 수 있다.
/// </summary>
public static class LoginPreferences
{
    private static string FilePath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pStock", "login.cfg");

    public static (string? UserId, string? Password) LoadSaved()
    {
        try
        {
            if (!File.Exists(FilePath)) return (null, null);
            var lines = File.ReadAllLines(FilePath);
            var userId = lines.Length > 0 ? lines[0] : null;
            string? password = null;
            if (lines.Length > 1 && !string.IsNullOrEmpty(lines[1]))
            {
                var encrypted = Convert.FromBase64String(lines[1]);
                var decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                password = Encoding.UTF8.GetString(decrypted);
            }
            return (userId, password);
        }
        catch
        {
            return (null, null);
        }
    }

    public static void Save(string userId, string password)
    {
        try
        {
            var dir = Path.GetDirectoryName(FilePath)!;
            Directory.CreateDirectory(dir);
            var encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(password), null, DataProtectionScope.CurrentUser);
            File.WriteAllLines(FilePath, new[] { userId, Convert.ToBase64String(encrypted) });
        }
        catch
        {
            // 로컬 파일 저장이 실패해도(권한 문제 등) 로그인 자체는 계속 진행한다 — 치명적이지 않음.
        }
    }

    public static void Clear()
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
