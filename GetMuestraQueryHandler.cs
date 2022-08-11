using AutoMapper;
using Paltarumi.Acopio.Muestreo.Dto.Muestreo.Muestra;
using Paltarumi.Acopio.Dto.Base;
using Paltarumi.Acopio.Muestreo.Domain.Queries.Base;
using Paltarumi.Acopio.Muestreo.Repository.Abstractions.Base;

namespace Paltarumi.Acopio.Muestreo.Domain.Queries.Muestreo.Muestra
{
    public class GetMuestraQueryHandler : QueryHandlerBase<GetMuestraQuery, GetMuestraDto>
    {
        private readonly IRepository<Entity.LoteCodigoMuestra> _muestraRepository;

        public GetMuestraQueryHandler(
            IMapper mapper,
            GetMuestraQueryValidator validator,
            IRepository<Entity.LoteCodigoMuestra> muestraRepository
        ) : base(mapper, validator)
        {
            _muestraRepository = muestraRepository;
        }

        protected override async Task<ResponseDto<GetMuestraDto>> HandleQuery(GetMuestraQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<GetMuestraDto>();
            var muestra = await _muestraRepository.GetByAsync(x => x.IdLoteCodigoMuestra == request.Id);
            var muestraDto = _mapper?.Map<GetMuestraDto>(muestra);

            if (muestra != null && muestraDto != null)
            {
                response.UpdateData(muestraDto);
            }

            return await Task.FromResult(response);
        }
    }
}
