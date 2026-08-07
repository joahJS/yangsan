using Microsoft.Data.SqlClient;

namespace pStock.Data;

/// <summary>
/// 원본 DB_MD.pas의 Tdm_NK(TDataModule) / DBNK(TDatabase)를 대체.
/// 델파이 BDE alias 'chang' → SQL Server 데이터베이스 'CHANG' 접속을 담당한다.
/// 원본 DFM에서 확인된 접속 정보: DatabaseName=CHANG, USER NAME=sa (SQL Server 인증)
/// 운영 환경에 배포할 때는 비밀번호를 소스에 두지 말고 앱 설정(App.config / 환경변수 /
/// Windows 자격 증명 관리자)으로 옮기는 것을 권장합니다. 아래 값은 임시 자리표시자입니다.
/// </summary>
public static class AppDb
{
    /// <summary>서버 주소. 원본은 로컬 BDE alias였으므로 실제 서버명/인스턴스로 교체 필요.</summary>
    public static string Server { get; set; } = "121.66.17.30, 16433";
    public static string Database { get; set; } = "CHANG01_TEMP";
    public static string UserId { get; set; } = "pineit";
    public static string Password { get; set; } = "pineit0401";

    public static string ConnectionString =>
        $"Server={Server};Database={Database};User Id={UserId};Password={Password};TrustServerCertificate=True;";

    private static SqlConnection? _connection;

    /// <summary>원본 DBNK.Connected := true; 에 대응.</summary>
    public static SqlConnection Connection
    {
        get
        {
            _connection ??= new SqlConnection(ConnectionString);
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
            return _connection;
        }
    }

    public static bool IsConnected => _connection is { State: System.Data.ConnectionState.Open };

    public static void Disconnect()
    {
        _connection?.Close();
        _connection?.Dispose();
        _connection = null;
    }

    /// <summary>원본 AppException에서 쓰던 트랜잭션 롤백 처리(진행 중 트랜잭션이 있을 때).</summary>
    private static SqlTransaction? _currentTransaction;

    public static SqlTransaction BeginTransaction()
    {
        _currentTransaction = Connection.BeginTransaction();
        return _currentTransaction;
    }

    public static bool InTransaction => _currentTransaction != null;

    /// <summary>현재 진행 중인 트랜잭션(없으면 null). DbQuery가 SqlCommand.Transaction에 반드시 대입해야 하는 값.</summary>
    public static SqlTransaction? CurrentTransaction => _currentTransaction;

    public static void Commit()
    {
        _currentTransaction?.Commit();
        _currentTransaction = null;
    }

    public static void Rollback()
    {
        _currentTransaction?.Rollback();
        _currentTransaction = null;
    }
}
