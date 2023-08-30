using System.ComponentModel.DataAnnotations;

namespace MovieApp.Models
{
    public class MovieModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Название Фильма")]        
        public string Title { get; set; }


        [Required(ErrorMessage = "Полfе должно быть установлено")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Режиссёр")]
        public string Director { get; set; }


        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Жанр")]
        public string Genre { get; set; }


        [Required]       
        [Display(Name = "Год выпуска")]
        [Range(1890,2023)]
        public int ReleaseYear { get; set; }

        public string? PosterUrl { get; set; }


        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(800, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 800 символов")]
        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}