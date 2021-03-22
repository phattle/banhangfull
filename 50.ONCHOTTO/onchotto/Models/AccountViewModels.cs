using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnChotto.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Ghi nhớ trên trình duyệt")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Địa chỉ Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Yêu cầu Bạn nhập Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        [System.Web.Mvc.Remote("IsUserExists", "Validating", ErrorMessage = "{0} đã tồn tại!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu Bạn nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Độ dài {0} yêu cầu lớn hơn {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không đúng.")]
        public string ConfirmPassword { get; set; }
    }
    public class RegisterSellerViewModel
    {
        [Required(ErrorMessage = "Yêu cầu Bạn nhập Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        [System.Web.Mvc.Remote("IsUserExists", "Validating", ErrorMessage = "{0} đã tồn tại!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu Bạn nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Độ dài {0} yêu cầu lớn hơn {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không đúng.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Loại hình")]
        public string Type  { get; set; }

        [Required(ErrorMessage = "Yêu cầu Bạn nhập địa chỉ kho")]
        [Display(Name = "Địa chỉ kho")]
        public string AddressWarehouse { get; set; }

        [Required(ErrorMessage = "Yêu cầu Bạn nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "IsRead")]
        public bool IsRead { get; set; }

        [Required(ErrorMessage = "Yêu cầu Bạn nhập tên gian hàng")]
        [Display(Name = "Tên gian hàng")]
        public string StandName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Yêu cầu Bạn nhập Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu Bạn nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Độ dài {0} yêu cầu lớn hơn {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không đúng.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
