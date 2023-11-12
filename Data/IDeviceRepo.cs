using HuddlyAssignment.Models;

namespace HuddlyAssignment.Data
{
    public interface IDeviceRepo
    {
        bool SaveChanges();
        Device GetDeviceById(string deviceId);
        void CreateDevice(Device device);
        List<Device> GetDevices();

        void DeleteDeviceById(string deviceId);
    }
}
