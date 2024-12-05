using goldenCenterNew.Models;
using goldenCenterNew.Data;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace goldenCenterNew.Controllers
{
    public class DeviceController : Controller
    {
        private readonly GoldenCenterContext _context;

        public DeviceController(GoldenCenterContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("updateCyclesBatch")]
        public IActionResult UpdateCyclesBatch([FromBody] List<DeviceUpdateModel> devices)
        {
            if (devices == null || devices.Count == 0)
            {
                return BadRequest(new { message = "No devices provided." });
            }

            var results = new List<object>();
            var currentDate = DateTime.Now;
            var currentWeek = GetIso8601WeekOfYear(currentDate);
            var currentYear = currentDate.Year;

            foreach (var device in devices)
            {
                if (string.IsNullOrEmpty(device.SerialNumber) || device.FKTypeID <= 0)
                {
                    results.Add(new { SerialNumber = device.SerialNumber, Message = "Invalid device data provided." });
                    continue;
                }

                var existingDevice = _context.CT_Devices
                    .FirstOrDefault(d => d.SerialNumber == device.SerialNumber && d.FKTypeID == device.FKTypeID);

                if (existingDevice == null)
                {
                    var newDevice = new CT_Devices
                    {
                        SerialNumber = device.SerialNumber,
                        FKTypeID = device.FKTypeID,
                        Cycles = 1,
                        LastUpdated = currentDate,
                        Available = true
                    };

                    _context.CT_Devices.Add(newDevice);
                    _context.SaveChanges();

                    _context.CR_CyclesHistories.Add(new CR_CyclesHistories
                    {
                        FKDeviceID = newDevice.PKDeviceID,
                        Week = currentWeek,
                        Year = currentYear,
                        WeeklyCycles = 1,
                        LastUpdated = currentDate,
                        Available = true
                    });

                    _context.SaveChanges();

                    var deviceTypeNew = _context.CT_DeviceTypes.FirstOrDefault(dt => dt.PKDeviceTypeID == newDevice.FKTypeID);
                    results.Add(new
                    {
                        SerialNumber = newDevice.SerialNumber,
                        DeviceType = deviceTypeNew.Description,
                        Cycles = newDevice.Cycles,
                        Available = newDevice.Available,
                        Message = "Device added and registered successfully."
                    });

                    continue;
                }

                existingDevice.Cycles += 1;
                existingDevice.LastUpdated = currentDate;

                var deviceType = _context.CT_DeviceTypes.FirstOrDefault(dt => dt.PKDeviceTypeID == existingDevice.FKTypeID);
                if (deviceType != null)
                {
                    var usagePercentage = (double)existingDevice.Cycles / deviceType.CyclesLimit * 100;

                    if (usagePercentage >= 80 && usagePercentage < 100)
                    {
                        results.Add(new
                        {
                            SerialNumber = existingDevice.SerialNumber,
                            DeviceType = deviceType.Description,
                            Cycles = existingDevice.Cycles,
                            Available = existingDevice.Available,
                            Message = "Warning: Device is at 80% or more of its cycle limit."
                        });
                    }

                    if (existingDevice.Cycles >= deviceType.CyclesLimit)
                    {
                        existingDevice.Available = false;
                        _context.SaveChanges();

                        return Ok(new
                        {
                            SerialNumber = existingDevice.SerialNumber,
                            DeviceType = deviceType.Description,
                            Cycles = existingDevice.Cycles,
                            Available = existingDevice.Available,
                            Message = "Device has been blocked due to reaching the usage limit. You must replace the device (Serial: " +
                                      $"{existingDevice.SerialNumber}, Type: {deviceType.Description}) and try again. Further processing has been stopped."
                        });
                    }

                    if (!existingDevice.Available && existingDevice.Failures >= 3)
                    {
                        results.Add(new {
                            SerialNumber = existingDevice.SerialNumber,
                            Message = "Device is blocked due to excessive failures. Please replace or review the device."
                        });
                        return Ok(results);
                    }
                }

                var history = _context.CR_CyclesHistories
                    .FirstOrDefault(h => h.FKDeviceID == existingDevice.PKDeviceID && h.Week == currentWeek && h.Year == currentYear);

                if (history == null)
                {
                    history = new CR_CyclesHistories
                    {
                        FKDeviceID = existingDevice.PKDeviceID,
                        Week = currentWeek,
                        Year = currentYear,
                        WeeklyCycles = 1,
                        LastUpdated = currentDate,
                        Available = true
                    };
                    _context.CR_CyclesHistories.Add(history);
                }
                else
                {
                    history.WeeklyCycles += 1;
                    history.LastUpdated = currentDate;
                }

                _context.SaveChanges();

                results.Add(new
                {
                    SerialNumber = existingDevice.SerialNumber,
                    DeviceType = deviceType?.Description,
                    Cycles = existingDevice.Cycles,
                    Available = existingDevice.Available,
                    Message = "Device updated successfully."
                });
            }

            return Ok(results);
        }

        [HttpPost]
        [Route("registerFailure")]
        public IActionResult RegisterFailure([FromBody] FailureRequest request)
        {
            var device = _context.CT_Devices.FirstOrDefault(d => d.SerialNumber == request.SerialNumber);
            if (device == null)
            {
                return NotFound(new { message = "Dispositivo no encontrado." });
            }

            device.Failures += 1;
            if (device.Failures >= 3)
            {
                device.Available = false;
                _context.SaveChanges();
                return Ok(new { message = "Dispositivo bloqueado por múltiples fallas." });
            }

            _context.SaveChanges();
            return Ok(new { message = "Falla registrada correctamente." });
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
