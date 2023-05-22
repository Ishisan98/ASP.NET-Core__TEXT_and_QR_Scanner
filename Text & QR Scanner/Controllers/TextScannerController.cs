using IronOcr;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Text___QR_Scanner.Models;

namespace Text___QR_Scanner.Controllers
{
    public class TextScannerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TextScannerController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SaveImage(ImageData imageData)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = DateTime.Now.ToString("yymmssfff") + ".png";
            string path = Path.Combine(wwwRootPath + "/images/", fileName);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))

                {
                    byte[] data = Convert.FromBase64String(imageData.data);
                    bw.Write(data);
                    bw.Close();
                }
            }

            OcrResult result = new IronTesseract().Read(path);
            string text = result.Text;

            if (path != null)
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            if (text == "")
            {
                ViewBag.Result = "Cannot Detect Text";
                return View("Index");
            }
            else
            {
                ViewBag.Result = text;
                return View("Index");
            }
        }

    }
}
