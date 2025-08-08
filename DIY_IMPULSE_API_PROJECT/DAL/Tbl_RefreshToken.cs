using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIY_IMPULSE_API_PROJECT.DAL
{
    [Table("Tbl_RefreshToken")]
    public class Tbl_RefreshToken
    {
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [Key, Column(Order = 1)]
            public int? Id { get; set; }
            public string? UserId { get; set; }
            public string? Token { get; set; }
            public string? TokenExpiry { get; set; }
            public string? RefreshToken { get; set; }
            public DateTime? RefreshTokenExpiry { get; set; }
            public bool? IsActive { get; set; }
            public string? CreatedOn { get; set; }
            public DateTime? ModifyOn { get; set; }
    }
}
