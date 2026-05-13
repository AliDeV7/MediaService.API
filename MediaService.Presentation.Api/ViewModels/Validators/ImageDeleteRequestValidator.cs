using FluentValidation;

namespace MediaService.Presentation.Api.ViewModels.Validators
{
    /// <summary>
    /// Validates image deletion requests.
    /// </summary>
    public sealed class ImageDeleteRequestValidator : AbstractValidator<ImageDeleteRequest>
    {
        public ImageDeleteRequestValidator()
        {
            RuleFor(x => x.RelativePath)
                .NotEmpty()
                .WithMessage("Relative path is required.")
                .Must(BeValidPath)
                .WithMessage("Invalid file path format.");
        }

        private static bool BeValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            // Prevent path traversal attacks
            if (path.Contains("..") || path.Contains("//"))
                return false;

            return true;
        }
    }
}
