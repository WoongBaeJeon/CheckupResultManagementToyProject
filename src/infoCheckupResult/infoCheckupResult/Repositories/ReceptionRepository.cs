using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using infoCheckupResult.Data;
using infoCheckupResult.Models;

namespace infoCheckupResult.Repositories
{
    public class ReceptionRepository : IReceptionRepository
    {
        public IList<ReceptionDto> Search(
            DateTime? receptionDateFrom,
            DateTime? receptionDateTo,
            string searchKeyword)
        {
            DataTable dataTable = DbHelper.ExecuteDataTable(
                "dbo.USP_RECEPTION_SEARCH",
                CreateNullableParameter("@ReceptionDateFrom", SqlDbType.Date, receptionDateFrom),
                CreateNullableParameter("@ReceptionDateTo", SqlDbType.Date, receptionDateTo),
                CreateNullableParameter("@SearchKeyword", SqlDbType.NVarChar, searchKeyword, 50));

            List<ReceptionDto> items = new List<ReceptionDto>();

            foreach (DataRow row in dataTable.Rows)
                items.Add(MapReception(row));

            return items;
        }

        public IList<CheckupStructureDto> GetExamStructure(int receptionId)
        {
            DataTable dataTable = DbHelper.ExecuteDataTable(
                "dbo.USP_RECEPTION_EXAM_STRUCTURE_SELECT",
                new SqlParameter("@ReceptionId", SqlDbType.Int) { Value = receptionId });

            List<CheckupStructureDto> items = new List<CheckupStructureDto>();

            foreach (DataRow row in dataTable.Rows)
            {
                items.Add(new CheckupStructureDto
                {
                    CheckupClassId = Convert.ToInt32(row["CheckupClassId"]),
                    ClassCode = ToNullableString(row, "ClassCode"),
                    ClassName = ToNullableString(row, "ClassName"),
                    ClassSortOrder = Convert.ToInt32(row["ClassSortOrder"]),
                    CheckupItemGroupId = Convert.ToInt32(row["CheckupItemGroupId"]),
                    GroupCode = ToNullableString(row, "GroupCode"),
                    GroupName = ToNullableString(row, "GroupName"),
                    GroupSortOrder = Convert.ToInt32(row["GroupSortOrder"])
                });
            }

            return items;
        }

        public ReceptionResultData GetReceptionResult(int receptionId)
        {
            DataSet dataSet = DbHelper.ExecuteDataSet(
                "dbo.USP_RECEPTION_RESULT_SELECT",
                new SqlParameter("@ReceptionId", SqlDbType.Int) { Value = receptionId });

            if (dataSet.Tables.Count < 3)
                throw new DataException("결과 조회 프로시저의 Result Set 구성이 올바르지 않습니다.");

            ReceptionResultData result = new ReceptionResultData();

            if (dataSet.Tables[0].Rows.Count > 0)
                result.Header = MapReceptionHeader(dataSet.Tables[0].Rows[0]);

            foreach (DataRow row in dataSet.Tables[1].Rows)
                result.Results.Add(MapCheckupResult(row));

            foreach (DataRow row in dataSet.Tables[2].Rows)
                result.CommonCodes.Add(MapCommonCode(row));

            return result;
        }

        public SaveResultDto SaveResults(
            int receptionId,
            IEnumerable<CheckupResultDto> results,
            string processedBy)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("CheckupItemId", typeof(int));
            resultTable.Columns.Add("ResultValue", typeof(string));

            foreach (CheckupResultDto item in results)
            {
                DataRow row = resultTable.NewRow();
                row["CheckupItemId"] = item.CheckupItemId;
                row["ResultValue"] = string.IsNullOrWhiteSpace(item.ResultValue)
                    ? (object)DBNull.Value
                    : item.ResultValue.Trim();
                resultTable.Rows.Add(row);
            }

            SqlParameter resultParameter = new SqlParameter("@Results", SqlDbType.Structured);
            resultParameter.TypeName = "dbo.CheckupResultSaveType";
            resultParameter.Value = resultTable;

