using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IVideoService : IServiceBase, IServiceModel<Video>
    {
        IEnumerable<VideoDto> VideoListele(IEnumerable<Parameter> parameters = null);
    }
}
