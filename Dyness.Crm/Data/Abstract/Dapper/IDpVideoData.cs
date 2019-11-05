using Core.Data;
using Core.Entities.Dto;

namespace Data.Abstract.Dapper
{
    public interface IDpVideoData : ISqlRepository<VideoDto>
    {
    }
}
