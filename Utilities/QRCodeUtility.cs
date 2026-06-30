using System.IO;
using System.Xml;
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
            string name = $"{firstName} {middleName} {lastName}".Trim();
            
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                name += $" {suffix}";
            }
                        string qrContent = $"{name} - {schoolStudentId}";

            // Generate QR code
            return QRCodeImageBuilder.GetPngBytes(qrContent);
        }

        public static GetQRCode(string schoolStudentId)
        {

        }
    }
}