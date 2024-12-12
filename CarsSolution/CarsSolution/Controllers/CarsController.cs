using CarsSolution.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarsSolution.Controllers
{
    public class CarsController: Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddCarViewModel viewModel)
        {
            return View();
        }
    }
}
