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

        // 1. Bir Rolün Sahip Olduğu Yetkileri Getir
        [HttpGet("getbyroleid")]
        public IActionResult GetByRoleId(int roleId)
        {
            var result = _ocpService.GetByOperationClaimId(roleId);
            return Ok(result);
        }

        // 2. Role Yetki Ata (Eşleştirme yap)
        // Body: { "operationClaimId": 1, "permissionId": 3 }
        [HttpPost("assign")]
        public IActionResult Assign(OperationClaimPermission operationClaimPermission)
        {
            // İleride buraya "Bu yetki zaten bu role atanmış mı?" kontrolü eklenebilir.
            _ocpService.Add(operationClaimPermission);
            return Ok("Yetki role başarıyla atandı.");
        }
    }
}
