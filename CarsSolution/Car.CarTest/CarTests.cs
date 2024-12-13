using Cars.Core.Domain;
using Cars.Data;
using CarsSolution.Controllers;
using CarsSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Car.CarTest
{
    public class CarsControllerTests
    {
        [Fact]
        public async Task Add_Post_ValidModel_RedirectsToList()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AddCarTestDb").Options;

            using var context = new ApplicationDbContext(options);
            var controller = new CarsController(context);
            var viewModel = new AddCarViewModel
            {
                Brand = "Toyota",
                Model = "Corolla",
                Year = 2020,
                Horsepower = 150,
                Price = 20000
            };

            var result = await controller.Add(viewModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirectResult.ActionName);
        }

        [Fact]
        public async Task List_ReturnsViewWithCars()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ListCarsTestDb").Options;

            using var context = new ApplicationDbContext(options);
            context.Cars.Add(new Cars.Core.Domain.Car { Brand = "BMW", Model = "M3", Year = 2019, Horsepower = 425, Price = 60000 });
            context.Cars.Add(new Cars.Core.Domain.Car { Brand = "Audi", Model = "RS5", Year = 2021, Horsepower = 450, Price = 70000 });
            await context.SaveChangesAsync();

            var controller = new CarsController(context);

            var result = await controller.List();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Cars.Core.Domain.Car>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Edit_Post_ValidModel_UpdatesCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EditCarTestDb").Options;

            var carId = Guid.NewGuid();

            using (var context = new ApplicationDbContext(options))
            {
                context.Cars.Add(new Cars.Core.Domain.Car
                {
                    Id = carId,
                    Brand = "Mercedes",
                    Model = "C-Class",
                    Year = 2018,
                    Horsepower = 300,
                    Price = 50000
                });
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new CarsController(context);
                var updatedCar = new Cars.Core.Domain.Car
                {
                    Id = carId,
                    Brand = "Mercedes",
                    Model = "E-Class",
                    Year = 2019,
                    Horsepower = 350,
                    Price = 55000
                };

                var result = await controller.Edit(updatedCar);

                var carInDb = await context.Cars.FindAsync(carId);
                Assert.Equal("E-Class", carInDb.Model);
                Assert.Equal(2019, carInDb.Year);
            }
        }

        [Fact]
        public async Task Delete_Post_RemovesCarFromDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteCarTestDb").Options;

            var carId = Guid.NewGuid();

            using (var context = new ApplicationDbContext(options))
            {
                context.Cars.Add(new Cars.Core.Domain.Car { Id = carId, Brand = "Ford", Model = "Mustang", Year = 2022, Horsepower = 450, Price = 55000 });
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new CarsController(context);

                var result = await controller.Delete(carId);

                var carInDb = await context.Cars.FindAsync(carId);
                Assert.Null(carInDb);
            }
        }
    }
}