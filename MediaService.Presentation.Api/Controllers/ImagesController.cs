using FluentValidation;
using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Core.Common;
using MediaService.Core.Constants;
using MediaService.Presentation.Api.Filters;
using MediaService.Presentation.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MediaService.Presentation.Api.Controllers
{
    /// <summary>
    /// Handles image file upload, retrieval, and deletion.
    /// Maps framework-specific ViewModels to framework-agnostic DTOs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageUseCase _imageUseCase;
        private readonly IValidator<ImageUploadFileRequest> _validator;

        public ImagesController(
            IImageUseCase mediaUseCase,
            IValidator<ImageUploadFileRequest> validator)
        {
            _imageUseCase = mediaUseCase;
            _validator = validator;
        }

        /// <summary>
        /// Uploads a single image file.
        /// </summary>
        /// <param name="viewModel">Upload request from multipart/form-data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload response with file metadata and URLs.</returns>
        [HttpPost("upload-file")]
        [Consumes("multipart/form-data")]
        [ValidateWithFluentValidation<ImageUploadFileRequest>]
        [ProducesResponseType(typeof(ApiResponse<UploadResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UploadFile(
            [FromForm] ImageUploadFileRequest viewModel,
            CancellationToken cancellationToken)
        {
            // Map ViewModel (Presentation) → DTO (Core) with default values
            var request = new UploadImageFileDto
            {
                FileStream = viewModel.File.OpenReadStream(),
                FileName = viewModel.File.FileName,
                ContentType = viewModel.File.ContentType,
                FileSize = viewModel.File.Length,
                GenerateThumbnail = viewModel.GenerateThumbnail ?? ImageProcessingDefaults.GenerateThumbnail,
                ConvertToWebP = viewModel.ConvertToWebP ?? ImageProcessingDefaults.ConvertToWebP,
                ThumbnailWidth = viewModel.ThumbnailWidth ?? ImageProcessingDefaults.ThumbnailWidth,
                WebPQuality = viewModel.WebPQuality ?? ImageProcessingDefaults.WebPQuality
            };

            var response = await _imageUseCase.UploadFileAsync(request, cancellationToken);

            return Ok(ApiResponse<UploadResponseDto>.SuccessResponse(response));
        }

        /// <summary>
        /// Deletes a file by relative path.
        /// </summary>
        /// <param name="relativePath">Relative file path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Success response if deleted, 404 if not found.</returns>
        [HttpDelete("{*relativePath}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFile(
            string relativePath,
            CancellationToken cancellationToken)
        {
            var deleted = await _imageUseCase.DeleteFileAsync(relativePath, cancellationToken);

            if (!deleted)
            {
                var errorResponse = ApiResponse<object>.FailureResponse(
                    "FILE_NOT_FOUND",
                    $"File not found: {relativePath}"
                );

                return NotFound(errorResponse);
            }

            var successResponse = ApiResponse<object>.SuccessResponse(
                new { Message = "File deleted successfully", Path = relativePath }
            );

            return Ok(successResponse);
        }
    }
}
