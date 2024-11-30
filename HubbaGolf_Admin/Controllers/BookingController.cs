using HubbaGolf_Admin.Models;
using HubbaGolfAdmin.Database.Models;
using HubbaGolfAdmin.Database;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Authorization;

namespace HubbaGolf_Admin.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly EComDbContext _EComDbContext;
        private readonly SessionStore _SessionStore;

        public BookingController(ILogger<BookingController> logger, EComDbContext eComDbContext, SessionStore sessionStore)
        {
            _logger = logger;
            _EComDbContext = eComDbContext;
            _SessionStore = sessionStore;
        }

        public IActionResult Index()
        {
            List<Events> events = _EComDbContext.Events
               .Where(e => e.StatusRecord != 99)
               .OrderByDescending(e => e.OrderNumber)
               .ToList();
            return View(events);
        }
        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] Events e)
        {
            try
            {
                var lastOrder = _EComDbContext.Events
                    .Where(item => item.OrderNumber != null && (item.OrderNumber.Substring(0, 4) == DateTime.Now.Year.ToString()))
                    .OrderByDescending(item => item.OrderNumber)
                    .FirstOrDefault();

                if (lastOrder == null)
                {
                    e.OrderNumber = DateTime.Now.Year.ToString() + "00001";
                }
                else
                {
                    int numberPart = int.Parse(lastOrder.OrderNumber.Substring(4));
                    numberPart++;
                    e.OrderNumber = $"{DateTime.Now.Year.ToString()}{numberPart:D5}";
                }

                e.Title = e.Name;
                if (e.StartDate == null)
                {
                    e.StartDate = DateTime.Now;
                }
                if (e.EndDate == null)
                {
                    e.EndDate = DateTime.Now;
                }
                _EComDbContext.Add(e);
                await _EComDbContext.SaveChangesAsync();
                return Ok(new { success = true, data = e });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UpdateEvent(int id, DateTime? start, DateTime? end)
        {
            var eventToUpdate = _EComDbContext.Events.Where(e => e.Id == id).FirstOrDefault();

            if (eventToUpdate != null)
            {
                eventToUpdate.StartDate = start;
                eventToUpdate.EndDate = end;
                _EComDbContext.SaveChanges();

                return Ok(new { success = true });
            }

            return NotFound(new { success = false });
        }

        [HttpPost]
        public IActionResult UpdateFullInfoEvent([FromBody] Events e)
        {
            var eventToUpdate = _EComDbContext.Events.Where(ev => ev.Id == e.Id).FirstOrDefault();

            if (eventToUpdate != null)
            {
                eventToUpdate.Title = e.Name;
                eventToUpdate.Name = e.Name;
                eventToUpdate.Phone = e.Phone;
                eventToUpdate.Email = e.Email;
                eventToUpdate.StartDate = e.StartDate;
                eventToUpdate.EndDate = e.EndDate;
                eventToUpdate.PlayerNumber = e.PlayerNumber;
                eventToUpdate.Country = e.Country;
                eventToUpdate.Course = e.Course;
                _EComDbContext.SaveChanges();
                return Ok(new { success = true });
            }

            return NotFound(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventToUpdate = _EComDbContext.Events.Where(e => e.Id == id).FirstOrDefault();

            if (eventToUpdate != null)
            {
                eventToUpdate.StatusRecord = 99;
                _EComDbContext.Update(eventToUpdate);
                _EComDbContext.SaveChanges();

                return Ok(new { success = true });
            }

            return NotFound(new { success = false });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var eventToUpdate = _EComDbContext.Events.Where(e => e.Id == id).FirstOrDefault();

            if (eventToUpdate != null)
            {
                eventToUpdate.StatusRecord = 99;

                _EComDbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return NotFound(new { success = false });
        }
        public async Task<IActionResult> New()
        {
            return View("~/Views/Booking/New.cshtml", new Events());
        }
        public async Task<IActionResult> GetInfo(int id)
        {
            var zEvent = _EComDbContext.Events.Where(e => e.Id == id).FirstOrDefault();

            return View("~/Views/Booking/Detail.cshtml", zEvent);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public async Task<IActionResult> GetEvent(string orderNumber, string email)
        {
            try
            {
                if (orderNumber.Length > 0 && orderNumber.Substring(0, 1) == "#")
                {
                    orderNumber = orderNumber.Substring(1);
                }

                var order = _EComDbContext.Events
                    .Where(item => item.OrderNumber != null && (item.OrderNumber == orderNumber))
                    .FirstOrDefault();

                if (order == null)
                {
                    return NotFound("Not found " + orderNumber);
                }
                else
                {
                    if (order.Email == email)
                    {
                        return Ok(new { success = true, data = order });
                    }
                    else
                    {
                        return BadRequest("Info is incorrect");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
