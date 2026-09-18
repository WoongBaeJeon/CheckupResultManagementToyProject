/*
  검진 결과 관리 프로그램 - MSSQL DDL
  DBMS: Microsoft SQL Server
*/
USE [check_result_db];
GO

IF DB_NAME() <> N'check_result_db'
    THROW 50000, N'check_result_db 데이터베이스에서 실행해야 합니다.', 1;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    /* =========================================================
       1. 환자정보
       ========================================================= */
    CREATE TABLE dbo.InfoPatient
    (
        PatientId      INT            IDENTITY(1,1) NOT NULL,
        ChartNo        VARCHAR(15)    NOT NULL,
        PatientName    NVARCHAR(50)   NOT NULL,
        Gender         CHAR(1)        NULL,
        SocialNumber   NVARCHAR(13)   NULL,

        CONSTRAINT PK_InfoPatient
            PRIMARY KEY (PatientId)
    );

    CREATE UNIQUE INDEX UX_InfoPatient_ChartNo
        ON dbo.InfoPatient (ChartNo);

    /* =========================================================
       2. 접수정보
       ========================================================= */
    CREATE TABLE dbo.InfoReception
    (
        ReceptionId    INT            IDENTITY(1,1) NOT NULL,
        PatientId      INT            NOT NULL,
        ReceptionDate  DATE           NOT NULL,
        PatientAge     SMALLINT       NULL,
        ResultStatus   CHAR(1)        NOT NULL CONSTRAINT DF_InfoReception_ResultStatus DEFAULT ('N'),
        ReceptionMemo  NVARCHAR(500)  NULL,

        CONSTRAINT PK_InfoReception
            PRIMARY KEY (ReceptionId),

        CONSTRAINT FK_InfoReception_InfoPatient
            FOREIGN KEY (PatientId)
            REFERENCES dbo.InfoPatient (PatientId),

        CONSTRAINT CK_InfoReception_ResultStatus
            CHECK (ResultStatus IN ('N', 'S', 'F'))
    );

    CREATE INDEX IX_InfoReception_ReceptionDate
        ON dbo.InfoReception (ReceptionDate);

    /* =========================================================
       3. 검진종류 마스터
       ========================================================= */
    CREATE TABLE dbo.InfoCheckupClass
    (
        CheckupClassId INT             IDENTITY(1,1) NOT NULL,
        ClassCode      VARCHAR(20)     NOT NULL,
        ClassName      NVARCHAR(100)   NOT NULL,
        SortOrder      INT             NOT NULL CONSTRAINT DF_InfoCheckupClass_SortOrder DEFAULT (0),
        UseYn          CHAR(1)         NOT NULL CONSTRAINT DF_InfoCheckupClass_UseYn DEFAULT ('Y'),

        CONSTRAINT PK_InfoCheckupClass
            PRIMARY KEY (CheckupClassId),

        CONSTRAINT CK_InfoCheckupClass_UseYn
            CHECK (UseYn IN ('Y', 'N'))
    );

    CREATE UNIQUE INDEX UX_InfoCheckupClass_ClassCode
        ON dbo.InfoCheckupClass (ClassCode);

    /* =========================================================
       4. 검사항목 그룹 마스터
       ========================================================= */
    CREATE TABLE dbo.InfoCheckupItemGroup
    (
        CheckupItemGroupId INT             IDENTITY(1,1) NOT NULL,
        CheckupClassId     INT             NOT NULL,
        GroupCode          VARCHAR(20)     NOT NULL,
        GroupName          NVARCHAR(100)   NOT NULL,
        SortOrder          INT             NOT NULL CONSTRAINT DF_InfoCheckupItemGroup_SortOrder DEFAULT (0),
        UseYn              CHAR(1)         NOT NULL CONSTRAINT DF_InfoCheckupItemGroup_UseYn DEFAULT ('Y'),

        CONSTRAINT PK_InfoCheckupItemGroup
            PRIMARY KEY (CheckupItemGroupId),

        CONSTRAINT FK_InfoCheckupItemGroup_InfoCheckupClass
            FOREIGN KEY (CheckupClassId)
            REFERENCES dbo.InfoCheckupClass (CheckupClassId),

        CONSTRAINT CK_InfoCheckupItemGroup_UseYn
            CHECK (UseYn IN ('Y', 'N'))
    );

    CREATE UNIQUE INDEX UX_InfoCheckupItemGroup_CheckupClassId_GroupCode
        ON dbo.InfoCheckupItemGroup (CheckupClassId, GroupCode);

    /* =========================================================
       5. 공통코드 그룹 마스터
       ========================================================= */
    CREATE TABLE dbo.InfoCommonCodeGroup
    (
        CodeGroupId INT             IDENTITY(1,1) NOT NULL,
        GroupCode   VARCHAR(30)     NOT NULL,
        GroupName   NVARCHAR(100)   NOT NULL,
        UseYn       CHAR(1)         NOT NULL CONSTRAINT DF_InfoCommonCodeGroup_UseYn DEFAULT ('Y'),

        CONSTRAINT PK_InfoCommonCodeGroup
            PRIMARY KEY (CodeGroupId),

        CONSTRAINT CK_InfoCommonCodeGroup_UseYn
            CHECK (UseYn IN ('Y', 'N'))
    );

    CREATE UNIQUE INDEX UX_InfoCommonCodeGroup_GroupCode
        ON dbo.InfoCommonCodeGroup (GroupCode);

    /* =========================================================
       6. 공통코드 상세 마스터
       ========================================================= */
    CREATE TABLE dbo.InfoCommonCode
    (
        CommonCodeId INT             IDENTITY(1,1) NOT NULL,
        CodeGroupId  INT             NOT NULL,
        Code         VARCHAR(30)     NOT NULL,
        InputCode    VARCHAR(10)     NOT NULL,
        CodeName     NVARCHAR(100)   NOT NULL,
        SortOrder    INT             NOT NULL CONSTRAINT DF_InfoCommonCode_SortOrder DEFAULT (0),
        UseYn        CHAR(1)         NOT NULL CONSTRAINT DF_InfoCommonCode_UseYn DEFAULT ('Y'),

        CONSTRAINT PK_InfoCommonCode
            PRIMARY KEY (CommonCodeId),

        CONSTRAINT FK_InfoCommonCode_InfoCommonCodeGroup
            FOREIGN KEY (CodeGroupId)
            REFERENCES dbo.InfoCommonCodeGroup (CodeGroupId),

        CONSTRAINT CK_InfoCommonCode_UseYn
            CHECK (UseYn IN ('Y', 'N'))
    );

    CREATE UNIQUE INDEX UX_InfoCommonCode_CodeGroupId_Code
        ON dbo.InfoCommonCode (CodeGroupId, Code);

    CREATE UNIQUE INDEX UX_InfoCommonCode_CodeGroupId_InputCode
        ON dbo.InfoCommonCode (CodeGroupId, InputCode);

    /* =========================================================
       7. 검사항목 마스터
       ========================================================= */
    CREATE TABLE dbo.InfoCheckupItem
    (
        CheckupItemId      INT             IDENTITY(1,1) NOT NULL,
        CheckupItemGroupId INT             NOT NULL,
        ItemCode           VARCHAR(30)     NOT NULL,
        ItemName           NVARCHAR(100)   NOT NULL,
        InputType          VARCHAR(10)     NOT NULL,
        CodeGroupId        INT             NULL,
        Unit               NVARCHAR(20)    NULL,
        RequiredYn         CHAR(1)         NOT NULL CONSTRAINT DF_InfoCheckupItem_RequiredYn DEFAULT ('N'),
        MinValue           DECIMAL(18,4)   NULL,
        MaxValue           DECIMAL(18,4)   NULL,
        MaxLength          INT             NULL,
        SortOrder          INT             NOT NULL CONSTRAINT DF_InfoCheckupItem_SortOrder DEFAULT (0),
        UseYn              CHAR(1)         NOT NULL CONSTRAINT DF_InfoCheckupItem_UseYn DEFAULT ('Y'),

        CONSTRAINT PK_InfoCheckupItem
            PRIMARY KEY (CheckupItemId),

        CONSTRAINT FK_InfoCheckupItem_InfoCheckupItemGroup
            FOREIGN KEY (CheckupItemGroupId)
            REFERENCES dbo.InfoCheckupItemGroup (CheckupItemGroupId),

        CONSTRAINT FK_InfoCheckupItem_InfoCommonCodeGroup
            FOREIGN KEY (CodeGroupId)
            REFERENCES dbo.InfoCommonCodeGroup (CodeGroupId),

        CONSTRAINT CK_InfoCheckupItem_InputType
            CHECK (InputType IN ('NUM', 'TEXT', 'CODE', 'MEMO')),

        CONSTRAINT CK_InfoCheckupItem_RequiredYn
            CHECK (RequiredYn IN ('Y', 'N')),

        CONSTRAINT CK_InfoCheckupItem_UseYn
            CHECK (UseYn IN ('Y', 'N'))
    );

    CREATE UNIQUE INDEX UX_InfoCheckupItem_CheckupItemGroupId_ItemCode
        ON dbo.InfoCheckupItem (CheckupItemGroupId, ItemCode);

    /* =========================================================
       8. 접수-검진종류 매핑
       ========================================================= */
    CREATE TABLE dbo.InfoReceptionCheckup
    (
        ReceptionCheckupId INT IDENTITY(1,1) NOT NULL,
        ReceptionId        INT NOT NULL,
        CheckupClassId     INT NOT NULL,

        CONSTRAINT PK_InfoReceptionCheckup
            PRIMARY KEY (ReceptionCheckupId),

        CONSTRAINT FK_InfoReceptionCheckup_InfoReception
            FOREIGN KEY (ReceptionId)
            REFERENCES dbo.InfoReception (ReceptionId),

        CONSTRAINT FK_InfoReceptionCheckup_InfoCheckupClass
            FOREIGN KEY (CheckupClassId)
            REFERENCES dbo.InfoCheckupClass (CheckupClassId)
    );

    CREATE UNIQUE INDEX UX_InfoReceptionCheckup_ReceptionId_CheckupClassId
        ON dbo.InfoReceptionCheckup (ReceptionId, CheckupClassId);

    /* =========================================================
       9. 현재 검사결과
       ========================================================= */
    CREATE TABLE dbo.InfoCheckupResult
    (
        ResultId       INT             IDENTITY(1,1) NOT NULL,
        ReceptionId    INT             NOT NULL,
        CheckupItemId  INT             NOT NULL,
        ResultValue    NVARCHAR(2000)  NOT NULL,
        UpdatedAt      DATETIME2(0)    NOT NULL CONSTRAINT DF_InfoCheckupResult_UpdatedAt DEFAULT (GETDATE()),
        UpdatedBy      VARCHAR(50)     NOT NULL CONSTRAINT DF_InfoCheckupResult_UpdatedBy DEFAULT ('TEST_USER'),

        CONSTRAINT PK_InfoCheckupResult
            PRIMARY KEY (ResultId),

        CONSTRAINT FK_InfoCheckupResult_InfoReception
            FOREIGN KEY (ReceptionId)
            REFERENCES dbo.InfoReception (ReceptionId),

        CONSTRAINT FK_InfoCheckupResult_InfoCheckupItem
            FOREIGN KEY (CheckupItemId)
            REFERENCES dbo.InfoCheckupItem (CheckupItemId)
    );

    CREATE UNIQUE INDEX UX_InfoCheckupResult_ReceptionId_CheckupItemId
        ON dbo.InfoCheckupResult (ReceptionId, CheckupItemId);

    /* =========================================================
       10. 검사결과 변경 이력
       ========================================================= */
    CREATE TABLE dbo.HistCheckupResult
    (
        ResultHistId   BIGINT          IDENTITY(1,1) NOT NULL,
        ReceptionId    INT             NOT NULL,
        CheckupItemId  INT             NOT NULL,
        BeforeValue    NVARCHAR(2000)  NOT NULL,
        AfterValue     NVARCHAR(2000)  NULL,
        ProcessedAt    DATETIME2(0)    NOT NULL CONSTRAINT DF_HistCheckupResult_ProcessedAt DEFAULT (GETDATE()),
        ProcessedBy    VARCHAR(50)     NOT NULL CONSTRAINT DF_HistCheckupResult_ProcessedBy DEFAULT ('TEST_USER'),

        CONSTRAINT PK_HistCheckupResult
            PRIMARY KEY (ResultHistId),

        CONSTRAINT FK_HistCheckupResult_InfoReception
            FOREIGN KEY (ReceptionId)
            REFERENCES dbo.InfoReception (ReceptionId),

        CONSTRAINT FK_HistCheckupResult_InfoCheckupItem
            FOREIGN KEY (CheckupItemId)
            REFERENCES dbo.InfoCheckupItem (CheckupItemId)
    );

    /* =========================================================
       11. 접수 결과 상태 변경 이력
       ========================================================= */
    CREATE TABLE dbo.HistReceptionStatus
    (
        StatusHistId  BIGINT        IDENTITY(1,1) NOT NULL,
        ReceptionId   INT           NOT NULL,
        BeforeStatus  CHAR(1)       NOT NULL,
        AfterStatus   CHAR(1)       NOT NULL,
        ProcessType   VARCHAR(20)   NOT NULL,
        ProcessedAt   DATETIME2(0)  NOT NULL CONSTRAINT DF_HistReceptionStatus_ProcessedAt DEFAULT (GETDATE()),
        ProcessedBy   VARCHAR(50)   NOT NULL CONSTRAINT DF_HistReceptionStatus_ProcessedBy DEFAULT ('TEST_USER'),

        CONSTRAINT PK_HistReceptionStatus
            PRIMARY KEY (StatusHistId),

        CONSTRAINT FK_HistReceptionStatus_InfoReception
            FOREIGN KEY (ReceptionId)
            REFERENCES dbo.InfoReception (ReceptionId),

        CONSTRAINT CK_HistReceptionStatus_BeforeStatus
            CHECK (BeforeStatus IN ('N', 'S', 'F')),

        CONSTRAINT CK_HistReceptionStatus_AfterStatus
            CHECK (AfterStatus IN ('N', 'S', 'F')),

        CONSTRAINT CK_HistReceptionStatus_ProcessType
            CHECK (ProcessType IN ('SAVE', 'FINALIZE', 'CANCEL', 'REFINALIZE'))
    );
    /* =========================================================
       12. 소견 상용구
       ========================================================= */
       CREATE TABLE dbo.InfoOpinionPhrase
    (
        OpinionPhraseId INT IDENTITY(1,1) NOT NULL,
        CheckupItemId   INT              NULL,
        PhraseName      NVARCHAR(100)    NOT NULL,
        PhraseText      NVARCHAR(2000)   NOT NULL,
        SortOrder       INT              NOT NULL
            CONSTRAINT DF_InfoOpinionPhrase_SortOrder DEFAULT (0),
        UseYn           CHAR(1)          NOT NULL
            CONSTRAINT DF_InfoOpinionPhrase_UseYn DEFAULT ('Y'),

        CONSTRAINT PK_InfoOpinionPhrase
            PRIMARY KEY (OpinionPhraseId),

        CONSTRAINT FK_InfoOpinionPhrase_InfoCheckupItem
            FOREIGN KEY (CheckupItemId)
            REFERENCES dbo.InfoCheckupItem (CheckupItemId),

        CONSTRAINT CK_InfoOpinionPhrase_UseYn
            CHECK (UseYn IN ('Y', 'N'))
    );

    CREATE INDEX IX_InfoOpinionPhrase_CheckupItemId
        ON dbo.InfoOpinionPhrase (CheckupItemId, UseYn, SortOrder);
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
go
    /* =========================================================
    13. 사용자 정의 테이블 형식
    ========================================================= */
    CREATE TYPE dbo.CheckupResultSaveType AS TABLE
    (
        CheckupItemId INT NOT NULL,
        ResultValue   NVARCHAR(2000) NULL
    );
go
