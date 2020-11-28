using Microsoft.Win32;
using System.Windows.Forms;

namespace BhavFinder.Utils.Persistence
{
    // See - http://csharphelper.com/blog/2018/06/build-an-mru-list-in-c/
    public class RegistryTools
    {
        // Save a value.
        public static void SaveSetting(string app_name, string name, object value)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey sub_key = reg_key.CreateSubKey(app_name);
            sub_key.SetValue(name, value);
        }

        // Get a value.
        public static object GetSetting(string app_name, string name, object default_value)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey sub_key = reg_key.CreateSubKey(app_name);
            return sub_key.GetValue(name, default_value);
        }

        // Load all of the saved settings.
        public static void LoadFormSettings(string app_name, Form frm)
        {
            // Load form settings.
            frm.SetBounds(
                (int)GetSetting(app_name, "FormLeft", frm.Left),
                (int)GetSetting(app_name, "FormTop", frm.Top),
                (int)GetSetting(app_name, "FormWidth", frm.Width),
                (int)GetSetting(app_name, "FormHeight", frm.Height));
            frm.WindowState = (FormWindowState)GetSetting(app_name,
                "FormWindowState", frm.WindowState);
        }

        // Save all of the form's settings.
        public static void SaveFormSettings(string app_name, Form frm)
        {
            // Save form settings.
            SaveSetting(app_name, "FormWindowState", (int)frm.WindowState);
            if (frm.WindowState == FormWindowState.Normal)
            {
                // Save current bounds.
                SaveSetting(app_name, "FormLeft", frm.Left);
                SaveSetting(app_name, "FormTop", frm.Top);
                SaveSetting(app_name, "FormWidth", frm.Width);
                SaveSetting(app_name, "FormHeight", frm.Height);
            }
            else
            {
                // Save bounds when we're restored.
                SaveSetting(app_name, "FormLeft", frm.RestoreBounds.Left);
                SaveSetting(app_name, "FormTop", frm.RestoreBounds.Top);
                SaveSetting(app_name, "FormWidth", frm.RestoreBounds.Width);
                SaveSetting(app_name, "FormHeight", frm.RestoreBounds.Height);
            }
        }

        // Delete a value.
        public static void DeleteSetting(string app_name, string name)
        {
            RegistryKey reg_key = Registry.CurrentUser.OpenSubKey("Software", true);
            RegistryKey sub_key = reg_key.CreateSubKey(app_name);
            try
            {
                sub_key.DeleteValue(name);
            }
            catch
            {
            }
        }
    }
}
