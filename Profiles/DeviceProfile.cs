using AutoMapper;
using HuddlyAssignment.Dtos;
using HuddlyAssignment.Models;

namespace HuddlyAssignment.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<CreateDeviceDto, Device>();
        }
    }
}
