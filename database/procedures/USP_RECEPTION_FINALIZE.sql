USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_RECEPTION_FINALIZE] 스크립트 날짜: 2026-09-17 오전 9:23:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   접수 결과 확정

   입력
     @ReceptionId : 확정할 접수 ID
     @ProcessedBy : 처리자

   처리
     - S 상태에서만 확정 가능
     - 전체 활성 필수항목이 저장되어 있고 값이 유효해야 함
     - S -> F 상태 변경 및 상태 이력 기록
     - CANCEL 이력이 있으면 REFINALIZE, 없으면 FINALIZE

   출력: 변경된 접수 상태 1행
   ============================================================= */
CREATE   PROCEDURE [dbo].[USP_RECEPTION_FINALIZE]
    @ReceptionId INT,
    @ProcessedBy VARCHAR(50) = 'TEST_USER'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @BeforeStatus  CHAR(1);
    DECLARE @ProcessType   VARCHAR(20);
    DECLARE @ErrorItemName NVARCHAR(100);
    DECLARE @ErrorMessage  NVARCHAR(2048);

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

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @BeforeStatus = R.ResultStatus
          FROM dbo.InfoReception R WITH (UPDLOCK, HOLDLOCK)
         WHERE R.ReceptionId = @ReceptionId;

        IF @BeforeStatus IS NULL
        BEGIN
            RAISERROR(N'존재하지 않는 접수입니다.', 16, 1);
        END;

        IF @BeforeStatus <> 'S'
        BEGIN
            RAISERROR(N'저장완료 상태의 접수만 확정할 수 있습니다.', 16, 1);
        END;

        /* 필수항목 누락 검사 */
        SELECT TOP 1 @ErrorItemName = ICI.ItemName
          FROM dbo.InfoReceptionCheckup IRC JOIN dbo.InfoCheckupClass ICC
                                              ON ICC.CheckupClassId = IRC.CheckupClassId
                                            JOIN dbo.InfoCheckupItemGroup ICIG
                                              ON ICIG.CheckupClassId = ICC.CheckupClassId
                                            JOIN dbo.InfoCheckupItem ICI
                                              ON ICI.CheckupItemGroupId = ICIG.CheckupItemGroupId
                                            LEFT JOIN dbo.InfoCheckupResult ICR
                                              ON ICR.ReceptionId = IRC.ReceptionId
                                             AND ICR.CheckupItemId = ICI.CheckupItemId
          WHERE IRC.ReceptionId = @ReceptionId
            AND ICC.UseYn = 'Y'
            AND ICIG.UseYn = 'Y'
            AND ICI.UseYn = 'Y'
            AND ICI.RequiredYn = 'Y'
            AND
                (
                    ICR.ResultId IS NULL
                    OR NULLIF(LTRIM(RTRIM(ICR.ResultValue)), N'') IS NULL
                )
          ORDER BY ICC.SortOrder, ICIG.SortOrder, ICI.SortOrder, ICI.CheckupItemId;

        IF @ErrorItemName IS NOT NULL
        BEGIN
            SET @ErrorMessage = N'필수 검사결과가 입력되지 않았습니다. 항목: ' + @ErrorItemName;
            RAISERROR(N'%s', 16, 1, @ErrorMessage);
        END;

        /* 저장된 숫자 결과 유효성 재검사 */
        SET @ErrorItemName = NULL;

        SELECT TOP 1 @ErrorItemName = ICI.ItemName
          FROM dbo.InfoReceptionCheckup IRC JOIN dbo.InfoCheckupClass ICC
                                              ON ICC.CheckupClassId = IRC.CheckupClassId
                                            JOIN dbo.InfoCheckupItemGroup ICIG
                                              ON ICIG.CheckupClassId = ICC.CheckupClassId
                                            JOIN dbo.InfoCheckupItem ICI
                                              ON ICI.CheckupItemGroupId = ICIG.CheckupItemGroupId
                                            JOIN dbo.InfoCheckupResult ICR
                                              ON ICR.ReceptionId = IRC.ReceptionId
                                             AND ICR.CheckupItemId = ICI.CheckupItemId
                                           WHERE IRC.ReceptionId = @ReceptionId
                                             AND ICC.UseYn = 'Y'
                                             AND ICIG.UseYn = 'Y'
                                             AND ICI.UseYn = 'Y'
                                             AND ICI.InputType = 'NUM'
                                             AND
                                                 (
                                                     TRY_CONVERT(DECIMAL(18,4), ICR.ResultValue) IS NULL
                                                     OR (ICI.MinValue IS NOT NULL
                                                         AND TRY_CONVERT(DECIMAL(18,4), ICR.ResultValue) < ICI.MinValue)
                                                     OR (ICI.MaxValue IS NOT NULL
                                                         AND TRY_CONVERT(DECIMAL(18,4), ICR.ResultValue) > ICI.MaxValue)
                                                 )
                                           ORDER BY ICI.CheckupItemId;

        IF @ErrorItemName IS NOT NULL
        BEGIN
            SET @ErrorMessage = N'숫자 형식 또는 허용 범위를 확인하세요. 항목: ' + @ErrorItemName;
            RAISERROR(N'%s', 16, 1, @ErrorMessage);
        END;

        /* 저장된 TEXT/MEMO 길이 유효성 재검사 */
        SET @ErrorItemName = NULL;

        SELECT TOP 1 @ErrorItemName = ICI.ItemName
          FROM dbo.InfoReceptionCheckup IRC JOIN dbo.InfoCheckupClass ICC
                                              ON ICC.CheckupClassId = IRC.CheckupClassId
                                            JOIN dbo.InfoCheckupItemGroup ICIG
                                              ON ICIG.CheckupClassId = ICC.CheckupClassId
                                            JOIN dbo.InfoCheckupItem ICI
                                              ON ICI.CheckupItemGroupId = ICIG.CheckupItemGroupId
                                            JOIN dbo.InfoCheckupResult ICR
                                              ON ICR.ReceptionId = IRC.ReceptionId
                                             AND ICR.CheckupItemId = ICI.CheckupItemId
          WHERE IRC.ReceptionId = @ReceptionId
            AND ICC.UseYn = 'Y'
            AND ICIG.UseYn = 'Y'
            AND ICI.UseYn = 'Y'
            AND ICI.InputType IN ('TEXT', 'MEMO')
            AND ICI.MaxLength IS NOT NULL
            AND LEN(ICR.ResultValue) > ICI.MaxLength
          ORDER BY ICI.CheckupItemId;

        IF @ErrorItemName IS NOT NULL
        BEGIN
            SET @ErrorMessage = N'최대 입력 길이를 초과했습니다. 항목: ' + @ErrorItemName;
            RAISERROR(N'%s', 16, 1, @ErrorMessage);
        END;

        /* 저장된 코드값 유효성 재검사 */
        SET @ErrorItemName = NULL;

        SELECT TOP 1 @ErrorItemName = ICI.ItemName
          FROM dbo.InfoReceptionCheckup IRC JOIN dbo.InfoCheckupClass ICC
                                             ON ICC.CheckupClassId = IRC.CheckupClassId
                                           JOIN dbo.InfoCheckupItemGroup ICIG
                                             ON ICIG.CheckupClassId = ICC.CheckupClassId
                                           JOIN dbo.InfoCheckupItem ICI
                                             ON ICI.CheckupItemGroupId = ICIG.CheckupItemGroupId
                                           JOIN dbo.InfoCheckupResult ICR
                                             ON ICR.ReceptionId = IRC.ReceptionId
                                            AND ICR.CheckupItemId = ICI.CheckupItemId
          WHERE IRC.ReceptionId = @ReceptionId
            AND ICC.UseYn = 'Y'
            AND ICIG.UseYn = 'Y'
            AND ICI.UseYn = 'Y'
            AND ICI.InputType = 'CODE'
            AND
                (
                    ICI.CodeGroupId IS NULL
                    OR NOT EXISTS
                       (
                           SELECT 1
                           FROM dbo.InfoCommonCodeGroup AS CG
                           JOIN dbo.InfoCommonCode AS CC
                             ON CC.CodeGroupId = CG.CodeGroupId
                           WHERE CG.CodeGroupId = ICI.CodeGroupId
                             AND CG.UseYn = 'Y'
                             AND CC.UseYn = 'Y'
                             AND CC.Code = ICR.ResultValue
                       )
                )
          ORDER BY ICI.CheckupItemId;

        IF @ErrorItemName IS NOT NULL
        BEGIN
            SET @ErrorMessage = N'사용할 수 없는 코드값입니다. 항목: ' + @ErrorItemName;
            RAISERROR(N'%s', 16, 1, @ErrorMessage);
        END;

        IF EXISTS
           (
               SELECT 1
                 FROM dbo.HistReceptionStatus
                WHERE ReceptionId = @ReceptionId
                  AND ProcessType = 'CANCEL'
           )
            SET @ProcessType = 'REFINALIZE';
        ELSE
            SET @ProcessType = 'FINALIZE';

        UPDATE dbo.InfoReception
           SET ResultStatus = 'F'
         WHERE ReceptionId = @ReceptionId;

        INSERT INTO dbo.HistReceptionStatus
               (ReceptionId, BeforeStatus, AfterStatus, ProcessType,
                ProcessedAt, ProcessedBy)
        VALUES (@ReceptionId, 'S', 'F', @ProcessType,
                SYSDATETIME(), @ProcessedBy);

        COMMIT TRANSACTION;

        SELECT @ReceptionId AS ReceptionId
             , CAST('F' AS CHAR(1)) AS ResultStatus
             , @ProcessType AS ProcessType;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO


