using Microsoft.AspNetCore.Mvc;
using goldenCenterNew.Data;
using goldenCenterNew.Models;

namespace goldenCenterNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisabledDevicesController : Controller
    {
        private readonly GoldenCenterContext _context;

        public DisabledDevicesController(GoldenCenterContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("check")]
        public IActionResult CheckDisabledDevices(string format = null)
        {
            var disabledDevices = new List<object>();

            // Iterar sobre todos los dispositivos activos
            var devices = _context.CT_Devices.ToList();
            foreach (var device in devices)
            {
                var deviceType = _context.CT_DeviceTypes.FirstOrDefault(dt => dt.PKDeviceTypeID == device.FKTypeID);
                if (deviceType != null)
                {
                    // Verificar si el dispositivo excede sus límites
                    if (device.Cycles >= deviceType.CyclesLimit)
                    {
                        device.Available = false; // Marcar como no disponible
                        _context.SaveChanges();

                        // Insertar alerta en CR_Alerts
                        _context.CR_Alerts.Add(new CR_Alerts
                        {
                            FKDeviceID = device.PKDeviceID,
                            Type = 1, // Tipo de alerta, puedes definirlo
                            Message = $"Device {device.SerialNumber} exceeded its usage limit.",
                            Date = DateTime.Now,
                            LastUpdated = DateTime.Now,
                            Available = true
                        });
                        _context.SaveChanges();

                        disabledDevices.Add(new
                        {
                            device.SerialNumber,
                            DeviceType = deviceType.Description,
                            device.Cycles,
                            device.Available,
                            Message = "Device marked as disabled due to exceeded limits."
                        });
                    }
                }
            }

            if (!string.IsNullOrEmpty(format) || format.ToLower() == "json")
            {
                return Ok(disabledDevices);
            }

            return View(disabledDevices);
        }
    }
}