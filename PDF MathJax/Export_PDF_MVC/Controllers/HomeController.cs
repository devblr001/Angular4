using System;

using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.Net;

namespace Export_PDF_MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            NorthwindEntities entities = new NorthwindEntities();
            return View(from customer in entities.Customers.Take(10)
                        select customer);
        }


        public string JSContentinString(string url)
        {
            var webRequest = WebRequest.Create(url);

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                var strContent = reader.ReadToEnd();
                return strContent;
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            try
            {
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    StringReader sr = new StringReader(GridHtml);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);

                    //js
                    var jsBuilder = JSContentinString("http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js");
                        jsBuilder += JSContentinString("https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.2/MathJax.js?config=TeX-AMS_HTML-full");
                   // var webRequest = WebRequest.Create(@"https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.2/MathJax.js?config=TeX-AMS_HTML-full");

                    //using (var response = webRequest.GetResponse())
                    //using (var content = response.GetResponseStream())
                    //using (var reader = new StreamReader(content))
                    //{
                    //    //   // var scriptcontent = "<script type = \"text/x-mathjax-config\" >MathJax.Hub.Config({tex2jax: { inlineMath: [[\"$\", \"$\"],[\"\\(\",\"\\)\"]]}});</script>";
                    //     var strContent = reader.ReadToEnd();

                        // var body = "<p>When $a \ne 0$, there are two solutions to \\(ax ^ 2 + bx + c = 0\\) and they are $$x = { -b \\pm \\sqrt{ b ^ 2 - 4ac} \\over 2a}.$$</p> ";

                        //Phrase phrase1 = new Phrase("Hello world. Checking Print Functionality");
                        //pdfDoc.Add(phrase1);

                        //Phrase phrase2 = new Phrase(body);
                        //pdfDoc.Add(phrase2);

                        // writer.AddJavaScript(scriptcontent + strContent + body + "this.print();");
                        //writer.AddJavaScript("this.print();");
                        // string jsText = "var res = app.setTimeOut('var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);', 200);";
                        // writer.AddJavaScript(webRequest);
                        //
                        //  }
                        pdfDoc.NewPage();

                        // var webRequest = WebRequest.Create(@"https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.2/MathJax.js?config=TeX-AMS_HTML-full");
                      //  string script = "MathJax.Hub.Config({tex2jax: { inlineMath: [[\"$\", \"$\"],[\"\\\\(\",\"\\\\)\"]]}});";
                        writer.AddJavaScript( "this.print();");



                        pdfDoc.Close();
                        return File(stream.ToArray(), "application/pdf", "Grid.pdf");
                    }
               
            }
            catch(Exception ex)
            {
                throw;
            }
                
            }
        }
    }
