using pStock.Data;

namespace pStock.Common;

/// <summary>
/// REFFPF(공통코드) 테이블의 그룹(RCDTP)별 목록을 세션당 1회만 조회해 캐싱한다.
/// 창고/저장위치/단위/구분 등 콤보박스를 채우는 화면(JA01/JA02/BA03/SS21A/SS32A 등)이
/// 열릴 때마다 매번 REFFPF를 다시 조회하던 것을, 최초 1회 조회 후 메모리에 유지하도록 바꿔
/// 화면 전환 속도를 개선한다.
///
/// BA02Form(공통코드 마스터)에서 REFFPF 자료를 저장/삭제하면 반드시 <see cref="Invalidate"/>를
/// 호출해 캐시를 비워야 한다(그렇지 않으면 방금 추가/수정/삭제한 공통코드가 다른 화면의
/// 콤보박스에 반영되지 않는다).
/// </summary>
public static class ReffpfCache
{
    private static readonly Dictionary<string, List<(string Refno, string Retxf)>> _cache = new();

    /// <summary>
    /// 지정한 RCDTP(그룹) 목록을 반환한다. 최초 호출시에만 DB를 조회하고, 이후에는 캐시된 값을 반환한다.
    /// </summary>
    public static List<(string Refno, string Retxf)> Get(string rcdtp)
    {
        if (_cache.TryGetValue(rcdtp, out var cached)) return cached;

        var list = new List<(string, string)>();
        using (var q = new DbQuery())
        {
            q.Add("SELECT * FROM REFFPF WHERE RCDTP=@RCDTP AND REFNO<>'' ORDER BY REFNO");
            q.ParamByName("RCDTP").AsString = rcdtp;
            q.Open();
            if (!q.IsEmpty)
            {
                q.First();
                while (!q.Eof)
                {
                    list.Add((q.FieldByName("REFNO").AsString, q.FieldByName("RETXF").AsString));
                    q.Next();
                }
            }
        }

        _cache[rcdtp] = list;
        return list;
    }

    /// <summary>REFFPF(공통코드) 자료가 변경되었을 때(BA02Form 저장/삭제 시) 호출해 캐시를 전부 비운다.</summary>
    public static void Invalidate() => _cache.Clear();
}
