using System;
using System.Collections.Generic;
using System.Text;

namespace LCDInfo {
    public class Weather {
        public static string WeatherConditions="N/A";
        static System.Random rnd = new Random();
        public static string RSSUrl = @"http://www.rssweather.com/rss.php?config=&forecast=zandh&place=vancouver&state=bc&zipcode=&country=ca&county=&zone=fpcn11.cwvr--greater%20vancouver:fpcn54.cwvr--greater%20vancouver&alt=rss20a";
        static System.Threading.Timer WeatherTimer;

        static Weather() {
            WeatherTimer = new System.Threading.Timer(new System.Threading.TimerCallback(UpdateWeather), null, 1000, 3600000);
        }
        private static void UpdateWeather(object state) {
            Unified.Rss.RssFeed f = Unified.Rss.RssFeed.Read(RSSUrl);
            lock (WeatherConditions) {
                Weather.WeatherConditions = f.Channels[0].Items[0].Description;
            }
        }

    }
}
