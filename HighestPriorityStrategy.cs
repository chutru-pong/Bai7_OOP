using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public class HighestPriorityStrategy<T> : ISelectionStrategy<T>
    {
        private readonly Func<T, double> scoreSelector;

        public HighestPriorityStrategy(Func<T, double> scoreSelector)
        {
            if (scoreSelector == null)
            {
                throw new ArgumentNullException("Hàm tính điểm ưu tiên không rỗng");
            }
            this.scoreSelector = scoreSelector;
        }

        public T Select(IReadOnlyList<T> candidates)
        {
            if (candidates == null || candidates.Count == 0)
            {
                throw new NoSuitableItemException("Danh sách rỗng, không thể chọn thiết bị");
            }

            T bestCandidate = candidates[0];
            double bestScore = scoreSelector(bestCandidate);

            for (int i = 1; i < candidates.Count; i++)
            {
                double score = scoreSelector(candidates[i]);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestCandidate = candidates[i];
                }
            }

            return bestCandidate;
        }
    }
}
