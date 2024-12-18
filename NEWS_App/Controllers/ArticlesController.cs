﻿using Microsoft.AspNetCore.Mvc;
using NEWS_App.Models.IRepository;
using NEWS_App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Http;
using NEWS_App.Models.IRepositoryImpl;

namespace NEWS_App.Controllers
{
    
    public class ArticlesController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotificationRepository _notiRepository;
        private readonly ILikeRepository _likeRepository;

        public ArticlesController(IArticleRepository articleRepository,ICategoryRepository categoryRepository, INotificationRepository notiRepository, ILikeRepository likeRepository)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _notiRepository = notiRepository;
            _likeRepository = likeRepository;
        }

        //top news
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.GetAllArticlesAsync();

            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var articleLikeViewModels = new List<ArticleLikeViewModel>();

            foreach (var article in articles)
            {
                // Await each call individually to get the result
                bool isLikedByUser = await _likeRepository.IsArticleLikedByUser(userId, article.Id);

                // Add the article and like status to the view model list
                articleLikeViewModels.Add(new ArticleLikeViewModel
                {
                    Article = article,
                    IsLikedByUser = isLikedByUser
                });
            }
            return View(articleLikeViewModels);
        }

        //category wise news
        public async Task<IActionResult> category(string name)
        {
          
            var category =await _categoryRepository.GetCategoryIdByNameAsync(name);
            if(category == null) return NotFound("not found  given category" + name);

            int id=category.Id;
            var articles= await _articleRepository.GetCategorywiseArticle(id);


            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;


            var articleLikeViewModels = new List<ArticleLikeViewModel>();

            foreach (var article in articles)
            {
                // Await each call individually to get the result
                bool isLikedByUser = await _likeRepository.IsArticleLikedByUser(userId, article.Id);

                // Add the article and like status to the view model list
                articleLikeViewModels.Add(new ArticleLikeViewModel
                {
                    Article = article,
                    IsLikedByUser = isLikedByUser
                });
            }

            ViewBag.CategoryName = name;
            return View(articleLikeViewModels);
        }

        public async Task<IActionResult> Unique(int category,int article)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            //when comment add then redirect to this page not any other
            HttpContext.Session.SetInt32("categoryId", category);
            HttpContext.Session.SetInt32("articleId", article);
            if (userId == 0)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return RedirectToAction("Login","User");
            }

            var UniqueArticle = await _articleRepository.GetArticleByArticleCategory(category,article);
            if (UniqueArticle == null) return NotFound("not found  that Article");
            return View(UniqueArticle);
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.Session.GetString("Username") ?? "temp";
            if(userId != "admin@gmail.com") return NotFound();
            var category = await _categoryRepository.GetAllCategoriesAsync();
            if (category == null) return NotFound();
            else
            {
                ViewBag.Category = new SelectList(category, "Id", "Name");
            }
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article, IFormFile imageFile, IFormFile videoFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFile.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                article.ImageUrl = "/images/" + imageFile.FileName;
            }
            else
            {
                article.ImageUrl = "No Images";
            }
            if (videoFile != null && videoFile.Length > 0)
            {

                var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos", videoFile.FileName);
                using (var stream = new FileStream(videoPath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }
                article.VideoUrl = "/videos/" + videoFile.FileName;
            }
            else
            {
                article.VideoUrl = "No Videos";
            }
            article.PublishedDate = DateTime.Now;
            article.LikeCount = 0;
            await _articleRepository.AddArticleAsync(article);

            var newArticle = await _articleRepository.GetLatestArticleAsync();

            await _notiRepository.NotifyAllUsersAboutNewArticleAsync(newArticle);
            return RedirectToAction("Index", "Articles");
        }


        public async Task<IActionResult> Search(String title)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId == 0)
            {
                var currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return RedirectToAction("Login", "User");
            }

            var articles= await _articleRepository.SearchArticlesByTitle(title);
            if (articles == null)
            {
                return NotFound("Not match your search with title .");
            }

            var articleLikeViewModels = new List<ArticleLikeViewModel>();

            foreach (var article in articles)
            {
                // Await each call individually to get the result
                bool isLikedByUser = await _likeRepository.IsArticleLikedByUser(userId, article.Id);

                // Add the article and like status to the view model list
                articleLikeViewModels.Add(new ArticleLikeViewModel
                {
                    Article = article,
                    IsLikedByUser = isLikedByUser
                });
            }
            return View(articleLikeViewModels);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article article)
        {
            if (!ModelState.IsValid) return View(article);
            await _articleRepository.UpdateArticleAsync(article);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
            {
                return Json(new { delete = false });
            }
            else
            {
                await _articleRepository.DeleteArticleAsync(id);
                return Json(new { delete = true });
            }
        }
    }

}
