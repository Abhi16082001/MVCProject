using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCoder;
using static QRCoder.PayloadGenerator;
using FirstTasks.Models;

namespace FirstTasks.Controllers
{
    public class QrCodeController : Controller
    {
        public ActionResult Index()
        {
            return View("QRView", new QrCode());
        }

        [HttpPost]
        public ActionResult CreateQR(QrCode qrmodel)
        {
            Payload payload = new Url(qrmodel.QRCodeText);
            QRCodeGenerator codegen = new QRCodeGenerator();
            QRCodeData codedata = codegen.CreateQrCode(payload);
            PngByteQRCode pngbyte = new PngByteQRCode(codedata);
            var qrbyte = pngbyte.GetGraphic(20);
            string base64url = Convert.ToBase64String(qrbyte);
            qrmodel.QRImageURL = "data:image/png;base64," + base64url;
            return View("QRView", qrmodel);
        }
    }
}