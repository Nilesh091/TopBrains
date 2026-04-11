using System;
using QRCoder;
namespace Enterprise_Two_Factor_Authentication.Services
{
    public class QRCodeService
    {
        public string GenerateQRCode(string url)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qRCodeData);
            return qrCode.GetGraphic(20);
        }

    }
}
