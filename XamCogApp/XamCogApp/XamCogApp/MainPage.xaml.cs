using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace XamCogApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void selectPicture()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var image = await CrossMedia.Current.PickPhotoAsync();
                var stream = image.GetStream();
                SelectedImage.Source = ImageSource.FromStream(() =>
                {
                    return stream;
                });
                var result = await GetImageDescription(image.GetStream());
                image.Dispose();
                foreach (string tag in result.Description.Tags)
                {
                    InfoLabel.Text = InfoLabel.Text + "\n" + tag;
                }
            }
        }

        public async Task<AnalysisResult> GetImageDescription(Stream imageStream)
        {
            VisionServiceClient visionClient = new VisionServiceClient("<<YOUR API KEY HERE>>");
            VisualFeature[] features = { VisualFeature.Tags, VisualFeature.Categories, VisualFeature.Description };
            return await visionClient.AnalyzeImageAsync(imageStream, features.ToList(), null);
        }


        void Handle_Clicked(object sender, System.EventArgs e)
        {
            selectPicture();
        }
    }
}
