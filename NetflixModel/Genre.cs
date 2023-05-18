using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NetflixModel;

[Table("Genre")]
public partial class Genre
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    //[Column("Genre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Genre1 { get; set; } = null!;

    [InverseProperty("Genre")]
    public virtual ICollection<Show> Shows { get; } = new List<Show>();
}
