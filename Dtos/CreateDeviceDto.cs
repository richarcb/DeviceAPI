using System.ComponentModel.DataAnnotations;

namespace HuddlyAssignment.Dtos
{
    public class CreateDeviceDto
    {
        [Required]
        public string DeviceId { get; set; }
        [Required]
        public string DeviceModel { get; set; }
        [Required]
        public string Room { get; set; }
        [Required]
        public string Organization { get; set; }
    }
}
