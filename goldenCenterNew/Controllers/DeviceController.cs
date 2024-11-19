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

        // Endpoint para actualizar múltiples dispositivos y registrar ciclos
        [HttpPost]
        [Route("updateCyclesBatch")]
        public IActionResult UpdateCyclesBatch([FromBody] List<DeviceUpdateModel> devices)
        {
            if (devices == null || devices.Count == 0)
            {
                return BadRequest(new { message = "No devices provided." });
            }

            var results = new List<object>();

            foreach (var device in devices)
            {
                if (string.IsNullOrEmpty(device.SerialNumber) || device.FKTypeID <= 0)
                {
                    results.Add(new { SerialNumber = device.SerialNumber, Message = "Invalid device data provided." });
                    continue;
                }

                // Buscar el dispositivo en la base de datos
                var existingDevice = _context.CT_Devices
                    .FirstOrDefault(d => d.SerialNumber == device.SerialNumber && d.FKTypeID == device.FKTypeID);

                if (existingDevice == null)
                {
                    // Si el dispositivo no existe, agregarlo como nuevo
                    var newDevice = new CT_Devices
                    {
                        SerialNumber = device.SerialNumber,
                        FKTypeID = device.FKTypeID,
                        Cycles = 0, // Inicializar ciclos
                        WeeklyCycles = 0, // Inicializar ciclos semanales
                        LastUpdated = DateTime.Now,
                        Available = true
                    };

                    _context.CT_Devices.Add(newDevice);
                    _context.SaveChanges();

                    results.Add(new
                    {
                        SerialNumber = newDevice.SerialNumber,
                        DeviceType = GetDeviceTypeName(newDevice.FKTypeID),
                        Cycles = newDevice.Cycles,
                        WeeklyCycles = newDevice.WeeklyCycles,
                        Available = newDevice.Available,
                        Message = "New device added successfully."
                    });

                    continue; // Pasar al siguiente dispositivo
                }

                // Actualizar ciclos si el dispositivo ya existe
                existingDevice.Cycles += 1;
                existingDevice.WeeklyCycles += 1;
                existingDevice.LastUpdated = DateTime.Now;

                var deviceType = _context.CT_DeviceTypes.FirstOrDefault(dt => dt.PKDeviceTypeID == existingDevice.FKTypeID);
                if (deviceType != null && existingDevice.WeeklyCycles > deviceType.WeeklyCyclesLimit)
                {
                    existingDevice.Available = false; // Marcar como no disponible
                    _context.SaveChanges();

                    results.Add(new
                    {
                        SerialNumber = existingDevice.SerialNumber,
                        DeviceType = GetDeviceTypeName(existingDevice.FKTypeID),
                        Cycles = existingDevice.Cycles,
                        WeeklyCycles = existingDevice.WeeklyCycles,
                        Available = existingDevice.Available,
                        Message = "Device exceeded weekly usage limit. Maintenance required."
                    });

                    continue; // No seguir procesando este dispositivo
                }

                _context.SaveChanges();

                results.Add(new
                {
                    SerialNumber = existingDevice.SerialNumber,
                    DeviceType = GetDeviceTypeName(existingDevice.FKTypeID),
                    Cycles = existingDevice.Cycles,
                    WeeklyCycles = existingDevice.WeeklyCycles,
                    Available = existingDevice.Available,
                    Message = "Cycles updated successfully."
                });
            }

            return Ok(results);
        }

        // Método para convertir FKTypeID a un nombre descriptivo
        private string GetDeviceTypeName(int fkTypeId)
        {
            return fkTypeId switch
            {
                1 => "Batería",
                2 => "Rectificador",
                3 => "Umihebi",
                _ => "Tipo desconocido"
            };
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