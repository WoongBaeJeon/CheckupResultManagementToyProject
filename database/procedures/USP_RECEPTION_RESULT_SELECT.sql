USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_RECEPTION_RESULT_SELECT] 스크립트 날짜: 2026-09-17 오전 9:36:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* 결과 조회 시 화면 입력코드도 함께 반환한다. */
CREATE   PROCEDURE [dbo].[USP_RECEPTION_RESULT_SELECT]
    @ReceptionId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @ReceptionId IS NULL
        THROW 51004, N'접수 ID는 필수입니다.', 1;

    IF NOT EXISTS
       (
           SELECT 1
           FROM dbo.InfoReception
           WHERE ReceptionId = @ReceptionId
       )
    BEGIN
        ;THROW 51005, N'존재하지 않는 접수입니다.', 1;
    END;

    /* Result Set 1: 접수/환자 기본정보 */
    SELECT R.ReceptionId,
           R.ReceptionDate,
           P.PatientId,
           P.ChartNo,
           P.PatientName,
           P.Gender,
           P.SocialNumber,
           R.PatientAge,
           R.ResultStatus,
           CASE R.ResultStatus
               WHEN 'N' THEN N'미입력'
               WHEN 'S' THEN N'저장완료'
               WHEN 'F' THEN N'확정'
           END AS ResultStatusName,
           R.ReceptionMemo,
           CONVERT(BIT,
               CASE WHEN R.ResultStatus = 'F' THEN 1 ELSE 0 END) AS IsFinalized
      FROM dbo.InfoReception AS R
      JOIN dbo.InfoPatient AS P
        ON P.PatientId = R.PatientId
     WHERE R.ReceptionId = @ReceptionId;

    /* Result Set 2: 전체 검사항목과 현재 결과 */
    SELECT C.CheckupClassId,
           C.ClassCode,
           C.ClassName,
           C.SortOrder AS ClassSortOrder,
           G.CheckupItemGroupId,
           G.GroupCode,
           G.GroupName,
           G.SortOrder AS GroupSortOrder,
           I.CheckupItemId,
           I.ItemCode,
           I.ItemName,
           I.InputType,
           I.CodeGroupId,
           I.Unit,
           I.RequiredYn,
           I.MinValue,
           I.MaxValue,
           I.MaxLength,
           I.SortOrder AS ItemSortOrder,
           CR.ResultId,
           CR.ResultValue,
           CR.UpdatedAt,
           CR.UpdatedBy
      FROM dbo.InfoReceptionCheckup AS RC
      JOIN dbo.InfoCheckupClass AS C
        ON C.CheckupClassId = RC.CheckupClassId
      JOIN dbo.InfoCheckupItemGroup AS G
        ON G.CheckupClassId = C.CheckupClassId
      JOIN dbo.InfoCheckupItem AS I
        ON I.CheckupItemGroupId = G.CheckupItemGroupId
      LEFT JOIN dbo.InfoCheckupResult AS CR
        ON CR.ReceptionId = RC.ReceptionId
       AND CR.CheckupItemId = I.CheckupItemId
     WHERE RC.ReceptionId = @ReceptionId
       AND C.UseYn = 'Y'
       AND G.UseYn = 'Y'
       AND I.UseYn = 'Y'
     ORDER BY C.SortOrder,
              C.CheckupClassId,
              G.SortOrder,
              G.CheckupItemGroupId,
              I.SortOrder,
              I.CheckupItemId;

    /* Result Set 3: 현재 접수에서 사용하는 CODE 항목 선택지 */
    SELECT DISTINCT
           CG.CodeGroupId,
           CG.GroupCode,
           CG.GroupName,
           CC.CommonCodeId,
           CC.Code,
           CC.InputCode,
           CC.CodeName,
           CC.SortOrder
      FROM dbo.InfoReceptionCheckup AS RC
      JOIN dbo.InfoCheckupClass AS C
        ON C.CheckupClassId = RC.CheckupClassId
      JOIN dbo.InfoCheckupItemGroup AS G
        ON G.CheckupClassId = C.CheckupClassId
      JOIN dbo.InfoCheckupItem AS I
        ON I.CheckupItemGroupId = G.CheckupItemGroupId
      JOIN dbo.InfoCommonCodeGroup AS CG
        ON CG.CodeGroupId = I.CodeGroupId
      JOIN dbo.InfoCommonCode AS CC
        ON CC.CodeGroupId = CG.CodeGroupId
     WHERE RC.ReceptionId = @ReceptionId
       AND C.UseYn = 'Y'
       AND G.UseYn = 'Y'
       AND I.UseYn = 'Y'
       AND I.InputType = 'CODE'
       AND CG.UseYn = 'Y'
       AND CC.UseYn = 'Y'
     ORDER BY CG.CodeGroupId,
              CC.SortOrder,
              CC.CommonCodeId;
END;

GO


