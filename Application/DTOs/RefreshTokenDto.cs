using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    /// <summary>
    /// DTO for the refresh token
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// Refresh token
        /// </summary>
        [Required(ErrorMessage = "Refresh token is required")]
        public string TokenRefresh { get; set; }
    }
}
