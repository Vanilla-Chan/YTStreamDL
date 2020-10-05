using System;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
namespace StreamDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            String videoId="";
            String url="";
            String channelId = "";
            Console.WriteLine("1:Channel");
            Console.WriteLine("2:Link");
            Console.Write("Eingabe: ");
            if (Console.ReadLine()=="1")
            {
                Console.WriteLine("Welcome to Vanillas bad stream downloader.");
                Console.WriteLine("Please enter the Channel link. bsp https://www.youtube.com/channel/UCP0BspO_AMEe3aQqqpo89Dg");
                Console.Write("Link: ");
                channelId = Console.ReadLine();

                while (IsStreaming(channelId.Replace("https://www.youtube.com/channel/","")) == "false")
                {
                    Thread.Sleep(20000);
                }
                videoId = IsStreaming(channelId.Replace("https://www.youtube.com/channel/", ""));
                url = "https://www.youtube.com/watch?v=" + videoId;

            }
            else
            {
                Console.WriteLine("Welcome to Vanillas bad stream downloader.");
                Console.WriteLine("Please enter the YoutubeLink. bsp https://www.youtube.com/watch?v=_HYTbLF_5eI");
                Console.Write("Link: ");
                url = Console.ReadLine();
                videoId = url.Substring(url.IndexOf("v=") + 2);
            }


            while (IsOnline(videoId) == false)
            {
                Thread.Sleep(20000);
            }
            Process process = new Process();
            process.StartInfo.FileName = "youtube-dl.exe";
            process.StartInfo.Arguments = url;
            process.Start();
        }
        static string IsStreaming(string channelId)
        {
            WebClient client = new WebClient();
            dynamic streamJson = JsonConvert.DeserializeObject(client.DownloadString("https://www.googleapis.com/youtube/v3/search?part=snippet&channelId="+ channelId + "&type=video&eventType=live&key=AIzaSyBAFT3fhyqKnA-5O30ePaWu2dZCtFCLyKw"));
            if (streamJson.pageInfo.totalResults != 0)
            {
                return streamJson.items[0].id.videoId;
            }
            return "false";

        }
        static bool IsOnline(string videoId)
        {
            WebClient client = new WebClient();
            dynamic streamJson = JsonConvert.DeserializeObject(client.DownloadString("https://www.googleapis.com/youtube/v3/videos?id=" + videoId + "&part=snippet,liveStreamingDetails&key=AIzaSyBAFT3fhyqKnA-5O30ePaWu2dZCtFCLyKw"));
            Console.WriteLine(streamJson.items[0].snippet.liveBroadcastContent);
            if (streamJson.items[0].snippet.liveBroadcastContent == "live")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
