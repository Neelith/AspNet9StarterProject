using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using YourProjectName.Application.Commons;
using YourProjectName.Domain.Commons;

namespace YourProjectName.WebApi.Middlewares;

public class ValidationMiddleware(IEnumerable<IValidator<IRequest>> validators) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = await context.Request.ReadFromJsonAsync<IRequest>();
        var validationContext = new ValidationContext<IRequest>(request);

        var validationFailures = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(validationContext)));

        //var errors = validationFailures
        //    .Where(validationResult => !validationResult.IsValid)
        //    .SelectMany(validationResult => validationResult.Errors)
        //    .Select(validationFailure => new ValidationError(
        //        validationFailure.PropertyName,
        //        validationFailure.ErrorMessage))
        //    .ToList();

        //if (errors.Any())
        //{
        //    throw new Exceptions.ValidationException(errors);
        //}

        await next(context);
    }
}
