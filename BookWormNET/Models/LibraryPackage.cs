using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWormNET.Models;


[Table("library_package")]
public class LibraryPackage
{
    [Key]
    [Column("package_id")]
    public int PackageId { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [Column("cost", TypeName = "decimal(10,2)")]
    public decimal Cost { get; set; }

    [Column("validity_days")]
    public int? ValidityDays { get; set; }

    [Column("book_limit")]
    public int? BookLimit { get; set; }

    [Required]
    [Column("description")]
    public string Description { get; set; } = null!;

    public LibraryPackage()
    {
        MyLibraries = new HashSet<MyLibrary>();
    }

    public virtual ICollection<MyLibrary> MyLibraries { get; set; }
}
