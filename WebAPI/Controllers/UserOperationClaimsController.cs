using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOperationClaimsController : ControllerBase
    {
        private readonly IUserOperationClaimService _uocService;

        public UserOperationClaimsController(IUserOperationClaimService uocService)
        {
            _uocService = uocService;
        }

        // 1. TÜM KULLANICI-ROL EŞLEŞMELERİNİ LİSTELE
        // GET api/useroperationclaims/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _uocService.GetAll();
            return Ok(result);
        }

        // 2. BELİRLİ BİR KULLANICININ TÜM ROLLERİNİ GETİR
        // GET api/useroperationclaims/getbyuserid?userId=3
        [HttpGet("getbyuserid")]
        public IActionResult GetByUserId(int userId)
        {
            var result = _uocService.GetByUserId(userId);
            return Ok(result);
        }

        // 3. KULLANICIYA ROL ATA (Eşleştirme Ekle)
        // POST api/useroperationclaims/add
        [HttpPost("add")]
        public IActionResult Add(UserOperationClaim userOperationClaim)
        {
            _uocService.Add(userOperationClaim);
            return Ok("Kullanıcıya rol başarıyla atandı.");
        }

        // 4. KULLANICININ ROLÜNÜ GÜNCELLE
        // PUT api/useroperationclaims/update
        [HttpPut("update")]
        public IActionResult Update(UserOperationClaim userOperationClaim)
        {
            var existingUoc = _uocService.GetById(userOperationClaim.Id);
            if (existingUoc == null)
            {
                return BadRequest("Güncellenmek istenen atama kaydı bulunamadı.");
            }

            existingUoc.UserId = userOperationClaim.UserId;
            existingUoc.OperationClaimId = userOperationClaim.OperationClaimId;

            _uocService.Update(existingUoc);
            return Ok("Kullanıcı rol ataması başarıyla güncellendi.");
        }

        // 5. KULLANICIDAN ROLÜ GERİ AL (Soft-Delete)
        // DELETE api/useroperationclaims/delete?id=1
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var uocToDelete = _uocService.GetById(id);
            if (uocToDelete == null)
            {
                return BadRequest("Silinmek istenen atama kaydı bulunamadı.");
            }

            _uocService.Delete(uocToDelete);
            return Ok("Kullanıcı rol ataması başarıyla kaldırıldı (silindi).");
        }
    }
}
