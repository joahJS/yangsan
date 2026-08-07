namespace pStock.Data;

/// <summary>
/// 원본 PublicLib.pas의 fncItemblUpdate / fncMisuUpdate 이식(간략화 버전).
/// 원본은 대상 레코드가 없으면 신규 생성(fncItemblAdd/fncMisufAdd)까지 하지만,
/// 여기서는 마감 작업처럼 이미 레코드가 존재한다고 가정하는 시나리오 위주로 갱신만 수행한다.
/// 레코드가 없는 경우는 안전하게 true를 반환(원본의 add-then-update 흐름 생략)한다.
/// </summary>
public static class LedgerUpdates
{
    /// <summary>
    /// 원본 fncItemblUpdate(pYear,pMonth,pItnbr,pHouse,pQty1,pQty2,pQty3):
    /// ITEMBL의 해당 월 I1QTnn(+pQty1) / O1QTnn(+pQty2) / IOQTnn(+pQty3)를 증감.
    /// </summary>
    public static bool ItemblUpdate(string year, string month, string itnbr, string house,
        int deltaI1Qty, int deltaO1Qty, int deltaIoQty)
    {
        try
        {
            using var check = new DbQuery();
            check.Add("SELECT * FROM ITEMBL WHERE IYEAR=@YEAR AND ITNBR=@ITNBR AND HOUSE=@HOUSE");
            check.ParamByName("YEAR").AsString = year;
            check.ParamByName("ITNBR").AsString = itnbr;
            check.ParamByName("HOUSE").AsString = house;
            check.Open();
            if (check.IsEmpty) return true;

            using var upd = new DbQuery();
            upd.Add($"UPDATE ITEMBL SET I1QT{month} = I1QT{month} + @D1,");
            upd.Add($"       O1QT{month} = O1QT{month} + @D2,");
            upd.Add($"       IOQT{month} = IOQT{month} + @D3, MDATE=GETDATE()");
            upd.Add(" WHERE IYEAR=@YEAR AND ITNBR=@ITNBR AND HOUSE=@HOUSE");
            upd.ParamByName("D1").AsInteger = deltaI1Qty;
            upd.ParamByName("D2").AsInteger = deltaO1Qty;
            upd.ParamByName("D3").AsInteger = deltaIoQty;
            upd.ParamByName("YEAR").AsString = year;
            upd.ParamByName("ITNBR").AsString = itnbr;
            upd.ParamByName("HOUSE").AsString = house;
            upd.ExecSQL();
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// 원본 fncMisuUpdate(pYear,pMonth,pCvcod,pGu,pAmt1,pAmt2,pAmt3):
    /// MISUF의 해당 월 OAMTnn(pGu=1, +pAmt1) 또는 SAMTnn(pGu=2, +pAmt2)를 증감.
    /// </summary>
    public static bool MisuUpdate(string year, string month, string cvcod, int gu,
        int amt1, int amt2, int amt3)
    {
        try
        {
            using var check = new DbQuery();
            check.Add("SELECT * FROM MISUF WHERE MYEAR=@YEAR AND CVCOD=@CVCOD");
            check.ParamByName("YEAR").AsString = year;
            check.ParamByName("CVCOD").AsString = cvcod;
            check.Open();
            if (check.IsEmpty) return true;

            var col = gu == 2 ? "SAMT" : "OAMT";
            var delta = gu == 2 ? amt2 : amt1;

            using var upd = new DbQuery();
            upd.Add($"UPDATE MISUF SET {col}{month} = {col}{month} + @DELTA, MDATE=GETDATE()");
            upd.Add(" WHERE MYEAR=@YEAR AND CVCOD=@CVCOD");
            upd.ParamByName("DELTA").AsInteger = delta;
            upd.ParamByName("YEAR").AsString = year;
            upd.ParamByName("CVCOD").AsString = cvcod;
            upd.ExecSQL();
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// 원본 fncSaveJob: 입고/수정 시 보관전표(IPCHF)를 자동 생성/갱신한다.
    /// pJob='I'면 신규(SEQNO는 5001부터 자동채번), 그 외에는 KEYNO로 기존 전표를 찾아 UPDATE.
    /// </summary>
    public static bool SaveJob(string job, string date, string cvcod, string itnbr, string danwi,
        string house, string bigo, string keyNo, int ioQty, int oDan, int oAmt, int jamt1, int jamt2, int jamt3, int tpro)
    {
        try
        {
            int seqNo;
            if (job == "I")
            {
                using var maxQ = new DbQuery();
                maxQ.Add("SELECT MAX(SEQNO) AS MAXSEQ FROM IPCHF WHERE TDATE=@TDATE AND GUBN1='1' AND SEQNO>5000");
                maxQ.ParamByName("TDATE").AsString = date;
                maxQ.Open();
                seqNo = maxQ.IsEmpty || maxQ.FieldByName("MAXSEQ").IsNull ? 5001 : maxQ.FieldByName("MAXSEQ").AsInteger + 1;

                using var ins = new DbQuery();
                ins.Add("INSERT INTO IPCHF (TDATE,SEQNO,GUBN1,CVCOD,ITNBR,DANWI,IOQTY,");
                ins.Add("       ODAN,OAMT,JAMT1,JAMT2,JAMT3,");
                ins.Add("       TPRO,HOUSE,TBIGO,KEYNO,MDATE)");
                ins.Add("  VALUES(@TDATE,@SEQNO,@GUBN1,@CVCOD,@ITNBR,@DANWI,@IOQTY,");
                ins.Add("       @ODAN,@OAMT,@JAMT1,@JAMT2,@JAMT3,");
                ins.Add("       @TPRO,@HOUSE,@TBIGO,@KEYNO,@MDATE)");
                BindSaveJobParams(ins, date, seqNo, cvcod, itnbr, danwi, house, bigo, keyNo, ioQty, oDan, oAmt, jamt1, jamt2, jamt3, tpro);
                ins.ExecSQL();
            }
            else
            {
                using var findQ = new DbQuery();
                findQ.Add("SELECT * FROM IPCHF WHERE TDATE=@TDATE AND KEYNO=@KEYNO");
                findQ.ParamByName("TDATE").AsString = date;
                findQ.ParamByName("KEYNO").AsString = keyNo;
                findQ.Open();
                if (findQ.IsEmpty) return true; // 원본은 존재를 가정; 없으면 갱신할 것이 없음
                seqNo = findQ.FieldByName("SEQNO").AsInteger;

                using var upd = new DbQuery();
                upd.Add("UPDATE IPCHF SET GUBN1=@GUBN1,CVCOD=@CVCOD,ITNBR=@ITNBR,");
                upd.Add("    DANWI=@DANWI,IOQTY=@IOQTY,ODAN=@ODAN,OAMT=@OAMT,");
                upd.Add("    JAMT1=@JAMT1,JAMT2=@JAMT2,JAMT3=@JAMT3,TPRO=@TPRO,");
                upd.Add("    HOUSE=@HOUSE,TBIGO=@TBIGO,KEYNO=@KEYNO,MDATE=@MDATE");
                upd.Add(" WHERE TDATE=@TDATE AND SEQNO=@SEQNO");
                BindSaveJobParams(upd, date, seqNo, cvcod, itnbr, danwi, house, bigo, keyNo, ioQty, oDan, oAmt, jamt1, jamt2, jamt3, tpro);
                upd.ExecSQL();
            }
            return true;
        }
        catch { return false; }
    }

    private static void BindSaveJobParams(DbQuery q, string date, int seqNo, string cvcod, string itnbr,
        string danwi, string house, string bigo, string keyNo, int ioQty, int oDan, int oAmt,
        int jamt1, int jamt2, int jamt3, int tpro)
    {
        q.ParamByName("TDATE").AsString = date;
        q.ParamByName("SEQNO").AsInteger = seqNo;
        q.ParamByName("GUBN1").AsString = "1";
        q.ParamByName("CVCOD").AsString = cvcod;
        q.ParamByName("ITNBR").AsString = itnbr;
        q.ParamByName("DANWI").AsString = danwi;
        q.ParamByName("IOQTY").AsInteger = ioQty;
        q.ParamByName("ODAN").AsInteger = oDan;
        q.ParamByName("OAMT").AsInteger = oAmt;
        q.ParamByName("JAMT1").AsInteger = jamt1;
        q.ParamByName("JAMT2").AsInteger = jamt2;
        q.ParamByName("JAMT3").AsInteger = jamt3;
        q.ParamByName("TPRO").AsInteger = tpro;
        q.ParamByName("HOUSE").AsString = house;
        q.ParamByName("TBIGO").AsString = bigo;
        q.ParamByName("KEYNO").AsString = keyNo;
        q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
