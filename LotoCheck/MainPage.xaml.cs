using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.IO;

namespace LotoCheck
{
    public partial class MainPage : ContentPage
    {
        const string subscriptionKey = "9720e509590b47f99ffde9c21c87b760";

        const string uriBase =
            "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/ocr";


        public MainPage()
        {
            InitializeComponent();
        }




        static async Task MakeOCRRequest(Stream imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Read the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n\n{0}\n",
                    JToken.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageStream">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(Stream imageStream)
        {
            // Read the file's contents into a byte array.
            BinaryReader binaryReader = new BinaryReader(imageStream);
            return binaryReader.ReadBytes((int)imageStream.Length);
        }


        private void Limpar_Clicked(object sender, EventArgs e)
        {
            lblResult.Text = string.Empty;
            imgScanned.Source = null;
        }

        private async void RealOcr_Clicked(object sender, EventArgs e)
        {
            OcrResults text1;
            var media = Plugin.Media.CrossMedia.Current;
            await media.Initialize();
            var file = await media.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 40,
                SaveToAlbum = false
            });
            imgScanned.Source = ImageSource.FromStream(() => file.GetStream());

            await MakeOCRRequest(file.GetStream());



            /* var visionclient = new VisionServiceClient(subscriptionKey, uriBase);
             text1 = await visionclient.RecognizeTextAsync(file.GetStream());
             foreach (var region in text1.Regions)
             {
                 foreach (var line in region.Lines)
                 {
                     if(line.Words.Length == 5)
                     {

                     }
                     foreach (var word in line.Words)
                     {
                         lblResult.Text += Convert.ToString(word.Text) + " ";
                     }
                     lblResult.Text += "\n";
                 }
             }
             */
        }

        private async void ScanImage(string txt)
        {
            /*AnalysisResult analysisResult;
            OcrResults ocr;
            var features = new VisualFeature[] { VisualFeature.Tags, VisualFeature.Description, VisualFeature.Categories };
            string result = string.Empty;

            foreach (var region in ocrR.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        result += Convert.ToString(word.Text);
                    }
                }
            }

             * using (var fs = new FileStream(@"C:\Vision\Sample.jpg", FileMode.Open))
                {
                  analysisResult = await visionClient.AnalyzeImageAsync(fs, features);
                }


            var get = analysisResult.Description.Captions;
            */

            //ScanImage("http://www.e-farsas.com/wp-content/uploads/aposta_mega_sena.jpg");
        }
    }
}
