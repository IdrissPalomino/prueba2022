using Paltarumi.Acopio.Muestreo.Dto.Muestreo.Muestra;
using Paltarumi.Acopio.Muestreo.Domain.Queries.Base;

namespace Paltarumi.Acopio.Muestreo.Domain.Queries.Muestreo.Muestra
{
    public class GetMuestraQuery : QueryBase<GetMuestraDto>
    {
        public GetMuestraQuery(int id) => Id = id;
        public int Id { get; set; }
    }
}
