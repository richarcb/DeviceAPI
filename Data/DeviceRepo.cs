using HuddlyAssignment.Models;
using CsvHelper;
using System.Globalization;

namespace HuddlyAssignment.Data
{
    public class DeviceRepo : IDeviceRepo
    {
        private readonly IConfiguration _configuration;
        private readonly string _csvFilePath;

        public DeviceRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _csvFilePath = _configuration["CSVFilePath"];
        }
 

        public void CreateDevices(List<Device> devices)
        {
            try
            {
                using (var writer = new StreamWriter(_csvFilePath))
                {
                    foreach (var device in devices) 
                    {
                        var line = $"{device.DeviceId},{device.DeviceModel},{device.Room},{device.Organization},{device.DateAdded}";
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void CreateDeviceById(Device device)
        {
            var devices = GetDevices();
            if(device != null)
            {
                devices.Add(device);
                CreateDevices(devices);
            }
        }

        public void DeleteDeviceById(string deviceId)
        {
            var devices = GetDevices();
            var deviceToDelete = devices.FirstOrDefault(d => d.DeviceId == deviceId);

            if(deviceToDelete != null)
            {
                devices.Remove(deviceToDelete);
                CreateDevices(devices);
            }
        }

        public Device GetDeviceById(string deviceId)
        {
            var devices = GetDevices();
            return devices.FirstOrDefault(d => d.DeviceId == deviceId);
        }

        public List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();
            try
            {
                using (var reader = new StreamReader(_csvFilePath))
                {
                    while(!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        var values = row.Split(',');

                        var device = new Device
                        {
                            DeviceId = values[0],
                            DeviceModel = values[1],
                            Room = values[2],
                            Organization = values[3],
                            DateAdded = values[4]
                        };

                        devices.Add(device);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV: {ex.Message}");

            }
            return devices;
        }

        public bool DeviceExists(string deviceId)
        {
            var devices = GetDevices();
            foreach (var device in devices)
            {
                if(deviceId == device.DeviceId)
                {
                    return true;
                }
            }
            return false;
        }
        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        
    }
}
