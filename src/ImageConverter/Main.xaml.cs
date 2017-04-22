//-----------------------------------------------------------------------
// <copyright file="Main.xaml.cs" company="Ranjith Venkatesh">
//     Copyright (c) Ranjith Venkatesh. All rights reserved.
// </copyright>
// <author>Ranjith Venkatesh</author>
//-----------------------------------------------------------------------
namespace ImgConverter
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using WPFLocalizeExtension.Engine;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     Class Instance to select images to convert
        /// </summary>
        private OpenFileDialog openFileDialog = new OpenFileDialog();

        /// <summary>
        ///     Class Instance to select destination folder to save converted images
        /// </summary>
        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        /// <summary>
        ///     Background worker to do conversion
        /// </summary>
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        /// <summary>
        ///     Instance of the delegate function
        /// </summary>
        private UpdateDelegate update;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += this.BackgroundWorker_DoWork;
            this.backgroundWorker.RunWorkerCompleted += this.BackgroundWorker_RunWorkerCompleted;
            this.update = new UpdateDelegate(this.MarkImage);
        }

        /// <summary>
        ///     Delegate function to report progress of conversion
        /// </summary>
        /// <param name="index">Index of file being converted</param>
        /// <param name="conversionSuccess">Success of conversion</param>
        private delegate void UpdateDelegate(int index, bool conversionSuccess);

        /// <summary>
        ///     Function where the looping conversion is done
        /// </summary>
        /// <param name="sender">Sender of the task</param>
        /// <param name="e">DoWork arguments</param>
        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ImgConverter.Helper.ConversionInformation conversionInformation = (ImgConverter.Helper.ConversionInformation)e.Argument;

            for (int i = 0; i < conversionInformation.FileNames.Length; i++)
            {
                string fileName = conversionInformation.FileNames.GetValue(i).ToString();
                bool conversionSuccess = Helper.Convert(fileName, conversionInformation.DestinationPath, conversionInformation.PixelFormat, conversionInformation.ImageType);
                this.listBoxFiles.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, this.update, i, conversionSuccess);
                if (this.backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        /// <summary>
        ///     Tasks to be done after conversion
        /// </summary>
        /// <param name="sender">Sender of the task</param>
        /// <param name="e">RunWorkerCompleted arguments</param>
        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.buttonConvert.IsEnabled = this.buttonCancel.IsEnabled = false;        
        }

        /// <summary>
        ///     Mark the converted image as green or red
        /// </summary>
        /// <param name="index">index of the image</param>
        /// <param name="conversionSuccess">success of the conversion</param>
        private void MarkImage(int index, bool conversionSuccess)
        {
            ListBoxItem listBoxItem = this.listBoxFiles.Items[index] as ListBoxItem;
            if (conversionSuccess)
            {
                listBoxItem.Background = System.Windows.Media.Brushes.GreenYellow;
            }
            else
            {
                listBoxItem.Background = System.Windows.Media.Brushes.IndianRed;
            }
        }

        /// <summary>
        ///     Select images to convert
        /// </summary>
        /// <param name="sender">Triggering source</param>
        /// <param name="e">Event Arguments</param>
        private void ButtonSelectImages_Click(object sender, RoutedEventArgs e)
        {
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Filter = "Image files (*.jpg;*.png;*.bmp;*.gif)|*.jpg;*.png;*.bmp;*.gif";
            this.listBoxFiles.Items.Clear();
            if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in this.openFileDialog.FileNames)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = System.IO.Path.GetFileName(fileName);
                    this.listBoxFiles.Items.Add(item);
                }

                this.buttonConvert.IsEnabled = true;
            }
        }
        
        /// <summary>
        ///     Select Destination Folder to save converted images
        /// </summary>
        /// <param name="sender">Triggering source</param>
        /// <param name="e">Event Arguments</param>
        private void ButtonSelectDestinationFolder_Click(object sender, RoutedEventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.DestinationFolderPath.Content = this.folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        ///     Convert images
        /// </summary>
        /// <param name="sender">Triggering source</param>
        /// <param name="e">Event Arguments</param>
        private void ButtonConvert_Click(object sender, RoutedEventArgs e)
        {
            this.buttonConvert.IsEnabled = false;
            this.buttonCancel.IsEnabled = true;

            ImgConverter.Helper.ConversionInformation conversionInformation = new ImgConverter.Helper.ConversionInformation();
            conversionInformation.DestinationPath = this.DestinationFolderPath.Content.ToString();
            conversionInformation.FileNames = this.openFileDialog.FileNames;
            conversionInformation.ImageType = this.comboBoxImageFormat.Text;
            conversionInformation.PixelFormat = this.comboBoxPixelFormat.Text;
            this.backgroundWorker.RunWorkerAsync(conversionInformation);       
        }

        /// <summary>
        ///     Stop the conversion after the current image being converted
        /// </summary>
        /// <param name="sender">Sender of the task</param>
        /// <param name="e">RoutedEvent arguments</param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.backgroundWorker.CancelAsync();
        }

        /// <summary>
        ///     Change language to English
        /// </summary>
        /// <param name="sender">Sender of the task</param>
        /// <param name="e">RoutedEvent arguments</param>
        private void ButtonEN_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateLanguageStrings("en");
        }

        /// <summary>
        ///     Change language to German
        /// </summary>
        /// <param name="sender">Sender of the task</param>
        /// <param name="e">RoutedEvent arguments</param>
        private void ButtonDE_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateLanguageStrings("de");
        }

        /// <summary>
        ///     Set Localization Extension Language
        /// </summary>
        /// <param name="language">Language to be set</param>
        private void UpdateLanguageStrings(string language)
        {
            LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo(language);
        }
    }
}
