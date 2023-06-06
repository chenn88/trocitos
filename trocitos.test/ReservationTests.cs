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
            Assert.Equal("Table is available.", message);


        }
    }
}








