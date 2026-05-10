using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Presentation.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

        public MediaController(IMediaUseCase mediaUseCase)
        {
            _mediaUseCase = mediaUseCase;
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
            // Map ViewModel (Presentation) → DTO (Core)
            var request = new FileUploadRequest
            {
                FileStream = viewModel.File.OpenReadStream(),
                FileName = viewModel.File.FileName,
                ContentType = viewModel.File.ContentType,
                FileSize = viewModel.File.Length,
                Title = viewModel.Title,
                AltText = viewModel.AltText,
                GenerateThumbnail = viewModel.GenerateThumbnail,
                ConvertToWebP = viewModel.ConvertToWebP,
                ThumbnailWidth = viewModel.ThumbnailWidth,
                WebPQuality = viewModel.WebPQuality,
                SortingOrder = viewModel.SortingOrder
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
