using AutoMapper;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.Service
{
    public class SharingService: ISharingService
    {
        private readonly UnitOfWork _unit;
        private readonly IMapper _mapper;

        public SharingService(UnitOfWork _unit, IMapper mapper)
        {
            this._unit = _unit;
            this._mapper = mapper;
        }
    }
}
