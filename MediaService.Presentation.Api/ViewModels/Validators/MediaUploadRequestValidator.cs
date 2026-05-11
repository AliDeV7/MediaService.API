using FluentValidation;
using MediaService.Core.Constants;

namespace MediaService.Presentation.Api.ViewModels.Validators
{
    /// <summary>
    /// Validator for MediaUploadRequest ViewModel.
    /// Validates HTTP-specific concerns and optional parameter ranges.
    /// Business rules (file size, type, content) are validated in the Infrastructure layer.
    /// </summary>
    public class MediaUploadRequestValidator : AbstractValidator<MediaUploadRequest>
    {
        public MediaUploadRequestValidator()
        {
            // File presence validation
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required.");

            RuleFor(x => x.File)
                .Must(file => file != null && file.Length > 0)
                .WithMessage("File cannot be empty.")
                .When(x => x.File != null);

            // Optional parameter range validation
            RuleFor(x => x.ThumbnailWidth)
                .InclusiveBetween(
                    MediaServiceConstants.MinThumbnailWidth,
                    MediaServiceConstants.MaxThumbnailWidth)
                .WithMessage($"Thumbnail width must be between {MediaServiceConstants.MinThumbnailWidth} and {MediaServiceConstants.MaxThumbnailWidth} pixels.")
                .When(x => x.ThumbnailWidth.HasValue);

            RuleFor(x => x.WebPQuality)
                .InclusiveBetween(
                    MediaServiceConstants.MinQuality,
                    MediaServiceConstants.MaxQuality)
                .WithMessage($"WebP quality must be between {MediaServiceConstants.MinQuality} and {MediaServiceConstants.MaxQuality}.")
                .When(x => x.WebPQuality.HasValue);
        }
    }
}
