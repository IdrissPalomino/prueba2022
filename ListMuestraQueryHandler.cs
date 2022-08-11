using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Paltarumi.Acopio.Dto.Base;
using Paltarumi.Acopio.Muestreo.Dto.Muestreo.Muestra;
using Paltarumi.Acopio.Muestreo.Domain.Queries.Base;
using Paltarumi.Acopio.Muestreo.Repository.Abstractions.Base;

namespace Paltarumi.Acopio.Muestreo.Domain.Queries.Muestreo.Muestra
{
    public class ListMuestraQueryHandler : QueryHandlerBase<ListMuestraQuery, IEnumerable<ListMuestraDto>>
    {
        private readonly IRepository<Entity.LoteCodigoMuestra> _repository;

        public ListMuestraQueryHandler(
            IMapper mapper,
            IRepository<Entity.LoteCodigoMuestra> repository
        ) : base(mapper)
        {
            _repository = repository;
        }

        protected override async Task<ResponseDto<IEnumerable<ListMuestraDto>>> HandleQuery(ListMuestraQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<IEnumerable<ListMuestraDto>>();
            var list = await _repository.FindAll().ToListAsync(cancellationToken);
            var listDtos = _mapper?.Map<IEnumerable<ListMuestraDto>>(list);

            response.UpdateData(listDtos ?? new List<ListMuestraDto>());

            return await Task.FromResult(response);
        }
    }
}
