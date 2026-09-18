USE [check_result_db]
GO

/****** 개체: StoredProcedure [dbo].[USP_OPINION_PHRASE_LIST] 스크립트 날짜: 2026-09-17 오전 9:23:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/* =============================================================
   소견 상용구 조회

   입력: @CheckupItemId - 현재 선택한 MEMO 검사항목 ID

   exec [USP_OPINION_PHRASE_LIST] @CheckupItemId=1
   ============================================================= */
CREATE   PROCEDURE [dbo].[USP_OPINION_PHRASE_LIST]
    @CheckupItemId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
          FROM dbo.InfoCheckupItem
         WHERE CheckupItemId = @CheckupItemId
           AND InputType = 'MEMO'
           AND UseYn = 'Y'
    )
    BEGIN
        RAISERROR(N'사용 가능한 소견 검사항목이 아닙니다.', 16, 1);
        RETURN;
    END;

    SELECT OpinionPhraseId
         , CheckupItemId
         , CASE WHEN CheckupItemId IS NULL
               THEN N'공통'
               ELSE N'검사항목'
           END AS ScopeName
         , PhraseName
         , PhraseText
         , SortOrder
     FROM dbo.InfoOpinionPhrase 
    WHERE UseYn = 'Y'
      AND (CheckupItemId IS NULL OR CheckupItemId = @CheckupItemId)
    ORDER BY CASE WHEN CheckupItemId IS NULL 
                  THEN 0 ELSE 1 
                  END
           , SortOrder
           , OpinionPhraseId;
END;
GO


