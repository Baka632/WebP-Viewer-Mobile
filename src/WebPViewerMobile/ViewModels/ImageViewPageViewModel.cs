using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Universal.WebP;
using WebPViewerMobile.Helpers;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace WebPViewerMobile.ViewModels
{
    public class ImageViewPageViewModel : NotificationObject
    {
        private ObservableCollection<BitmapSource> bitmapSources;
        private bool _isLoading = true;
        private bool isImageAvailable = true;
        private string _errorMessage = string.Empty;

        public ImageViewPageViewModel()
        {
            bitmapSources = new ObservableCollection<BitmapSource>();
        }

        public bool IsImageAvailable
        {
            get => isImageAvailable;
            set
            {
                isImageAvailable = value;
                OnPropertiesChanged();
            }
        }

        public ObservableCollection<BitmapSource> ImageSources
        {
            get => bitmapSources;
            set
            {
                bitmapSources = value;
                OnPropertiesChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;
                OnPropertiesChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                _errorMessage = value;
                OnPropertiesChanged();
            }
        }

        public async Task PrepareFiles(IReadOnlyList<IStorageItem> items)
        {
            //假设图像是可用的
            IsImageAvailable = true;

            foreach (IStorageItem item in items)
            {
                await PrepareFile(item);
            }

            IsImageAvailable = ImageSources.Any();
        }

        public async Task PrepareFile(IStorageItem item)
        {
            IsLoading = true;

            if (item.IsOfType(StorageItemTypes.File))
            {
                StorageFile file = item as StorageFile;
                try
                {
                    WriteableBitmap bitmap = await GetBitmap(file);
                    ImageSources.Add(bitmap);
                }
                catch
                {
                    ErrorMessage = ReswHelper.GetReswString("NotSupportedAnimatedWebP.Text");
                }
            }
            else if (item.IsOfType(StorageItemTypes.Folder))
            {
                StorageFolder folder = item as StorageFolder;

                IReadOnlyList<StorageFile> files = await folder.GetFilesAsync(CommonFileQuery.DefaultQuery);
                foreach (StorageFile file in files)
                {
                    try
                    {
                        WriteableBitmap bitmap = await GetBitmap(file);
                        ImageSources.Add(bitmap);
                    }
                    catch
                    {
                        ErrorMessage = ReswHelper.GetReswString("NotSupportedAnimatedWebP.Text");
                    }
                }
            }

            IsLoading = false;
        }

        private async Task<WriteableBitmap> GetBitmap(StorageFile file)
        {
            Stream stream = await file.OpenStreamForReadAsync();
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            byte[] bytes = memoryStream.ToArray();

            WebPDecoder webp = new WebPDecoder();

            var pixelData = (await webp.DecodeBgraAsync(bytes)).ToArray();
            var size = await webp.GetSizeAsync(bytes);
            var bitmap = new WriteableBitmap((int)size.Width, (int)size.Height);
            await bitmap.PixelBuffer.AsStream().WriteAsync(pixelData, 0, pixelData.Length);
            return bitmap;
        }
    }
}
