using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Core.Common;
using MediaService.Presentation.Api.Attributes;
using MediaService.Presentation.Api.Filters;
using MediaService.Presentation.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaService.Presentation.Api.Controllers
{
    /// <summary>
    /// Handles image file upload, retrieval, and deletion.
    /// Maps framework-specific ViewModels to framework-agnostic DTOs.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageUseCase _imageUseCase;

        public ImagesController(
            IImageUseCase mediaUseCase)
        {
            _imageUseCase = mediaUseCase;
        }

        /// <summary>
        /// Uploads a single image file.
        /// </summary>
        /// <param name="viewModel">Upload request from multipart/form-data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload response with file metadata and URLs.</returns>
        [HttpPost("upload-file")]
        [Consumes("multipart/form-data")]
        [RequirePermission("media:write")]
        [ValidateWithFluentValidation<ImageUploadFileRequest>]
        [ProducesResponseType(typeof(ApiResponse<UploadResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UploadFile(
            [FromForm] ImageUploadFileRequest viewModel,
            CancellationToken cancellationToken)
        {
            // Map ViewModel → DTO (only HTTP-specific data extraction, no business logic)
            using var fileStream = viewModel.File.OpenReadStream();

            var request = new UploadImageFileDto
            {
                FileStream = fileStream,
                FileName = viewModel.File.FileName,
                GenerateThumbnail = viewModel.GenerateThumbnail,
                ConvertToWebP = viewModel.ConvertToWebP,
                ThumbnailWidth = viewModel.ThumbnailWidth,
                WebPQuality = viewModel.WebPQuality
            };

            var response = await _imageUseCase.UploadFileAsync(request, cancellationToken);

            return Ok(ApiResponse<UploadResponseDto>.SuccessResponse(response));
        }

        /// <summary>
        /// Uploads a single image from Base64 data.
        /// </summary>
        /// <param name="viewModel">Upload request containing Base64 image data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Upload response with file metadata and URLs.</returns>
        [HttpPost("upload-base64")]
        [Consumes("application/json")]
        [RequirePermission("media:write")]
        [ValidateWithFluentValidation<ImageUploadBase64Request>]
        [ProducesResponseType(typeof(ApiResponse<UploadResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadBase64(
            [FromBody] ImageUploadBase64Request viewModel,
            CancellationToken cancellationToken)
        {
            // Map ViewModel → DTO (only HTTP-specific data extraction, no business logic)
            var request = new UploadImageBase64Dto
            {
                Base64Data = viewModel.Base64Data,
                FileName = viewModel.FileName,
                GenerateThumbnail = viewModel.GenerateThumbnail,
                ConvertToWebP = viewModel.ConvertToWebP,
                ThumbnailWidth = viewModel.ThumbnailWidth,
                WebPQuality = viewModel.WebPQuality
            };

            var response = await _imageUseCase.UploadBase64Async(request, cancellationToken);

            return Ok(ApiResponse<UploadResponseDto>.SuccessResponse(response));
        }


        /// <summary>
        /// Deletes an image file by relative path.
        /// </summary>
        /// <param name="viewModel">Delete request containing relative path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Delete response with metadata about deleted files.</returns>
        [HttpDelete]
        [RequirePermission("media:delete")]
        [ValidateWithFluentValidation<ImageDeleteRequest>]
        [ProducesResponseType(typeof(ApiResponse<DeleteResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFile(
            [FromBody] ImageDeleteRequest viewModel,
            CancellationToken cancellationToken)
        {
            // Map ViewModel → DTO
            var request = new DeleteImageDto
            {
                RelativePath = viewModel.RelativePath
            };

            var response = await _imageUseCase.DeleteFileAsync(request, cancellationToken);
            return Ok(ApiResponse<DeleteResponseDto>.SuccessResponse(response));

        }

    }
}
