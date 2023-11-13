using AutoMapper;
using HuddlyAssignment.Data;
using HuddlyAssignment.Dtos;
using HuddlyAssignment.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HuddlyAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly IDeviceRepo _repository;
        private readonly IMapper _mapper;

        public DeviceController(IDeviceRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;//TODO: lage CreateDTO og mappe over til DeviceModel
        }

        [HttpGet("{deviceId}", Name = "GetDeviceById")] 
        public ActionResult<Device> GetDeviceById (string deviceId) 
        {
            Console.WriteLine($"--> Getting Device with Id {deviceId}");
            var device = _repository.FetchDeviceById(deviceId);
            if(device == null)
            {
                Console.WriteLine($"Could not find device with Id {deviceId}");
            }
            return Ok(device);
        }

        [HttpGet]
        public ActionResult<List<Device>> GetAllDevices()
        {
            Console.WriteLine("--> Getting all Devices");
            var devices = _repository.FetchDevices();
            return Ok(devices);
        }

        [HttpPost]
        public ActionResult<Device> CreateDevice(CreateDeviceDto device)
        {
            Console.WriteLine("--> Creating Device");
            if (_repository.DeviceExists(device.DeviceId))
            {
                Console.WriteLine($"Error, device already exists with Id {device.DeviceId}");
                return StatusCode(409);
            }
            
            var deviceWithDate = _mapper.Map<Device>(device);

            deviceWithDate.DateAdded = DateTime.Now.ToString("yyyyMMdd");
            
            _repository.CreateDevice(deviceWithDate);

            return CreatedAtRoute(nameof(GetDeviceById), new { deviceId = deviceWithDate.DeviceId }, deviceWithDate);

        }

        [HttpDelete("{deviceId}")]
        public ActionResult<string> DeleteDevice(string deviceId)
        {
            Console.WriteLine($"--> Deleting device with Id {deviceId}");
            if(!_repository.DeviceExists(deviceId))
            {
                Console.WriteLine("Error, device not found");
                return NotFound();
            }
            _repository.DeleteDeviceById(deviceId);
            return Ok(deviceId);
        }

    }
}
