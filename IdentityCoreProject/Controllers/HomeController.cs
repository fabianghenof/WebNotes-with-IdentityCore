using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using IdentityCoreProject.Data;

namespace IdentityCoreProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var notes = _context.WebNotes.OrderBy(note => note.OrderIndex).ToList();
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
        public IActionResult Save(WebNote webNote)
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


        public IActionResult Error()
        {
            return View();
        }
    }
}
