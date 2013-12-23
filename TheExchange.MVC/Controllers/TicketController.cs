using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using PagedList;
//using TheExchange.WebUI.Filters;
//using Mvc.Mailer;
//using TheExchange.WebUI.Mailers;
//using TheExchange.Domain.Abstract;
using TheExchange.Domain.Concrete;
using TheExchange.Domain.Entities;

namespace TheExchange.MVC.Controllers
{ 
    public class TicketController : Controller
    {
        /*private IAppointmentRepository apptRepository;

        public AppointmentController() : this(new AppointmentRepository()) { }        

        public AppointmentController(IAppointmentRepository ApptRepository)
        {
            this.apptRepository = ApptRepository;
        }*/

        #region OLD CODE
        private TheExchangeDbContext db = new TheExchangeDbContext();

        //private IApptMailer _apptMailer = new ApptMailer();
        //public IApptMailer ApptMailer
        //{
        //    get { return _apptMailer; }
        //    set { _apptMailer = value; }
        //}


        ////
        //// GET: /Appointment/

        //[Authorize(Roles="Admin")]
        //public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        //{            
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
        //    ViewBag.StatusSortParm = sortOrder == "Status" ? "Status desc" : "Status";
        //    ViewBag.VenueSortParm = sortOrder == "Venue" ? "Venue desc" : "Venue";           
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
        //    ViewBag.RepSortParm = sortOrder == "Rep" ? "Rep desc" : "Rep";

        //    if (Request.HttpMethod == "GET")
        //    {
        //        searchString = currentFilter;
        //    }
        //    else
        //    {
        //        page = 1;
        //    }
        //    ViewBag.CurrentFilter = searchString;

        //    var appointments = from a in db.Appointments
        //                 select a;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        appointments = appointments.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper()) 
        //            || s.LastName.ToUpper().Contains(searchString.ToUpper()));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "Date":
        //            appointments = appointments.OrderBy(s => s.AddedDate);
        //            break;
        //        case "Name":
        //            appointments = appointments.OrderBy(s => s.LastName);
        //            break;
        //        case "Name desc":
        //            appointments = appointments.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Venue":
        //            appointments = appointments.OrderBy(s => s.Venue.Name);
        //            break;
        //        case "Venue desc":
        //            appointments = appointments.OrderByDescending(s => s.Venue.Name);
        //            break;
        //        case "Status":
        //            appointments = appointments.OrderBy(s => s.Status.Name);
        //            break;
        //        case "Status desc":
        //            appointments = appointments.OrderByDescending(s => s.Status.Name);
        //            break;
        //        case "Rep":
        //            appointments = appointments.OrderBy(s => s.Rep.UserName);
        //            break;
        //        case "Rep desc":
        //            appointments = appointments.OrderByDescending(s => s.Rep.UserName);
        //            break;

        //        default:
        //            appointments = appointments.OrderByDescending(s => s.AddedDate);
        //            break;
        //    }

        //    int pageSize = 10;
        //    int pageNumber = (page ?? 1);            
        //    return View(appointments.ToPagedList(pageNumber, pageSize));

        //}

