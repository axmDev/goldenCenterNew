using goldenCenterNew.Data;
using goldenCenterNew.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace goldenCenterNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly GoldenCenterContext _context;

        public DeviceController(GoldenCenterContext context)
        {
            _context = context;
        }

        // Endpoint para actualizar dispositivos y registrar ciclos
        [HttpPost]
        [Route("updateCycles")]
        public IActionResult UpdateCycles([FromBody] DeviceUpdateModel device)
        {
            if (device == null || string.IsNullOrEmpty(device.SerialNumber) || device.FKTypeID <= 0)
            {
                return BadRequest(new { message = "Invalid device data provided." });
            }

            var existingDevice = _context.CT_Devices
                .FirstOrDefault(d => d.SerialNumber == device.SerialNumber && d.FKTypeID == device.FKTypeID);

            if (existingDevice == null)
            {
                return NotFound(new { message = "Device not found." });
            }

            existingDevice.Cycles += 1;
            existingDevice.WeeklyCycles += 1;
            existingDevice.LastUpdated = DateTime.Now;

            // Verificar si excede el límite semanal
            var deviceType = _context.CT_DeviceTypes.FirstOrDefault(dt => dt.PKDeviceTypeID == existingDevice.FKTypeID);
            if (deviceType != null && existingDevice.WeeklyCycles > deviceType.WeeklyCyclesLimit)
            {
                existingDevice.Available = false;
                _context.SaveChanges();
                return Ok(new { message = "Device reached weekly limit. Maintenance required." });
            }

            _context.SaveChanges();
            return Ok(new { message = "Cycles updated successfully." });
        }

        private int GetIso8601WeekOfYear(DateTime time)
        {
            var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}