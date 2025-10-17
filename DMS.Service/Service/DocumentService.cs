using AutoMapper;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.Service
{
    public class DocumentService: IDocumentService
    {
        private readonly UnitOfWork _unit;
        private readonly IMapper _mapper;
        public DocumentService(UnitOfWork _unit, IMapper mapper)
        {
            this._unit = _unit;
            _mapper = mapper;
        }

    }
}
