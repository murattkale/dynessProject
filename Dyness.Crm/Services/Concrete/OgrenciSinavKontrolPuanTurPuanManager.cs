using Core.Entities.Dto;
using Data.Abstract.Dapper;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class OgrenciSinavKontrolPuanTurPuanManager : IOgrenciSinavKontrolPuanTurPuanService
    {
        IDpOgrenciSinavKontrolPuanTurPuanData dpData;

        public OgrenciSinavKontrolPuanTurPuanManager(IDpOgrenciSinavKontrolPuanTurPuanData dpData)
        {
            this.dpData = dpData;
        }

        public EntityOperationResult<OgrenciSinavKontrolPuanTurPuan> Add(OgrenciSinavKontrolPuanTurPuan entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSinavKontrolPuanTurPuan> Update(OgrenciSinavKontrolPuanTurPuan entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateDto(OgrenciSinavKontrolPuanTurPuanDto dto)
        {
            var newParameters = new List<Parameter>();

            newParameters.Add(new Parameter("OgrenciSinavKontrolId", dto.OgrenciSinavKontrolId));
            newParameters.Add(new Parameter("PuanTurId", dto.PuanTurId));
            newParameters.Add(new Parameter("GenelSira", dto.GenelSira));
            newParameters.Add(new Parameter("SubeSira", dto.SubeSira));
            newParameters.Add(new Parameter("SinifSira", dto.SinifSira));

            dpData.Execute("OgrenciSinavKontrolPuanTurPuan_Guncelle", newParameters);
        }

        public EntityOperationResult<OgrenciSinavKontrolPuanTurPuan> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<OgrenciSinavKontrolPuanTurPuan, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public OgrenciSinavKontrolPuanTurPuan Get(Expression<Func<OgrenciSinavKontrolPuanTurPuan, bool>> expression = null, params Expression<Func<OgrenciSinavKontrolPuanTurPuan, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OgrenciSinavKontrolPuanTurPuan> List(Expression<Func<OgrenciSinavKontrolPuanTurPuan, bool>> expression = null, params Expression<Func<OgrenciSinavKontrolPuanTurPuan, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public List<OgrenciSinavKontrolPuanTurPuanDto> ListDto(int sinavId)
        {
            var newParameters = new List<Parameter>();

            newParameters.Add(new Parameter("SinavId", sinavId));

            var dtos = dpData.GetEntities("OgrenciSinavKontrolPuanTurPuan_Listele", newParameters).ToList();

            return dtos;
        }

        public EntityPagedDataSource<OgrenciSinavKontrolPuanTurPuan> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
