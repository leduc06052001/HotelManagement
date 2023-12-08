using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapNews
    {
        HMEntities db = new HMEntities();

        //------------------* ALL NEWS & SEARCH(Load Page) *------------------//
        public List<News> AllNews(string title, string author, int page, int size)
        {
            IQueryable<News> data = db.News;
            if (!string.IsNullOrEmpty(title))
            {
                data = db.News.Where(p => p.NewsTitle.ToLower().Contains(title));
            }
            if (!string.IsNullOrEmpty(author))
            {
                data = db.News.Where(p => p.Author.ToLower().Contains(author));
            }
            return data.OrderBy(p => p.NewsTitle).Skip((page - 1) * size).Take(size).ToList();
        }

        //------------------* GET DETAIL *------------------//
        public News GetDetail(int id)
        {
            return db.News.Find(id);
        }

        //------------------* CREATE *------------------//
        public int CreateNews(News news)
        {
            if (news == null)
            {
                return 0;
            }
            db.News.Add(news);
            db.SaveChanges();
            return news.NewsID;
        }

        //------------------* UPDATE *------------------//
        public bool UpdateNews(News news)
        {
            var newsInfo = db.News.Find(news.NewsID);
            if (newsInfo == null)
            {
                return false;
            }
            newsInfo.NewsTitle = news.NewsTitle;
            newsInfo.Author = news.Author;
            newsInfo.PublishDate = news.PublishDate;
            newsInfo.NewsContent = news.NewsContent;
            newsInfo.IsActive = news.IsActive;
            db.SaveChanges();
            return true;
        }

        //------------------* DELETE *------------------//
        public void DeleteNews(int id)
        {
            var newsInfo = db.News.Find(id);
            db.News.Remove(newsInfo);
            db.SaveChanges();
        }
    }
}
