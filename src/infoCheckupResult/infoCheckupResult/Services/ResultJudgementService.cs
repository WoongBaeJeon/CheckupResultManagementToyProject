using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using infoCheckupResult.Models;

namespace infoCheckupResult.Services
{
    /// <summary>
    /// 판정(혈압, 시력)
    /// </summary>
    public sealed class ResultJudgementService
    {
        private const string SystolicCode = "BLOOD_PRESSURE_SYSTOLIC";
        private const string DiastolicCode = "BLOOD_PRESSURE_DIASTOLIC";

        public void Evaluate(IList<CheckupResultDto> results)
        {
            if (results == null)
                return;

            foreach (CheckupResultDto item in results)
                ClearJudgement(item);

            EvaluateBloodPressure(results);

            foreach (CheckupResultDto item in results)
            {
                if (IsVisionItem(item.ItemCode))
                    EvaluateVision(item);
            }
        }

        private static void EvaluateBloodPressure(
            IList<CheckupResultDto> results)
        {
            CheckupResultDto systolic = results.FirstOrDefault(
                x => x.ItemCode == SystolicCode);
            CheckupResultDto diastolic = results.FirstOrDefault(
                x => x.ItemCode == DiastolicCode);

            if (systolic == null || diastolic == null)
                return;

            decimal systolicValue;
            decimal diastolicValue;

            if (!TryGetNumber(systolic.ResultValue, out systolicValue) ||
                !TryGetNumber(diastolic.ResultValue, out diastolicValue))
            {
                SetJudgement(
                    systolic,
                    ResultJudgementStatus.Pending,
                    "대기",
                    "수축기와 이완기 혈압을 모두 입력해야 합니다.");
                return;
            }

            ResultJudgementStatus status;
            string judgementText;

            if (systolicValue >= 140M || diastolicValue >= 90M)
            {
                status = ResultJudgementStatus.Hypertension;
                judgementText = "고혈압";
            }
            else if (systolicValue >= 130M || diastolicValue >= 81M)
            {
                status = ResultJudgementStatus.Prehypertension;
                judgementText = "고혈압 전";
            }
            else if (systolicValue < 90M || diastolicValue < 60M)
            {
                status = ResultJudgementStatus.LowBloodPressure;
                judgementText = "저혈압 주의";
            }
            else if (systolicValue >= 121M)
            {
                status = ResultJudgementStatus.Caution;
                judgementText = "주의";
            }
            else
            {
                status = ResultJudgementStatus.Normal;
                judgementText = "정상";
            }

            SetJudgement(
                systolic,
                status,
                judgementText,
                string.Format(
                    "측정값: {0}/{1} mmHg, 판정: {2}",
                    systolicValue,
                    diastolicValue,
                    judgementText));
        }

        private static void EvaluateVision(CheckupResultDto item)
        {
            decimal value;
            if (!TryGetNumber(item.ResultValue, out value))
            {
                SetJudgement(
                    item,
                    ResultJudgementStatus.Pending,
                    "대기",
                    "시력을 입력해야 합니다.");
                return;
            }

            bool normal = value >= 0.8M;
            SetJudgement(
                item,
                normal
                    ? ResultJudgementStatus.Normal
                    : ResultJudgementStatus.Abnormal,
                normal ? "정상" : "비정상",
                string.Format("측정값: {0}, 정상 기준: 0.8 이상", value));
        }

        private static bool IsVisionItem(string itemCode)
        {
            return itemCode == "VISION_LEFT" ||
                   itemCode == "VISION_RIGHT" ||
                   itemCode == "STUDENT_VISION_LEFT" ||
                   itemCode == "STUDENT_VISION_RIGHT";
        }

        private static bool TryGetNumber(string value, out decimal number)
        {
            return decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.CurrentCulture,
                out number);
        }

        private static void ClearJudgement(CheckupResultDto item)
        {
            SetJudgement(item, ResultJudgementStatus.None, string.Empty, string.Empty);
        }

        private static void SetJudgement(
            CheckupResultDto item,
            ResultJudgementStatus status,
            string text,
            string reason)
        {
            item.JudgementStatus = status;
            item.JudgementText = text;
            item.JudgementReason = reason;
        }
    }
}
