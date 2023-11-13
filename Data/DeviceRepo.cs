using HuddlyAssignment.Models;
using CsvHelper;
using System.Globalization;
using System.Reflection.PortableExecutable;
using System.Reflection;
using HuddlyAssignment.Dtos;

namespace HuddlyAssignment.Data
{
    public class DeviceRepo : IDeviceRepo
    {
        private readonly string _csvFilePath;
        private Mutex _fileMutex = new Mutex();
        public DeviceRepo(string filePath)
        {
            _csvFilePath = filePath; //_configuration["CSVFilePath"];
        
        }

        public void CreateDevices(List<Device> devices)
        {
            try
            {
                _fileMutex.WaitOne();
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
            finally 
            {
                _fileMutex.ReleaseMutex(); 
            }  
        }
        public void CreateDevice(Device device)
        {
            var devices = FetchDevices();
            if(device != null)
            {

                devices.Add(device);
                CreateDevices(devices);
            }
        }

        public void DeleteDeviceById(string deviceId)
        {
            var devices = FetchDevices();
            var deviceToDelete = devices.FirstOrDefault(d => d.DeviceId == deviceId);

            if(deviceToDelete != null)
            {
                devices.Remove(deviceToDelete);
                CreateDevices(devices);
            }
        }

        public Device FetchDeviceById(string deviceId)
        {
            var devices = FetchDevices();
            return devices.FirstOrDefault(d => d.DeviceId == deviceId);
        }

        public List<Device> FetchDevices()
        {
            List<Device> devices = new List<Device>();
            try
            {
                _fileMutex.WaitOne();
                using (var reader = new StreamReader(_csvFilePath))
                {
                    string fileContents = reader.ReadToEnd();
                    _fileMutex.ReleaseMutex();
                    devices = ParseStringContent(fileContents);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV: {ex.Message}");

            }
            return devices;
        }

        public List<Device> ParseStringContent(string stringContent)
        {
            var devices = new List<Device>();
            PropertyInfo[] deviceProperties = typeof(Device).GetProperties();

            try
            {
                
                using (StringReader stringReader = new StringReader(stringContent))
                {
                    string line;
                    while ((line = stringReader.ReadLine()) != null)
                    {
                        var values = line.Split(',');

                        if(values.Length != deviceProperties.Length)
                        {
                            Console.WriteLine("CSV entry did not have all fields populated");
                            continue;
                        }

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
                Console.WriteLine(ex.Message);
            }
            

            return devices;
        }

        public bool DeviceExists(string deviceId)
        {
            var devices = FetchDevices();
            foreach (var device in devices)
            {
                if(deviceId == device.DeviceId)
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}
