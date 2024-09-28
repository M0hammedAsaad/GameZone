using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Device
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Icon { get; set; }

        public ICollection<GameDevice> GameDevices { get; set; }
    }
}
