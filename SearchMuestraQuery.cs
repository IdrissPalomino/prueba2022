using Paltarumi.Acopio.Dto.Base;
using Paltarumi.Acopio.Muestreo.Dto.Muestreo.Muestra;
using Paltarumi.Acopio.Muestreo.Domain.Queries.Base;

namespace Paltarumi.Acopio.Muestreo.Domain.Queries.Muestreo.Muestra
{
    public class SearchMuestraQuery : SearchQueryBase<SearchMuestraFilterDto, SearchMuestraDto>
    {
        public SearchMuestraQuery(SearchParamsDto<SearchMuestraFilterDto> searchParams) : base(searchParams)
        {

        }
    }
}
