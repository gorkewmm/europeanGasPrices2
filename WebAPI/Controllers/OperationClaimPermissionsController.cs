using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimPermissionsController : ControllerBase
    {
        private readonly IOperationClaimPermissionService _ocpService;

        public OperationClaimPermissionsController(IOperationClaimPermissionService ocpService)
        {
            _ocpService = ocpService;
        }

        // 1. TÜM ROL-YETKİ EŞLEŞMELERİNİ LİSTELE
        // GET api/operationclaimpermissions/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _ocpService.GetAll();
            return Ok(result);
        }

        // 2. ID İLE EŞLEŞME DETAYINI GETİR
        // GET api/operationclaimpermissions/getbyid?id=1
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _ocpService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Eşleşme kaydı bulunamadı veya silinmiş.");
        }

        // 3. BELİRLİ BİR ROLÜN TÜM YETKİ EŞLEŞMELERİNİ GETİR
        // GET api/operationclaimpermissions/getbyroleid?roleId=1
        [HttpGet("getbyroleid")]
        public IActionResult GetByRoleId(int roleId)
        {
            var result = _ocpService.GetByOperationClaimId(roleId);
            return Ok(result);
        }

        // 4. ROLE YETKİ ATA (Eşleştirme Ekle)
        // POST api/operationclaimpermissions/add
        [HttpPost("add")]
        public IActionResult Add(OperationClaimPermission operationClaimPermission)
        {
            _ocpService.Add(operationClaimPermission);
            return Ok("Yetki role başarıyla atandı.");
        }

        // 5. ROLÜN YETKİSİNİ GÜNCELLE
        // PUT api/operationclaimpermissions/update
        [HttpPut("update")]
        public IActionResult Update(OperationClaimPermission operationClaimPermission)
        {
            var existingOcp = _ocpService.GetById(operationClaimPermission.Id);
            if (existingOcp == null)
            {
                return BadRequest("Güncellenmek istenen eşleşme kaydı bulunamadı.");
            }

            existingOcp.OperationClaimId = operationClaimPermission.OperationClaimId;
            existingOcp.PermissionId = operationClaimPermission.PermissionId;

            _ocpService.Update(existingOcp);
            return Ok("Rol yetki ataması başarıyla güncellendi.");
        }

        // 6. ROLDEN YETKİYİ GERİ AL (Soft-Delete)
        // DELETE api/operationclaimpermissions/delete?id=1
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var ocpToDelete = _ocpService.GetById(id);
            if (ocpToDelete == null)
            {
                return BadRequest("Silinmek istenen eşleşme kaydı bulunamadı.");
            }

            _ocpService.Delete(ocpToDelete);
            return Ok("Yetki rolden başarıyla kaldırıldı (silindi).");
        }
    }
}
