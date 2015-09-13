using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Roamr
{

    public class city_data
    {
        public int serial { get; set; }
        public string cityId_lat { get; set; }
        public string cityId_long { get; set; }
        public AdByLocation ads { get; set; }
    }
    public class cities
    {
        public cities()
        {
            this.Items = new List<city_data>();
        }
        public List<city_data> Items { get; set; }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AdByLocation allData = new AdByLocation();
        int resPerPage = 0;
        cities my_list = new cities();
        
        public MainPage()
        {
            this.InitializeComponent();
            my_list.Items.Add(new city_data() { serial = 0, cityId_lat = null, cityId_long = null, ads = new AdByLocation() });
            my_list.Items.Add(new city_data() { serial = 1, cityId_lat = null, cityId_long = null, ads = new AdByLocation() });
            my_list.Items.Add(new city_data() { serial = 2, cityId_lat = null, cityId_long = null, ads = new AdByLocation() });
            my_list.Items.Add(new city_data() { serial = 3, cityId_lat = null, cityId_long = null, ads = new AdByLocation() });
            my_list.Items.Add(new city_data() { serial = 4, cityId_lat = null, cityId_long = null, ads = new AdByLocation() });

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            

            //getToken();

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            OptionsPanel.Visibility = Visibility.Collapsed;
            Display.Visibility = Visibility.Collapsed;

            allData.AdsByLocationResponse = new AdsByLocationResponse();
            allData.AdsByLocationResponse.AdsByLocationData = new AdsByLocationData();
            allData.AdsByLocationResponse.AdsByLocationData.docs = new List<Doc>();

            int flag = 0;
            if (resPerPage != 0)
                foreach (city_data cd in my_list.Items)
                    if(cd.cityId_lat != null && cd.cityId_long != null)
                    {
                        await fetchQuikly(cd.cityId_lat, cd.cityId_long, cd.serial);
                        flag = 1;
                    }
             
            if (flag==0)
            {
                MessageDialog msgbox = new MessageDialog("Please select cities and no. of results");
                await msgbox.ShowAsync();
                return;
            }



            for (int i = 0; i < resPerPage; i++)
                foreach (city_data cd in my_list.Items)
                    if (cd.ads.AdsByLocationResponse != null)
                        allData.AdsByLocationResponse.AdsByLocationData.docs.Add(cd.ads.AdsByLocationResponse.AdsByLocationData.docs.ElementAt(i));
            Display.DataContext = allData.AdsByLocationResponse.AdsByLocationData;

            Display.Visibility = Visibility.Visible;
            //await waitToLoad();
            //string QuikrApiUri = "https://api.quikr.com/public/adsByLocation?lat=28.64649963&lon=77.22570038&from=0&size=1";
            //HttpWebRequest request =  (HttpWebRequest)HttpWebRequest.Create(QuikrApiUri);
            ////request.Method = "GET";
            ////request.ContentType = "application/json";
            //request.Headers["X-Quikr-App-Id"] = "533";
            //request.Headers["X-Quikr-Token-Id"] = "3033368";
            //request.Headers["X-Quikr-Signature"] = "4f5c4fdc1704d6f1aee5e67eeee08ac26e17eabf";
            //await request.BeginGetResponse(GetDataCallback, request);

            //await 
            //Butn1.Content = "Data Received";
        }

        async private Task fetchQuikly(string lati,string longi,int serial)
        {

            //string QuikrApiUri = string.Format("https://api.quikr.com/public/adsByCategory?categoryId={0}&city={1}&from=0&size={2}", catID, cityId, resPerPage);
            string QuikrApiUri = string.Format("https://api.quikr.com/public/adsByLocation?lat={0}&lon={1}&from=0&size={2}",lati,longi,resPerPage);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(QuikrApiUri);
            //request.Method = "GET";
            //request.ContentType = "application/json";
            request.Headers["X-Quikr-App-Id"] = "533";
            request.Headers["X-Quikr-Token-Id"] = "3033368";
            request.Headers["X-Quikr-Signature"] = "4f5c4fdc1704d6f1aee5e67eeee08ac26e17eabf";
            Task<WebResponse> tres = request.GetResponseAsync();

            Butn1.Content = "Retrieving Data..";
            Butn1.IsEnabled = false;
            Options.IsEnabled = false;
            

            WebResponse res = await tres;
            Stream ReceiveStream = res.GetResponseStream();
            Encoding encode = Encoding.GetEncoding("utf-8");
            StreamReader readStream = new StreamReader(ReceiveStream, encode);
            string data = readStream.ReadToEnd();

            AdByLocation test1 = new AdByLocation();
            test1 = JsonConvert.DeserializeObject<AdByLocation>(data);
            my_list.Items.ElementAt(serial).ads = test1;

            Butn1.Content = "Shop around";
            Butn1.IsEnabled = true;
            Options.IsEnabled = true;

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(OptionsPanel.Visibility == Visibility.Collapsed)
            {
                OptionsPanel.Visibility = Visibility.Visible;
                Display.Visibility = Visibility.Collapsed;
            }
            else
            {
                OptionsPanel.Visibility = Visibility.Collapsed;
                Display.Visibility = Visibility.Visible;
            }
        }
        

        async private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock sp = (TextBlock)e.OriginalSource;
            await Windows.System.Launcher.LaunchUriAsync(new Uri(sp.Tag.ToString(), UriKind.Absolute));
        }

        /*async*/ private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = (ComboBox)sender;
            if(((ComboBoxItem)cbb.SelectedItem).Tag == null)
            {
                my_list.Items.ElementAt(int.Parse(cbb.Tag.ToString()) - 1).cityId_lat = null;
                my_list.Items.ElementAt(int.Parse(cbb.Tag.ToString()) - 1).cityId_long = null;
            }
            else
            {
                my_list.Items.ElementAt(int.Parse(cbb.Tag.ToString()) - 1).cityId_lat = ((ComboBoxItem)cbb.SelectedItem).Tag.ToString().Remove(8, 10);
                my_list.Items.ElementAt(int.Parse(cbb.Tag.ToString()) - 1).cityId_long = ((ComboBoxItem)cbb.SelectedItem).Tag.ToString().Remove(0, 10);
            }
            
            //MessageDialog msgbox = new MessageDialog(((ComboBox)sender).Tag.ToString());            
            //await msgbox.ShowAsync();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = (ComboBox)sender;
            resPerPage = int.Parse(((ComboBoxItem)cbb.SelectedItem).Content.ToString());
        }

        //async private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ComboBox cbb = (ComboBox)e.OriginalSource;
        //    ComboBoxItem cbi = (ComboBoxItem)cbb.SelectedItem;
        //    MessageDialog msgbox = new MessageDialog(cbi.Content.ToString());
        //    //Calling the Show method of MessageDialog class  
        //    //which will show the MessageBox  
        //    await msgbox.ShowAsync();
        //}



        //async Task waitToLoad()
        //{
        //    string QuikrApiUri = "https://api.quikr.com/public/adsByLocation?lat=28.64649963&lon=77.22570038&from=0&size=1";
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(QuikrApiUri);
        //    //request.Method = "GET";
        //    //request.ContentType = "application/json";
        //    request.Headers["X-Quikr-App-Id"] = "533";
        //    request.Headers["X-Quikr-Token-Id"] = "3033368";
        //    request.Headers["X-Quikr-Signature"] = "4f5c4fdc1704d6f1aee5e67eeee08ac26e17eabf";
        //    Task<WebResponse> tres = request.GetResponseAsync();

        //    WebResponse res = await tres;
        //    Stream ReceiveStream = res.GetResponseStream();
        //    Encoding encode = Encoding.GetEncoding("utf-8");
        //    StreamReader readStream = new StreamReader(ReceiveStream, encode);
        //    string data = readStream.ReadToEnd();
        //    test1 = JsonConvert.DeserializeObject<AdByLocation>(data);
        //    Butn1.Content = "Data Received";
        //}

        //async private void getToken()
        //{
        //    string QuikrApiUri = "https://api.quikr.com/app/auth/access_token";
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(QuikrApiUri);
        //    request.Method = "POST";
        //    request.ContentType = "application/json";
        //    using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream,
        //                                                 request.EndGetRequestStream, null))
        //    {
        //        //create some json string
        //        string json = "{ \"appId\" : \"533\", \"signature\" : \"fcbd9d287a11548314eb67ed8ea2fcc1b2577cf4\"}";

        //        // convert json to byte array
        //        byte[] jsonAsBytes = Encoding.UTF8.GetBytes(json);

        //        // Write the bytes to the stream
        //        await stream.WriteAsync(jsonAsBytes, 0, jsonAsBytes.Length);
        //    }
        //    request.BeginGetResponse(GetDataCallback1, request);
        //}

        //void GetDataCallback1(IAsyncResult result)
        //{
        //    HttpWebRequest request = result.AsyncState as HttpWebRequest;
        //    if (request != null)
        //    {
        //        try
        //        {
        //            WebResponse response = request.EndGetResponse(result);
        //            Stream ReceiveStream = response.GetResponseStream();
        //            Encoding encode = Encoding.GetEncoding("utf-8");
        //            StreamReader readStream = new StreamReader(ReceiveStream, encode);
        //            string data = readStream.ReadToEnd();
        //            Token t = JsonConvert.DeserializeObject<Token>(data);
        //            TokID = t.Token_Id;
        //        }
        //        catch (WebException e)
        //        {
        //            return;
        //        }
        //    }
        //}

        //async Task GetDataCallback(IAsyncResult result)
        //{
        //    HttpWebRequest request = result.AsyncState as HttpWebRequest;
        //    if (request != null)
        //    {
        //        try
        //        {
        //            WebResponse response = request.EndGetResponse(result);
        //            Stream ReceiveStream = response.GetResponseStream();
        //            Encoding encode = Encoding.GetEncoding("utf-8");
        //            StreamReader readStream = new StreamReader(ReceiveStream, encode);
        //            string data = readStream.ReadToEnd();
        //            test1 = JsonConvert.DeserializeObject<AdByLocation>(data);
        //        }
        //        catch (WebException e)
        //        {
        //            return;
        //        }
        //    }
        //}
    }
}
