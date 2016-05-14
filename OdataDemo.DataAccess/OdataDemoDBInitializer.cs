using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ODataDemo.Models;

namespace OdataDemo.DataAccess
{
    public class OdataDemoDBInitializer : DropCreateDatabaseAlways<OdataDemoDbContext>
    {
        protected override void Seed(OdataDemoDbContext context)
        {

            var Dillon = new Person
            {
                Name = "Dillon",
                Surname = "Goeda",
                Devices = new List<Device>
                {
                    new Device {DeviceDescription = "Android Smartphone"},
                    new Device {DeviceDescription = "Samsung Laptop"},
                    new Device {DeviceDescription = "Huawei 4G Router"}
                }
                
            };

            var Jacques = new Person
            {
                Name = "Jacque",
                Surname = "Cunningham",

                Devices = new List<Device>
                {
                    new Device {DeviceDescription = "Samsung Android Smartphone"},
                    new Device {DeviceDescription = "Lenovo Laptop"},
                    new Device {DeviceDescription = "3G USB Dongle"}
                }
            };

            var Curtis = new Person
            {
                Devices = new List<Device>
                {
                    new Device {DeviceDescription = "Sony Xperia Smartphone"},
                    new Device {DeviceDescription = "HP Laptop"},
                    new Device {DeviceDescription = "3G Mifi Device"}
                }
                ,
                Name = "Curtis",
                Surname = "Nelson"
            };

            var people = new List<Person>()
            {
                Dillon,
                Curtis,
                Jacques
                   
            };

            Dillon.Friends = new List<Person> {Jacques, Curtis};
            Jacques.Friends = new List<Person> {Curtis, Dillon};
            Curtis.Friends = new List<Person> {Dillon, Jacques};
            context.People.AddRange(people);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
