using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LCDInfo {
    public class Weather {
        private static string _weatherConditions = "N/A";
        private static Random _rnd = new Random();
        private static string _RSSUrl = @"http://www.rssweather.com/rss.php?config=&forecast=zandh&place=vancouver&state=bc&zipcode=&country=ca&county=&zone=fpcn11.cwvr--greater%20vancouver:fpcn54.cwvr--greater%20vancouver&alt=rss20a";
        private static Timer _weatherTimer;

        static Weather()
        {
            _weatherTimer = new Timer(new System.Threading.TimerCallback(UpdateWeather), null, 1000, 3600000);
        }

        private static void UpdateWeather(object state) {
            Unified.Rss.RssFeed f = Unified.Rss.RssFeed.Read(_RSSUrl);
            lock (_weatherConditions) {
                Weather._weatherConditions = f.Channels[0].Items[0].Description;
            }
        }
    }
}
