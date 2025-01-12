using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codixa.Core.Models.sharedModels
{
    public class FileEntity
    {
        public FileEntity()
        {
            FileId = Guid.NewGuid().ToString("N"); // Generate a GUID without hyphens
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FileId { get; set; }
        public string FileName { get; set; }    
        public string FilePath { get; set; }

    }
}
