namespace pStock.Common;

/// <summary>
/// 여러 화면에서 공유하는 SQL 조각 모음. 원본 Delphi 코드에서도 vSql(TStringList)로
/// 화면별 FormCreate에 복사해두고 재사용하던 패턴을 그대로 옮긴 것.
/// </summary>
public static class SqlFragments
{
    /// <summary>
    /// ITEMBL(월별 수불) + ITEMAS + CVMAST 조인 기본 SELECT.
    /// 원본 JA01.dfm / JA02.dfm의 qryList.SQL.Strings에서 추출.
    /// </summary>
    public const string ItemblBaseSql =
        "SELECT I1QT01,I1QT02,I1QT03,I1QT04,I1QT05,I1QT06,I1QT07,I1QT08,I1QT09,I1QT10,I1QT11,I1QT12," +
        "       O1QT01,O1QT02,O1QT03,O1QT04,O1QT05,O1QT06,O1QT07,O1QT08,O1QT09,O1QT10,O1QT11,O1QT12," +
        "       IOQT01,IOQT02,IOQT03,IOQT04,IOQT05,IOQT06,IOQT07,IOQT08,IOQT09,IOQT10,IOQT11,IOQT12," +
        "       IYEAR,A.ITNBR,HOUSE,BBALQ,ITDSC,ISPEC,DANWI,ICOST,BCOST,OCOST,(ITDSC+' '+ISPEC) CODNAM," +
        "       CVCOD,CVNAM FROM ITEMBL A" +
        "  LEFT OUTER JOIN ITEMAS B ON A.ITNBR=B.ITNBR" +
        "  LEFT OUTER JOIN CVMAST C ON SUBSTRING(A.ITNBR,1,4)=C.CVCOD" +
        " WHERE IYEAR = @YEAR";
}
