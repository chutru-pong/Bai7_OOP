using System;

namespace DeviceManagement
{
    public class AllocationService<T> where T : TeachingDevice
    {
        private readonly IRepository<T> repository;
        private ISelectionStrategy<T> strategy;

        public AllocationService(IRepository<T> repository, ISelectionStrategy<T> strategy)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("Kho lưu trữ không rỗng");
            }
            if (strategy == null)
            {
                throw new ArgumentNullException("Chiến lược không rỗng");
            }

            this.repository = repository;
            this.strategy = strategy;
        }

        public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
        {
            if (newStrategy == null)
            {
                throw new ArgumentNullException("Chiến lược mới không rỗng");
            }
            this.strategy = newStrategy;
        }

        public T Allocate(Predicate<T> additionalCondition)
        {
            // 1. lấy đối tượng từ repository: IsReadyForUse = true và thỏa mãn additionalCondition
            var candidates = repository.FindAll (delegate(T item)
            {
                if (!item.IsReadyForUse())
                {
                    return false;
                }
                if (additionalCondition != null && !additionalCondition(item))
                {
                    return false;
                }
                return true;
            });

            // 2. Dùng chiến lược hiện tại để chọn thiết bị
            T selected = strategy.Select(candidates);

            // 3. Gọi Borrow() trên thiết bị được chọn
            selected.Borrow();

            // 4. Trả về thiết bị đã cấp phát
            return selected;
        }
    }
}
