using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using FUNews.DAL;
using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Pages.NewsArticles
{
    public class DeleteModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IHubContext<SignalrServer> _hubContext;
        public DeleteModel(INewsArticleService newsArticleService, IHubContext<SignalrServer> hubContext)
        {
            _newsArticleService = newsArticleService;
            _hubContext = hubContext;
        }



        public async Task<IActionResult> OnPostAsync(string id)
        {
            await _newsArticleService.DeleteNewsArticleAsync(id);
            await _hubContext.Clients.All.SendAsync("LoadAllArticles");
            return RedirectToPage("/NewsArticles/Index");
        }
    }
}
