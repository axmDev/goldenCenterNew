using goldenCenterNew.Data;
using Microsoft.AspNetCore.Mvc;

namespace goldenCenterNew.Controllers
{
    public class HomeController : Controller
    {
        private readonly GoldenCenterContext _context;

        public HomeController(GoldenCenterContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var deviceStats = _context.CT_DeviceTypes.Select(dt => new
            {
                dt.Description,
                TotalDevices = _context.CT_Devices.Count(d => d.FKTypeID == dt.PKDeviceTypeID)
            }).ToList();

            ViewBag.DeviceStats = deviceStats;

            return View();
        }
    }
}