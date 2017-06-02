using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using fletnix.Models;
using fletnix.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace fletnix.Controllers
{
    public class ReportController : WalledGarden
    {
        private IFletnixRepository _repository;

        public ReportController(IFletnixRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> AverageRating(string type)
        {
            List<PriceRatingIndexViewModel> report = null;
            switch(type)
            {
             case "highestAverage":
                 report = _repository.HighestAverageRatingReport();
                 break;
             case "lowestAverage":
                 report = _repository.LowestAverageRatingReport();
                 break;
             case "highestAveragePriceIndex":
                 report = _repository.HighestAveragePriceIndexRatingReport();
                 break;
             case "lowestAveragePriceIndex":
                 report = _repository.LowestAveragePriceIndexRatingReport();
                 break;
             default:
                 report = _repository.HighestAverageRatingReport();
                 break;
            }

            return View(report);
        } 

        public async Task<IActionResult> AwardReport(int? fromyear, int? tillyear, int? page)
        {
            ViewData["header"] = "Viewing all movies with awards";
            if(fromyear != null && tillyear != null) ViewData["header"] = "Viewing all movies with awards from " + fromyear + " till " + tillyear;
            if (page != null)
            {
                ViewData["header"] += " (Page " + page + ")";
            }
            var pagedResult = await _repository.GetAwardReport(fromyear, tillyear, 25, page);
            var map = new Dictionary<int?, Dictionary<string, AwardReportViewModel>>();
            foreach (var ma in pagedResult)
            {
                if(!map.ContainsKey(ma.Movie.PublicationYear)) map.Add(ma.Movie.PublicationYear, new Dictionary<string, AwardReportViewModel>());
                if (!map[ma.Movie.PublicationYear].ContainsKey(ma.Movie.Title))
                {
                    ma.MovieAwardList.Add(ma.MovieAward);
                    map[ma.Movie.PublicationYear][ma.Movie.Title] = ma;
                }
                else
                {
                    map[ma.Movie.PublicationYear][ma.Movie.Title].MovieAwardList.Add(ma.MovieAward);
                }

                if (ma.MovieAward != null)
                {
                    if (ma.MovieAward.Result.ToLower() == "nominated")
                        map[ma.Movie.PublicationYear][ma.Movie.Title].Nominated++;
                    if (ma.MovieAward.Result.ToLower() == "won")
                        map[ma.Movie.PublicationYear][ma.Movie.Title].Won++;
                }
    
            }

            ViewData["Report"] = map;
            ViewData["from"] = fromyear;
            ViewData["till"] = tillyear;
            
            return View(pagedResult);
        }
    }
}