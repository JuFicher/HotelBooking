using NUnit.Framework;
using System.Threading.Tasks;
using Application.Booking.DTO;
using Application;
using Application.MercadoPago;

namespace PaymentsUnitTests;

public class PaymentProcessorFactoryTests
{
    [Test]
    public void ShouldReturnNotImplementedPaymentProviderWhenAskingForStripeProvider()
    {
        var factory = new PaymentProcessorFactory();
        var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

        Assert.AreEqual(provider.GetType(), typeof(NotImplementedPaymentProvider));
    }

    [Test]
    public void ShouldReturnMercadoPagoAdapterProvider()
    {
        var factory = new PaymentProcessorFactory();
        var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

        Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
    }

    [Test]
    public async Task ShouldReturnFalseWhenCapturingPaymentForNotImplementedPaymentProvider()
    {
        var factory = new PaymentProcessorFactory();
        var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);
        var response = await provider.CapturePayment("https://myprovider.com/test");

        Assert.False(response.Success);
        Assert.AreEqual(response.ErrorCode, ErrorCodes.PAYMENT_PROVIDER_NOT_IMPLEMENTED);
        Assert.AreEqual(response.Message, "The selected payment provider is not available at the moment");
    }
}