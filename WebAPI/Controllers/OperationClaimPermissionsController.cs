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
        private readonly IOperationClaimPermissionService _operationClaimPermissionService;

        public OperationClaimPermissionsController(IOperationClaimPermissionService operationClaimPermissionService)
        {
            _operationClaimPermissionService = operationClaimPermissionService;
        }
        // 1. TÜM ROL-YETKİ İLİŞKİLERİNİ LİSTELE
        // GET api/operationclaimpermissions/getall
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _operationClaimPermissionService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 2. ID İLE DETAY GETİR
        // GET api/operationclaimpermissions/getbyid?id=1
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _operationClaimPermissionService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 3. BELİRLİ BİR ROLE AİT TÜM YETKİLERİ GETİR
        // GET api/operationclaimpermissions/getbyoperationclaimid?operationClaimId=1
        [HttpGet("getbyoperationclaimid")]
        public IActionResult GetByOperationClaimId(int operationClaimId)
        {
            var result = _operationClaimPermissionService.GetByOperationClaimId(operationClaimId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 4. YENİ ROL-YETKİ ATAMASI EKLE
        // POST api/operationclaimpermissions/add
        [HttpPost("add")]
        public IActionResult Add(OperationClaimPermission operationClaimPermission)
        {
            var result = _operationClaimPermissionService.Add(operationClaimPermission);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 5. ROL-YETKİ ATAMASINI GÜNCELLE
        // PUT api/operationclaimpermissions/update
        [HttpPut("update")]
        public IActionResult Update(OperationClaimPermission operationClaimPermission)
        {
            var result = _operationClaimPermissionService.Update(operationClaimPermission);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // 6. ROL-YETKİ ATAMASINI SİL (Soft-Delete)
        // DELETE api/operationclaimpermissions/delete?id=1
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var result = _operationClaimPermissionService.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
