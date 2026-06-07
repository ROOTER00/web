using System.ComponentModel.DataAnnotations;

namespace Kurdixane.Web.ViewModels;

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Ad Soyad zorunludur."), StringLength(200)]
    [Display(Name = "Ad Soyad")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur."), EmailAddress, StringLength(200)]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(40)]
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Adres zorunludur."), StringLength(500)]
    [Display(Name = "Adres")]
    public string Address { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Şehir")]
    public string? City { get; set; }

    [StringLength(1000)]
    [Display(Name = "Sipariş Notu")]
    public string? Note { get; set; }

    public CartViewModel Cart { get; set; } = new();
}
