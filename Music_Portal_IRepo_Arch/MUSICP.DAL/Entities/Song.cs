using System.ComponentModel.DataAnnotations;
namespace MusicPortal.DAL.Entities
{
    public class Song
    {
        public int? Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Singer { get; set; }
        
        public string? AudioFile { get; set; }
        
        public string? ImageFile { get; set; }


        public int? GenreId { get; set; } 
        public virtual Genre? Genre { get; set; } 
    }
}