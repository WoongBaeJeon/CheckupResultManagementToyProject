/*
  검진 결과 관리 - 초기 마스터 및 테스트 데이터
  대상 DB : check_result_db

  구성
    1. 공통코드 그룹/코드
    2. 검진종류
    3. 검사그룹
    4. 검사항목 (NUM/TEXT/CODE/MEMO 유형별 테스트 항목)
    5. 환자정보 (주민번호는 NULL)
    6. 접수정보 및 접수별 검진종류

  특징
    - 자연키를 기준으로 UPDATE 후 INSERT하므로 재실행해도 중복되지 않는다.
    - 기존 환자의 SocialNumber는 수정하지 않는다.
    - 결과/이력 데이터는 Stored Procedure 테스트 단계에서 별도로 생성한다.
*/

USE [check_result_db];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_NAME() <> N'check_result_db'
    THROW 50000, N'check_result_db 데이터베이스에서 실행해야 합니다.', 1;

BEGIN TRY
    BEGIN TRANSACTION;

    /* =========================================================
       1. 공통코드 그룹
       ========================================================= */
    DECLARE @CodeGroup TABLE
    (
        GroupCode VARCHAR(30)   NOT NULL,
        GroupName NVARCHAR(100) NOT NULL,
        UseYn     CHAR(1)       NOT NULL
    );

    INSERT INTO @CodeGroup (GroupCode, GroupName, UseYn)
    VALUES
        ('POS_NEG',         N'음성/양성',       'Y'),
        ('NORMAL_ABNORMAL', N'정상/비정상',     'Y'),
        ('YES_NO',          N'아니오/예',       'Y'),
        ('DETECTION',       N'미검출/검출',     'Y');

    UPDATE T
       SET T.GroupName = S.GroupName,
           T.UseYn     = S.UseYn
      FROM dbo.InfoCommonCodeGroup AS T
      JOIN @CodeGroup AS S
        ON S.GroupCode = T.GroupCode;

    INSERT INTO dbo.InfoCommonCodeGroup (GroupCode, GroupName, UseYn)
    SELECT S.GroupCode, S.GroupName, S.UseYn
      FROM @CodeGroup AS S
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoCommonCodeGroup AS T
                WHERE T.GroupCode = S.GroupCode
           );

    /* =========================================================
       2. 공통코드
       ========================================================= */
    DECLARE @CommonCode TABLE
    (
        GroupCode VARCHAR(30)   NOT NULL,
        Code      VARCHAR(30)   NOT NULL,
        InputCode VARCHAR(10)   NOT NULL,
        CodeName  NVARCHAR(100) NOT NULL,
        SortOrder INT           NOT NULL,
        UseYn     CHAR(1)       NOT NULL
    );

    INSERT INTO @CommonCode (GroupCode, Code, InputCode, CodeName, SortOrder, UseYn)
    VALUES
        ('POS_NEG',         'NEG',          '0', N'음성',   1, 'Y'),
        ('POS_NEG',         'POS',          '1', N'양성',   2, 'Y'),
        ('NORMAL_ABNORMAL', 'NORMAL',       '0', N'정상',   1, 'Y'),
        ('NORMAL_ABNORMAL', 'ABNORMAL',     '1', N'비정상', 2, 'Y'),
        ('YES_NO',          'N',            '0', N'아니오', 1, 'Y'),
        ('YES_NO',          'Y',            '1', N'예',     2, 'Y'),
        ('DETECTION',       'NOT_DETECTED', '0', N'미검출', 1, 'Y'),
        ('DETECTION',       'DETECTED',     '1', N'검출',   2, 'Y');

    UPDATE T
       SET T.InputCode = S.InputCode,
           T.CodeName  = S.CodeName,
           T.SortOrder = S.SortOrder,
           T.UseYn     = S.UseYn
      FROM dbo.InfoCommonCode AS T
      JOIN dbo.InfoCommonCodeGroup AS G
        ON G.CodeGroupId = T.CodeGroupId
      JOIN @CommonCode AS S
        ON S.GroupCode = G.GroupCode
       AND S.Code      = T.Code;

    INSERT INTO dbo.InfoCommonCode
           (CodeGroupId, Code, InputCode, CodeName, SortOrder, UseYn)
    SELECT G.CodeGroupId, S.Code, S.InputCode, S.CodeName, S.SortOrder, S.UseYn
      FROM @CommonCode AS S
      JOIN dbo.InfoCommonCodeGroup AS G
        ON G.GroupCode = S.GroupCode
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoCommonCode AS T
                WHERE T.CodeGroupId = G.CodeGroupId
                  AND T.Code = S.Code
           );

    /* =========================================================
       3. 검진종류
       ========================================================= */
    DECLARE @CheckupClass TABLE
    (
        ClassCode VARCHAR(20)   NOT NULL,
        ClassName NVARCHAR(100) NOT NULL,
        SortOrder INT           NOT NULL,
        UseYn     CHAR(1)       NOT NULL
    );

    INSERT INTO @CheckupClass (ClassCode, ClassName, SortOrder, UseYn)
    VALUES
        ('NATIONAL', N'공단검진', 1, 'Y'),
        ('STUDENT',  N'학생검진', 2, 'Y'),
        ('CANCER',   N'암검진',   3, 'Y'),
        ('OTHER',    N'기타검진', 4, 'Y');

    UPDATE T
       SET T.ClassName = S.ClassName,
           T.SortOrder = S.SortOrder,
           T.UseYn     = S.UseYn
      FROM dbo.InfoCheckupClass AS T
      JOIN @CheckupClass AS S
        ON S.ClassCode = T.ClassCode;

    INSERT INTO dbo.InfoCheckupClass (ClassCode, ClassName, SortOrder, UseYn)
    SELECT S.ClassCode, S.ClassName, S.SortOrder, S.UseYn
      FROM @CheckupClass AS S
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoCheckupClass AS T
                WHERE T.ClassCode = S.ClassCode
           );

    /* =========================================================
       4. 검사그룹
       입력유형을 확인하기 쉽도록 검진종류별 4개 그룹으로 구성한다.
       ========================================================= */
    DECLARE @ItemGroup TABLE
    (
        ClassCode VARCHAR(20)   NOT NULL,
        GroupCode VARCHAR(20)   NOT NULL,
        GroupName NVARCHAR(100) NOT NULL,
        SortOrder INT           NOT NULL,
        UseYn     CHAR(1)       NOT NULL
    );

    INSERT INTO @ItemGroup (ClassCode, GroupCode, GroupName, SortOrder, UseYn)
    VALUES
        ('NATIONAL', 'NAT_PHYSICAL', N'신체계측',   1, 'Y'),
        ('NATIONAL', 'NAT_TEXT',     N'측정결과',   2, 'Y'),
        ('NATIONAL', 'NAT_CODE',     N'검체판정',   3, 'Y'),
        ('NATIONAL', 'NAT_MEMO',     N'종합소견',   4, 'Y'),

        ('STUDENT',  'STU_PHYSICAL', N'학생 신체계측', 1, 'Y'),
        ('STUDENT',  'STU_TEXT',     N'시청각검사',    2, 'Y'),
        ('STUDENT',  'STU_CODE',     N'학생 판정검사', 3, 'Y'),
        ('STUDENT',  'STU_MEMO',     N'학생검진 소견', 4, 'Y'),

        ('CANCER',   'CAN_MARKER',   N'종양표지자검사', 1, 'Y'),
        ('CANCER',   'CAN_TEXT',     N'검사 식별정보',  2, 'Y'),
        ('CANCER',   'CAN_CODE',     N'암검진 판정',    3, 'Y'),
        ('CANCER',   'CAN_MEMO',     N'암검진 소견',    4, 'Y'),

        ('OTHER',    'OTH_MEASURE',  N'기타 측정검사', 1, 'Y'),
        ('OTHER',    'OTH_TEXT',     N'기타 문진정보', 2, 'Y'),
        ('OTHER',    'OTH_CODE',     N'기타 판정검사', 3, 'Y'),
        ('OTHER',    'OTH_MEMO',     N'기타검진 소견', 4, 'Y');

    UPDATE T
       SET T.GroupName = S.GroupName,
           T.SortOrder = S.SortOrder,
           T.UseYn     = S.UseYn
      FROM dbo.InfoCheckupItemGroup AS T
      JOIN dbo.InfoCheckupClass AS C
        ON C.CheckupClassId = T.CheckupClassId
      JOIN @ItemGroup AS S
        ON S.ClassCode = C.ClassCode
       AND S.GroupCode = T.GroupCode;

    INSERT INTO dbo.InfoCheckupItemGroup
           (CheckupClassId, GroupCode, GroupName, SortOrder, UseYn)
    SELECT C.CheckupClassId, S.GroupCode, S.GroupName, S.SortOrder, S.UseYn
      FROM @ItemGroup AS S
      JOIN dbo.InfoCheckupClass AS C
        ON C.ClassCode = S.ClassCode
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoCheckupItemGroup AS T
                WHERE T.CheckupClassId = C.CheckupClassId
                  AND T.GroupCode = S.GroupCode
           );

    /* =========================================================
       5. 검사항목
       NUM/TEXT/CODE/MEMO 유형별 검사항목
       현재 과제 기준에 따라 모든 항목을 필수(Y)로 설정한다.
       ========================================================= */
    DECLARE @CheckupItem TABLE
    (
        ClassCode    VARCHAR(20)    NOT NULL,
        GroupCode    VARCHAR(20)    NOT NULL,
        ItemCode     VARCHAR(30)    NOT NULL,
        ItemName     NVARCHAR(100)  NOT NULL,
        InputType    VARCHAR(10)    NOT NULL,
        CodeGroup    VARCHAR(30)    NULL,
        Unit         NVARCHAR(20)   NULL,
        RequiredYn   CHAR(1)        NOT NULL,
        MinValue     DECIMAL(18,4)  NULL,
        MaxValue     DECIMAL(18,4)  NULL,
        MaxLength    INT            NULL,
        SortOrder    INT            NOT NULL,
        UseYn        CHAR(1)        NOT NULL
    );

    INSERT INTO @CheckupItem
           (ClassCode, GroupCode, ItemCode, ItemName, InputType, CodeGroup,
            Unit, RequiredYn, MinValue, MaxValue, MaxLength, SortOrder, UseYn)
    VALUES
        -- 공단검진
        ('NATIONAL', 'NAT_PHYSICAL', 'HEIGHT',              N'신장',             'NUM',  NULL,              N'cm',     'Y', NULL, NULL,  NULL, 1, 'Y'),
        ('NATIONAL', 'NAT_PHYSICAL', 'WEIGHT',              N'체중',             'NUM',  NULL,              N'kg',     'Y', NULL, NULL,  NULL, 2, 'Y'),
        ('NATIONAL', 'NAT_TEXT',     'BLOOD_PRESSURE_SYSTOLIC', N'수축기 혈압(고)',   'NUM',  NULL,              N'mmHg',   'Y', NULL, NULL, NULL, 1, 'Y'),
        ('NATIONAL', 'NAT_TEXT',     'BLOOD_PRESSURE_DIASTOLIC', N'이완기 혈압(저)',  'NUM',  NULL,              N'mmHg',   'Y', NULL, NULL, NULL, 2, 'Y'),
        ('NATIONAL', 'NAT_TEXT',     'VISION_LEFT',         N'좌안 시력',        'NUM',  NULL,              NULL,      'Y', NULL,  NULL,  NULL, 3, 'Y'),
        ('NATIONAL', 'NAT_TEXT',     'VISION_RIGHT',        N'우안 시력',        'NUM',  NULL,              NULL,      'Y', NULL,  NULL,  NULL, 4, 'Y'),
        ('NATIONAL', 'NAT_CODE',     'HBSAG',               N'B형간염 항원',     'CODE', 'DETECTION',       NULL,      'Y', NULL, NULL, NULL, 1, 'Y'),
        ('NATIONAL', 'NAT_CODE',     'URINE_PROTEIN',       N'요단백',           'CODE', 'POS_NEG',         NULL,      'Y', NULL, NULL, NULL, 2, 'Y'),
        ('NATIONAL', 'NAT_MEMO',     'GENERAL_OPINION',     N'종합소견',         'MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 1, 'Y'),
        ('NATIONAL', 'NAT_MEMO',     'LIFESTYLE_OPINION',   N'생활습관 소견',    'MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 2, 'Y'),

        -- 학생검진
        ('STUDENT',  'STU_PHYSICAL', 'STUDENT_HEIGHT',      N'학생 신장',        'NUM',  NULL,              N'cm',     'Y', NULL, NULL,  NULL, 1, 'Y'),
        ('STUDENT',  'STU_PHYSICAL', 'STUDENT_WEIGHT',      N'학생 체중',        'NUM',  NULL,              N'kg',     'Y', NULL, NULL,  NULL, 2, 'Y'),
        ('STUDENT',  'STU_TEXT',     'STUDENT_VISION_LEFT', N'학생 좌안 시력',   'NUM',  NULL,              NULL,      'Y', NULL,  NULL,  NULL, 1, 'Y'),
        ('STUDENT',  'STU_TEXT',     'STUDENT_VISION_RIGHT',N'학생 우안 시력',   'NUM',  NULL,              NULL,      'Y', NULL,  NULL,  NULL, 2, 'Y'),
        ('STUDENT',  'STU_TEXT',     'STUDENT_HEARING_LEFT',N'학생 좌측 청력',   'CODE', 'NORMAL_ABNORMAL', NULL,      'Y', NULL, NULL, NULL, 3, 'Y'),
        ('STUDENT',  'STU_TEXT',     'STUDENT_HEARING_RIGHT',N'학생 우측 청력',  'CODE', 'NORMAL_ABNORMAL', NULL,      'Y', NULL, NULL, NULL, 4, 'Y'),
        ('STUDENT',  'STU_CODE',     'SPINE_STATUS',        N'척추 상태',        'CODE', 'NORMAL_ABNORMAL', NULL,      'Y', NULL, NULL, NULL, 1, 'Y'),
        ('STUDENT',  'STU_CODE',     'ORAL_STATUS',         N'구강 상태',        'CODE', 'NORMAL_ABNORMAL', NULL,      'Y', NULL, NULL, NULL, 2, 'Y'),
        ('STUDENT',  'STU_MEMO',     'STUDENT_OPINION',     N'학생검진 종합소견','MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 1, 'Y'),
        ('STUDENT',  'STU_MEMO',     'ORAL_OPINION',        N'구강검진 소견',    'MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 2, 'Y'),

        -- 암검진: NUM 2 / TEXT 2 / CODE 2 / MEMO 2
        ('CANCER',   'CAN_MARKER',   'AFP',                 N'AFP',              'NUM',  NULL,              N'ng/mL',  'Y', NULL, NULL,NULL, 1, 'Y'),
        ('CANCER',   'CAN_MARKER',   'CEA',                 N'CEA',              'NUM',  NULL,              N'ng/mL',  'Y', NULL, NULL,NULL, 2, 'Y'),
        ('CANCER',   'CAN_TEXT',     'LESION_LOCATION',     N'병변 위치',        'TEXT', NULL,              NULL,      'Y', NULL, NULL, 100,  1, 'Y'),
        ('CANCER',   'CAN_TEXT',     'PATHOLOGY_NO',        N'병리번호',         'TEXT', NULL,              NULL,      'Y', NULL, NULL, 30,   2, 'Y'),
        ('CANCER',   'CAN_CODE',     'GASTRIC_RESULT',      N'위암검진 판정',    'CODE', 'NORMAL_ABNORMAL', NULL,      'Y', NULL, NULL, NULL, 1, 'Y'),
        ('CANCER',   'CAN_CODE',     'OCCULT_BLOOD',        N'분변잠혈검사',     'CODE', 'POS_NEG',         NULL,      'Y', NULL, NULL, NULL, 2, 'Y'),
        ('CANCER',   'CAN_MEMO',     'ENDOSCOPY_OPINION',   N'내시경 소견',      'MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 1, 'Y'),
        ('CANCER',   'CAN_MEMO',     'CANCER_OPINION',      N'암검진 종합소견',  'MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 2, 'Y'),

        -- 기타검진: NUM 2 / TEXT 2 / CODE 2 / MEMO 2
        ('OTHER',    'OTH_MEASURE',  'PULSE_RATE',          N'맥박수',           'NUM',  NULL,              N'bpm',    'Y', NULL, NULL,  NULL, 1, 'Y'),
        ('OTHER',    'OTH_MEASURE',  'OXYGEN_SATURATION',   N'산소포화도',       'NUM',  NULL,              N'%',      'Y', NULL, NULL,  NULL, 2, 'Y'),
        ('OTHER',    'OTH_TEXT',     'ALLERGY_INFO',        N'알레르기 정보',    'TEXT', NULL,              NULL,      'Y', NULL, NULL, 200,  1, 'Y'),
        ('OTHER',    'OTH_TEXT',     'MEDICATION_INFO',     N'복용약 정보',      'TEXT', NULL,              NULL,      'Y', NULL, NULL, 200,  2, 'Y'),
        ('OTHER',    'OTH_CODE',     'CHEST_XRAY',          N'흉부 X선 판정',    'CODE', 'NORMAL_ABNORMAL', NULL,      'Y', NULL, NULL, NULL, 1, 'Y'),
        ('OTHER',    'OTH_CODE',     'PREGNANCY_YN',        N'임신 여부',        'CODE', 'YES_NO',          NULL,      'Y', NULL, NULL, NULL, 2, 'Y'),
        ('OTHER',    'OTH_MEMO',     'XRAY_OPINION',        N'흉부 X선 소견',    'MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 1, 'Y'),
        ('OTHER',    'OTH_MEMO',     'OTHER_OPINION',       N'기타검진 종합소견','MEMO', NULL,              NULL,      'Y', NULL, NULL, 2000, 2, 'Y');

    UPDATE T
       SET T.ItemName    = S.ItemName,
           T.InputType  = S.InputType,
           T.CodeGroupId = CG.CodeGroupId,
           T.Unit        = S.Unit,
           T.RequiredYn  = S.RequiredYn,
           T.MinValue    = S.MinValue,
           T.MaxValue    = S.MaxValue,
           T.MaxLength   = S.MaxLength,
           T.SortOrder   = S.SortOrder,
           T.UseYn       = S.UseYn
      FROM dbo.InfoCheckupItem AS T
      JOIN dbo.InfoCheckupItemGroup AS IG
        ON IG.CheckupItemGroupId = T.CheckupItemGroupId
      JOIN dbo.InfoCheckupClass AS C
        ON C.CheckupClassId = IG.CheckupClassId
      JOIN @CheckupItem AS S
        ON S.ClassCode = C.ClassCode
       AND S.GroupCode = IG.GroupCode
       AND S.ItemCode  = T.ItemCode
      LEFT JOIN dbo.InfoCommonCodeGroup AS CG
        ON CG.GroupCode = S.CodeGroup;

    INSERT INTO dbo.InfoCheckupItem
           (CheckupItemGroupId, ItemCode, ItemName, InputType, CodeGroupId,
            Unit, RequiredYn, MinValue, MaxValue, MaxLength, SortOrder, UseYn)
    SELECT IG.CheckupItemGroupId, S.ItemCode, S.ItemName, S.InputType, CG.CodeGroupId,
           S.Unit, S.RequiredYn, S.MinValue, S.MaxValue, S.MaxLength, S.SortOrder, S.UseYn
      FROM @CheckupItem AS S
      JOIN dbo.InfoCheckupClass AS C
        ON C.ClassCode = S.ClassCode
      JOIN dbo.InfoCheckupItemGroup AS IG
        ON IG.CheckupClassId = C.CheckupClassId
       AND IG.GroupCode = S.GroupCode
      LEFT JOIN dbo.InfoCommonCodeGroup AS CG
        ON CG.GroupCode = S.CodeGroup
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoCheckupItem AS T
                WHERE T.CheckupItemGroupId = IG.CheckupItemGroupId
                  AND T.ItemCode = S.ItemCode
           );

    /* =========================================================
       6. 환자정보
       SocialNumber는 NULL로 유지한다. ChartNo 기준으로 재실행한다.
       ========================================================= */
    DECLARE @Patient TABLE
    (
        ChartNo     VARCHAR(15)  NOT NULL,
        PatientName NVARCHAR(50) NOT NULL,
        Gender      CHAR(1)      NULL
    );

    INSERT INTO @Patient (ChartNo, PatientName, Gender)
    VALUES
        ('P000001', N'김건강', 'M'),
        ('P000002', N'이새봄', 'F'),
        ('P000003', N'박하늘', 'M'),
        ('P000004', N'최다은', 'F'),
        ('P000005', N'정민준', 'M'),
        ('P000006', N'한서윤', 'F'),
        ('P000007', N'오지훈', 'M'),
        ('P000008', N'윤채원', 'F'),
        ('P000009', N'강도현', 'M'),
        ('P000010', N'문예린', 'F');

    UPDATE T
       SET T.PatientName = S.PatientName,
           T.Gender      = S.Gender
           -- SocialNumber는 사용자가 입력하므로 갱신하지 않는다.
      FROM dbo.InfoPatient AS T
      JOIN @Patient AS S
        ON S.ChartNo = T.ChartNo;

    INSERT INTO dbo.InfoPatient (ChartNo, PatientName, Gender, SocialNumber)
    SELECT S.ChartNo, S.PatientName, S.Gender, NULL
      FROM @Patient AS S
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoPatient AS T
                WHERE T.ChartNo = S.ChartNo
           );

    /* =========================================================
       7. 접수정보
       모든 접수는 결과 미입력 상태(N)로 생성한다.
       ========================================================= */
    DECLARE @Reception TABLE
    (
        ChartNo       VARCHAR(15)   NOT NULL,
        ReceptionDate DATE          NOT NULL,
        PatientAge    SMALLINT      NULL,
        ReceptionMemo NVARCHAR(500) NOT NULL
    );

    INSERT INTO @Reception (ChartNo, ReceptionDate, PatientAge, ReceptionMemo)
    VALUES
        ('P000001', '2026-09-02', 45, N'[INIT-T01] 공단검진 테스트'),
        ('P000002', '2026-09-02', 17, N'[INIT-T02] 학생검진 테스트'),
        ('P000003', '2026-09-03', 52, N'[INIT-T03] 암검진 테스트'),
        ('P000004', '2026-09-03', 34, N'[INIT-T04] 기타검진 테스트'),
        ('P000005', '2026-09-04', 61, N'[INIT-T05] 공단·암검진 복합 테스트'),
        ('P000006', '2026-09-04', 16, N'[INIT-T06] 학생검진 테스트'),
        ('P000007', '2026-09-05', 39, N'[INIT-T07] 공단·기타검진 복합 테스트'),
        ('P000008', '2026-09-05', 48, N'[INIT-T08] 암검진 테스트'),
        ('P000009', '2026-09-06', 28, N'[INIT-T09] 기타검진 테스트'),
        ('P000010', '2026-09-07', 55, N'[INIT-T10] 전체 검진종류 복합 테스트'),
        ('P000001', '2026-09-07', 45, N'[INIT-T11] 동일 수검자 재접수 테스트');

    INSERT INTO dbo.InfoReception
           (PatientId, ReceptionDate, PatientAge, ResultStatus, ReceptionMemo)
    SELECT P.PatientId, S.ReceptionDate, S.PatientAge, 'N', S.ReceptionMemo
      FROM @Reception AS S
      JOIN dbo.InfoPatient AS P
        ON P.ChartNo = S.ChartNo
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoReception AS R
                WHERE R.PatientId = P.PatientId
                  AND R.ReceptionDate = S.ReceptionDate
                  AND R.ReceptionMemo = S.ReceptionMemo
           );

    /* =========================================================
       8. 접수별 검진종류 연결
       ========================================================= */
    DECLARE @ReceptionClass TABLE
    (
        ReceptionMemo NVARCHAR(500) NOT NULL,
        ClassCode     VARCHAR(20)   NOT NULL
    );

    INSERT INTO @ReceptionClass (ReceptionMemo, ClassCode)
    VALUES
        (N'[INIT-T01] 공단검진 테스트',                'NATIONAL'),
        (N'[INIT-T02] 학생검진 테스트',                'STUDENT'),
        (N'[INIT-T03] 암검진 테스트',                  'CANCER'),
        (N'[INIT-T04] 기타검진 테스트',                'OTHER'),
        (N'[INIT-T05] 공단·암검진 복합 테스트',        'NATIONAL'),
        (N'[INIT-T05] 공단·암검진 복합 테스트',        'CANCER'),
        (N'[INIT-T06] 학생검진 테스트',                'STUDENT'),
        (N'[INIT-T07] 공단·기타검진 복합 테스트',      'NATIONAL'),
        (N'[INIT-T07] 공단·기타검진 복합 테스트',      'OTHER'),
        (N'[INIT-T08] 암검진 테스트',                  'CANCER'),
        (N'[INIT-T09] 기타검진 테스트',                'OTHER'),
        (N'[INIT-T10] 전체 검진종류 복합 테스트',      'NATIONAL'),
        (N'[INIT-T10] 전체 검진종류 복합 테스트',      'STUDENT'),
        (N'[INIT-T10] 전체 검진종류 복합 테스트',      'CANCER'),
        (N'[INIT-T10] 전체 검진종류 복합 테스트',      'OTHER'),
        (N'[INIT-T11] 동일 수검자 재접수 테스트',      'NATIONAL');

    INSERT INTO dbo.InfoReceptionCheckup (ReceptionId, CheckupClassId)
    SELECT R.ReceptionId, C.CheckupClassId
      FROM @ReceptionClass AS S
      JOIN dbo.InfoReception AS R
        ON R.ReceptionMemo = S.ReceptionMemo
      JOIN dbo.InfoCheckupClass AS C
        ON C.ClassCode = S.ClassCode
     WHERE NOT EXISTS
           (
               SELECT 1
                 FROM dbo.InfoReceptionCheckup AS RC
                WHERE RC.ReceptionId = R.ReceptionId
                  AND RC.CheckupClassId = C.CheckupClassId
           );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO

/* =============================================================
   주민번호 입력 예시
   아래 값은 예시 자리표시자이므로 실제 사용할 테스트 값으로 교체한 뒤
   필요한 행만 실행한다. 하이픈 없이 13자리로 저장한다.
   ============================================================= */
-- UPDATE dbo.InfoPatient SET SocialNumber = N'직접입력13자리' WHERE ChartNo = 'P000001';
-- UPDATE dbo.InfoPatient SET SocialNumber = N'직접입력13자리' WHERE ChartNo = 'P000002';

/* =============================================================
   입력 결과 확인
   ============================================================= */
SELECT G.GroupCode, G.GroupName, C.Code, C.InputCode, C.CodeName, C.SortOrder, C.UseYn
  FROM dbo.InfoCommonCodeGroup AS G
  JOIN dbo.InfoCommonCode AS C
    ON C.CodeGroupId = G.CodeGroupId
 ORDER BY G.GroupCode, C.SortOrder;

SELECT C.ClassCode, C.ClassName,
       G.GroupCode, G.GroupName,
       I.ItemCode, I.ItemName, I.InputType,
       CG.GroupCode AS CodeGroupCode,
       I.Unit, I.RequiredYn, I.MinValue, I.MaxValue, I.MaxLength
  FROM dbo.InfoCheckupClass AS C
  JOIN dbo.InfoCheckupItemGroup AS G
    ON G.CheckupClassId = C.CheckupClassId
  JOIN dbo.InfoCheckupItem AS I
    ON I.CheckupItemGroupId = G.CheckupItemGroupId
  LEFT JOIN dbo.InfoCommonCodeGroup AS CG
    ON CG.CodeGroupId = I.CodeGroupId
 ORDER BY C.SortOrder, G.SortOrder, I.SortOrder;

SELECT R.ReceptionId, R.ReceptionDate, R.ResultStatus,
       P.PatientId, P.ChartNo, P.PatientName, P.Gender, P.SocialNumber,
       C.ClassCode, C.ClassName, R.ReceptionMemo
  FROM dbo.InfoReception AS R
  JOIN dbo.InfoPatient AS P
    ON P.PatientId = R.PatientId
  JOIN dbo.InfoReceptionCheckup AS RC
    ON RC.ReceptionId = R.ReceptionId
  JOIN dbo.InfoCheckupClass AS C
    ON C.CheckupClassId = RC.CheckupClassId
 WHERE R.ReceptionMemo LIKE N'[[]INIT-T%'
 ORDER BY R.ReceptionDate, R.ReceptionId, C.SortOrder;
GO
