# Database Setup

검진 결과 관리 프로그램에서 사용하는 SQL Server 데이터베이스 구성 스크립트입니다.

## 요구 환경

- Microsoft SQL Server 또는 SQL Server Express
- SQL Server Management Studio(SSMS)
- 데이터베이스명: `check_result_db`
- Windows 통합 인증

## 파일 구성

```text
database/
├─ 01.Table_schema_Script.sql  # 테이블, 제약조건, 인덱스, 사용자 정의 테이블 형식
├─ 02.master_test_data.sql     # 마스터 및 가상 테스트 데이터
└─ procedures/                 # Stored Procedure
```

`02.master_test_data.sql`에 포함된 수검자명, 차트번호, 접수정보는 프로그램 실행 확인을 위한 가상 테스트 데이터입니다. 주민등록번호는 저장하지 않습니다.

## 설치 순서

### 1. 데이터베이스 생성

SSMS에서 다음 쿼리를 실행합니다.

```sql
IF DB_ID(N'check_result_db') IS NULL
    CREATE DATABASE [check_result_db];
GO
```

### 2. 스키마 생성

`01.Table_schema_Script.sql`을 실행합니다.

이 스크립트는 다음 객체를 생성합니다.

- 테이블과 PK, FK, CHECK 제약조건
- 인덱스
- Stored Procedure에서 사용하는 `dbo.CheckupResultSaveType`

### 3. 마스터 및 테스트 데이터 생성

`02.master_test_data.sql`을 실행합니다.

마스터데이터는 자연키를 기준으로 갱신한 뒤 없는 데이터만 추가하므로 다시 실행해도 동일한 테스트 데이터가 중복 생성되지 않습니다.

### 4. Stored Procedure 생성

`procedures` 폴더에 있는 SQL 파일을 모두 실행합니다.

```text
USP_EXAM_RESULT_HIST_SELECT.sql
USP_OPINION_PHRASE_LIST.sql
USP_RECEPTION_EXAM_STRUCTURE_SELECT.sql
USP_RECEPTION_FINALIZE.sql
USP_RECEPTION_FINALIZE_CANCEL.sql
USP_RECEPTION_RESULT_SAVE.sql
USP_RECEPTION_RESULT_SELECT.sql
USP_RECEPTION_SEARCH.sql
USP_RECEPTION_STATUS_HIST_SELECT.sql
```

각 파일은 `check_result_db`를 대상으로 실행되도록 작성되어 있습니다.

## 애플리케이션 연결 설정

애플리케이션의 `App.config`에는 다음과 같은 로컬 SQL Server Express 연결이 설정되어 있습니다.

```xml
Data Source=localhost\SQLEXPRESS;
Initial Catalog=check_result_db;
Integrated Security=True;
TrustServerCertificate=True;
```

SQL Server 인스턴스 이름이 다른 경우 `Data Source`만 로컬 환경에 맞게 변경합니다. 저장소에 DB 계정이나 비밀번호를 기록하지 마십시오.

## 실행 확인

설치가 끝난 후 다음 항목을 확인합니다.

1. 프로그램에서 테스트 접수 목록이 조회되는지 확인합니다.
2. 검진 결과 저장과 재조회가 정상적으로 동작하는지 확인합니다.
3. 결과 확정과 확정 취소가 정상적으로 동작하는지 확인합니다.
4. 결과 및 상태 변경 이력이 조회되는지 확인합니다.

## 주의사항

- 운영 DB나 회사 공유 DB가 아닌 로컬 테스트 DB에서 실행하십시오.
- `.mdf`, `.ldf`, `.bak` 파일과 실제 개인정보는 저장소에 포함하지 마십시오.
- `01.Table_schema_Script.sql`은 신규 데이터베이스 구성을 위한 스크립트입니다. 이미 객체가 존재하는 DB에서 다시 실행하면 오류가 발생할 수 있습니다.
- 테스트 데이터 스크립트의 주민등록번호 입력 예시는 주석 상태로 유지하십시오.
