using Application.Booking;
using Application.Booking.DTO;
using Application.Payment.Dtos;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Domain.Booking.Ports;
using Domain.Ports;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ApplicationTests
{
    public class BookingManagerTest
    {
        [Test]
        public async Task Should_PayForABooking()
        {
            var dto = new PaymentRequestDto
            {
                SelectedPaymentProvider = SupportedPaymentProviders.MercadoPago,
                paymentIntention = "https://www.mercadopago.com.br/exemple",
                SelectedPaymentMethod = SupportedPaymentMethods.CreditCard
            };

            var bookingRepository = new Mock<IBookingRepository>();
            var roomRepository = new Mock<IRoomRepository>();
            var guestRepository = new Mock<IGuestRepository>();
            var paymentProcessorFactory = new Mock<IPaymentProcessorFactory>();
            var paymentProcessor = new Mock<IPaymentProcessor>();

            var responseDto = new PaymentStateDto
            {
                CreatedDate = DateTime.Now,
                Message = $"Successfully paid {dto.paymentIntention}",
                PaymentId = "123",
                Status = Status.Success
            };

            var response = new PaymentResponse
            {
                Data = responseDto,
                Success = true,
                Message = "Payment successfully processed"
            };

            paymentProcessor
                            .Setup(x => x.CapturePayment(dto.paymentIntention))
                            .Returns(Task.FromResult(response));

            paymentProcessorFactory
                            .Setup(x => x.GetPaymentProcessor(dto.SelectedPaymentProvider))
                            .Returns(paymentProcessor.Object);

            var bookingManager = new BookingManager(
                bookingRepository.Object,
                roomRepository.Object,
                guestRepository.Object,
                paymentProcessorFactory.Object);

            var res = await bookingManager.PayForABooking(dto);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Message, "Payment successfully processed");
        }        
    }
}