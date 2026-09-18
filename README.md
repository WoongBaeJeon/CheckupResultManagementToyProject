# 검진결과관리

검진 접수 건을 조회하고 검사항목별 결과를 입력, 저장, 확정하며 변경 이력과 결과지를 관리하는 Windows 데스크톱 애플리케이션입니다.

## 주요 기능

- 접수일자와 검색어를 이용한 검진 접수 조회
- 접수별 검진종류와 검사그룹을 Tree 형태로 표시
- 숫자, 문자, 코드, 메모 유형별 검사결과 입력
- 필수 입력, 숫자 형식, 코드값, 최대 길이 검증
- 혈압 등 일부 검사항목의 결과 판정 표시
- 검진 소견 상용구 조회 및 입력
- 검사결과 저장 및 변경 내용 확인
- 결과 확정과 확정 취소
- 검사결과 변경 이력 및 접수 상태 변경 이력 조회
- 확정된 검진 결과의 PDF 저장

## 업무 상태 흐름

```text
미입력(N)
   │ 결과 저장
   ▼
저장(S)
   │ 결과 확정
   ▼
확정(F)
   │ 확정 취소
   └──────────► 저장(S)
```

결과 저장, 확정 및 확정 취소의 최종 업무 규칙과 데이터 변경은 SQL Server Stored Procedure에서 처리합니다.

## 기술 구성

- C#
- .NET Framework 4.6.1
- Windows Forms
- DevExpress WinForms 20.2.6
- Microsoft SQL Server Express
- Stored Procedure 기반 데이터 접근

## 애플리케이션 구조

```text
WinForms UI
   └─ Controls
       ├─ Services
       └─ Repositories
           └─ DbHelper / DbConnectionFactory
               └─ SQL Server Stored Procedure
```

- `Controls`: 화면 구성, 사용자 입력, 이벤트 및 화면 상태 처리
- `Services`: 결과 판정과 PDF 출력 등 화면에서 분리한 기능
- `Repositories`: Stored Procedure 호출과 조회 결과 매핑
- `Models`: 화면과 데이터 접근 계층에서 사용하는 DTO
- `Data`: DB 연결 생성과 공통 Stored Procedure 실행

## 저장소 구성

```text
.
├─ src/
│  └─ infoCheckupResult/
│     ├─ infoCheckupResult.sln
│     └─ infoCheckupResult/
├─ database/
│  ├─ 01.Table_schema_Script.sql
│  ├─ 02.master_test_data.sql
│  ├─ procedures/
│  └─ README.md
├─ tests/
├─ .gitignore
└─ README.md
```

## 실행 환경 준비

### 1. 필수 프로그램

- Visual Studio 2019 이상
- .NET Framework 4.6.1 Developer Pack
- DevExpress WinForms 20.2.6
- SQL Server 또는 SQL Server Express
- SQL Server Management Studio(SSMS)

DevExpress는 상용 라이브러리이므로 배포 DLL과 라이선스 키는 저장소에 포함하지 않습니다. 프로젝트를 빌드하려면 개발 PC에 해당 버전의 DevExpress가 설치되어 있어야 합니다.

### 2. 데이터베이스 구성

SSMS에서 `check_result_db` 데이터베이스를 생성한 후 다음 순서로 스크립트를 실행합니다.

1. `database/01.Table_schema_Script.sql`
2. `database/02.master_test_data.sql`
3. `database/procedures/` 폴더의 SQL 파일 전체

상세한 DB 설치 방법과 주의사항은 [`database/README.md`](database/README.md)를 참고하십시오.

마스터 및 테스트 데이터에는 프로그램 실행 확인을 위한 가상의 수검자명, 차트번호와 접수정보만 포함되어 있으며 주민등록번호는 저장하지 않습니다.

### 3. DB 연결 확인

기본 연결 문자열은 `src/infoCheckupResult/infoCheckupResult/App.config`에 있습니다.

```xml
Data Source=localhost\SQLEXPRESS;
Initial Catalog=check_result_db;
Integrated Security=True;
TrustServerCertificate=True;
```

SQL Server 인스턴스 이름이 다른 경우 `Data Source`를 로컬 환경에 맞게 변경합니다.

### 4. 빌드 및 실행

1. Visual Studio에서 `src/infoCheckupResult/infoCheckupResult.sln`을 엽니다.
2. 참조된 .NET Framework와 DevExpress 버전을 확인합니다.
3. 솔루션을 빌드합니다.
4. `infoCheckupResult` 프로젝트를 시작 프로젝트로 실행합니다.

## 주요 Stored Procedure

| Stored Procedure | 역할 |
|---|---|
| `USP_RECEPTION_SEARCH` | 접수 목록 검색 |
| `USP_RECEPTION_EXAM_STRUCTURE_SELECT` | 접수별 검진종류와 검사그룹 조회 |
| `USP_RECEPTION_RESULT_SELECT` | 접수 기본정보, 검사결과 및 입력 코드 조회 |
| `USP_RECEPTION_RESULT_SAVE` | 검사결과 저장과 변경 이력 기록 |
| `USP_RECEPTION_FINALIZE` | 검사결과 확정과 상태 이력 기록 |
| `USP_RECEPTION_FINALIZE_CANCEL` | 결과 확정 취소 |
| `USP_EXAM_RESULT_HIST_SELECT` | 검사결과 변경 이력 조회 |
| `USP_RECEPTION_STATUS_HIST_SELECT` | 접수 상태 변경 이력 조회 |
| `USP_OPINION_PHRASE_LIST` | 검사항목별 소견 상용구 조회 |

## 공개 범위

이 저장소에는 애플리케이션 실행과 구조 확인에 필요한 C# 소스, 프로젝트 파일, DDL, Stored Procedure 및 가상 테스트 데이터를 포함합니다.

다음 항목은 포함하지 않습니다.

- 실제 개인정보 및 검진 데이터
- DB 계정과 비밀번호
- 로컬 DB 및 백업 파일(`.mdf`, `.ldf`, `.bak`)
- Visual Studio 사용자 설정과 빌드 결과물(`.vs`, `bin`, `obj`)
- DevExpress 배포 DLL과 라이선스 키
- 업무 분석 문서, 발표자료 및 기타 제출 산출물

## 참고사항

- DB 연결은 Windows 통합 인증을 사용합니다.
- 스키마 스크립트는 신규 로컬 데이터베이스 구성을 기준으로 작성했습니다.
- 테스트 데이터와 프로그램은 학습 및 기능 확인 목적으로 사용하십시오.
