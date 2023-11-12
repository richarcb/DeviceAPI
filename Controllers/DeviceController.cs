using AutoMapper;
using HuddlyAssignment.Data;
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
            _mapper = mapper;
        }

        [HttpGet("{deviceId}")] 
        public ActionResult<Device> GetDeviceById (string deviceId) 
        {
            Console.WriteLine("--> Getting Device by Id");
            var device = _repository.GetDeviceById(deviceId);
            return Ok(device);
        }

        [HttpGet]
        public ActionResult<List<Device>> GetAllDevices()
        {
            Console.WriteLine("--> Getting all Devices");
            var devices = _repository.GetDevices();
            return Ok(devices);
        }
        [HttpGet("id/{deviceId}")]
        public ActionResult<Device> Test(string deviceId)
        {
            return Ok(deviceId);
        }
        [HttpPost]
        public ActionResult<Device> CreateDevice(Device device)
        {
            //validering - if deviceId exists
            _repository.CreateDevice(device);
            return Ok(device);

        }

    }
}
