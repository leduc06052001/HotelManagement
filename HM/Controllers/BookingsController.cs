using Antlr.Runtime.Tree;
using DAL;
using DAL.Entity;
using HM.App_Start;
using HM.Common;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using static HM.Models.PayPalConfiguration;

namespace HM.Controllers
{
    public class BookingsController : Controller
    {
        HMEntities db = new HMEntities();
        public ActionResult Index()
        {
            return View();
        }

        //------------------* BOOKING *------------------//
        public ActionResult Booking(int ID)
        {
            var sessionCus = Session["user"] as Customer;
            if(sessionCus == null)
            {
                return Redirect("/Home/Login");
            }
            var room = db.Rooms.Find(ID);
            if (room == null)
            {
                return HttpNotFound();
            }
            var booking = new Booking
            {
                RoomID = room.RoomID,
                BookingDate = DateTime.UtcNow
            };

            ViewBag.RoomDetail = room;
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Booking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                Session["booking"] = booking;
                var room = db.Rooms.Find(booking.RoomID);
                if (room == null)
                {
                    return HttpNotFound();
                }
                booking.Room = room;
                booking.BookingDate = DateTime.UtcNow;
                new mapBookings().TotalAmout(booking);
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("BookingDetail", "Bookings", new { id = booking.BookingID });
            }
            else
            {
                //gán lại các giá trị
                var room = db.Rooms.Find(booking.RoomID);
                ViewBag.RoomDetail = room;
                return View(booking);
            }
        }

        public ActionResult BookingDetail(int ID)
        {
            var booking = db.Bookings.Find(ID);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public ActionResult BookingDetail()
        {
            Booking booking = Session["booking"] as Booking;
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/templateEmail/SendEmailtoCustomer.html"));
            content = content.Replace("{{CustomerName}}", booking.CustomerName);
            content = content.Replace("{{Email}}", booking.Email);
            content = content.Replace("{{Phone}}", booking.Phone);
            content = content.Replace("{{RoomType}}", booking.Room.RoomType.RoomTypeName);
            content = content.Replace("{{Adult}}", booking.Adult.ToString());
            content = content.Replace("{{Child}}", booking.Child.ToString());
            content = content.Replace("{{BookingDate}}", booking.BookingDate.ToString());
            content = content.Replace("{{TotalAmount}}", booking.TotalAmount.ToString());

            var toEmail = booking.Email.ToString();
            var toAdmin = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new MailHelper().SendEmail("Artemis Hotel", toAdmin, "Đơn đặt phòng mới từ '" + booking.CustomerName + "'", content);
            new MailHelper().SendEmail("Artemis Hotel", toEmail, "Thông tin đơn đặt phòng", content);
            /*if (paymentMethod == "paypal")
            {
                return RedirectToAction("PaymentWithPaypal");
            }
            if (paymentMethod == "vnpay")
            {
                return RedirectToAction("PaymentWithVNpay");
            }*/
            return RedirectToAction("SuccessView");
        }


        public ActionResult Checkout(Booking booking)
        {
            return RedirectToAction("BookingDetail", "Bookings", new { id = booking.BookingID });
        }

        //------------------* PAYMENT WITH PAYPAL *------------------//
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Bookings/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var booking = Session["booking"] as Booking;
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = booking.Room.RoomType.RoomTypeName.ToString(),
                currency = "USD",
                price = Math.Round((decimal)booking.Room.Price / 24270).ToString(),
                quantity = "1",
                sku = booking.BookingID.ToString()
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = new mapBookings().TotalAmout_USD(booking).ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = new mapBookings().TotalAmout_USD(booking).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        public ActionResult SuccessView()
        {
            return View();
        }
        public ActionResult FailureView()
        {
            return View();
        }


    }
}

/*private void SendInfoBookingToCustomer(Booking booking)
        {
            // Thông tin tài khoản gửi mail
            string fromEmail = "artemishotel.contact@gmail.com";
            string emailPassword = "tzff yhmr weaj ctof";

            // Email nhận thông tin của khách hàng
            string customerEmail = booking.Email;
            // Tiêu đề email
            string subject = "Đơn đặt phòng của bạn từ Artemis";
            // Nội dung Email
            string body = "Thông tin đơn đặt phòng của bạn \n";
            body += "Họ tên: " + booking.CustomerName + "\n";
            body += "Loại phòng: " + booking.Room.RoomType.RoomTypeName + "\n";
            body += "Email: " + booking.Email + "\n";
            body += "Số điện thoại: " + booking.Phone + "\n";
            body += "Người lớn: " + booking.Adult + "\n";
            body += "Trẻ em: " + booking.Child + "\n";
            body += "Ngày đặt phòng: " + booking.BookingDate + "\n";
            body += "Ghi chú: " + booking.Note + "\n";

            MailMessage message = new MailMessage(fromEmail, customerEmail, subject, body);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromEmail, emailPassword);
            smtpClient.Send(message);
        }*/