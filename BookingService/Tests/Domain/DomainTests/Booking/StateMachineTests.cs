using NUnit.Framework;
using Domain.Entities;
using Domain.Enums;

namespace DomainTests.Bookings;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ShouldAlwaysStartWithCreatedStatus()
    {
        var booking = new Booking();
        Assert.AreEqual(booking.Status, Status.Created);
    }

    [Test]
    public void ShouldSetStatusToPaid()
    {
        var booking = new Booking();
        booking.ChangeState(Action.Pay);
        Assert.AreEqual(booking.Status, Status.Paid);
    }

    public void ShouldSetStatusToCanceled()
    {
        var booking = new Booking();
        booking.ChangeState(Action.Cancel);
        Assert.AreEqual(booking.Status, Status.Canceled);
    }

    public void ShouldSetStatusToFinished()
    {
        var booking = new Booking();

        booking.ChangeState(Action.Pay);
        booking.ChangeState(Action.Finish);
        Assert.AreEqual(booking.Status, Status.Finished);
    }

    public void ShouldSetStatusToRefunded()
    {
        var booking = new Booking();

        booking.ChangeState(Action.Pay);
        booking.ChangeState(Action.Refound);
        Assert.AreEqual(booking.Status, Status.Refounded);
    }

    public void ShouldSetStatusToCreatedWhenReopeningACanceledBooking()
    {
        var booking = new Booking();

        booking.ChangeState(Action.Cancel);
        booking.ChangeState(Action.Reopen);
        Assert.AreEqual(booking.Status, Status.Created);
    }

    public void ShouldNotChangeStatusWhenRefoundingABookingWithCreatedStatus()
    {
        var booking = new Booking();

        booking.ChangeState(Action.Refound);
        Assert.AreEqual(booking.Status, Status.Created);
    }

    public void ShouldNotChangeStatusWhenRefoundingABookingWithFinishedStatus()
    {
        var booking = new Booking();

        booking.ChangeState(Action.Pay);
        booking.ChangeState(Action.Finish);
        booking.ChangeState(Action.Refound);
        Assert.AreEqual(booking.Status, Status.Finished);
    }
}