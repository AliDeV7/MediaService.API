using FluentValidation;

namespace MediaService.Presentation.Api.ViewModels.Validators
{
    /// <summary>
    /// Validator for ImageUploadBase64Request ViewModel.
    /// Validates HTTP-specific concerns (Base64 format, file name presence).
    /// Business rules (file size, type, extension, content) are validated in the Infrastructure layer.
    /// </summary>
    public class ImageUploadBase64RequestValidator : ImageUploadRequestValidatorBase<ImageUploadBase64Request>
    {
        public ImageUploadBase64RequestValidator()
        {
            // Base64 data validation
            RuleFor(x => x.Base64Data)
                .NotEmpty()
                .WithMessage("Base64 data is required.");

            RuleFor(x => x.Base64Data)
                .Must(BeValidBase64)
                .WithMessage("Invalid Base64 format. Must be a valid Base64 string or data URI.")
                .When(x => !string.IsNullOrWhiteSpace(x.Base64Data));
        }

        /// <summary>
        /// Validates if the string is a valid Base64 format.
        /// Accepts both raw Base64 and data URI format.
        /// </summary>
        private static bool BeValidBase64(string base64Data)
        {
            if (string.IsNullOrWhiteSpace(base64Data))
                return false;

            var base64String = base64Data;
            if (base64Data.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                var commaIndex = base64Data.IndexOf(',');
                if (commaIndex == -1)
                    return false;

                base64String = base64Data[(commaIndex + 1)..];
            }

            base64String = base64String.Trim();

            if (string.IsNullOrWhiteSpace(base64String))
                return false;

            if (base64String.Length % 4 != 0)
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
