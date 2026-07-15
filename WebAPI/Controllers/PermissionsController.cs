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

        // 1. TÜM YETKİLERİ LİSTELE
        // GET api/permissions/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _permissionService.GetAll();
            return Ok(result);
        }

        // 2. ID İLE YETKİ DETAYINI GETİR
        // GET api/permissions/getbyid?id=1
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _permissionService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Yetki bulunamadı veya silinmiş.");
        }

        // 3. YENİ YETKİ EKLE
        // POST api/permissions/add
        [HttpPost("add")]
        public IActionResult Add(Permission permission)
        {
            _permissionService.Add(permission);
            return Ok("Yetki başarıyla eklendi.");
        }

        // 4. YETKİ BİLGİLERİNİ GÜNCELLE
        // PUT api/permissions/update
        [HttpPut("update")]
        public IActionResult Update(Permission permission)
        {
            var existingPermission = _permissionService.GetById(permission.Id);
            if (existingPermission == null)
            {
                return BadRequest("Güncellenmek istenen yetki bulunamadı.");
            }

            // Name ve Description alanlarını güncelliyoruz
            existingPermission.Name = permission.Name;
            existingPermission.Description = permission.Description;

            _permissionService.Update(existingPermission);
            return Ok("Yetki başarıyla güncellendi.");
        }

        // 5. YETKİ SİL (Soft-Delete)
        // DELETE api/permissions/delete?id=1
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var permissionToDelete = _permissionService.GetById(id);
            if (permissionToDelete == null)
            {
                return BadRequest("Silinmek istenen yetki bulunamadı.");
            }

            _permissionService.Delete(permissionToDelete);
            return Ok("Yetki başarıyla silindi (IsDeleted=true yapıldı).");
        }
    }
}
