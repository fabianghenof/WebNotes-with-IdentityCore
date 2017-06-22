using System;
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
        public byte[] FileData { get; set; }


        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
