USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_RECEPTION_SEARCH] 스크립트 날짜: 2026-09-17 오전 9:20:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   접수 목록 조회

   입력
     @ReceptionDateFrom : 접수 시작일, NULL이면 제한 없음
     @ReceptionDateTo   : 접수 종료일, NULL이면 제한 없음
     @SearchKeyword    : 차트번호/수검자명 부분검색, NULL/공백이면 제한 없음

   exec [dbo].[USP_RECEPTION_SEARCH] @ReceptionDateFrom=null, @ReceptionDateTo=null, @SearchKeyword=null
   ============================================================= */
CREATE PROCEDURE [dbo].[USP_RECEPTION_SEARCH]
    @ReceptionDateFrom DATE = NULL,
    @ReceptionDateTo   DATE = NULL,
    @SearchKeyword     NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @SearchKeyword =
        NULLIF(LTRIM(RTRIM(@SearchKeyword)), N'');

    IF @ReceptionDateFrom IS NOT NULL
       AND @ReceptionDateTo IS NOT NULL
       AND @ReceptionDateFrom > @ReceptionDateTo
    BEGIN
        ;THROW 51001, N'접수 시작일은 종료일보다 늦을 수 없습니다.', 1;
    END;

    SELECT R.ReceptionId
         , R.ReceptionDate
         , P.PatientId
         , P.ChartNo
         , P.PatientName
         , P.Gender
         , P.SocialNumber
         , R.PatientAge
         , R.ResultStatus
         , CASE R.ResultStatus
                WHEN 'N' THEN N'미입력'
                WHEN 'S' THEN N'저장완료'
                WHEN 'F' THEN N'확정'
           END AS ResultStatusName
         , R.ReceptionMemo
     FROM dbo.InfoReception AS R JOIN dbo.InfoPatient AS P
                                   ON P.PatientId = R.PatientId
     WHERE (@ReceptionDateFrom IS NULL
            OR R.ReceptionDate >= @ReceptionDateFrom)
       AND (@ReceptionDateTo IS NULL
            OR R.ReceptionDate < DATEADD(DAY, 1, @ReceptionDateTo))
       AND
       (
           @SearchKeyword IS NULL
           OR P.ChartNo LIKE
              '%' + CONVERT(VARCHAR(50), @SearchKeyword) + '%'
           OR P.PatientName LIKE
              N'%' + @SearchKeyword + N'%'
       )
    ORDER BY R.ReceptionDate DESC,
             R.ReceptionId DESC;
END;
GO


