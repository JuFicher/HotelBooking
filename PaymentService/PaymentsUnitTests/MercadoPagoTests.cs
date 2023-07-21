using NUnit.Framework;
using Application.Booking.DTO;
using Application;
using System.Threading.Tasks;
using Application.MercadoPago;

namespace PaymentsUnitTests;

public class Tests
{
    [Test]
    public void ShouldReturnMercadoPagoAdapterProvider()
    {   
        var factory = new PaymentProcessorFactory();

        var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

        Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
    }

    [Test]
    public async void ShouldFailWhenPaymentIntentionStringIsInvalid()
    {   
        var factory = new PaymentProcessorFactory();

        var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

        var response = await provider.CapturePayment("");

        Assert.False(response.Success);
        Assert.AreEqual(response.ErrorCode, ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION);
        Assert.AreEqual(response.Message, "The selected payment intention is invalid");
    }

    [Test]
    public async Task ShouldSuccessfullyProcessPayment()
    {
        var factory = new PaymentProcessorFactory();

        var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

        var response = await provider.CapturePayment("https://www.mercadopago.com.br/exemple");

        Assert.IsTrue(response.Success);
        Assert.AreEqual(response.Message, "Payment successfully processed");
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.CreatedDate);
        Assert.NotNull(response.Data.PaymentId);
    }
}