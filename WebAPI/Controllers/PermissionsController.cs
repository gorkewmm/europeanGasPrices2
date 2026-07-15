using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // 1. Tüm Yetkileri Listele
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _permissionService.GetAll();
            return Ok(result);
        }

        // 2. Yetki Ekle (Örn: Name: "fuelprice.add", Description: "Fiyat ekleyebilir")
        [HttpPost("add")]
        public IActionResult Add(Permission permission)
        {
            _permissionService.Add(permission);
            return Ok("Yetki başarıyla eklendi.");
        }
    }
}
