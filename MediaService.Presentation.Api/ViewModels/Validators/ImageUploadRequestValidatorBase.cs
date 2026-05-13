using FluentValidation;
using MediaService.Core.Configuration;

namespace MediaService.Presentation.Api.ViewModels.Validators
{
    /// <summary>
    /// Base validator for image upload requests.
    /// Validates common optional processing parameters.
    /// </summary>
    /// <typeparam name="T">Type of request inheriting from ImageUploadRequestBase.</typeparam>
    public abstract class ImageUploadRequestValidatorBase<T> : AbstractValidator<T>
        where T : ImageUploadRequestBase
    {
        protected ImageUploadRequestValidatorBase()
        {
            // Optional parameter range validation - only when values are provided
            RuleFor(x => x.ThumbnailWidth)
                .InclusiveBetween(
                    ImageProcessingDefaults.MinThumbnailWidth,
                    ImageProcessingDefaults.MaxThumbnailWidth)
                .WithMessage($"Thumbnail width must be between {ImageProcessingDefaults.MinThumbnailWidth} and {ImageProcessingDefaults.MaxThumbnailWidth} pixels.")
                .When(x => x.ThumbnailWidth.HasValue);

            RuleFor(x => x.WebPQuality)
                .InclusiveBetween(
                    ImageProcessingDefaults.MinQuality,
                    ImageProcessingDefaults.MaxQuality)
                .WithMessage($"WebP quality must be between {ImageProcessingDefaults.MinQuality} and {ImageProcessingDefaults.MaxQuality}.")
                .When(x => x.WebPQuality.HasValue);
        }
    }
}
