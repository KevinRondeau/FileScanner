using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FileScanner.Models
{
    public class Files
    {
        public string Name { get; set; }
        public string Image { get; set; }


        public Files(string P)
        {
            Name = P;
            if (checkDir(P) == false)
            {
                Image = "/file.png";
            }
            if (checkDir(P) == true)
            {
                Image = "/folder.jpg";
            }

        }
        private Boolean checkDir(string n)
        {
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(n);

            if (attr.HasFlag(FileAttributes.Directory))
                return true;
            else
                return false;


            ////--original method---////
            //// get the file attributes for file or directory
            //FileAttributes attr = File.GetAttributes(@"c:\Temp");

            //if (attr.HasFlag(FileAttributes.Directory))
            //    MessageBox.Show("Its a directory");
            //else
            //    MessageBox.Show("Its a file");
        }

    }

    
}
