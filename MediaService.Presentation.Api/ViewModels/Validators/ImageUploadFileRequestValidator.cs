using FluentValidation;

namespace MediaService.Presentation.Api.ViewModels.Validators
{
    /// <summary>
    /// Validator for ImageUploadFileRequest ViewModel.
    /// Validates HTTP-specific concerns (file presence) and inherits optional parameter validation.
    /// Business rules (file size, type, content) are validated in the Infrastructure layer.
    /// </summary>
    public class ImageUploadFileRequestValidator : ImageUploadRequestValidatorBase<ImageUploadFileRequest>
    {
        public ImageUploadFileRequestValidator()
        {
            // File presence validation
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");

            RuleFor(x => x.File)
                .Must(file => file != null && file.Length > 0)
                .WithMessage("File cannot be empty.")
                .When(x => x.File != null);
        }
    }
}
