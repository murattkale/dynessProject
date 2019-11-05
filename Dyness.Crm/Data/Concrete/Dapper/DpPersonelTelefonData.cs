using Core.Data.Dapper;
using Core.Entities.Dto;
using Data.Abstract.Dapper;

namespace Data.Concrete.Dapper
{
    public class DpPersonelTelefonData : DapperRepositoryBase<PersonelTelefonDto>, IDpPersonelTelefonData
    {
    }
}
