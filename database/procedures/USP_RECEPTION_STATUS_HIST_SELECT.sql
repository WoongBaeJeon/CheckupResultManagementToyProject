USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_RECEPTION_STATUS_HIST_SELECT] 스크립트 날짜: 2026-09-17 오전 9:20:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   접수 상태 이력 조회

   입력
     @ReceptionId : 접수 ID

     exec [USP_RECEPTION_STATUS_HIST_SELECT] @ReceptionId=11
   ============================================================= */
CREATE PROCEDURE [dbo].[USP_RECEPTION_STATUS_HIST_SELECT]
    @ReceptionId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @ReceptionId IS NULL
        THROW 51008, N'접수 ID는 필수입니다.', 1;

    IF NOT EXISTS
       (
           SELECT 1
           FROM dbo.InfoReception
           WHERE ReceptionId = @ReceptionId
       )
    BEGIN
        ;THROW 51009, N'존재하지 않는 접수입니다.', 1;
    END;

    SELECT HRS.StatusHistId
         , HRS.ReceptionId
         , HRS.BeforeStatus
         , CASE HRS.BeforeStatus
              WHEN 'N' THEN N'미입력'
              WHEN 'S' THEN N'저장완료'
              WHEN 'F' THEN N'확정'
           END AS BeforeStatusName
         , HRS.AfterStatus
         , CASE HRS.AfterStatus
              WHEN 'N' THEN N'미입력'
              WHEN 'S' THEN N'저장완료'
              WHEN 'F' THEN N'확정'
           END AS AfterStatusName
         , HRS.ProcessType
         , CASE HRS.ProcessType
              WHEN 'SAVE'       THEN N'저장'
              WHEN 'FINALIZE'   THEN N'확정'
              WHEN 'CANCEL'     THEN N'확정취소'
              WHEN 'REFINALIZE' THEN N'재확정'
           END AS ProcessTypeName
         , HRS.ProcessedAt
         , HRS.ProcessedBy
     FROM dbo.HistReceptionStatus AS HRS
    WHERE HRS.ReceptionId = @ReceptionId
    ORDER BY HRS.ProcessedAt DESC
           , HRS.StatusHistId DESC;
END;
GO


