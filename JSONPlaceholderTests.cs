using HTWebServices.JSONMappers;
using Newtonsoft.Json;
using System.Net;

namespace HTWebServices
{
    public class JSONPlaceholderTests
    {
        string apiURL;
        HttpWebRequest httpWebRequest;
        HttpWebResponse response;

        [SetUp]
        public void Setup()
        {
            apiURL = "https://jsonplaceholder.typicode.com/users";

            httpWebRequest = (HttpWebRequest)WebRequest.Create(apiURL);

            httpWebRequest.Method = "GET";

            response = (HttpWebResponse)httpWebRequest.GetResponse();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void Verify_StatusCode()
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Verify_ResponseHeader()
        {
            string [] headers = response.Headers.AllKeys;
            bool isContentTypeHeaderPresent = false;
            int contentTypeHeaderIndex = 0;
            for(int i =0; i<headers.Length; i++)
            {
                if (headers[i].Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                {
                    isContentTypeHeaderPresent = true;
                    contentTypeHeaderIndex = i;
                    break;
                }
            }
            Assert.That(isContentTypeHeaderPresent, Is.True);

            string contentTypeHeaderValue = response.Headers[contentTypeHeaderIndex];
            Assert.That(contentTypeHeaderValue, Is.EqualTo("application/json; charset=utf-8"));
        }

        [Test]
        public void Verify_ResponseBody()
        {
            string responseBody;

            Stream stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                responseBody = reader.ReadToEnd();
            }

            List<User> usersList = JsonConvert.DeserializeObject<List<User>>(responseBody);
            Assert.That(usersList.Count, Is.EqualTo(10));
        }
    }
}