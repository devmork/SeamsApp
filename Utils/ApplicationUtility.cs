using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trackit.Utils
{
    public class ApplicationUtility
    {
        public static void ClearFields(object[] controls)
        {
            foreach (Control control in controls)
            {
                if (control is TextEdit textEdit)
                {
                    textEdit.Text = string.Empty;
                }
                if (control is ComboBoxEdit comboBoxEdit)
                {
                    comboBoxEdit.Text = string.Empty;
                }
                if (control is DateEdit dateEdit)
                {
                    dateEdit.EditValue = null;
                }
                if (control is SpinEdit spinEdit)
                {
                    spinEdit.Value = 0;
                }
                if (control is LookUpEdit lookUpEdit)
                {
                    lookUpEdit.EditValue = null;
                }
            }
        }

        public static DateTime? GetDEValueIfEmpty(DateEdit dateEdit)
        {
            bool isEmpty = !string.IsNullOrWhiteSpace(dateEdit.Text.Trim());
            return isEmpty ? Convert.ToDateTime(dateEdit.EditValue) : (DateTime?)null;
        }

        public static DateTime? GetDateValueIfEmpty(object date)
        {
            bool isEmpty = !string.IsNullOrWhiteSpace(Convert.ToString(date));
            return isEmpty ? Convert.ToDateTime(date) : (DateTime?)null;
        }

        public static string GetDateValueIfEmptyFormatted(object date)
        {
            bool isEmpty = !string.IsNullOrWhiteSpace(Convert.ToString(date));
            return isEmpty ? Convert.ToDateTime(date).ToString("MMMM dd, yyyy") : "";
        }

        public static byte[] GetByteArrayFromImage(Image image)
        {
            byte[] imgData = null;

            using (MemoryStream ms = new MemoryStream())
            {
                if (image != null)
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    imgData = ms.ToArray();
                }
            }

            return imgData;
        }

        public static List<string> GetDecimalRange(decimal from, decimal to)
        {
            var decimalRange = new List<string>();

            for (decimal x = from; x >= to; x--)
            {
                decimalRange.Add(x.ToString("0.00"));
            }

            return decimalRange.ToList();
        }


        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        public static bool CheckHashPassword(string password , string hashPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashPassword);
        }
    }
}
