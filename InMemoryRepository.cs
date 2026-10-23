using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public class InMemoryRepository<T> : IRepository<T> where T : IIdentifiable
    {
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Phần tử thêm vào kho không rỗng");
            }

            if (string.IsNullOrEmpty(item.Id))
            {
                throw new ArgumentException("Mã định danh không hợp lệ");
            }

            if (_items.ContainsKey(item.Id))
            {
                throw new DuplicateIdException(item.Id);
            }

            _items.Add(item.Id, item);
        }

        public bool Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            return _items.Remove(id);
        }

        public T FindById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return default(T);
            }

            T item;
            if (_items.TryGetValue(id, out item))
            {
                return item;
            }
            return default(T);
        }

        public IReadOnlyList<T> GetAll()
        {
            List<T> list = new List<T>(_items.Values);
            return list.AsReadOnly();
        }

        public IReadOnlyList<T> FindAll(Predicate<T> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException( "Điều kiện không rỗng");
            }

            List<T> result = new List<T>();
            foreach (T item in _items.Values)
            {
                if (condition(item))
                {
                    result.Add(item);
                }
            }
            return result.AsReadOnly();
        }
    }
}
