﻿using Chat.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Domain.DAOs
{
    [Table("Avatar")]
    public class Avatar : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Path is required")]
        public string? Path { get; set; }
        public DateTime? Created { get; set; }
        public virtual UserApp? UserApp { get; set; }
    }
}
