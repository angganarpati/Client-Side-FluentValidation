using ClientSideFluentValidation.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientSideFluentValidation.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Basic()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Basic(BasicPersonViewModel person)
        {
            if (!ModelState.IsValid)
            { // re-render the view when validation failed.
                return View("Basic", person);
            }

            TempData["Message"] = "Person successfully created";
            return RedirectToAction("Basic");
        }
    }
}