            DataTable responseTable = DbHelper.ExecuteDataTable(
                "dbo.USP_RECEPTION_RESULT_SAVE",
                new SqlParameter("@ReceptionId", SqlDbType.Int) { Value = receptionId },
                resultParameter,
                new SqlParameter("@ProcessedBy", SqlDbType.VarChar, 50)
                {
                    Value = string.IsNullOrWhiteSpace(processedBy)
                        ? "TEST_USER"
                        : processedBy.Trim()
                });

            if (responseTable.Rows.Count == 0)
            {
                throw new DataException(
                    "저장 프로시저의 처리 결과가 없습니다.");
            }

            DataRow resultRow = responseTable.Rows[0];

            return new SaveResultDto
            {
                ReceptionId = Convert.ToInt32(resultRow["ReceptionId"]),
                ResultStatus = ToNullableString(resultRow, "ResultStatus"),
                InsertedCount = Convert.ToInt32(resultRow["InsertedCount"]),
                UpdatedCount = Convert.ToInt32(resultRow["UpdatedCount"]),
                DeletedCount = Convert.ToInt32(resultRow["DeletedCount"]),
                SavedResultCount = Convert.ToInt32(resultRow["SavedResultCount"])
            };
        }

        public StatusChangeResultDto FinalizeReception(
            int receptionId,
            string processedBy)
        {
            return ChangeReceptionStatus(
                "dbo.USP_RECEPTION_FINALIZE",
                receptionId,
                processedBy);
        }

        public StatusChangeResultDto CancelReceptionFinalize(
            int receptionId,
            string processedBy)
        {
            return ChangeReceptionStatus(
                "dbo.USP_RECEPTION_FINALIZE_CANCEL",
                receptionId,
                processedBy);
        }

        public IList<ResultHistoryDto> GetResultHistory(int receptionId)
        {
            DataTable dataTable = DbHelper.ExecuteDataTable(
                "dbo.USP_EXAM_RESULT_HIST_SELECT",
                new SqlParameter("@ReceptionId", SqlDbType.Int) { Value = receptionId });

            List<ResultHistoryDto> items = new List<ResultHistoryDto>();

            foreach (DataRow row in dataTable.Rows)
                items.Add(MapResultHistory(row));

            return items;
        }

        public IList<StatusHistoryDto> GetStatusHistory(int receptionId)
        {
            DataTable dataTable = DbHelper.ExecuteDataTable(
                "dbo.USP_RECEPTION_STATUS_HIST_SELECT",
                new SqlParameter("@ReceptionId", SqlDbType.Int) { Value = receptionId });

            List<StatusHistoryDto> items = new List<StatusHistoryDto>();

            foreach (DataRow row in dataTable.Rows)
                items.Add(MapStatusHistory(row));

            return items;
        }

        public IList<OpinionPhraseDto> GetOpinionPhrases(int checkupItemId)
        {
            DataTable dataTable = DbHelper.ExecuteDataTable(
                "dbo.USP_OPINION_PHRASE_LIST",
                new SqlParameter("@CheckupItemId", SqlDbType.Int)
                {
                    Value = checkupItemId
                });

            List<OpinionPhraseDto> items = new List<OpinionPhraseDto>();

            foreach (DataRow row in dataTable.Rows)
            {
                items.Add(new OpinionPhraseDto
                {
                    OpinionPhraseId = Convert.ToInt32(row["OpinionPhraseId"]),
                    CheckupItemId = ToNullableInt32(row, "CheckupItemId"),
                    ScopeName = ToNullableString(row, "ScopeName"),
                    PhraseName = ToNullableString(row, "PhraseName"),
                    PhraseText = ToNullableString(row, "PhraseText"),
                    SortOrder = Convert.ToInt32(row["SortOrder"])
                });
            }

            return items;
        }

