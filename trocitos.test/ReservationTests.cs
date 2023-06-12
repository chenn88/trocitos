using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using trocitos.mvc.Controllers;
using trocitos.mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Trocitos.Tests
{
    public class ReservationControllerTests
    {
        [Fact]
        public void CheckAvailability_ReturnsAvailableTable()
        {

            var dataTables = new List<Table>
                {
                    new Table { TableNo = 1, Capacity = 4, Location = "outside" }
                }.AsQueryable();

            var dataReservations = new List<Reservation>().AsQueryable();

            var mockSetTable = new Mock<DbSet<Table>>();
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Provider).Returns(dataTables.Provider);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Expression).Returns(dataTables.Expression);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.ElementType).Returns(dataTables.ElementType);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.GetEnumerator()).Returns(dataTables.GetEnumerator());

            var mockSetReservation = new Mock<DbSet<Reservation>>();
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(dataReservations.Provider);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(dataReservations.Expression);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(dataReservations.ElementType);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(dataReservations.GetEnumerator());

            var mockContext = new Mock<TrocitosDbContext>();
            mockContext.Setup(c => c.TableCatalogue).Returns(mockSetTable.Object);
            mockContext.Setup(c => c.Reservations).Returns(mockSetReservation.Object);

            var controller = new ReservationController(mockContext.Object);

            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string startTime = "18:00";
            int capacity = 4;
            string location = "outside";


            var actionResult = controller.CheckAvailability(date, startTime, capacity, location);


            var jsonResult = actionResult as JsonResult;


            Assert.NotNull(jsonResult);


            var successProperty = jsonResult?.Value?.GetType().GetProperty("success");
            var messageProperty = jsonResult?.Value?.GetType().GetProperty("message");


            Assert.NotNull(successProperty);
            Assert.NotNull(messageProperty);


            bool? success = successProperty.GetValue(jsonResult?.Value, null) as bool?;
            string? message = messageProperty.GetValue(jsonResult?.Value, null) as string;


            Assert.True(success);
            Assert.Equal("A table is available! Please fill in details below to reserve.", message);


        }

        [Fact]
        public void CheckAvailability_ReturnsNoTableAvailable()
        {

            var dataTables = new List<Table>
            {
                new Table { TableNo = 1, Capacity = 2, Location = "ground floor" }
            }.AsQueryable();

            var dataReservations = new List<Reservation>().AsQueryable();

            var mockSetTable = new Mock<DbSet<Table>>();
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Provider).Returns(dataTables.Provider);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Expression).Returns(dataTables.Expression);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.ElementType).Returns(dataTables.ElementType);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.GetEnumerator()).Returns(dataTables.GetEnumerator());

            var mockSetReservation = new Mock<DbSet<Reservation>>();
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(dataReservations.Provider);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(dataReservations.Expression);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(dataReservations.ElementType);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(dataReservations.GetEnumerator());

            var mockContext = new Mock<TrocitosDbContext>();
            mockContext.Setup(c => c.TableCatalogue).Returns(mockSetTable.Object);
            mockContext.Setup(c => c.Reservations).Returns(mockSetReservation.Object);

            var controller = new ReservationController(mockContext.Object);

            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string startTime = "18:00";
            int capacity = 4;
            string location = "outside";


            var actionResult = controller.CheckAvailability(date, startTime, capacity, location);

            var jsonResult = actionResult as JsonResult;
            Assert.NotNull(jsonResult);

            var successProperty = jsonResult?.Value?.GetType().GetProperty("success");
            var messageProperty = jsonResult?.Value?.GetType().GetProperty("message");

            Assert.NotNull(successProperty);
            Assert.NotNull(messageProperty);

            bool? success = successProperty.GetValue(jsonResult?.Value, null) as bool?;
            string? message = messageProperty.GetValue(jsonResult?.Value, null) as string;

            Assert.False(success);
            Assert.Equal("No tables available for the given capacity and location.", message);
        }


        [Fact]
        public void Book_ReturnsSuccessfulReservation()
        {

            var dataTables = new List<Table>
            {
                new Table { TableNo = 1, Capacity = 4, Location = "outside" }
            }.AsQueryable();

            var dataReservations = new List<Reservation>().AsQueryable();

            var mockSetTable = new Mock<DbSet<Table>>();
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Provider).Returns(dataTables.Provider);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Expression).Returns(dataTables.Expression);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.ElementType).Returns(dataTables.ElementType);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.GetEnumerator()).Returns(dataTables.GetEnumerator());

            var mockSetReservation = new Mock<DbSet<Reservation>>();
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(dataReservations.Provider);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(dataReservations.Expression);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(dataReservations.ElementType);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(dataReservations.GetEnumerator());

            var mockContext = new Mock<TrocitosDbContext>();
            mockContext.Setup(c => c.TableCatalogue).Returns(mockSetTable.Object);
            mockContext.Setup(c => c.Reservations).Returns(mockSetReservation.Object);
            mockContext.Setup(c => c.Reservations.Add(It.IsAny<Reservation>())).Verifiable();
            mockContext.Setup(c => c.SaveChanges()).Verifiable();

            var controller = new ReservationController(mockContext.Object);

            string firstName = "John";
            string surname = "Doe";
            string phoneNo = "123456789";
            string email = "john.doe@example.com";
            DateTime date = DateTime.Now;
            TimeSpan? startTime = new TimeSpan(18, 00, 00);
            TimeSpan? endTime = startTime.Value.Add(TimeSpan.FromHours(2.5));
            int capacity = 4;
            string location = "outside";


            var actionResult = controller.Book(firstName, surname, phoneNo, email, date, startTime, endTime, capacity, location);


            var jsonResult = actionResult as JsonResult;
            Assert.NotNull(jsonResult);

            var successProperty = jsonResult?.Value?.GetType().GetProperty("success");
            var messageProperty = jsonResult?.Value?.GetType().GetProperty("message");

            Assert.NotNull(successProperty);
            Assert.NotNull(messageProperty);

            bool? success = successProperty.GetValue(jsonResult?.Value, null) as bool?;
            string? message = messageProperty.GetValue(jsonResult?.Value, null) as string;

            Assert.True(success);
            Assert.Equal("Reservation has been successfully booked:", message);
            mockContext.Verify(c => c.Reservations.Add(It.IsAny<Reservation>()), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CancelReservation_SuccessfulCancellation()
        {

            var dataReservations = new List<Reservation>
            {
                 new Reservation
                     {ReservationId = 1,FirstName = "John",Surname = "Doe",PhoneNo = "123456789",Email = "john.doe@example.com",Cancellation = false}
            }.AsQueryable();

            var mockSetReservation = new Mock<DbSet<Reservation>>();
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(dataReservations.Provider);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(dataReservations.Expression);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(dataReservations.ElementType);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(dataReservations.GetEnumerator());

            var mockContext = new Mock<TrocitosDbContext>();
            mockContext.Setup(c => c.Reservations).Returns(mockSetReservation.Object);
            mockContext.Setup(c => c.SaveChanges()).Verifiable();

            var controller = new ReservationController(mockContext.Object);

            int reservationId = 1;
            string contactInfo = "123456789";


            var actionResult = controller.CancelReservation(reservationId, contactInfo);


            var jsonResult = actionResult as JsonResult;
            Assert.NotNull(jsonResult);

            var successProperty = jsonResult?.Value?.GetType().GetProperty("success");
            var messageProperty = jsonResult?.Value?.GetType().GetProperty("message");

            Assert.NotNull(successProperty);
            Assert.NotNull(messageProperty);

            bool? success = successProperty.GetValue(jsonResult?.Value, null) as bool?;
            string? message = messageProperty.GetValue(jsonResult?.Value, null) as string;

            Assert.True(success);
            Assert.Equal("Reservation has been successfully cancelled.", message);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }


        [Fact]
        public void Book_ReturnsErrorWhenConflictingReservationExists()
        {

            var dataTables = new List<Table>
            {
                new Table { TableNo = 1, Capacity = 4, Location = "outside" }
            }.AsQueryable();

            DateTime testDate = new DateTime(2023, 6, 1);

            var dataReservations = new List<Reservation>
            {
                new Reservation {
                    ReservationId = 1,
                    ReservationDate = testDate,
                    RsvStart = TimeOnly.FromTimeSpan(TimeSpan.Parse("18:00")),
                    RsvEnd = TimeOnly.FromTimeSpan(TimeSpan.Parse("18:00")).Add(TimeSpan.FromHours(2.5)),
                    TableNo = 1
                }
            }.AsQueryable();

            var mockSetTable = new Mock<DbSet<Table>>();
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Provider).Returns(dataTables.Provider);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.Expression).Returns(dataTables.Expression);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.ElementType).Returns(dataTables.ElementType);
            mockSetTable.As<IQueryable<Table>>().Setup(m => m.GetEnumerator()).Returns(dataTables.GetEnumerator());

            var mockSetReservation = new Mock<DbSet<Reservation>>();
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(dataReservations.Provider);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(dataReservations.Expression);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(dataReservations.ElementType);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(dataReservations.GetEnumerator());

            var mockContext = new Mock<TrocitosDbContext>();
            mockContext.Setup(c => c.TableCatalogue).Returns(mockSetTable.Object);
            mockContext.Setup(c => c.Reservations).Returns(mockSetReservation.Object);

            var controller = new ReservationController(mockContext.Object);


            var actionResult = controller.Book("John", "Doe", "123456789", "john.doe@example.com", testDate, TimeSpan.Parse("18:00"), TimeSpan.Parse("20:30"), 4, "outside");


            var jsonResult = actionResult as JsonResult;
            Assert.NotNull(jsonResult);

            var successProperty = jsonResult?.Value?.GetType().GetProperty("success");
            var messageProperty = jsonResult?.Value?.GetType().GetProperty("message");

            Assert.NotNull(successProperty);
            Assert.NotNull(messageProperty);

            bool? success = successProperty.GetValue(jsonResult?.Value, null) as bool?;
            string? message = messageProperty.GetValue(jsonResult?.Value, null) as string;

            Assert.False(success);
            Assert.Equal("There is a reservation already in the requested time slot.", message);
        }

        [Fact]
        public void ReservationExists_ReturnsTrueWhenReservationExists()
        {

            var dataReservations = new List<Reservation>
            {
                new Reservation {
                    ReservationId = 1,
                    FirstName = "John",
                    Surname = "Doe",
                    PhoneNo = "123456789",
                    Email = "john.doe@example.com",
                    Cancellation = false}
            }.AsQueryable();

            var mockSetReservation = new Mock<DbSet<Reservation>>();
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(dataReservations.Provider);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(dataReservations.Expression);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(dataReservations.ElementType);
            mockSetReservation.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(dataReservations.GetEnumerator());

            var mockContext = new Mock<TrocitosDbContext>();
            mockContext.Setup(c => c.Reservations).Returns(mockSetReservation.Object);

            var controller = new ReservationController(mockContext.Object);

            var actionResult = controller.ReservationExists(1, "123456789");

            var jsonResult = actionResult as JsonResult;
            Assert.NotNull(jsonResult);

            var existsProperty = jsonResult?.Value?.GetType().GetProperty("exists");

            Assert.NotNull(existsProperty);

            bool? exists = existsProperty.GetValue(jsonResult?.Value, null) as bool?;

            Assert.True(exists);
        }

    }
}








