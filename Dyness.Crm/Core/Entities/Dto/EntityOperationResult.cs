using System.Collections.Generic;

namespace Core.Entities.Dto
{
    public enum MessageInfoType
    {
        Success,
        Error
    }

    public struct MessageInfo
    {
        public string Field { get; set; }

        public MessageInfoType MessageInfoType { get; set; }

        public string Message { get; set; }
    }

    public class EntityOperationResult<TEntity> where TEntity : class
    {
        public TEntity Model { get; set; }

        public bool Status { get; set; }

        public List<MessageInfo> MessageInfos { get; set; }

        public EntityOperationResult(TEntity entity = null)
        {
            Model = entity;
            MessageInfos = new List<MessageInfo>();
        }
    }

    public class EntityOperationResult
    {
        public bool Status { get; set; }

        public List<MessageInfo> MessageInfos { get; set; }
    }
}