        private static StatusChangeResultDto ChangeReceptionStatus(
            string procedureName,
            int receptionId,
            string processedBy)
        {
            DataTable dataTable = DbHelper.ExecuteDataTable(
                procedureName,
                new SqlParameter("@ReceptionId", SqlDbType.Int)
                {
                    Value = receptionId
                },
                new SqlParameter("@ProcessedBy", SqlDbType.VarChar, 50)
                {
                    Value = string.IsNullOrWhiteSpace(processedBy)
                        ? "TEST_USER"
                        : processedBy.Trim()
                });

            if (dataTable.Rows.Count == 0)
            {
                throw new DataException(
                    "상태 변경 프로시저의 처리 결과가 없습니다.");
            }

            DataRow row = dataTable.Rows[0];

            return new StatusChangeResultDto
            {
                ReceptionId = Convert.ToInt32(row["ReceptionId"]),
                ResultStatus = ToNullableString(row, "ResultStatus"),
                ProcessType = ToNullableString(row, "ProcessType")
            };
        }

        private static ReceptionDto MapReception(DataRow row)
        {
            return new ReceptionDto
            {
                ReceptionId = Convert.ToInt32(row["ReceptionId"]),
                ReceptionDate = Convert.ToDateTime(row["ReceptionDate"]),
                PatientId = Convert.ToInt32(row["PatientId"]),
                ChartNo = ToNullableString(row, "ChartNo"),
                PatientName = ToNullableString(row, "PatientName"),
                Gender = ToNullableString(row, "Gender"),
                SocialNumber = ToNullableString(row, "SocialNumber"),
                PatientAge = ToNullableInt16(row, "PatientAge"),
                ResultStatus = ToNullableString(row, "ResultStatus"),
                ResultStatusName = ToNullableString(row, "ResultStatusName"),
                ReceptionMemo = ToNullableString(row, "ReceptionMemo")
            };
        }

        private static ReceptionHeaderDto MapReceptionHeader(DataRow row)
        {
            return new ReceptionHeaderDto
            {
                ReceptionId = Convert.ToInt32(row["ReceptionId"]),
                ReceptionDate = Convert.ToDateTime(row["ReceptionDate"]),
                PatientId = Convert.ToInt32(row["PatientId"]),
                ChartNo = ToNullableString(row, "ChartNo"),
                PatientName = ToNullableString(row, "PatientName"),
                Gender = ToNullableString(row, "Gender"),
                SocialNumber = ToNullableString(row, "SocialNumber"),
                PatientAge = ToNullableInt16(row, "PatientAge"),
                ResultStatus = ToNullableString(row, "ResultStatus"),
                ResultStatusName = ToNullableString(row, "ResultStatusName"),
                ReceptionMemo = ToNullableString(row, "ReceptionMemo"),
                IsFinalized = Convert.ToBoolean(row["IsFinalized"])
            };
        }

        private static CheckupResultDto MapCheckupResult(DataRow row)
        {
            return new CheckupResultDto
            {
                CheckupClassId = Convert.ToInt32(row["CheckupClassId"]),
                ClassCode = ToNullableString(row, "ClassCode"),
                ClassName = ToNullableString(row, "ClassName"),
                ClassSortOrder = Convert.ToInt32(row["ClassSortOrder"]),
                CheckupItemGroupId = Convert.ToInt32(row["CheckupItemGroupId"]),
                GroupCode = ToNullableString(row, "GroupCode"),
                GroupName = ToNullableString(row, "GroupName"),
                GroupSortOrder = Convert.ToInt32(row["GroupSortOrder"]),
                CheckupItemId = Convert.ToInt32(row["CheckupItemId"]),
                ItemCode = ToNullableString(row, "ItemCode"),
                ItemName = ToNullableString(row, "ItemName"),
                InputType = ToNullableString(row, "InputType"),
                CodeGroupId = ToNullableInt32(row, "CodeGroupId"),
                Unit = ToNullableString(row, "Unit"),
                RequiredYn = ToNullableString(row, "RequiredYn"),
                MinValue = ToNullableDecimal(row, "MinValue"),
                MaxValue = ToNullableDecimal(row, "MaxValue"),
                MaxLength = ToNullableInt32(row, "MaxLength"),
                ItemSortOrder = Convert.ToInt32(row["ItemSortOrder"]),
                ResultId = ToNullableInt64(row, "ResultId"),
                ResultValue = ToNullableString(row, "ResultValue"),
                UpdatedAt = ToNullableDateTime(row, "UpdatedAt"),
                UpdatedBy = ToNullableString(row, "UpdatedBy")
            };
        }

