using DynaDevAPI.Models;
using DynaDevAPI.ViewModels;

namespace DynaDevAPI.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
