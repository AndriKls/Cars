using Cars.Core.Domain;
using Cars.Data;
using CarsSolution.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarsSolution.Controllers
{
    public class CarsController: Controller
    {
        private readonly ApplicationDbContext dbContext;
        public CarsController(ApplicationDbContext dbContext) 
        { 
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCarViewModel viewModel)
        {
            var car = new Car
            {
                Brand = viewModel.Brand,
                Model = viewModel.Model,
                Year = viewModel.Year,
                Horsepower = viewModel.Horsepower,
                Price = viewModel.Price,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await dbContext.Cars.AddAsync(car);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }



        [HttpGet]
        public async Task<IActionResult> List()
        {
            var cars = await dbContext.Cars.ToListAsync();

            return View(cars);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var car = await dbContext.Cars.FindAsync(id);

            if (car is null)
            {
                return NotFound();
            }

            return View(car);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Car viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var car = await dbContext.Cars.FindAsync(viewModel.Id);

            if (car is not null)
            {
                car.Brand = viewModel.Brand;
                car.Model = viewModel.Model;
                car.Year = viewModel.Year;
                car.Horsepower = viewModel.Horsepower;
                car.Price = viewModel.Price;
                car.UpdatedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await dbContext.Cars.FindAsync(id);

            if (car is not null)
            {
                dbContext.Cars.Remove(car);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List");
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var car = await dbContext.Cars.FindAsync(id);

            if (car is null)
            {
                return NotFound();
            }

            return View(car);
        }


    }
}
