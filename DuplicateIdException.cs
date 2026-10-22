using System;
namespace DeviceManagement 
{
    public class DuplicateIdException : Exception
    {
        public string DuplicateId { get; private set;}

        public DuplicateIdException(string duplicateId) : base("Trùng mã định danh")
        {
            DuplicateId = duplicateId;
        }

        public DuplicateIdException(string duplicateId, string message) : base(message) 
        {
            DuplicateId = duplicateId;
        }
    }
}