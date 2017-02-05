using System;
using System.Linq;
using QRCoder;

namespace HiddenSound.API.Areas.API.Services
{
    public class QRService : IQRService
    {
        public string Create(string contents)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(contents, QRCodeGenerator.ECCLevel.Q);
            var qrCodeBmp = new BitmapByteQRCode(qrCodeData);
            var qrCodeImage = qrCodeBmp.GetGraphic(20);

            return $"data:image/bmp;base64,{Convert.ToBase64String(qrCodeImage)}";
        }
    }
}
