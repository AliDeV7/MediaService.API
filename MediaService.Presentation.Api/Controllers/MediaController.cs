using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Core.Constants;
using MediaService.Presentation.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace MediaService.Presentation.Api.Controllers
{
    /// <summary>
    /// Handles media file upload, retrieval, and deletion.
    /// Maps framework-specific ViewModels to framework-agnostic DTOs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaUseCase _mediaUseCase;
        private readonly IValidator<MediaUploadRequest> _validator;

        public MediaController(
            IMediaUseCase mediaUseCase,
            IValidator<MediaUploadRequest> validator)
        {
            _mediaUseCase = mediaUseCase;
            _validator = validator;
        }

        /// <summary>
        /// Uploads a single media file.
        /// </summary>
        /// <param name="viewModel">Upload request from multipart/form-data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload response with file metadata and URLs.</returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(
            [FromForm] MediaUploadRequest viewModel,
            CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _validator.ValidateAsync(viewModel, cancellationToken);

            if (!validationResult.IsValid)
            {
                // Add errors to ModelState
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return BadRequest(ModelState);
            }

            // Map ViewModel (Presentation) → DTO (Core) with default values
            var request = new FileUploadRequest
            {
                FileStream = viewModel.File.OpenReadStream(),
                FileName = viewModel.File.FileName,
                ContentType = viewModel.File.ContentType,
                FileSize = viewModel.File.Length,
                GenerateThumbnail = viewModel.GenerateThumbnail ?? MediaServiceConstants.DefaultGenerateThumbnail,
                ConvertToWebP = viewModel.ConvertToWebP ?? MediaServiceConstants.DefaultConvertToWebP,
                ThumbnailWidth = viewModel.ThumbnailWidth ?? MediaServiceConstants.DefaultThumbnailWidth,
                WebPQuality = viewModel.WebPQuality ?? MediaServiceConstants.DefaultWebPQuality
            };

            var response = await _mediaUseCase.UploadFileAsync(request, cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Deletes a file by relative path.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>204 No Content if deleted, 404 if not found.</returns>
        [HttpDelete("{*relativePath}")]
        public async Task<IActionResult> DeleteFile(
            string relativePath,
            CancellationToken cancellationToken)
        {
            var deleted = await _mediaUseCase.DeleteFileAsync(relativePath, cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}
