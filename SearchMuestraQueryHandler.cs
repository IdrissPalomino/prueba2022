using AutoMapper;
using Paltarumi.Acopio.Dto.Base;
using Paltarumi.Acopio.Muestreo.Dto.Muestreo.Muestra;
using Paltarumi.Acopio.Muestreo.Domain.Queries.Base;
using Paltarumi.Acopio.Muestreo.Repository.Abstractions.Base;
using Paltarumi.Acopio.Muestreo.Repository.Extensions;
using System.Linq.Expressions;
using Paltarumi.Acopio.Muestreo.Common;

namespace Paltarumi.Acopio.Muestreo.Domain.Queries.Muestreo.Muestra
{
    public class SearchMuestraQueryHandler : SearchQueryHandlerBase<SearchMuestraQuery, SearchMuestraFilterDto, SearchMuestraDto>
    {
        private readonly IRepository<Entity.LoteCodigoMuestra> _muestraRepository;
        private readonly IRepository<Entity.LoteMuestreo> _loteMuestreoRepository;
        public SearchMuestraQueryHandler(
            IMapper mapper,
            IRepository<Entity.LoteCodigoMuestra> muestraRepository,
            IRepository<Entity.LoteMuestreo> loteMuestreoRepository
        ) : base(mapper)
        {
            _muestraRepository = muestraRepository;
            _loteMuestreoRepository = loteMuestreoRepository;
        }

        protected override async Task<ResponseDto<SearchResultDto<SearchMuestraDto>>> HandleQuery(SearchMuestraQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<SearchResultDto<SearchMuestraDto>>();

            Expression<Func<Entity.LoteCodigoMuestra, bool>> filter = x => true;

            var filters = request.SearchParams?.Filter;

            if (filters?.FechaDesde.HasValue == true || filters?.FechaHasta.HasValue == true)
            {
                if (filters?.FechaDesde.HasValue == true)
                {
                    var fechaDesde = filters.FechaDesde.GetStartDate();
                    filter = filter.And(x => (x.FechaMuestreo >= fechaDesde || x.FechaMuestreo >= fechaDesde));
                }

                if (filters?.FechaHasta.HasValue == true)
                {
                    var fechaHasta = filters.FechaHasta.GetEndDate();
                    filter = filter.And(x => (x.FechaMuestreo < fechaHasta || x.FechaMuestreo < fechaHasta));
                }
            }

            if (!string.IsNullOrEmpty(filters?.Proveedor))
            {
                var proveedores = filters.Proveedor.Split(" ");
                proveedores.ToList().ForEach(p =>
                {
                    filter = filter.And(x =>
                    (x.IdProveedorNavigation.Ruc.Contains(p) || x.IdProveedorNavigation.RazonSocial.Contains(p)));
                });
            }

            filter = filter.And(x => x.Activo == true);

            var muestras = await _muestraRepository.SearchByAsNoTrackingAsync(
                request.SearchParams?.Page?.Page ?? 1,
                request.SearchParams?.Page?.PageSize ?? 10,
                null,
                filter,
                x => x.IdCanchaNavigation,
                x => x.IdMuestraCondicionNavigation,
                x => x.IdMuestraEstadoNavigation,
                x => x.IdDuenoMuestraNavigation,
                x => x.IdProveedorNavigation,
                x => x.IdTurnoNavigation
            );

            var muestraDtos = _mapper?.Map<IEnumerable<SearchMuestraDto>>(muestras.Items);

            var CodigoLotes = muestraDtos.Select(x => x.CodigoLote).ToList();

            var lotesMuestras = await _loteMuestreoRepository.FindByAsync(
                x => CodigoLotes.Contains(x.CodigoLote),
                x => x.IdTipoMineralNavigation
            );

            muestraDtos.ToList().ForEach(item =>
            {
                var tipoMineral = lotesMuestras.Where(x => x.CodigoLote == item.CodigoLote).FirstOrDefault(new Entity.LoteMuestreo());
                item.TipoMineral = tipoMineral.IdTipoMineralNavigation?.Descripcion;
            });

            var searchResult = new SearchResultDto<SearchMuestraDto>(
                muestraDtos ?? new List<SearchMuestraDto>(),
                muestras.Total,
                request.SearchParams
            );

            response.UpdateData(searchResult);

            return await Task.FromResult(response);
        }
    }
}
