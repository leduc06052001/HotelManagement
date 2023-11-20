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
        public List<News> LoadData()
        {
            return db.News.ToList();
        }
        public List<News> LoadPage(int page, int size)
        {
            return db.News.ToList().Skip((page-1)*size).Take(size).ToList();
        }

        public News GetDetail(int id)
        {
            return db.News.Find(id);
        }

        //------------------* Create *------------------//
        public int CreateNews(News news)
        {
            if(news.NewsTitle != null)
            {
                db.News.Add(news);
                db.SaveChanges();
                return news.NewsID;
            }
            return 0;
        }

        //------------------* Update *------------------//
        public bool UpdateNews(News news)
        {
            var newsInfo = db.News.Find(news.NewsID);
            if (newsInfo != null)
            {
                newsInfo.NewsTitle = news.NewsTitle;
                newsInfo.Author = news.Author;
                newsInfo.PublishDate = news.PublishDate;
                newsInfo.NewsContent = news.NewsContent;
                newsInfo.Image = news.Image;
                db.SaveChanges();
                return true;
            }
            return false;
        }

        //------------------* Delete *------------------//
        public void DeleteNews(int id)
        {
            var newsInfo = db.News.Find(id);
            db.News.Remove(newsInfo);
            db.SaveChanges();
        }
    }
}
