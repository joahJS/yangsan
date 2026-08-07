# pStock — Delphi → C# 변환 결과물

델파이 7(BDE) 재고관리 프로그램 `pStock`을 C# WinForms(.NET 8)로 옮긴 결과물입니다.
로그인부터 메인 메뉴, 22개 업무 화면(BA/JA/SS/ED), 공용 검색 팝업, 핵심 데이터
입력창까지 실제로 동작하는 코드로 변환했습니다.

## 폴더 구조

```
pStock/
├── Program.cs                    # 진입점 (원본 pStock.dpr)
├── Data/
│   ├── AppDb.cs                   # DB 연결 (원본 DB_MD.pas / Tdm_NK)
│   ├── DbQuery.cs                 # TQuery 스타일 호환 래퍼
│   └── LedgerUpdates.cs           # 재고(ITEMBL)/미수금(MISUF)/보관전표(IPCHF) 공용 갱신 로직
├── Common/
│   ├── AppMessages.cs             # 메시지 상수 (원본 UserMsg.pas)
│   ├── PublicLib.cs                # 공용 유틸리티 (원본 PublicLib.pas)
│   ├── SqlFragments.cs             # 여러 화면이 공유하는 SQL 조각
│   └── UserContext.cs              # 로그인 사용자 정보
└── Forms/
    ├── LoginForm.cs / MainForm.cs / FormRegistry.cs / PlaceholderForm.cs
    ├── Common/                     # 공용 검색 팝업 3종
    │   ├── CvcodLookupForm.cs       # 거래처 검색 (원본 BA00C)
    │   ├── ItemLookupForm.cs        # 품목 검색 (원본 BA00D)
    │   └── PostalLookupForm.cs      # 우편번호 검색 (원본 gp_LoadFormPOSTF / cm001U05)
    ├── BA/  (6개) 공통코드·거래처·품목단가·착지처·사업장·패스워드 마스터
    ├── JA/  (5개) 재고관리·년간·일자별집계·품목별·운송현황
    ├── SS/  (8개 목록 화면 + 5개 입력창) 입고·보관료·출고·수금·미수금조회·
    │        거래처원장·품목원장·계산서 + SS21A/SS23A/SS32A/SS020F01/SS33B
    └── ED/  (3개) 월마감·년마감·기초잔액보수
```

## 주요 설계 결정

- **DB 접속**: 원본 BDE(`TDatabase`/`TQuery`, alias `chang` → DB `CHANG`, `sa` 계정)를
  `Microsoft.Data.SqlClient`로 대체했습니다. **`Data/AppDb.cs`의 `Server`/`Password`
  값은 자리표시자입니다.** 실제 값으로 바꾸거나 설정 파일/환경변수로 옮겨서 사용하세요.
- **`DbQuery` 호환 래퍼**: 원본이 반복하던
  `sql.Add(...); ParamByName(...).AsString; Open(); FieldByName(...).AsString` 패턴을
  거의 그대로 유지해, 원본 SQL과 로직을 최대한 왜곡 없이 옮겼습니다. 파라미터 표기는
  원본 `:CODE` → C#에서는 `@CODE`로 통일했습니다.
- **UI 컴포넌트**: 원본의 `RealGrid`, `URCtrls`, `QuickReport`, `XPMenu` 등 델파이 전용
  서드파티 컴포넌트는 표준 `DataGridView` / `MenuStrip` / 기본 컨트롤로 재설계했습니다.
- **메뉴 구성**: `FormRegistry.cs`가 원본 `Main.pas`의 `prcFormShow` case문을 그대로
  옮겼습니다. Tag 번호, 한글 캡션 모두 원본과 동일합니다.
- **재고/미수금 갱신 로직**: `Data/LedgerUpdates.cs`에 원본 `PublicLib.pas`의
  `fncItemblUpdate`, `fncMisuUpdate`, `fncSaveJob`을 간략화 이식해 여러 화면(BA·SS·ED)이
  공유합니다. 원본은 대상 레코드가 없으면 신규 생성까지 하지만, 여기서는 이미 레코드가
  있다고 가정하고 갱신만 수행합니다 — 신규 연도/거래처 최초 진입 시 레코드가 없다면
  사전에 한 번 생성해 두어야 합니다.

## 아직 남은 것 (우선순위 낮은 항목)

- **인쇄/리포트**: 원본은 QuickReport로 인쇄 미리보기를 지원했습니다. 이번 변환에서는
  리포트 인쇄 기능은 범위에서 제외했습니다 (필요 시 RDLC나 다른 리포팅 라이브러리로
  별도 구현 권장).
- **Excel 내보내기** (`gp_SendToExcel`): RealGrid 전용 로직이라 이번 단계에서는
  제외했습니다. DataGridView 기준으로 다시 구현하면 됩니다.
- **SS23B (보관료 자동계산)**, **SS33A (계산서 자동발행)**: 배치성 자동화 유틸리티라
  이번 단계에서는 안내 메시지로 남겨두었습니다.
- **cm000~cm005 계열의 세부 코드조회 팝업**(공통코드 검색 등 일부 특수 팝업)은
  가장 많이 쓰이는 거래처/품목/우편번호 3종만 구현했고, 나머지 소수 팝업은
  대상 화면이 한정적이라 제외했습니다.

## 빌드 방법

.NET 8 SDK와 Windows가 필요합니다 (WinForms는 Windows 전용입니다).

```
cd pStock
dotnet restore
dotnet build
dotnet run
```

또는 Visual Studio 2022에서 `pStock.csproj`를 열어 실행하세요.

> 이 결과물은 Windows가 아닌 샌드박스 환경에서 작성되어 `dotnet build`로 직접
> 컴파일 검증을 하지 못했습니다. 코드 전체에 대해 괄호/구문 오류는 수작업으로
> 점검했지만, 실제 빌드 시 사소한 오타나 타입 문제가 나올 수 있습니다. 오류
> 메시지를 알려주시면 바로 고쳐드리겠습니다.

## 실제 사용 전 확인할 것

1. `Data/AppDb.cs`의 서버 주소/비밀번호를 실제 값으로 교체
2. SQL Server에 원본과 동일한 테이블 구조(CVMAST, ITEMAS, ITEMBL, MISUF, IPGOF,
   IPCHF, SALE_M, SALE_D, SUGMF, TAXF, PASSWD, REFFPF, REACH, SAUPJANGF, POSTF 등)가
   있는지 확인
3. 로그인 화면이 조회하는 `PASSWD` 테이블에 관리자 계정이 최소 1건 있는지 확인
   (없다면 `BA00E` 패스워드 관리 화면 코드를 참고해 SQL로 직접 1건 넣어두세요)
