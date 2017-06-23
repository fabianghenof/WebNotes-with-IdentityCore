﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCoreProject.Models
{
    public class FileAttachment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileData { get; set; }
        public string Type { get; set; }

        public string UserId { get; set; }
        public int WebNoteId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("WebNoteId")]
        public WebNote WebNote { get; set; }
    }
}