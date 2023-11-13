using HuddlyAssignment.Models;

namespace HuddlyAssignment.Data
{
    public interface IDeviceRepo
    {
        Device FetchDeviceById(string deviceId);
        void CreateDevice(Device device);
        List<Device> FetchDevices();

        void DeleteDeviceById(string deviceId);
        bool DeviceExists(string deviceId);
    }
}
