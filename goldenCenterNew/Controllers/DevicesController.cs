using Microsoft.AspNetCore.Mvc;
using goldenCenterNew.Data;
using goldenCenterNew.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace goldenCenterNew.Controllers
{
    public class DevicesController : Controller
{
    private readonly GoldenCenterContext _context;

    public DevicesController(GoldenCenterContext context)
    {
        _context = context;
    }

    public IActionResult Index(int? typeFilter)
    {
        var devicesQuery = _context.CT_Devices.AsQueryable();

        if (typeFilter.HasValue)
        {
            devicesQuery = devicesQuery.Where(d => d.FKTypeID == typeFilter.Value);
        }

        //var devices = _context.CT_Devices.Select(d => new DeviceModel { Id = d.PKDeviceID, SerialNumber = d.SerialNumber, FKTypeID = d.FKTypeID, Cycles = d.Cycles, WeeklyCycles = d.WeeklyCycles, DeviceTypeName = d.FKTypeID == 1 ? "Bateria" : d.FKTypeID == 2 ? "Rectificador" : d.FKTypeID == 3 ? "Umihebi" : "Desconocido"}).ToList();
        var devices = devicesQuery.Select(d => new DeviceModel { Id = d.PKDeviceID, SerialNumber = d.SerialNumber, FKTypeID = d.FKTypeID, Cycles = d.Cycles, WeeklyCycles = d.WeeklyCycles, DeviceTypeName = d.FKTypeID == 1 ? "Bateria" : d.FKTypeID == 2 ? "Rectificador" : d.FKTypeID == 3 ? "Umihebi" : "Desconocido" }).ToList();


        ViewBag.TypeFilterOptions = new List<SelectListItem>
        {
            new SelectListItem { Text = "Todos", Value = "", Selected = !typeFilter.HasValue },
            new SelectListItem { Text = "Baterías", Value = "1", Selected = typeFilter == 1 },
            new SelectListItem { Text = "Rectificadores", Value = "2", Selected = typeFilter == 2 },
            new SelectListItem { Text = "Umihebi", Value = "3", Selected = typeFilter == 3 }
        };

        return View(devices);
    }

    public IActionResult Create()
    {
        DeviceModel deviceModel = new DeviceModel();

        ViewBag.DeviceTypes = new SelectList(new[]
        {
            new {Id = 0, Name = "Seleccionar"},
            new { Id = 1, Name = "Bateria"},
            new { Id = 2, Name = "Rectificador" },
            new { Id = 3, Name = "Umihebi" }
        }, "Id", "Name");

        return View(deviceModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(DeviceModel deviceModel)
    {
        if (ModelState.IsValid)
        {
            CT_Devices device = new CT_Devices
            {
                SerialNumber = deviceModel.SerialNumber,
                FKTypeID = deviceModel.FKTypeID,
                Cycles = deviceModel.Cycles,
                WeeklyCycles = deviceModel.WeeklyCycles,
                Available = true,
                LastUpdated = DateTime.Now,
            };

            _context.CT_Devices.Add(device);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        ViewBag.DeviceTypes = new SelectList(new[]
{
            new {Id = 0, Name = "Seleccionar"},
            new { Id = 1, Name = "Bateria"},
            new { Id = 2, Name = "Rectificador" },
            new { Id = 3, Name = "Umihebi" }
        }, "Id", "Name");

        return View(deviceModel);
    }

    public IActionResult Delete(int id)
    {
        //if (id <= 0)
        //{
        //    return NotFound();
        //}

        var device = _context.CT_Devices.FirstOrDefault(d => d.PKDeviceID == id);

        if (device == null)
        {
            return NotFound();
        }

        return View(device);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var device = _context.CT_Devices.Find(id);

        if (device == null)
        {
            return NotFound();
        }

        _context.CT_Devices.Remove(device);
        _context.SaveChanges();

        ViewBag.Message = "El dispositivo se elimino exitosamente";

        return View("Delete");
    }
}
}
