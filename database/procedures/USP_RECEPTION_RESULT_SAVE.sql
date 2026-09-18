USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_RECEPTION_RESULT_SAVE] 스크립트 날짜: 2026-09-17 오전 9:22:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   접수별 검사결과 저장

   입력
     @ReceptionId : 저장할 접수 ID
     @Results     : 접수 화면 전체 검사항목과 입력값
     @ProcessedBy : 처리자. 로그인 미구현 단계에서는 TEST_USER 사용

   처리
     - N/S 상태에서만 저장 가능
     - 화면 전체 활성 검사항목이 TVP에 포함되어야 함
     - 공백은 NULL로 정규화
     - 부분 입력 허용, 단 저장 결과가 0건인 것은 허용하지 않음
     - 최초 입력은 결과 변경 이력에서 제외
     - 기존값 변경/삭제만 HistCheckupResult에 기록
     - 최초 저장이면 N -> S 및 SAVE 상태 이력 기록

   출력: 저장 처리 요약 1행
   ============================================================= */
CREATE PROCEDURE [dbo].[USP_RECEPTION_RESULT_SAVE]
    @ReceptionId INT,
    @Results     dbo.CheckupResultSaveType READONLY,
    @ProcessedBy VARCHAR(50) = 'TEST_USER'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @BeforeStatus   CHAR(1);
    DECLARE @InsertedCount  INT = 0;
    DECLARE @UpdatedCount   INT = 0;
    DECLARE @DeletedCount   INT = 0;
    DECLARE @SavedCount     INT = 0;
    DECLARE @ErrorItemName  NVARCHAR(100);
    DECLARE @ErrorMessage   NVARCHAR(2048);

    SET @ProcessedBy = NULLIF(LTRIM(RTRIM(@ProcessedBy)), '');

    IF @ProcessedBy IS NULL
        SET @ProcessedBy = 'TEST_USER';

    IF @ReceptionId IS NULL
    BEGIN
        RAISERROR(N'접수 ID는 필수입니다.', 16, 1);
        RETURN;
    END;

    IF LEN(@ProcessedBy) > 50
    BEGIN
        RAISERROR(N'처리자는 50자를 초과할 수 없습니다.', 16, 1);
        RETURN;
    END;

    /* 검진 항목 임시 테이블 */
    DECLARE @ExpectedItems TABLE
    (
        CheckupItemId INT            NOT NULL PRIMARY KEY,
        ItemName      NVARCHAR(100)  NOT NULL,
        InputType     VARCHAR(10)    NOT NULL,
        CodeGroupId   INT            NULL,
        MinValue      DECIMAL(18,4)  NULL,
        MaxValue      DECIMAL(18,4)  NULL,
        MaxLength     INT            NULL
    );

    DECLARE @NormalizedResults TABLE
    (
        CheckupItemId INT             NOT NULL PRIMARY KEY,
        ResultValue   NVARCHAR(2000)  NULL
    );

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @BeforeStatus = R.ResultStatus
          FROM dbo.InfoReception R WITH (UPDLOCK, HOLDLOCK)
         WHERE R.ReceptionId = @ReceptionId;

        IF @BeforeStatus IS NULL
        BEGIN
            RAISERROR(N'존재하지 않는 접수입니다.', 16, 1);
        END;

        IF @BeforeStatus NOT IN ('N', 'S')
        BEGIN
            RAISERROR(N'확정된 접수는 결과를 저장할 수 없습니다. 확정취소 후 저장하세요.', 16, 1);
        END;

        INSERT INTO @ExpectedItems 
                    ( CheckupItemId
                      , ItemName
                      , InputType
                      , CodeGroupId
                      , MinValue
                      , MaxValue
                      , MaxLength)
        SELECT DISTINCT ICI.CheckupItemId
                      , ICI.ItemName
                      , ICI.InputType
                      , ICI.CodeGroupId
                      , ICI.MinValue
                      , ICI.MaxValue
                      , ICI.MaxLength
          FROM dbo.InfoReceptionCheckup IRC JOIN dbo.InfoCheckupClass ICC
                                             ON ICC.CheckupClassId = IRC.CheckupClassId
                                           JOIN dbo.InfoCheckupItemGroup ICIG
                                             ON ICIG.CheckupClassId = ICC.CheckupClassId
                                           JOIN dbo.InfoCheckupItem ICI
                                             ON ICI.CheckupItemGroupId = ICIG.CheckupItemGroupId
         WHERE IRC.ReceptionId = @ReceptionId
           AND ICC.UseYn = 'Y'
           AND ICIG.UseYn = 'Y'
           AND ICI.UseYn = 'Y';

        IF NOT EXISTS (SELECT 1 FROM @ExpectedItems)
        BEGIN
            RAISERROR(N'접수에 연결된 활성 검사항목이 없습니다.', 16, 1);
        END;

        IF EXISTS
           (
               SELECT 1 FROM @Results R LEFT JOIN @ExpectedItems E
                                               ON E.CheckupItemId = R.CheckupItemId
                WHERE E.CheckupItemId IS NULL
           )
        BEGIN
            RAISERROR(N'현재 접수에 속하지 않은 검사항목이 전달되었습니다.', 16, 1);
        END;

        IF EXISTS
           (
               SELECT E.CheckupItemId
                 FROM @ExpectedItems E
               EXCEPT
               SELECT R.CheckupItemId
                 FROM @Results R
           )
        BEGIN
            RAISERROR(N'접수의 전체 검사항목이 전달되지 않았습니다. Tree 필터와 관계없이 전체 항목을 전달해야 합니다.', 16, 1);
        END;

        INSERT INTO @NormalizedResults (CheckupItemId, ResultValue)
        SELECT R.CheckupItemId,
               NULLIF(LTRIM(RTRIM(R.ResultValue)), N'')
        FROM @Results R;

        IF NOT EXISTS
           (
               SELECT 1
               FROM @NormalizedResults
               WHERE ResultValue IS NOT NULL
           )
        BEGIN
            RAISERROR(N'최소 한 개 이상의 검사결과를 입력해야 합니다.', 16, 1);
        END;

        SELECT TOP (1) @ErrorItemName = E.ItemName
          FROM @NormalizedResults N JOIN @ExpectedItems E
                                         ON E.CheckupItemId = N.CheckupItemId
         WHERE N.ResultValue IS NOT NULL
           AND E.InputType = 'NUM'
           AND
               (
                   TRY_CONVERT(DECIMAL(18,4), N.ResultValue) IS NULL
                   OR (E.MinValue IS NOT NULL
                       AND TRY_CONVERT(DECIMAL(18,4), N.ResultValue) < E.MinValue)
                   OR (E.MaxValue IS NOT NULL
                       AND TRY_CONVERT(DECIMAL(18,4), N.ResultValue) > E.MaxValue)
               )
         ORDER BY E.CheckupItemId;

        IF @ErrorItemName IS NOT NULL
        BEGIN
            SET @ErrorMessage = N'숫자 형식 또는 허용 범위를 확인하세요. 항목: ' + @ErrorItemName;
            RAISERROR(N'%s', 16, 1, @ErrorMessage);
        END;

        SET @ErrorItemName = NULL;

        SELECT TOP (1) @ErrorItemName = E.ItemName
          FROM @NormalizedResults N JOIN @ExpectedItems E
                                      ON E.CheckupItemId = N.CheckupItemId
         WHERE N.ResultValue IS NOT NULL
           AND E.InputType IN ('TEXT', 'MEMO')
           AND E.MaxLength IS NOT NULL
           AND LEN(N.ResultValue) > E.MaxLength
         ORDER BY E.CheckupItemId;

        IF @ErrorItemName IS NOT NULL
        BEGIN
            SET @ErrorMessage = N'최대 입력 길이를 초과했습니다. 항목: ' + @ErrorItemName;
            RAISERROR(N'%s', 16, 1, @ErrorMessage);
        END;

        SET @ErrorItemName = NULL;

        SELECT TOP (1) @ErrorItemName = E.ItemName
          FROM @NormalizedResults N JOIN @ExpectedItems E
                                      ON E.CheckupItemId = N.CheckupItemId
         WHERE N.ResultValue IS NOT NULL
           AND E.InputType = 'CODE'
           AND
               (
                   E.CodeGroupId IS NULL
                   OR NOT EXISTS
                      (
                          SELECT 1
                            FROM dbo.InfoCommonCodeGroup CG
                            JOIN dbo.InfoCommonCode CC
                              ON CC.CodeGroupId = CG.CodeGroupId
                           WHERE CG.CodeGroupId = E.CodeGroupId
                             AND CG.UseYn = 'Y'
                             AND CC.UseYn = 'Y'
                             AND CC.Code = N.ResultValue
                      )
               )
         ORDER BY E.CheckupItemId;

        IF @ErrorItemName IS NOT NULL
        BEGIN
            SET @ErrorMessage = N'사용할 수 없는 코드값입니다. 항목: ' + @ErrorItemName;
            RAISERROR(N'%s', 16, 1, @ErrorMessage);
        END;

        /* 기존 저장값의 실제 변경/삭제만 변경 이력으로 기록 */
        INSERT INTO dbo.HistCheckupResult
               (ReceptionId, CheckupItemId, BeforeValue, AfterValue,
                ProcessedAt, ProcessedBy)
        SELECT @ReceptionId
             , ICR.CheckupItemId
             , ICR.ResultValue
             , NR.ResultValue
             , SYSDATETIME()
             , @ProcessedBy
          FROM dbo.InfoCheckupResult ICR JOIN @NormalizedResults NR
                                           ON NR.CheckupItemId = ICR.CheckupItemId
         WHERE ICR.ReceptionId = @ReceptionId
           AND
               (
                   NR.ResultValue IS NULL
                   OR ICR.ResultValue <> NR.ResultValue
               );

        DELETE CR
          FROM dbo.InfoCheckupResult CR JOIN @NormalizedResults N
                                          ON N.CheckupItemId = CR.CheckupItemId
         WHERE CR.ReceptionId = @ReceptionId
           AND N.ResultValue IS NULL;

        SET @DeletedCount = @@ROWCOUNT;

        UPDATE CR
           SET CR.ResultValue = N.ResultValue,
               CR.UpdatedAt   = SYSDATETIME(),
               CR.UpdatedBy   = @ProcessedBy
          FROM dbo.InfoCheckupResult CR JOIN @NormalizedResults N
                                          ON N.CheckupItemId = CR.CheckupItemId
         WHERE CR.ReceptionId = @ReceptionId
           AND N.ResultValue IS NOT NULL
           AND CR.ResultValue <> N.ResultValue;

        SET @UpdatedCount = @@ROWCOUNT;

        INSERT INTO dbo.InfoCheckupResult
               (ReceptionId, CheckupItemId, ResultValue, UpdatedAt, UpdatedBy)
        SELECT @ReceptionId
             , N.CheckupItemId
             , N.ResultValue
             , SYSDATETIME()
             , @ProcessedBy
          FROM @NormalizedResults N
         WHERE N.ResultValue IS NOT NULL
           AND NOT EXISTS
               (
                   SELECT 1
                     FROM dbo.InfoCheckupResult CR
                    WHERE CR.ReceptionId = @ReceptionId
                      AND CR.CheckupItemId = N.CheckupItemId
               );

        SET @InsertedCount = @@ROWCOUNT;

        IF @BeforeStatus = 'N'
        BEGIN
            UPDATE dbo.InfoReception
               SET ResultStatus = 'S'
             WHERE ReceptionId = @ReceptionId;

            INSERT INTO dbo.HistReceptionStatus
                   (ReceptionId, BeforeStatus, AfterStatus, ProcessType,
                    ProcessedAt, ProcessedBy)
            VALUES (@ReceptionId, 'N', 'S', 'SAVE',
                    SYSDATETIME(), @ProcessedBy);
        END;

        SELECT @SavedCount = COUNT(*)
          FROM dbo.InfoCheckupResult
         WHERE ReceptionId = @ReceptionId;

        COMMIT TRANSACTION;

        SELECT @ReceptionId AS ReceptionId
             , CAST('S' AS CHAR(1)) AS ResultStatus
             , @InsertedCount AS InsertedCount
             , @UpdatedCount AS UpdatedCount
             , @DeletedCount AS DeletedCount
             , @SavedCount AS SavedResultCount;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO


