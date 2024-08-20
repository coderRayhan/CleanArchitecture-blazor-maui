using Application.Features.Lookups.Commands;

namespace Application.Features.Lookups.Validators;
internal class CreateLookupCommandValidator
    : AbstractValidator<CreateLookupCommand>
{
    public CreateLookupCommandValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(e => e.NameBN)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(e => e.Code)
            .NotEmpty()
            .MaximumLength(10);
    }

    //private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    //{

    //}
}
