using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trackit.Utils
{
    public class DialogHandler
    {
        public static void ShowError(string caption, string message)
        {
            XtraMessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarning(string caption, string message)
        {
            XtraMessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInformation(string caption, string message)
        {
            XtraMessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowConfirmationOKCancel(string caption, string message)
        {
            return XtraMessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        public static DialogResult ShowConfirmationYesNo(string caption, string message)
        {
            return XtraMessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
    }
}
