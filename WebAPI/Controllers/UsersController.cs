using Business.Abstract;
using Entities.DTOs.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // 1. ID İLE KULLANICI DETAYI GETİR
        // GET api/users/getbyid?id=3
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Kullanıcı bulunamadı veya silinmiş.");
        }

        // 2. KULLANICI SİL (Soft-Delete)
        // DELETE api/users/delete?id=3
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            // Önce silinecek kullanıcının varlığını kontrol ediyoruz
            var userToDelete = _userService.GetById(id);
            if (userToDelete == null)
            {
                return BadRequest("Silinmek istenen kullanıcı bulunamadı.");
            }

            // DB'den çektiğimiz nesneyi Delete metoduna gönderiyoruz
            _userService.Delete(userToDelete);
            return Ok("Kullanıcı başarıyla silindi (IsDeleted=true yapıldı).");
        }

        // 3. KULLANICI BİLGİLERİNİ GÜNCELLE
        // PUT api/users/update
        [HttpPut("update")]
        public IActionResult Update(UserForUpdateDto userForUpdateDto)
        {
            // Önce güncellenecek kullanıcının varlığını kontrol ediyoruz
            var userToUpdate = _userService.GetById(userForUpdateDto.Id);
            if (userToUpdate == null)
            {
                return BadRequest("Güncellenmek istenen kullanıcı bulunamadı.");
            }

            // Sadece DTO'dan gelen alanları güncelliyoruz (Şifre Hash/Salt korunuyor)
            userToUpdate.FirstName = userForUpdateDto.FirstName;
            userToUpdate.LastName = userForUpdateDto.LastName;
            userToUpdate.Email = userForUpdateDto.Email;
            userToUpdate.NickName = userForUpdateDto.NickName;

            _userService.Update(userToUpdate);
            return Ok("Kullanıcı bilgileri başarıyla güncellendi.");
        }
    }
}
