using System.ComponentModel.DataAnnotations;

namespace DatingApp_BackEnd.Dto
{
    public class UserForRegisterDto
    {
        [Required]
        public string username {get; set;}
        
        [Required]
        [StringLength(8, MinimumLength= 4, ErrorMessage = "Password is too short")]
        public string password {get; set;}
    }
}