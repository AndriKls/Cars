using Cars.Core.Domain;
using Cars.Data;
using CarsSolution.Models;
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
        public async Task <IActionResult> Add(AddCarViewModel viewModel)
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

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var cars = await dbContext.Cars.ToListAsync();

            return View(cars);
        }
    }
}
