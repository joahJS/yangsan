using System.Data;
using Microsoft.Data.SqlClient;

namespace pStock.Data;

/// <summary>
/// 델파이 BDE TQuery의 사용 패턴(Sql.Add, ParamByName, Open, FieldByName, IsEmpty,
/// RecordCount, Next/First, ExecSQL 등)을 최대한 그대로 흉내 낸 호환 래퍼.
///
/// 원본 코드가 대부분 아래와 같은 패턴을 매우 많이 반복하므로:
///
///   vQuery := TQuery.Create(nil);
///   with vQuery do begin
///     DataBaseName := DM_NK.DBNK.DatabaseName;
///     close; sql.Clear;
///     sql.Add('SELECT * FROM ITEMBL WHERE ITNBR=:ITNBR');
///     ParamByName('ITNBR').AsString := pItnbr;
///     open;
///     if not IsEmpty then ... FieldByName('X').AsString ...
///   end;
///
/// 이 클래스를 쓰면 BA/JA/SS 화면을 옮길 때 SQL과 흐름을 그대로 유지한 채
/// 문법만 C#으로 바꾸는 식으로 빠르고 안전하게(원본 로직 왜곡 최소화) 이식할 수 있다.
/// 신규 코드를 새로 짤 때는 가능하면 일반적인 ADO.NET/Dapper 스타일을 권장한다.
/// </summary>
public sealed class DbQuery : IDisposable
{
    public List<string> Sql { get; } = new();
    private readonly SqlCommand _cmd = new();
    private DataTable? _table;
    private int _pos = -1;

    public DbQuery()
    {
        _cmd.Connection = AppDb.Connection;
    }

    public void Clear() => Sql.Clear();

    public void Add(string line) => Sql.Add(line);

    public string CommandText => string.Join(" ", Sql);

    public SqlParameterAccessor ParamByName(string name) => new(this, name);

    internal SqlCommand Command => _cmd;

    public void Close()
    {
        _table = null;
        _pos = -1;
        _cmd.Parameters.Clear();
    }

    /// <summary>SELECT 실행 (TQuery.Open 대응)</summary>
    public void Open()
    {
        _cmd.CommandText = CommandText;
        _cmd.Transaction = AppDb.CurrentTransaction;
        using var adapter = new SqlDataAdapter(_cmd);
        _table = new DataTable();
        adapter.Fill(_table);
        TrimStringColumns(_table);
        _pos = _table.Rows.Count > 0 ? 0 : -1;
    }

    /// <summary>
    /// SQL Server의 CHAR(고정폭) 컬럼은 뒤쪽이 공백으로 채워진 채로 반환되는데,
    /// 원본 BDE는 이를 자동으로 잘라줬었다. ADO.NET은 그대로 반환하므로,
    /// 콤보박스 선택값 비교(IndexOf 등)나 화면 표시가 어긋나는 것을 막기 위해
    /// 조회 직후 모든 문자열 컬럼의 뒤쪽 공백을 일괄 제거한다.
    /// </summary>
    private static void TrimStringColumns(DataTable table)
    {
        var stringCols = table.Columns.Cast<DataColumn>().Where(c => c.DataType == typeof(string)).ToList();
        if (stringCols.Count == 0) return;

        foreach (DataRow row in table.Rows)
        {
            foreach (var col in stringCols)
            {
                if (row[col] is string s)
                {
                    var trimmed = s.TrimEnd();
                    if (trimmed.Length != s.Length) row[col] = trimmed;
                }
            }
        }
    }

    /// <summary>INSERT/UPDATE/DELETE 실행 (TQuery.ExecSQL 대응). 영향받은 행 수 반환.</summary>
    public int ExecSQL()
    {
        _cmd.CommandText = CommandText;
        _cmd.Transaction = AppDb.CurrentTransaction;
        return _cmd.ExecuteNonQuery();
    }

    public bool IsEmpty => _table == null || _table.Rows.Count == 0;
    public int RecordCount => _table?.Rows.Count ?? 0;
    public bool Eof => _table == null || _pos < 0 || _pos >= _table.Rows.Count;
    public bool Bof => _pos <= 0;

    public void First() => _pos = (_table?.Rows.Count ?? 0) > 0 ? 0 : -1;
    public void Next() { if (_table != null && _pos < _table.Rows.Count - 1) _pos++; else _pos = _table?.Rows.Count ?? 0; }
    public void Prior() { if (_pos > 0) _pos--; }

    public FieldAccessor FieldByName(string name) => new(this, name);
    public FieldAccessor this[string name] => FieldByName(name);

    internal object? GetCurrentValue(string field)
    {
        if (_table == null || _pos < 0 || _pos >= _table.Rows.Count) return null;
        var v = _table.Rows[_pos][field];
        return v == DBNull.Value ? null : v;
    }

    public DataTable? Table => _table;

    public void Dispose()
    {
        _cmd.Dispose();
    }
}

/// <summary>ParamByName('X').AsString / .AsInteger / .AsDateTime 스타일 지원</summary>
public readonly struct SqlParameterAccessor
{
    private readonly DbQuery _q;
    private readonly string _name;

    internal SqlParameterAccessor(DbQuery q, string name)
    {
        _q = q; _name = name;
    }

    public string AsString
    {
        set => _q.Command.Parameters.AddWithValue("@" + _name.TrimStart('@'), (object?)value ?? DBNull.Value);
    }
    public int AsInteger
    {
        set => _q.Command.Parameters.AddWithValue("@" + _name.TrimStart('@'), value);
    }
    public decimal AsCurrency
    {
        set => _q.Command.Parameters.AddWithValue("@" + _name.TrimStart('@'), value);
    }
    public DateTime AsDateTime
    {
        set => _q.Command.Parameters.AddWithValue("@" + _name.TrimStart('@'), value);
    }
    public bool AsBoolean
    {
        set => _q.Command.Parameters.AddWithValue("@" + _name.TrimStart('@'), value);
    }
    public byte[] AsBinary
    {
        set => _q.Command.Parameters.Add("@" + _name.TrimStart('@'), System.Data.SqlDbType.VarBinary, -1).Value = (object?)value ?? DBNull.Value;
    }
}

/// <summary>FieldByName('X').AsString / .AsInteger / .AsDateTime / .AsCurrency 스타일 지원</summary>
public readonly struct FieldAccessor
{
    private readonly DbQuery _q;
    private readonly string _name;

    internal FieldAccessor(DbQuery q, string name)
    {
        _q = q; _name = name;
    }

    public string AsString => _q.GetCurrentValue(_name)?.ToString()?.TrimEnd() ?? string.Empty;
    public int AsInteger => _q.GetCurrentValue(_name) is { } v ? Convert.ToInt32(v) : 0;
    public decimal AsCurrency => _q.GetCurrentValue(_name) is { } v ? Convert.ToDecimal(v) : 0m;
    public double AsFloat => _q.GetCurrentValue(_name) is { } v ? Convert.ToDouble(v) : 0d;
    public DateTime? AsDateTime => _q.GetCurrentValue(_name) is { } v ? Convert.ToDateTime(v) : null;
    public bool IsNull => _q.GetCurrentValue(_name) is null;
}
