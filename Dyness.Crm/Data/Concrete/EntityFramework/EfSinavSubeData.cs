﻿using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;

namespace Data.Concrete.EntityFramework
{
    public class EfSinavSubeData : EFEntityRepository<SinavSube, EFContext>, IEfSinavSubeData
    {
    }
}