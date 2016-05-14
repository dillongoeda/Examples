using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;

namespace ODataDemo.Models
{
    public class Person
    {

        [Key]
        public int PersonID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }

        [Contained]
        public ICollection<Device> Devices { get; set; }

        public ICollection<Person> Friends { get; set; } 
    }

    
    public class Device
    {
        [Key]
        public int DeviceID { get; set; }
        public string DeviceDescription { get; set; }

        public virtual Person Person { get; set; }
    }
}
