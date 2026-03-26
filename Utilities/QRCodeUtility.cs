// using System.Drawing;
// using System.IO;
// using System.Drawing.Imaging;
// using QRCoder;
// using static QRCoder.QRCodeGenerator;
// using System.Runtime.Versioning;

// namespace SeamsApp.Utilities
// {
//     public class QRCodeUtility
//     {
//         [SupportedOSPlatform("windows")]
//         public static byte[] GenerateQRCode(
//             string firstName, 
//             string middleName, 
//             string lastName, 
//             string schoolStudentId)
//         {
//             string qrContent = $"{firstName} {middleName} {lastName} - {schoolStudentId}";

//             var qrGenerator = new QRCoder.QRCodeGenerator();
//             var qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q);


//             using (var qrCode = new QRCode(qrCodeData))
//             {
//                 using (var bitmap = qrCode.GetGraphic(20))
//                 {
//                     using (var memoryStream = new MemoryStream())
//                     {
//                         bitmap.Save(memoryStream, ImageFormat.Png);
//                         return memoryStream.ToArray();
//                     }
//                 }
//             }
//         }
//     }
// }
using System.IO;
using SkiaSharp.QrCode.Image;

namespace SeamsApp.Utilities
{
    public class QRCodeUtility
    {
        public static byte[] GenerateQRCode(
            string firstName, 
            string middleName, 
            string lastName, 
            string? suffix,
            string schoolStudentId)
        {
            // Build the name with proper spacing and null handling
            string name = $"{firstName} {middleName} {lastName}".Trim();
            
            // Only add suffix if it's not null or whitespace
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                name += $" {suffix}";
            }
            
            // Format with proper spacing around dash
            string qrContent = $"{name} - {schoolStudentId}";

            // Generate QR code
            return QRCodeImageBuilder.GetPngBytes(qrContent);
        }
    }
}