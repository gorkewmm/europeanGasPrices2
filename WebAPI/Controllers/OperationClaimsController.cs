using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimsController : ControllerBase
    {
        private readonly IOperationClaimService _operationClaimService;

        public OperationClaimsController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        // 1. TÜM ROLLERİ LİSTELE
        // GET api/operationclaims/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _operationClaimService.GetAll();
            return Ok(result);
        }

        // 2. ID İLE ROL DETAYINI GETİR
        // GET api/operationclaims/getbyid?id=1
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _operationClaimService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Rol bulunamadı veya silinmiş.");
        }

        // 3. YENİ ROL EKLE
        // POST api/operationclaims/add
        [HttpPost("add")]
        public IActionResult Add(OperationClaim operationClaim)
        {
            _operationClaimService.Add(operationClaim);
            return Ok("Rol başarıyla eklendi.");
        }

        // 4. ROL BİLGİSİNİ GÜNCELLE
        // PUT api/operationclaims/update
        [HttpPut("update")]
        public IActionResult Update(OperationClaim operationClaim)
        {
            var existingClaim = _operationClaimService.GetById(operationClaim.Id);
            if (existingClaim == null)
            {
                return BadRequest("Güncellenmek istenen rol bulunamadı.");
            }

            existingClaim.Name = operationClaim.Name;

            _operationClaimService.Update(existingClaim);
            return Ok("Rol başarıyla güncellendi.");
        }

        // 5. ROL SİL (Soft-Delete)
        // DELETE api/operationclaims/delete?id=1
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var claimToDelete = _operationClaimService.GetById(id);
            if (claimToDelete == null)
            {
                return BadRequest("Silinmek istenen rol bulunamadı.");
            }

            _operationClaimService.Delete(claimToDelete);
            return Ok("Rol başarıyla silindi (IsDeleted=true yapıldı).");
        }
    }
}
