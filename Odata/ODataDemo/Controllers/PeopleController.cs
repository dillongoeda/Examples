using OdataDemo.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataDemo.Helpers;
using ODataDemo.Models;

namespace ODataDemo.Controllers
{
    public class PeopleController : ODataController
    {
        private OdataDemoDbContext _context = new OdataDemoDbContext();

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_context.People);
        }

        [EnableQuery]
        public IHttpActionResult Get([FromODataUri] int key)
        {

            var person = _context.People.Where(p => p.PersonID == key);

            if (!person.Any())
                return NotFound();

            return Ok(SingleResult.Create(person));
        }

        public IHttpActionResult Delete([FromODataUri] int key)
        {
            var person = _context.People.Include("Friends").FirstOrDefault(p => p.PersonID == key);

            if (person == null)
                return NotFound();

            var peopleWithCurrentPersonAsFriend =
                _context.People.Include("Friends")
                .Where(p => p.Friends.Select(f => f.PersonID).AsQueryable().Contains(key));

            foreach (var p in peopleWithCurrentPersonAsFriend.ToList())
            {
                person.Friends.Remove(p);
            }

            _context.People.Remove(person);
            _context.SaveChanges();
            return Ok();
        }

        public IHttpActionResult Post([FromBody] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.People.Add(person);
                _context.SaveChanges();
                return Created(person);
            }

            return BadRequest("The model state of the person is invalid");
        }

        [ODataRoute("People({key})/Name")]
        [ODataRoute("People({key})/Surname")]
        public IHttpActionResult GetPersonProperty([FromODataUri] int key)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonID == key);

            if (person == null)
                return NotFound();

            var property = Url.Request.RequestUri.Segments.Last();

            if (!person.HasProperty(property))
                return NotFound();

            var value = person.GetValue(property);

            if (string.IsNullOrWhiteSpace(value.ToString())) 
            return StatusCode(System.Net.HttpStatusCode.NoContent);

            return ODataHelpers.CreateOKHttpActionResult(this, value);
        }

        [ODataRoute("People({key})/Name/$value")]
        [ODataRoute("People({key})/Surname/$value")]
        public IHttpActionResult GetPersonRawProperty([FromODataUri] int key)
        {
            var person = _context.People.FirstOrDefault(p => p.PersonID == key);

            if (person == null)
                return NotFound();

            var property = Url.Request.RequestUri
                 .Segments[Url.Request.RequestUri.Segments.Length - 2].TrimEnd('/');

            if (!person.HasProperty(property))
                return NotFound();

            var value = person.GetValue(property);

            if (string.IsNullOrWhiteSpace(value.ToString()))
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            return ODataHelpers.CreateOKHttpActionResult(this, value.ToString());

        }

        [ODataRoute("People({key})/Devices")]
        [EnableQuery]
        public IHttpActionResult GetPersondDevices([FromODataUri] int key)
        {
            var person = _context.People.Include("Devices").FirstOrDefault(p => p.PersonID == key);

            if (person == null)
                return NotFound();

            var property = Url.Request.RequestUri.Segments.Last();

            if (!person.HasProperty(property))
                return NotFound();

            var value = person.GetValue(property);

            if (string.IsNullOrWhiteSpace(value.ToString()))
                return StatusCode(System.Net.HttpStatusCode.NoContent);

            return ODataHelpers.CreateOKHttpActionResult(this, value);
        }

        [ODataRoute("People({key})/Friends({friendKey})")]
        [EnableQuery]
        public IHttpActionResult GetFriends([FromODataUri] int key, [FromODataUri] int friendKey)
        {
            var person = _context.People.Include("Friends").FirstOrDefault(p => p.PersonID == key);

            if (person == null)
               return NotFound();

            var friend = person.Friends.Where(f => f.PersonID == friendKey);

            return Ok(person.Friends);
        }


        [ODataRoute("People({key})/Friends")]
        [EnableQuery]
        public IHttpActionResult GetFriends([FromODataUri] int key)
        {
            var person = _context.People.Include("Friends").FirstOrDefault(p => p.PersonID == key);

            if (person == null)
                return NotFound();

            return Ok(person.Friends);
        }
        

        [HttpPost]
        [ODataRoute("People({key})/Friends/$ref")]
        public IHttpActionResult CreateFriend([FromODataUri] int key, [FromBody] Uri link)
        {
            /*
            body : {"@odata.id" : "http://localhost:1234/People(1)"}
            */
            var person = _context.People.Include("Friends").FirstOrDefault(p => p.PersonID == key);

            if (person == null)
                return NotFound();

            var keyToLink = Request.GetKeyValue<int>(link);
            var friend = _context.People.FirstOrDefault(p => p.PersonID == keyToLink);

            if (friend == null)
                return NotFound();

            if (person.Friends.Any(p => p.PersonID == keyToLink))
                return BadRequest("Already friends");

            person.Friends.Add(friend);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [ODataRoute("People({key})/Friends({relatedKey})/$ref")]
        public IHttpActionResult DeleteFriend([FromODataUri] int key, [FromODataUri] int relatedKey)
        {
            var person = _context.People.Include("Friends").FirstOrDefault(p => p.PersonID == key);

            if (person == null)
                return NotFound();

            var friend = _context.People.FirstOrDefault(p => p.PersonID == relatedKey);

            if (friend == null)
                return NotFound();

            person.Friends.Remove(friend);

            return Ok();
        }


       protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}