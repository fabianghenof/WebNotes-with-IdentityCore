using IdentityCoreProject.Data;
using IdentityCoreProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;


namespace IdentityCoreProject.Services
{
    public class WebNoteService : IWebNoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WebNoteService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<WebNote> GetUsersNotes(string userId)
        {
            var notes = userId == null ? new List<WebNote>() : _context.WebNotes
                .Where(n => n.UserId == userId)
                .OrderBy(x => x.OrderIndex)
                .ToList();
            return notes;
        }

        public void UpdateTitle(int id, string title)
        {
            var toUpdate = _context.WebNotes.FirstOrDefault(x => x.Id == id);
            toUpdate.Title = title;
            _context.Update(toUpdate);
            _context.SaveChanges();
        }

        public void UpdateContent(int id, string content)
        {
            var toUpdate = _context.WebNotes.FirstOrDefault(x => x.Id == id);
            toUpdate.Content = content;
            _context.Update(toUpdate);
            _context.SaveChanges();
        }

        public void DeleteNote(int id)
        {
            var toDelete = _context.WebNotes.SingleOrDefault(x => x.Id == id);
            _context.WebNotes.Remove(toDelete);
            _context.SaveChanges();
        }

        public void CreateNote(WebNote webNote, ApplicationUser user)
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
            
            webNote.User = user;

            user.WebNotes.Add(webNote);
            var toUpdate = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            _context.Update(toUpdate);

            _context.Add(webNote);
            _context.SaveChanges();
        }
        
        public void MoveNoteUp(int idOfClickedNote, int idOfAboveNote)
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
        }

        public void MoveNoteDown(int idOfClickedNote, int idOfBelowNote)
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
        }



    }
}
