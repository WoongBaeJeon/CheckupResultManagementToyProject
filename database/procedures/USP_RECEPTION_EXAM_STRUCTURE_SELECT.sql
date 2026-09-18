USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_RECEPTION_EXAM_STRUCTURE_SELECT] 스크립트 날짜: 2026-09-17 오전 9:23:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   접수별 검진종류/검사그룹 Tree 조회

   입력
     @ReceptionId : 접수 ID
     1 : 공단검진
     2 : 학생검진
     3 : 암검진
     4 : 기타검진

   출력: 검진종류와 검사그룹 조합 1건당 1행
   주의: Tree는 화면 필터이며 저장/확정 범위를 제한하지 않는다.

   exec [dbo].[USP_RECEPTION_EXAM_STRUCTURE_SELECT] @ReceptionId=1
   ============================================================= */
CREATE PROCEDURE [dbo].[USP_RECEPTION_EXAM_STRUCTURE_SELECT]
    @ReceptionId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @ReceptionId IS NULL
        THROW 51002, N'접수 ID는 필수입니다.', 1;

    IF NOT EXISTS
       (
           SELECT 1
           FROM dbo.InfoReception
           WHERE ReceptionId = @ReceptionId
       )
    BEGIN
        ;THROW 51003, N'존재하지 않는 접수입니다.', 1;
    END;

    SELECT ICC.CheckupClassId
         , ICC.ClassCode
         , ICC.ClassName
         , ICC.SortOrder AS ClassSortOrder
         , ICIG.CheckupItemGroupId
         , ICIG.GroupCode
         , ICIG.GroupName
         , ICIG.SortOrder AS GroupSortOrder
      FROM dbo.InfoReceptionCheckup RC JOIN dbo.InfoCheckupClass ICC
                                         ON ICC.CheckupClassId = RC.CheckupClassId
                                       JOIN dbo.InfoCheckupItemGroup ICIG
                                         ON ICIG.CheckupClassId = ICC.CheckupClassId
      WHERE RC.ReceptionId = @ReceptionId
        AND ICC.UseYn = 'Y'
        AND ICIG.UseYn = 'Y'
        AND EXISTS
            (
                SELECT 1
                FROM dbo.InfoCheckupItem AS ICI
                WHERE ICI.CheckupItemGroupId = ICIG.CheckupItemGroupId
                  AND ICI.UseYn = 'Y'
            )
      ORDER BY ICC.SortOrder,
               ICC.CheckupClassId,
               ICIG.SortOrder,
               ICIG.CheckupItemGroupId;
END;
GO


