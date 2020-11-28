﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace BhavFinder.Utils.Persistence
{
    // See - http://csharphelper.com/blog/2018/06/build-an-mru-list-in-c/
    class MruList
    {
        // The application's name.
        private string AppRegKey;

        // A list of the files.
        private int NumFiles;
        private List<FileInfo> FileInfos;

        // The File menu.
        private ToolStripMenuItem MyMenu;

        // The menu items we use to display files.
        private ToolStripMenuItem[] MenuItems;

        // Raised when the user selects a file from the MRU list.
        public delegate void FileSelectedEventHandler(string file_name);
        public event FileSelectedEventHandler FileSelected;

        // Constructor.
        public MruList(string appRegKey, ToolStripMenuItem menu, int num_files)
        {
            AppRegKey = appRegKey;
            MyMenu = menu;
            NumFiles = num_files;
            FileInfos = new List<FileInfo>();

            // Make the menu items we may later need.
            MenuItems = new ToolStripMenuItem[NumFiles + 1];
            for (int i = 0; i < NumFiles; i++)
            {
                MenuItems[i] = new ToolStripMenuItem();
                MenuItems[i].Visible = false;
                MyMenu.DropDownItems.Add(MenuItems[i]);
            }

            // Reload items from the registry.
            LoadFiles();

            // Display the items.
            ShowFiles();
        }

        // Load saved items from the Registry.
        private void LoadFiles()
        {
            // Reload items from the registry.
            for (int i = 0; i < NumFiles; i++)
            {
                string file_name = (string)RegistryTools.GetSetting(AppRegKey, "FilePath" + i.ToString(), "");
                if (file_name != "")
                {
                    FileInfos.Add(new FileInfo(file_name));
                }
            }
        }

        // Display the files in the menu items.
        private void ShowFiles()
        {
            MyMenu.Enabled = (FileInfos.Count > 0);
            for (int i = 0; i < FileInfos.Count; i++)
            {
                MenuItems[i].Text = string.Format("&{0} {1}", i + 1, FileInfos[i].Name);
                MenuItems[i].Visible = true;
                MenuItems[i].Tag = FileInfos[i];
                MenuItems[i].Click -= File_Click;
                MenuItems[i].Click += File_Click;
            }
            for (int i = FileInfos.Count; i < NumFiles; i++)
            {
                MenuItems[i].Visible = false;
                MenuItems[i].Click -= File_Click;
            }
        }

        // Save the current items in the Registry.
        private void SaveFiles()
        {
            // Delete the saved entries.
            for (int i = 0; i < NumFiles; i++)
            {
                RegistryTools.DeleteSetting(AppRegKey, "FilePath" + i.ToString());
            }

            // Save the current entries.
            int index = 0;
            foreach (FileInfo file_info in FileInfos)
            {
                RegistryTools.SaveSetting(AppRegKey, "FilePath" + index.ToString(), file_info.FullName);
                index++;
            }
        }

        // Add a file to the list, rearranging if necessary.
        public void AddFile(string file_name)
        {
            // Remove the file from the list.
            RemoveFileInfo(file_name);

            // Add the file to the beginning of the list.
            FileInfos.Insert(0, new FileInfo(file_name));

            // If we have too many items, remove the last one.
            if (FileInfos.Count > NumFiles) FileInfos.RemoveAt(NumFiles);

            // Display the files.
            ShowFiles();

            // Update the Registry.
            SaveFiles();
        }

        // Remove a file's info from the list.
        private void RemoveFileInfo(string file_name)
        {
            // Remove occurrences of the file's information from the list.
            for (int i = FileInfos.Count - 1; i >= 0; i--)
            {
                if (FileInfos[i].FullName == file_name)
                    FileInfos.RemoveAt(i);
            }
        }

        // Remove a file from the list, rearranging if necessary.
        public void RemoveFile(string file_name)
        {
            // Remove the file from the list.
            RemoveFileInfo(file_name);

            // Display the files.
            ShowFiles();

            // Update the Registry.
            SaveFiles();
        }

        // The user selected a file from the menu.
        private void File_Click(object sender, EventArgs e)
        {
            // Don't bother if no one wants to catch the event.
            if (FileSelected != null)
            {
                // Get the corresponding FileInfo object.
                ToolStripMenuItem menu_item = sender as ToolStripMenuItem;
                FileInfo file_info = menu_item.Tag as FileInfo;

                // Raise the event.
                FileSelected(file_info.FullName);
            }
        }
    }
}
