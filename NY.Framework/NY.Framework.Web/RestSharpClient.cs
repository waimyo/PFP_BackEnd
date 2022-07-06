using RestSharp;
using NY.Framework.Infrastructure;
using RestSharp.Authenticators;
using NY.Framework.Web.Models;
using static NY.Framework.Web.Models.OperatorApiResponseViewModel;
using System.Net;
using Newtonsoft.Json;
using System.Web;

namespace NY.Framework.Web
{
    public class RestSharpClient
    {
        public IRestResponse SendSms(string _operator, string phonenumber, string smsshortcode, string sms)
        {
            IRestResponse restResponse = new RestResponse();
            string phno = "";
            if (_operator.Equals("telenor1111") || _operator.Equals("mpt1111") ||
                _operator.Equals("mytel1111"))
            {
                phno = string.Concat("95", phonenumber.Remove(0, 1));
            }
            else
            {
                phno = phonenumber;
            }
            if (_operator.Equals("mytel1111"))
            {
                restResponse = SendMytelSms(smsshortcode, phno, sms);
            }
            if (_operator.Equals("ooredoo1111"))
            {
                restResponse =SendOoredooSms(smsshortcode, phno, sms);
            }
            if (_operator.Equals("mpt1111"))
            {
                //MobileRegularExpression regularExpCls = new MobileRegularExpression();
                //if (regularExpCls.CheckMPTLength(phno))
                //{
                restResponse = SendMptOrTelenorSms(smsshortcode, phno, sms, Constants.MptUserName, Constants.MptPassword);
            //}
            }
            if (_operator.Equals("telenor1111"))
            {
                restResponse = SendMptOrTelenorSms(smsshortcode, phno, sms,Constants.TelenorUserName,Constants.TelenorPassword);
            }

            return restResponse;
        }

        public IRestResponse SendMytelSms(string from,string to,string msg)
        {
            RestClient client = new RestClient(Constants.MytelApiUrl);
            client.Authenticator = new HttpBasicAuthenticator(Constants.MytelUserName,Constants.MytelPassword);
            RestRequest request = new RestRequest("",Method.POST);
            var data = new { address = from, content =msg, number = to };
            //convert object to json,set content-type to application/json
            request.AddJsonBody(data);
            IRestResponse restSharpResponse = client.Execute(request);
            MytelApiResponseViewModel mytelApiResponse = JsonConvert.DeserializeObject<MytelApiResponseViewModel>(restSharpResponse.Content);
            if(mytelApiResponse != null &&!string.IsNullOrEmpty(mytelApiResponse.message) && mytelApiResponse.message.Equals("SUCCESS"))
            {
                restSharpResponse.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                restSharpResponse.StatusCode = 0;
            }
            return restSharpResponse;
            //response.ErrorException;
            // response.ErrorMessage;
            //response.StatusCode;       
            //response.Data.address
            // var response = client.Execute<MytelApiResponseViewModel>(request);

        }

        public IRestResponse SendOoredooSms(string from, string to, string msg)
        {
            RestClient client = new RestClient(Constants.OoredooApiUrl);
            // client.Authenticator = new JwtAuthenticator(Constants.OoredooApiKey);
            RestRequest request = new RestRequest("", Method.POST);     
            
            UnicodeTools unicodetool = new UnicodeTools();
            string encodestr = unicodetool.Encode(msg);

            request.AddParameter("numbers", to);
            request.AddParameter("sender", from);
            request.AddParameter("message", HttpUtility.UrlEncode("@U"+encodestr));
            request.AddParameter("apikey", Constants.OoredooApiKey);

            //var data = new { numbers = to, sender = from, message = msg, apikey =Constants.OoredooApiKey };
            //request.AddJsonBody(data);
            IRestResponse restSharpResponse = client.Execute(request);
            OoredooApiResponseViewModel ooredooApiResponse=JsonConvert.DeserializeObject<OoredooApiResponseViewModel>(restSharpResponse.Content);
            if (ooredooApiResponse!=null && !string.IsNullOrEmpty(ooredooApiResponse.status) && ooredooApiResponse.status.Equals("success"))
            {
                restSharpResponse.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                restSharpResponse.StatusCode = 0;
            }
            return restSharpResponse;
        }

        public IRestResponse SendMptOrTelenorSms(string _from,string _to,string msg,string user,string _password)
        {        
            RestClient client = new RestClient(Constants.MptTelenorApiUrl);
            RestRequest request = new RestRequest("",Method.POST);

            UnicodeTools unicodetool = new UnicodeTools();
            string encodestr=unicodetool.Encode(msg);

            request.AddParameter("coding", 8);
            request.AddParameter("to", _to);
            request.AddParameter("from", _from);
            request.AddParameter("hex-content", HttpUtility.UrlEncode(encodestr));
            request.AddParameter("username", user);
            request.AddParameter("password", _password);

            IRestResponse restSharpResponse = client.Execute(request);
            if (restSharpResponse != null && restSharpResponse.Content != null && restSharpResponse.Content.Contains("Success"))
            {
                restSharpResponse.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                restSharpResponse.StatusCode = 0;
            }
            return restSharpResponse;
        }
       
    }
}
