using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using piwebapi_cs_helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace piwebapi_cs_wpf_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /* This method takes the username and password specified to use basic authentication to connect to 
         * PI Web API. It then attempts to resolve the tag path provided and write to the tag. */
        private async void writeBtn_Click(object sender, RoutedEventArgs e)
        {
            string baseUrl = "https://dng-code.osisoft.int/piwebapi"; //change the baseUrl to your PI Web API service
            string userName = userNameTextBox.Text;
            string password = pwBox.Password;
            string tagPath = tagTextBox.Text;
            PIWebAPIClient piWebAPIClient = new PIWebAPIClient(userName, password);

            try
            {
                //Resolve tag path
                string requestUrl = baseUrl + "/points/?path=" + tagPath;
                Task<JObject> tget = piWebAPIClient.GetAsync(requestUrl);
                statusTextBlock.Text = "Processing...";

                //Attempt to write value to the tag
                Object payload = new
                {
                    value = valueTextBox.Text
                };
                string data = JsonConvert.SerializeObject(payload);
                JObject jobj = await tget;
                await piWebAPIClient.PostAsync(jobj["Links"]["Value"].ToString(), data);

                //Display final results if successful
                statusTextBlock.Text = "Write success!";
            }
            catch (HttpRequestException ex)
            {
                statusTextBlock.Text = ex.Message;
            }
            catch (Exception ex)
            {
                statusTextBlock.Text = ex.Message;
            }
            finally
            {
                //We are closing the HttpClient after every write in this simple example. This is not necessary.
                piWebAPIClient.Dispose();
            }
        }
    }
}
