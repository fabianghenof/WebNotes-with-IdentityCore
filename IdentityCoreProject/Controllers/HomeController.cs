using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityCoreProject.Models;
using Microsoft.AspNetCore.Hosting;
using IdentityCoreProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsvHelper;

namespace IdentityCoreProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var notes = userId == null ? new List<WebNote>() : _context.WebNotes
                .Where(n => n.UserId == userId)
                .OrderBy(x => x.OrderIndex)
                .ToList();
                return View(notes);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost("updateNoteTitle")]
        public IActionResult UpdateNoteTitle(int id, string title)
        {
            var toUpdate = _context.WebNotes.FirstOrDefault(x => x.Id == id);
            toUpdate.Title = title;
            _context.Update(toUpdate);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("updateNoteContent")]
        public IActionResult UpdateNoteContent(int id, string content)
        {
            var toUpdate = _context.WebNotes.FirstOrDefault(x => x.Id == id);
            toUpdate.Content = content;
            _context.Update(toUpdate);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPost("deleteNote")]
        public IActionResult DeleteNote(int id)
        {
            var toDelete = _context.WebNotes.SingleOrDefault(x => x.Id == id);
            _context.WebNotes.Remove(toDelete);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPost("saveNote")]
        public async Task<IActionResult> Save(WebNote webNote)
        {
            int nbrOfNotes = _context.WebNotes.Count();
            //query list here .ToList apoi foloseste asta in forloop
            for (int i = 0; i < nbrOfNotes; i++)
            {
                var noteToModify = _context.WebNotes.FirstOrDefault(x => x.OrderIndex == i);
                if (noteToModify != null)
                {
                    noteToModify.OrderIndex++;
                    _context.WebNotes.Update(noteToModify);
                }
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            webNote.User = user;

            user.WebNotes.Add(webNote);
            var toUpdate = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            _context.Update(toUpdate);

            _context.Add(webNote);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPost("moveNoteUp")]
        public IActionResult MoveNoteUp(int idOfClickedNote, int idOfAboveNote)
        {
            var noteClickedOn = _context.WebNotes.FirstOrDefault(x => x.Id == idOfClickedNote);
            var noteAbove = _context.WebNotes.FirstOrDefault(x => x.Id == idOfAboveNote);
            int orderIndexToMoveTo = noteAbove.OrderIndex;

            int temp = noteClickedOn.OrderIndex;

            noteClickedOn.OrderIndex = orderIndexToMoveTo;
            _context.Update(noteClickedOn);
            noteAbove.OrderIndex = temp;
            _context.Update(noteAbove);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("moveNoteDown")]
        public IActionResult MoveNoteDown(int idOfClickedNote, int idOfBelowNote)
        {
            var noteClickedOn = _context.WebNotes.FirstOrDefault(x => x.Id == idOfClickedNote);
            var noteBelow = _context.WebNotes.FirstOrDefault(x => x.Id == idOfBelowNote);
            int orderIndexToMoveTo = noteBelow.OrderIndex;

            int temp = noteClickedOn.OrderIndex;

            noteClickedOn.OrderIndex = orderIndexToMoveTo;
            _context.Update(noteClickedOn);

            noteBelow.OrderIndex = temp;
            _context.Update(noteBelow);

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("sendEmail")]
        public IActionResult SendEmail(string email)
        {
            //MAIL TEST
            //var engine = EngineFactory.CreatePhysical(System.IO.Path.Combine(_hostingEnvironment.ContentRootPath, "Templates", "Email"));
            //var model = new
            //{
            //    Name = "John Doe",
            //    Title = "RazorLight"
            //};
            //string result = engine.Parse("basic.cshtml", model);

            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Eu", "NEIMPLEMENTAT"));
            //message.To.Add(new MailboxAddress("Tu", email));
            //message.Subject = email;
            //message.Body = new TextPart("html")
            //{
            //    Text = result
            //};

            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.mailgun.org", 587, false);
            //    // Note: since we don't have an OAuth2 token, disable // the XOAUTH2 authentication mechanism
            //    client.AuthenticationMechanisms.Remove("XOAUTH2");
            //    // Note: only needed if the SMTP server requires authentication
            //    client.Authenticate("NEIMPLEMENTAT", "NEIMPLEMENTAT");

            //    client.Send(message);
            //    client.Disconnect(true);
            //}
            //
            return Ok();
        }

        [HttpGet]
        public FileResult DownloadNotes()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var myWebNotes = userId == null ? new List<WebNote>() : _context.WebNotes
                .Where(n => n.UserId == userId)
                .OrderBy(x => x.OrderIndex)
                .ToList();

            var trimmedWebNotes = myWebNotes.Select(note => new CsvNotes
            {
                Id = note.Id.ToString(),
                Title = note.Title,
                Content = note.Content
            }).ToList();
   
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            var csv = new CsvWriter(writer);
            csv.WriteRecords(trimmedWebNotes);
            writer.Flush();
            stream.Position = 0;

            return File(stream, "text/csv", "votes.csv");
        }

            public IActionResult Error()
        {
            return View();
        }
    }
}