        //[OutputCache(Duration = 7200, VaryByParam = "id")]
      public ActionResult VenueList(int id)
        {
            var list = from venue in db.Venues
                       where venue.VenueTypeId == id && venue.Active == true
                       orderby venue.Name
                       select new { VenueId = venue.VenueId, VenueName = venue.Name };

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        /*[OutputCache(Duration = 7200, VaryByParam = "id")]*/
        public ActionResult VenueServicesList(int id)
        {
            Venue venue = db.Venues.Include("Services").Where(v => v.VenueId == id).Single();
            
            var VenueServicesList = venue.Services.Select(s=>new {
                ServiceId = s.ServiceId,
                NName = s.Name
            });

            return Json(VenueServicesList, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Appointment/Details/5
        [Authorize(Roles = "Admin")]
        public ViewResult Details(int id)
        {
            Ticket appointment = db.Tickets.Find(id);
            return View(appointment);
        }

       
        ////[SwitchableAuthorization]
        ////[Authorize]
        public ActionResult Create()
        {
            
            ViewBag.VenueTypes = new SelectList(db.VenueTypes.OrderBy(v => v.TypeName).Where(v=>v.Active).ToList(), "VenueTypeId", "TypeName");
            ViewBag.Venues = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Locations = new SelectList(db.Locations.OrderBy(l=>l.Name), "LocationId", "Name");
            ViewBag.GenderCounts = new SelectList(Genders.Count, "Id", "Name");

            Ticket ticket = new Ticket { TicketDate = DateTime.Now, TicketTime = DateTime.Now.AddMinutes(30), MaleCount = 1, FemaleCount = 2, LocationId = 15, ContactPromoter = false, BottleService = false };
            ticket.Customer = new Customer { FirstName = "test", LastName = "test", Email = "test@yahoo.com", Phone = "8885551212" };
            
            return View(ticket);
        }



        //
        // POST: /Appointment/Create

        [HttpPost]
        //[Authorize]
        public ActionResult Create(Ticket appointment)
        {
            var date = appointment.TicketDate.Date.ToShortDateString();
            var time = ((DateTime)(appointment.TicketTime)).ToString("HH:mm:ss tt");
            //var dateTime = IsStripClub(appointment.VenueId) ? DateTime.Parse(date + " " + time) : DateTime.Parse(date);
            var dateTime = DateTime.Parse(date + " " + time);

            var repCode = Session["RepCode"] != null ? Session["RepCode"].ToString() : null;
            var valid = true;
            string userId = string.Empty;

            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                var user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                userId = user.Id;
            }

            //if (userId == null && !string.IsNullOrEmpty(repCode))
            //{
            //    var user = db.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            //    userId = user.UserId;
            //}

            if (IsStripClub(appointment.VenueTypeId) && dateTime < DateTime.Now.AddMinutes(-5))
            {
                valid = false;
                ModelState.AddModelError(string.Empty, "Appt Date/Time can't be in the past");
            }

            if (IsStripClub(appointment.VenueTypeId) && dateTime > DateTime.Now.AddDays(14))
            {
                valid = false;
                ModelState.AddModelError(string.Empty, "Appt Date/Time can't be too far in the future");
            }

            if (IsStripClub(appointment.VenueTypeId) && ((appointment.LocationId == null || appointment.LocationId == 0) && string.IsNullOrEmpty(appointment.OtherLocation)))
            {
                valid = false;
                ModelState.AddModelError(string.Empty, "Location is required");
            }

            if (!IsStripClub(appointment.VenueTypeId) && appointment.ServiceId == null)
            {
                valid = false;
                ModelState.AddModelError(string.Empty, "Service option is required for this Venue Type");
            }

            if ((appointment.MaleCount == null || appointment.MaleCount == 0) && (appointment.FemaleCount == null || appointment.FemaleCount == 0))
            {
                valid = false;
                ModelState.AddModelError(string.Empty, "Male or Female Count is required");
            }

            if (valid && ModelState.IsValid)
            {
                appointment.CreatedDate = DateTime.Now;
                appointment.UpdatedDate = DateTime.Now;
                appointment.CreatedBy = userId;
                appointment.UpdatedBy = userId;
                appointment.TicketDate = dateTime;
                appointment.StatusId = appointment.ServiceId == (int)VenueServices.GuestList ? (int)AppointmentStatus.Confirmed : (int)AppointmentStatus.PreConfirmed;
                appointment.PromoterId = userId;
                appointment.Customer.PromoterId = userId;                

                db.Tickets.Add(appointment);
                db.SaveChanges();               
                
                return RedirectToAction("Success");
            }

            ViewBag.VenueTypes = new SelectList(db.VenueTypes.OrderBy(v => v.TypeName).Where(v => v.Active).ToList(), "VenueTypeId", "TypeName");
            ViewBag.Venues = new SelectList(db.Venues.OrderBy(v => v.Name).Where(v => v.Active && (v.VenueTypeId == appointment.VenueTypeId)).ToList(), "VenueId", "Name");
            ViewBag.Locations = new SelectList(db.Locations.OrderBy(l => l.Name), "LocationId", "Name");
            ViewBag.GenderCounts = new SelectList(Genders.Count, "Id", "Name");

            return View(appointment);
        }

        private void LazyLoadAppoinmentObjects(Ticket appointment)
        {
            if (appointment.VenueId > 0)
            {
                appointment.Venue = db.Venues.Find(appointment.VenueId);
            }

            if (appointment.LocationId > 0)
            {
                appointment.Location = db.Locations.Find(appointment.LocationId);
            }

            if (appointment.ServiceId > 0)
            {
                appointment.Service = db.Services.Find(appointment.ServiceId);
            }
        }

        public ViewResult Success()
        {
            return View();
        }

        //
        // GET: /Appointment/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Ticket appointment = db.Tickets.Find(id);
            ViewBag.VenueId = new SelectList(db.Venues, "VenueId", "Name", appointment.VenueTypeId);
            ViewBag.VenueTypeId = new SelectList(db.VenueTypes.OrderBy(v => v.TypeName).Where(v => v.Active).ToList(), "VenueTypeId", "TypeName");
            ViewBag.LocationId = new SelectList(db.Locations.OrderBy(l => l.Name), "LocationId", "Name");
            return View(appointment);
        }

        //
        // POST: /Appointment/Edit/5

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Ticket appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "First", appointment.CustomerId);
            ViewBag.VenueId = new SelectList(db.Venues, "VenueId", "Name", appointment.VenueTypeId);
            //ViewBag.AgentId = new SelectList(db.Agents, "AgentId", "First", appointment.AgentId);
            return View(appointment);
        }

        //
        // GET: /Appointment/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Ticket appointment = db.Tickets.Find(id);
            return View(appointment);
        }

        //
        // POST: /Appointment/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket appointment = db.Tickets.Find(id);
            db.Tickets.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool IsStripClub(int venueTypeId)
        {
            if (venueTypeId == (int)VenueTypes.GentlemenClubs18 || venueTypeId == (int)VenueTypes.GentlemenClubs21)
                return true;

            return false;
        }

        #endregion
    }
}