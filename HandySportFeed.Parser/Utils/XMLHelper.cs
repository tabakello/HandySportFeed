using System;
using System.IO;
using System.Net;

namespace HandySportFeed.Parsers.Utils {
    public class XmlHelper {
        public static string GetXmlStringByUrl(string url) {
            var webRequest = (HttpWebRequest) WebRequest.Create(url);
            var webResponse = (HttpWebResponse) webRequest.GetResponse();
            var stream = webResponse.GetResponseStream();
            if (stream == null) {
                throw new Exception("stream is null");
            }

            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}