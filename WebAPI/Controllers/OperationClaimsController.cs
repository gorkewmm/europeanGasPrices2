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
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 2. ID İLE ROL DETAYINI GETİR
        // GET api/operationclaims/getbyid?id=1
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _operationClaimService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 3. YENİ ROL EKLE
        // POST api/operationclaims/add
        [HttpPost("add")]
        public IActionResult Add(OperationClaim operationClaim)
        {
            var result = _operationClaimService.Add(operationClaim);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 4. ROL BİLGİSİNİ GÜNCELLE
        // PUT api/operationclaims/update
        [HttpPut("update")]
        public IActionResult Update(OperationClaim operationClaim)
        {
            var result = _operationClaimService.Update(operationClaim);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 5. ROL SİL (Soft-Delete)
        // DELETE api/operationclaims/delete?id=1
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var result = _operationClaimService.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
