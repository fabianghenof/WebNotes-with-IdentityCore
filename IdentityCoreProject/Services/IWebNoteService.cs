using System.Collections.Generic;
using IdentityCoreProject.Models;
using System.IO;

namespace IdentityCoreProject.Services
{
    public interface IWebNoteService
    {
        void CreateNote(WebNote webNote, ApplicationUser user);
        void DeleteNote(int id);
        List<WebNote> GetUsersNotes(string userId, string sortingOption);
        void MoveNoteDown(int idOfClickedNote, int idOfBelowNote);
        void MoveNoteUp(int idOfClickedNote, int idOfAboveNote);
        void UpdateContent(int id, string content);
        void UpdateTitle(int id, string title);
        MemoryStream DownloadNotes(string userId, List<WebNote> notes);
        void SendEmail(WebNote note, string loggedInEmail, string email);
        string GetUsersSortingOption(string userId);
    }
}