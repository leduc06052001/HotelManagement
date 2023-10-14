using DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class mapNew
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
        public int CrateNews(News news)
        {
            if(news.Name != null)
            {
                db.News.Add(news);
                db.SaveChanges();
                return news.NewID;
            }
            return 0;
        }

        //------------------* Update *------------------//
        public bool UpdateNews(News news)
        {
            var newsInfo = db.News.Find(news.NewID);
            if (newsInfo != null)
            {
                newsInfo.Name = news.Name;
                newsInfo.Author = news.Author;
                newsInfo.PublishDate = news.PublishDate;
                newsInfo.NewContent = news.NewContent;
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
