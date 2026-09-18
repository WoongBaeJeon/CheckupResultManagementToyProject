using System;
using System.Collections.Generic;
using infoCheckupResult.Models;

namespace infoCheckupResult.Repositories
{
    public interface IReceptionRepository
    {
        IList<ReceptionDto> Search(
            DateTime? receptionDateFrom,
            DateTime? receptionDateTo,
            string searchKeyword);

        IList<CheckupStructureDto> GetExamStructure(int receptionId);
        ReceptionResultData GetReceptionResult(int receptionId);
        SaveResultDto SaveResults(
            int receptionId,
            IEnumerable<CheckupResultDto> results,
            string processedBy);
        StatusChangeResultDto FinalizeReception(
            int receptionId,
            string processedBy);
        StatusChangeResultDto CancelReceptionFinalize(
            int receptionId,
            string processedBy);
        IList<ResultHistoryDto> GetResultHistory(int receptionId);
        IList<StatusHistoryDto> GetStatusHistory(int receptionId);
        IList<OpinionPhraseDto> GetOpinionPhrases(int checkupItemId);
    }
}
