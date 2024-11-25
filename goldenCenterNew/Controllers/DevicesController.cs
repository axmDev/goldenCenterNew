using goldenCenterNew.Data;
using Microsoft.AspNetCore.Mvc;

namespace goldenCenterNew.Controllers
{
    public class DevicesController : Controller
    {
        private readonly GoldenCenterContext _context;

        public DevicesController(GoldenCenterContext context)
        {
            _context = context;
        }

        public IActionResult Battery()
        {
            var devices = _context.CT_Devices
                .Where(d => d.FKTypeID == 1)
                .ToList();

            ViewBag.DeviceType = "Baterías";
            return View("DeviceList", devices);
        }

        public IActionResult Rectifier()
        {
            var devices = _context.CT_Devices
                .Where(d => d.FKTypeID == 2)
                .ToList();

            ViewBag.DeviceType = "Rectificadores";
            return View("DeviceList", devices);
        }

        public IActionResult Umihebi()
        {
            var devices = _context.CT_Devices
                .Where(d => d.FKTypeID == 3)
                .ToList();

            ViewBag.DeviceType = "Umihebis";
            return View("DeviceList", devices);
        }

        public IActionResult Delete(int id)
        {
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
            var device = _context.CT_Devices.FirstOrDefault(d => d.PKDeviceID == id);
            if (device == null)
            {
                return NotFound();
            }

            var fkTypeId = device.FKTypeID;

            _context.CT_Devices.Remove(device);
            _context.SaveChanges();

            switch (fkTypeId)
            {
                case 1:
                    TempData["Message"] = "El dispositivo se eliminó exitosamente.";
                    return RedirectToAction("Battery");
                case 2:
                    TempData["Message"] = "El dispositivo se eliminó exitosamente.";
                    return RedirectToAction("Rectifier");
                case 3:
                    TempData["Message"] = "El dispositivo se eliminó exitosamente.";
                    return RedirectToAction("Umihebi");
                default:
                    return RedirectToAction("Battery");
            }
        }
    }
}