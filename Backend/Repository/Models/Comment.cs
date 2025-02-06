﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IDS_App.Repository.Models;

public partial class Comment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("PostID")]
    public int PostId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Comments")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Comments")]
    public virtual User? User { get; set; }
}
