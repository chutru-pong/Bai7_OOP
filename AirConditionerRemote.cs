using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public interface IReadOnlySet<T> : IReadOnlyCollection<T>
    {
        bool Contains(T item);
    }

    public class ReadOnlySet<T> : IReadOnlySet<T>
    {
        private readonly HashSet<T> _set;

        public ReadOnlySet(IEnumerable<T> collection)
        {
            _set = new HashSet<T>(collection);
        }

        public bool Contains(T item)
        {
            return _set.Contains(item);
        }

        public int Count
        {
            get { return _set.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _set.GetEnumerator();
        }
    }

    public class AirConditionerRemote : TeachingDevice
    {
        public IReadOnlySet<string> CompatibleRooms { get; private set; }

        public AirConditionerRemote(string id, string name, string storageLocation, IEnumerable<string> compatibleRooms, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, storageLocation, status)
        {
            if (compatibleRooms == null)
            {
                throw new ArgumentNullException("compatibleRooms", "Tập danh sách phòng tương thích không được null rỗng.");
            }

            CompatibleRooms = new ReadOnlySet<string>(compatibleRooms);
        }

        public bool IsCompatibleWith(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
            {
                return false;
            }
            return CompatibleRooms.Contains(roomId);
        }

        public override bool IsReadyForUse()
        {
            return Status == DeviceStatus.Available;
        }

        public override double GetDefaultPriorityScore()
        {
            return CompatibleRooms.Count > 0 ? 100.0 : 0.0;
        }

        public override string ToString()
        {
            List<string> roomList = new List<string>();
            foreach (string r in CompatibleRooms)
            {
                roomList.Add(r);
            }
            string rooms = string.Join(", ", roomList.ToArray());
            return base.ToString() + " | Phòng tương thích: [" + rooms + "] | Điểm ưu tiên: " + GetDefaultPriorityScore() + " | Sẵn sàng: " + IsReadyForUse();
        }
    }
}
