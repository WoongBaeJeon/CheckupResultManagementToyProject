USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_RECEPTION_FINALIZE_CANCEL] 스크립트 날짜: 2026-09-17 오전 9:23:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   접수 결과 확정취소

   입력
     @ReceptionId : 확정취소할 접수 ID
     @ProcessedBy : 처리자

   처리
     - F 상태에서만 확정취소 가능
     - 결과값은 유지
     - F -> S 상태 변경 및 CANCEL 이력 기록

   출력: 변경된 접수 상태 1행
   ============================================================= */
CREATE PROCEDURE [dbo].[USP_RECEPTION_FINALIZE_CANCEL]
    @ReceptionId INT,
    @ProcessedBy VARCHAR(50) = 'TEST_USER'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @BeforeStatus CHAR(1);

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
        FROM dbo.InfoReception AS R WITH (UPDLOCK, HOLDLOCK)
        WHERE R.ReceptionId = @ReceptionId;

        IF @BeforeStatus IS NULL
        BEGIN
            RAISERROR(N'존재하지 않는 접수입니다.', 16, 1);
        END;

        IF @BeforeStatus <> 'F'
        BEGIN
            RAISERROR(N'확정 상태의 접수만 확정취소할 수 있습니다.', 16, 1);
        END;

        UPDATE dbo.InfoReception
           SET ResultStatus = 'S'
         WHERE ReceptionId = @ReceptionId;

        INSERT INTO dbo.HistReceptionStatus
               (ReceptionId, BeforeStatus, AfterStatus, ProcessType,
                ProcessedAt, ProcessedBy)
        VALUES (@ReceptionId, 'F', 'S', 'CANCEL',
                SYSDATETIME(), @ProcessedBy);

        COMMIT TRANSACTION;

        SELECT
            @ReceptionId AS ReceptionId,
            CAST('S' AS CHAR(1)) AS ResultStatus,
            CAST('CANCEL' AS VARCHAR(20)) AS ProcessType;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO


