using Architecture;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    //[EnableCors("RestApi")]
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    //[ApiController]
    public class UploadController : BaseController<Business.Domains.File, Business.Domains.File>
    {
        private readonly IFileService fileService;

        public UploadController(IFileService service, IMapper mapper) : base(service, mapper)
        {
            this.fileService = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            if (!Request.HasFormContentType)
            {
                return BadRequest();
            }

            var formCollection = await Request.ReadFormAsync();
            var files = formCollection.Files;

            var folderName = Path.Combine("StaticFiles", "Upload", _account.Id.ToString());
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (files.Any(f => f.Length == 0))
            {
                return BadRequest("Corrupted file");
            }

            var response = await this.fileService.CreateAsync(files, pathToSave, _account.Id);

            return Ok(response);
        }
    }
}
