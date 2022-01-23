using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectChannel.WebApi.Models
{
    public class ToDoItemModel
    {
            [Key]
            public int ItemId { get; set; }

            [Required(ErrorMessage = "ItemName is required")]
            public string ItemName { get; set; }

            [Required(ErrorMessage = "ItemDescription is required")]
            public string ItemDescription { get; set; }

            [Required(ErrorMessage = "ItemStatus is required")]
            public bool ItemStatusCompleted { get; set; }
    }
}