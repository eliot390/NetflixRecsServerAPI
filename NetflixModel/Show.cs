using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NetflixModel;

public partial class Show
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "decimal(2, 1)")]
    public decimal Score { get; set; }

    public int Votes { get; set; }

    public int GenreID { get; set; }

    [ForeignKey("GenreID")]
    [InverseProperty("Shows")]
    public virtual Genre Genre { get; set; } = null!;
}
