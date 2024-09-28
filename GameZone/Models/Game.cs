using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Game
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(2500)]
        public string Description { get; set; }

        //[MaxLength(500)]
        public string Cover { get; set; }

        public int CaregoryId { get; set; }
        public Category Category { get; set; }

        // Navigation property
        public ICollection<GameDevice> GameDevices { get; set; }

    }
}
