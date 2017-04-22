using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace ImageConverter
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        System.Windows.Forms.OpenFileDialog aDialog = new System.Windows.Forms.OpenFileDialog();
        System.Windows.Forms.FolderBrowserDialog aFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
        public Window1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            aDialog.Multiselect = true;
            if (aDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in aDialog.FileNames)
                {
                    listBox1.Items.Add(System.IO.Path.GetFileName(fileName));
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (aFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DestinationFolderPath.Content = aFolderBrowserDialog.SelectedPath;  
            }

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string DestinationFolder = DestinationFolderPath.Content.ToString();

            foreach (string fileName in aDialog.FileNames)
            {
                Convert(fileName, DestinationFolder);        
            }
        }

        private void Convert(string fileName, string DestinationFolder)
        {
            Bitmap fillImage = new Bitmap(fileName);
            Bitmap skeletonImage = new Bitmap(fillImage.Width, fillImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); 
            Graphics g = Graphics.FromImage(skeletonImage);
            g.DrawImage(fillImage, 0, 0);
            string targetPath = DestinationFolder + "/" + System.IO.Path.GetFileNameWithoutExtension(fileName) + ".png";
            skeletonImage.Save(targetPath,System.Drawing.Imaging.ImageFormat.Png);
        }


    }
}
