namespace pStock.Common;

/// <summary>
/// 원본 UserMsg.pas 이식. 화면 전반에서 쓰는 공용 메시지 상수 모음.
/// </summary>
public static class AppMessages
{
    public const string MSG_DEFAULT = "화면처리 진행상태...";
    public const string MIP0001 = "서버 처리를 진행중입니다.";
    public const string MIP0002 = "시스템관리자에게 문의바랍니다.";
    public const string MIP0003 = "완료되었습니다.";
    public const string MIP0004 = "단독실행은 불가능합니다.";
    public const string MIP0005 = "로그인 후 실행하세요.";
    public const string MIP0006 = "데이터베이스 접속에 문제가 발생하였습니다.";

    // 파일 입출력오류
    public const string EII0001 = "파일을 읽을 수 없습니다.";
    public const string EII0002 = "파일을 쓸 수 없습니다.";
    public const string EII0003 = "파일을 찾을 수 없습니다.";
    public const string EII0004 = "파일을 저장할 수 없습니다.";
    public const string EII0010 = "인쇄 오류입니다.";
    public const string QII0001 = "변경사항을 저장하시겠습니까?";

    // Confirm 메세지
    public const string QU0001 = "수정하시겠습니까?";
    public const string QU0002 = "삭제하시겠습니까?";
    public const string QU0003 = "종료하시겠습니까?";
    public const string QU0004 = "인쇄하시겠습니까?";
    public const string QU0005 = "처리하시겠습니까?";
    public const string QU0006 = "전송하시겠습니까?";
    public const string QU0007 = "저장하시겠습니까?";
    public const string QU0008 = "부분삭제하시겠습니까";
    public const string QU0009 = "취소하시겠습니까?";
    public const string QU0010 = "전체자료 삭제하시겠습니까?";

    public const string EII0005 = "파일의 형식이 잘못 되었습니다.";
    public const string EII0009 = "프린터 서버 오류입니다.";
    public const string EII0011 = "트리메뉴를 만들지 못했습니다.";

    public const string EP0001 = "작업중 오류가 발생했습니다.\r\n잠시 후 다시 작업하십시오.";
    public const string EP0008 = "입력오류가 발생하였습니다.";
    public const string EP0009 = "수정오류가 발생하였습니다.";
    public const string EP0010 = "삭제오류가 발생하였습니다.";
    public const string EP0011 = "조회오류가 발생하였습니다.";
    public const string EP0012 = "처리오류가 발생하였습니다.";
    public const string EP0016 = "자료가 존재하지 않습니다.";
    public const string EP0017 = "자료의 길이가 너무 길어서 입력할 수 없습니다.";
    public const string IP0003 = "조회된 자료가 없습니다.";
    public const string IP0005 = "삭제할 자료가 없습니다.";
    public const string IP0006 = "수정할 자료가 없습니다.";
    public const string IP0007 = "처리할 자료가 없습니다.";
    public const string IP0009 = "삭제되었습니다.";
    public const string IP0010 = "수정되었습니다.";
    public const string IP0011 = "입력되었습니다.";
    public const string IP0012 = "조회되었습니다.";
    public const string IP0013 = "처리되었습니다.";
    public const string IP0014 = "저장되었습니다.";
    public const string IP0015 = "자료를 검색할수 없습니다. 조회하시겠습니까?";
    public const string IP0016 = "EXCEL이 설치되지 않았습니다.";
    public const string IP0017 = "취소 되었습니다.";

    // 보안 오류
    public const string ES0001 = "사용자 환경설정 정보를 가져오는데 실패했습니다.";
    public const string ES0002 = "사용자 환경설정 정보를 저장하는데 실패했습니다.";
    public const string WS1001 = "미등록 사용자입니다.\r\n귀하는 사용권한이 없습니다.";
    public const string WS1002 = "비밀번호가 잘못되었습니다.\r\n다시 입력하여 주십시오.";
    public const string WS1003 = "귀하는 더이상 작업권한이 없습니다.\r\n전산실로 문의바랍니다.";
    public const string WS1004 = "비밀번호가 잘못되었습니다.\r\n시스템을 자동종료합니다.";

    public const string WS0002 = "쓰기 권한이 없습니다.";
    public const string WS0003 = "읽기 권한이 없습니다.";
    public const string WS0004 = "입력 권한이 없습니다.";
    public const string WS0005 = "수정 권한이 없습니다.";
    public const string WS0006 = "삭제 권한이 없습니다.";
    public const string WS0007 = "조회 권한이 없습니다.";
    public const string WS0008 = "출력 권한이 없습니다.";
    public const string WS0009 = "자료잠금처리된 자료입니다.";

    public const string WI0001 = "자동채번이 아닌 경우 코드는 필수입력 항목입니다.";
    public const string WI0002 = "이미 입력된 자료입니다\r\n해당위치로 이동할까요?";
    public const string WI0003 = "자동채번을 하거나, 중복되지 않는 번호를 입력하세요.";
    public const string WIU004 = "중복된 코드입니다.\r\n확인하세요.";
    public const string WIU005 = "이미 존재하는 자료입니다.";
    public const string WI0006 = "이미 입력된 자료입니다\r\n중복으로 입력하시겠습니까?";
    public const string WI0007 = "사용중지된 품목입니다\r\n그래도 입력하시겠습니까?";
    public const string WI0008 = "이미 작성된 전표가 있습니다.\r\n추가하시겠습니까?";

    public const string WU0002 = "필수입력 항목이 빠졌습니다.";
    public const string WU0007 = "잘못된 값이 입력되었습니다";
    public const string WU0010 = "인쇄할 자료가 없습니다.";
    public const string WU0011 = "삭제할 수 없습니다.";
    public const string WU0012 = "수정할 수 없습니다.";
    public const string WU0013 = "입력할 수 없습니다.";
    public const string WU0014 = "처리할 수 없습니다.";
    public const string WU0017 = "입력일자가 부적절합니다.";

    public const string WU0023 = "상세내역을 입력하세요.";

    public const string WD0001 = "회계반영된 자료로 처리하실수 없습니다.";
}
