using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using QRCoder;
using static QRCoder.QRCodeGenerator;
using System.Runtime.Versioning;

namespace SeamsApp.Utilities
{
    public class QRCodeUtility
    {
        [SupportedOSPlatform("windows")]
        public static byte[] GenerateQRCode(
            string firstName, 
            string middleName, 
            string lastName, 
            string schoolStudentId)
        {
            string qrContent = $"{firstName} {middleName} {lastName} - {schoolStudentId}";

            var qrGenerator = new QRCoder.QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q);


            using (var qrCode = new QRCode(qrCodeData))
            {
                using (var bitmap = qrCode.GetGraphic(20))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        bitmap.Save(memoryStream, ImageFormat.Png);
                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}
