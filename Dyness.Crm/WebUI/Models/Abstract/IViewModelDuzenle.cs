using Core.Entities.Dto;

namespace WebUI.Models.Abstract
{
    public interface IViewModelDuzenle<TEntity> where TEntity : class
    {
        string Command { get; set; }

        TEntity Model { get; set; }

        bool MessageExists { get;  }

        EntityOperationResult<TEntity> OperationResult { get; set; }
    }
}