        private static CommonCodeDto MapCommonCode(DataRow row)
        {
            return new CommonCodeDto
            {
                CodeGroupId = Convert.ToInt32(row["CodeGroupId"]),
                GroupCode = ToNullableString(row, "GroupCode"),
                GroupName = ToNullableString(row, "GroupName"),
                CommonCodeId = Convert.ToInt32(row["CommonCodeId"]),
                Code = ToNullableString(row, "Code"),
                InputCode = ToNullableString(row, "InputCode"),
                CodeName = ToNullableString(row, "CodeName"),
                SortOrder = Convert.ToInt32(row["SortOrder"])
            };
        }

        private static ResultHistoryDto MapResultHistory(DataRow row)
        {
            return new ResultHistoryDto
            {
                ResultHistId = Convert.ToInt64(row["ResultHistId"]),
                ReceptionId = Convert.ToInt32(row["ReceptionId"]),
                CheckupClassId = Convert.ToInt32(row["CheckupClassId"]),
                ClassCode = ToNullableString(row, "ClassCode"),
                ClassName = ToNullableString(row, "ClassName"),
                CheckupItemGroupId = Convert.ToInt32(row["CheckupItemGroupId"]),
                GroupCode = ToNullableString(row, "GroupCode"),
                GroupName = ToNullableString(row, "GroupName"),
                CheckupItemId = Convert.ToInt32(row["CheckupItemId"]),
                ItemCode = ToNullableString(row, "ItemCode"),
                ItemName = ToNullableString(row, "ItemName"),
                BeforeValue = ToNullableString(row, "BeforeValue"),
                AfterValue = ToNullableString(row, "AfterValue"),
                ProcessedAt = Convert.ToDateTime(row["ProcessedAt"]),
                ProcessedBy = ToNullableString(row, "ProcessedBy")
            };
        }

        private static StatusHistoryDto MapStatusHistory(DataRow row)
        {
            return new StatusHistoryDto
            {
                StatusHistId = Convert.ToInt64(row["StatusHistId"]),
                ReceptionId = Convert.ToInt32(row["ReceptionId"]),
                BeforeStatus = ToNullableString(row, "BeforeStatus"),
                BeforeStatusName = ToNullableString(row, "BeforeStatusName"),
                AfterStatus = ToNullableString(row, "AfterStatus"),
                AfterStatusName = ToNullableString(row, "AfterStatusName"),
                ProcessType = ToNullableString(row, "ProcessType"),
                ProcessTypeName = ToNullableString(row, "ProcessTypeName"),
                ProcessedAt = Convert.ToDateTime(row["ProcessedAt"]),
                ProcessedBy = ToNullableString(row, "ProcessedBy")
            };
        }

        private static SqlParameter CreateNullableParameter(
            string name,
            SqlDbType type,
            object value,
            int size = 0)
        {
            SqlParameter parameter = size > 0
                ? new SqlParameter(name, type, size)
                : new SqlParameter(name, type);

            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        private static string ToNullableString(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? null : Convert.ToString(row[columnName]);
        }

        private static short? ToNullableInt16(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? (short?)null : Convert.ToInt16(row[columnName]);
        }

        private static int? ToNullableInt32(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? (int?)null : Convert.ToInt32(row[columnName]);
        }

        private static long? ToNullableInt64(DataRow row, string columnName)
        {
            return row.IsNull(columnName)
                ? (long?)null
                : Convert.ToInt64(row[columnName]);
        }

        private static decimal? ToNullableDecimal(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? (decimal?)null : Convert.ToDecimal(row[columnName]);
        }

        private static DateTime? ToNullableDateTime(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? (DateTime?)null : Convert.ToDateTime(row[columnName]);
        }
    }
}
