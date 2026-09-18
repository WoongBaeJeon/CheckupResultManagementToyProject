USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_EXAM_RESULT_HIST_SELECT] 스크립트 날짜: 2026-09-17 오전 9:06:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   검사결과 변경 이력 조회

   입력
     @ReceptionId : 접수 ID

   exec [dbo].[USP_EXAM_RESULT_HIST_SELECT] @ReceptionId=11
   ============================================================= */
CREATE PROCEDURE [dbo].[USP_EXAM_RESULT_HIST_SELECT]
    @ReceptionId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @ReceptionId IS NULL
        THROW 51006, N'접수 ID는 필수입니다.', 1;

    IF NOT EXISTS
       (
           SELECT 1
           FROM dbo.InfoReception
           WHERE ReceptionId = @ReceptionId
       )
    BEGIN
        ;THROW 51007, N'존재하지 않는 접수입니다.', 1;
    END;

    SELECT HCR.ResultHistId
         , HCR.ReceptionId
         , ICC.CheckupClassId
         , ICC.ClassCode
         , ICC.ClassName
         , ICIG.CheckupItemGroupId
         , ICIG.GroupCode
         , ICIG.GroupName
         , ICI.CheckupItemId
         , ICI.ItemCode
         , ICI.ItemName
         , HCR.BeforeValue
         , HCR.AfterValue
         , HCR.ProcessedAt
         , HCR.ProcessedBy
      FROM dbo.HistCheckupResult HCR JOIN dbo.InfoCheckupItem ICI
                                     ON ICI.CheckupItemId = HCR.CheckupItemId
                                   JOIN dbo.InfoCheckupItemGroup ICIG
                                     ON ICIG.CheckupItemGroupId = ICI.CheckupItemGroupId
                                   JOIN dbo.InfoCheckupClass ICC
                                     ON ICC.CheckupClassId = ICIG.CheckupClassId
      WHERE HCR.ReceptionId = @ReceptionId
      ORDER BY HCR.ProcessedAt DESC,
               HCR.ResultHistId DESC;
END;
GO


