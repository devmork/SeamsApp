using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackit.Properties;

namespace Trackit.Utils
{
    public class ErrorHandler
    {
        public static void SetControlsValidation(object[] controls, bool hasError)
        {
            foreach (object obj in controls)
            {
                if (obj is TextEdit)
                {
                    TextEdit textEdit = (TextEdit)obj;
                    textEdit.Properties.Appearance.BorderColor = hasError ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    textEdit.Properties.ContextImageOptions.Image = hasError ? Resources.exclamation_16 : null;
                }

                if (obj is MemoEdit)
                {
                    MemoEdit memoEdit = (MemoEdit)obj;
                    memoEdit.Properties.Appearance.BorderColor = hasError ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    memoEdit.Properties.ContextImageOptions.Image = hasError ? Resources.exclamation_16 : null;
                }
                if (obj is DateEdit)
                {
                    DateEdit dateEdit = (DateEdit)obj;
                    dateEdit.Properties.Appearance.BorderColor = hasError ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    dateEdit.Properties.ContextImageOptions.Image = hasError ? Resources.exclamation_16 : null;
                }

                if (obj is ComboBoxEdit)
                {
                    ComboBoxEdit comboBoxEdit = (ComboBoxEdit)obj;
                    comboBoxEdit.Properties.Appearance.BorderColor = hasError ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    comboBoxEdit.Properties.ContextImageOptions.Image = hasError ? Resources.exclamation_16 : null;
                }

                if (obj is SpinEdit)
                {
                    SpinEdit spinEdit = (SpinEdit)obj;
                    spinEdit.Properties.Appearance.BorderColor = hasError ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    spinEdit.Properties.ContextImageOptions.Image = hasError ? Resources.exclamation_16 : null;
                }

                if (obj is MRUEdit)
                {
                    MRUEdit mRUEdit = (MRUEdit)obj;
                    mRUEdit.Properties.Appearance.BorderColor = hasError ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    mRUEdit.Properties.ContextImageOptions.Image = hasError ? Resources.exclamation_16 : null;
                }

                if (obj is LookUpEdit)
                {
                    LookUpEdit lookUpEdit = (LookUpEdit)obj;
                    lookUpEdit.Properties.Appearance.BorderColor = hasError ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    lookUpEdit.Properties.ContextImageOptions.Image = hasError ? Resources.exclamation_16 : null;
                }
            }
        }

        public static bool ValidateAllControls(object[] controls)
        {
            bool isValid = true;

            foreach (object obj in controls)
            {
                if (obj is TextEdit)
                {
                    TextEdit textEdit = (TextEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(textEdit.Text.Trim()) || textEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                    textEdit.Properties.Appearance.BorderColor = isEmpty ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    textEdit.Properties.ContextImageOptions.Image = isEmpty ? Resources.exclamation_16 : null;
                }

                if (obj is MemoEdit)
                {
                    MemoEdit memoEdit = (MemoEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(memoEdit.Text.Trim()) || memoEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                    memoEdit.Properties.Appearance.BorderColor = isEmpty ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    memoEdit.Properties.ContextImageOptions.Image = isEmpty ? Resources.exclamation_16 : null;
                }
                if (obj is DateEdit)
                {
                    DateEdit dateEdit = (DateEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(dateEdit.Text.Trim()) || dateEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                    dateEdit.Properties.Appearance.BorderColor = isEmpty ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    dateEdit.Properties.ContextImageOptions.Image = isEmpty ? Resources.exclamation_16 : null;
                }

                if (obj is ComboBoxEdit)
                {
                    ComboBoxEdit comboBoxEdit = (ComboBoxEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(comboBoxEdit.Text.Trim()) || comboBoxEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                    comboBoxEdit.Properties.Appearance.BorderColor = isEmpty ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    comboBoxEdit.Properties.ContextImageOptions.Image = isEmpty ? Resources.exclamation_16 : null;
                }

                if (obj is SpinEdit)
                {
                    SpinEdit spinEdit = (SpinEdit)obj;
                    isValid &= !(spinEdit.Value <= 0);
                    spinEdit.Properties.Appearance.BorderColor = (spinEdit.Value <= 0) ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    spinEdit.Properties.ContextImageOptions.Image = (spinEdit.Value <= 0) ? Resources.exclamation_16 : null;
                }

                if (obj is MRUEdit)
                {
                    MRUEdit mRUEdit = (MRUEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(mRUEdit.Text.Trim()) || mRUEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                    mRUEdit.Properties.Appearance.BorderColor = isEmpty ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    mRUEdit.Properties.ContextImageOptions.Image = isEmpty ? Resources.exclamation_16 : null;
                }

                if (obj is LookUpEdit)
                {
                    LookUpEdit lookUpEdit = (LookUpEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(lookUpEdit.Text.Trim()) || lookUpEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                    lookUpEdit.Properties.Appearance.BorderColor = isEmpty ? Color.FromArgb(226, 87, 76) : Color.Empty;
                    lookUpEdit.Properties.ContextImageOptions.Image = isEmpty ? Resources.exclamation_16 : null;
                }
            }

            return isValid;
        }

        public static bool ValidateEmptyControls(object[] controls)
        {
            bool isValid = true;

            foreach (object obj in controls)
            {
                if (obj is TextEdit)
                {
                    TextEdit textEdit = (TextEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(textEdit.Text.Trim()) || textEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                }

                if (obj is MemoEdit)
                {
                    MemoEdit memoEdit = (MemoEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(memoEdit.Text.Trim()) || memoEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                }
                if (obj is DateEdit)
                {
                    DateEdit dateEdit = (DateEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(dateEdit.Text.Trim()) || dateEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                }

                if (obj is ComboBoxEdit)
                {
                    ComboBoxEdit comboBoxEdit = (ComboBoxEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(comboBoxEdit.Text.Trim()) || comboBoxEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                }

                if (obj is SpinEdit)
                {
                    SpinEdit spinEdit = (SpinEdit)obj;
                    isValid &= !(spinEdit.Value <= 0);
                }

                if (obj is MRUEdit)
                {
                    MRUEdit mRUEdit = (MRUEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(mRUEdit.Text.Trim()) || mRUEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                }

                if (obj is LookUpEdit)
                {
                    LookUpEdit lookUpEdit = (LookUpEdit)obj;
                    bool isEmpty = string.IsNullOrWhiteSpace(lookUpEdit.Text.Trim()) || lookUpEdit.Text.Trim() == ".";
                    isValid &= !isEmpty;
                }
            }

            return isValid;
        }
    }
}
