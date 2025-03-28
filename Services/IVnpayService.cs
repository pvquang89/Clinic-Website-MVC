using WebPhongKham.Models.ViewModel;

namespace WebPhongKham.Services
{
    public interface IVnpayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequest model);

        VnPaymentResponseModel PaymentExcute(IQueryCollection collection);
    }
}
