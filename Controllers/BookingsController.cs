using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLink.Data;
using ServiceLink.Models;
using ServiceLink.ViewModels;

namespace ServiceLink.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<BookingsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(ApplicationDbContext db, ILogger<BookingsController> logger, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: /Bookings/Create?serviceId=5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(int serviceId)
        {
            var service = await _db.Services.FindAsync(serviceId);
            if (service == null) return NotFound();

            var vm = new BookingCreateViewModel
            {
                ServiceId = service.ServiceId,
                ServiceTitle = service.Title,
                RequestedFor = DateTime.Today.AddDays(1)
            };

            return View(vm);
        }


        // POST: /Bookings/Create
        [HttpPost]
        [Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var service = await _db.Services.FindAsync(vm.ServiceId);
            if (service == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var booking = new Booking
            {
                ServiceId = vm.ServiceId,
                CustomerId = userId,
                ProviderId = service.ProviderId ?? User.FindFirstValue(ClaimTypes.NameIdentifier),
                RequestedFor = vm.RequestedFor,
                Notes = vm.Notes,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };


            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(MyBookings));
        }


        // GET: /Bookings/MyBookings
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = await _db.Bookings
                .Where(b => b.CustomerId == userId)
                .Include(b => b.Service)
                .Select(b => new BookingListItemViewModel
                {
                    BookingId = b.BookingId,
                    ServiceTitle = b.Service.Title,
                    RequestedFor = b.RequestedFor,
                    Status = b.Status
                })
                .ToListAsync();

            return View(bookings);
        }


        // POST: /Bookings/Cancel/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();

            if (booking.CustomerId != userId) return Forbid();

            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Only pending bookings can be cancelled.";
                return RedirectToAction(nameof(MyBookings));
            }

            booking.Status = BookingStatus.Cancelled;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking cancelled.";
            return RedirectToAction(nameof(MyBookings));
        }

        [Authorize(Roles = "Provider,MasterDemo")]
        public async Task<IActionResult> Incoming()
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = await _db.Bookings
                .Include(b => b.Service)
                .Where(b => b.ProviderId == providerId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingListItemViewModel
                {
                    BookingId = b.BookingId,
                    ServiceTitle = b.Service.Title,
                    RequestedFor = b.RequestedFor,
                    CreatedAt = b.CreatedAt,
                    Status = b.Status,
                    Notes = b.Notes
                })
                .ToListAsync();

            return View(bookings);
        }



        // POST: /Bookings/Accept/5
        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (providerId == null) return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();

            if (booking.ProviderId != providerId) return Forbid();
            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Booking status has changed. Action not allowed.";
                return RedirectToAction(nameof(Incoming));
            }

            booking.Status = BookingStatus.Accepted;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking accepted.";
            return RedirectToAction(nameof(Incoming));
        }

        // POST: /Bookings/Reject/5
        [HttpPost]
        [Authorize(Roles = "Provider,Admin,MasterDemo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (providerId == null) return Forbid();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
            if (booking == null) return NotFound();

            if (booking.ProviderId != providerId) return Forbid();
            if (booking.Status != BookingStatus.Pending)
            {
                TempData["ErrorMessage"] = "Booking status has changed. Action not allowed.";
                return RedirectToAction(nameof(Incoming));
            }

            booking.Status = BookingStatus.Rejected;
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking rejected.";
            return RedirectToAction(nameof(Incoming));
        }
    }
}
