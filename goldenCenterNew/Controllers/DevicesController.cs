using goldenCenterNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class DevicesController : Controller
{
    public IActionResult Create()
    {
        var deviceTypes = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Seleccionar"},
            new SelectListItem { Value = "1", Text = "Batería" },
            new SelectListItem { Value = "2", Text = "Rectificador" },
            new SelectListItem { Value = "3", Text = "Umihebi" }
        };

        ViewBag.DeviceTypes = deviceTypes;

        return View();
    }

    public IActionResult Index()
    {
        var devices = new List<DeviceModel>();
        return View(devices);
    }

    [HttpPost]
    public IActionResult Create(goldenCenterNew.Models.DeviceModel model)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        ViewBag.DeviceTypes = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Seleccionar"},
            new SelectListItem { Value = "1", Text = "Batería" },
            new SelectListItem { Value = "2", Text = "Rectificador" },
            new SelectListItem { Value = "3", Text = "Umihebi" }
        };

        return View(model);
    }
}