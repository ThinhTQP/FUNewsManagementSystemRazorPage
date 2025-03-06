using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using BusinessObjects.Entities;

namespace FUNewsManagementSystem.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public IndexModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public List<NewsArticle> NewsArticles { get; set; }

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            StartDate ??= startDate;
            EndDate ??= endDate;
            await FilterArticlesAsync();
            return Page();
        }


        public async Task<IActionResult> OnGetFilterAsync(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            await FilterArticlesAsync();
            return Page();
        }


        public async Task<IActionResult> OnGetExportToPDFAsync()
        {
            var articles = await _newsArticleService.GetAllNewsArticlesAsync();

            if (StartDate.HasValue)
                articles = articles.Where(a => a.CreatedDate >= StartDate.Value).ToList();
            if (EndDate.HasValue)
                articles = articles.Where(a => a.CreatedDate <= EndDate.Value).ToList();

            articles = articles.OrderByDescending(a => a.CreatedDate).ToList();

            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph("FPT University News Report"));
                pdfDoc.Add(new Paragraph("\n"));

                foreach (var article in articles)
                {
                    pdfDoc.Add(new Paragraph($"Title: {article.NewsTitle}"));
                    pdfDoc.Add(new Paragraph($"Headline: {article.Headline}"));
                    pdfDoc.Add(new Paragraph($"Content: {article.NewsContent.Substring(0, Math.Min(150, article.NewsContent.Length))}..."));
                    pdfDoc.Add(new Paragraph($"Date: {article.CreatedDate?.ToString("dd/MM/yyyy")}"));
                    pdfDoc.Add(new Paragraph("------------------------------------------------------"));
                }

                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "NewsReport.pdf");
            }
        }

        private async Task FilterArticlesAsync()
        {
            var articles = await _newsArticleService.GetAllNewsArticlesAsync();

            if (StartDate.HasValue)
                articles = articles.Where(a => a.CreatedDate >= StartDate.Value).ToList();
            if (EndDate.HasValue)
                articles = articles.Where(a => a.CreatedDate <= EndDate.Value).ToList();

            NewsArticles = articles.OrderByDescending(a => a.CreatedDate).ToList();
        }
    }
}