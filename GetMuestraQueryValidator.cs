using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Paltarumi.Acopio.Muestreo.Domain.Queries.Base;
using Paltarumi.Acopio.Muestreo.Repository.Abstractions.Base;

namespace Paltarumi.Acopio.Muestreo.Domain.Queries.Muestreo.Muestra
{
    public class GetMuestraQueryValidator : QueryValidatorBase<GetMuestraQuery>
    {
        private readonly IRepository<Entity.LoteCodigoMuestra> _muestraRepository;

        public GetMuestraQueryValidator(IRepository<Entity.LoteCodigoMuestra> muestraRepository)
        {
            _muestraRepository = muestraRepository;

            RequiredField(x => x.Id, Resources.Muestreo.Muestra.IdMuestra)
                .DependentRules(() =>
                {
                    RuleFor(x => x.Id)
                        .MustAsync(ValidateExistenceAsync)
                        .WithCustomValidationMessage();
                });
        }

        protected async Task<bool> ValidateExistenceAsync(GetMuestraQuery command, int id, ValidationContext<GetMuestraQuery> context, CancellationToken cancellationToken)
        {
            var exists = await _muestraRepository.FindAll().Where(x => x.IdLoteCodigoMuestra == id).AnyAsync(cancellationToken);
            if (!exists) return CustomValidationMessage(context, Resources.Common.GetRecordNotFound);
            return true;
        }
    }
}
