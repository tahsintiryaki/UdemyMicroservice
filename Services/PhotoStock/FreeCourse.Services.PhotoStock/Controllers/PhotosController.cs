using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.BaseController;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            //cancellationtoken, file create işlemi sırasında kullanıcı  tarayıcıyı kapatır vs. darklı bir  işlem olursa file Create işlemi otomatik olarak sonlanmasını sağlayacaktır.
            if (photo is null && photo.Length <= 0)
            {
                return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await photo.CopyToAsync(stream, cancellationToken);

                var returnFilePath = $"photos/{photo.FileName}";

                PhotoDto photoDto = new() { Url = returnFilePath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }
        }
        [HttpGet]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
