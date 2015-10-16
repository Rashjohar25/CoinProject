using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoinsProject
{
    public class Currency
    {
        public int id { get; set; }
        public String name { get; set; }
        public String code { get; set; }
        public String maintenance { get; set; }
    }
    public class Last_trade
    {
        public String price { get; set; }
        public String date { get; set; }
        public String timestamp { get; set; }
    }
    public class _24HR{
        public String volume { get; set; }
        public String volume_btc { get; set; }
        public String price_high { get; set; }
        public String price_low { get; set; }
    }
    public class Market
    {
        public int id { get; set; }
        public String label{get;set;}
        public int coin_currency_id { get; set; }
        public int market_currency_id { get; set; }
        public int maintenance_mode { get; set; }
        public Boolean verifiedonly { get; set; }
        public virtual _24HR _24hr { get; set; }
        public virtual Last_trade last_trade { get; set; }
    }
    public class Sellorders
    {
        public String price { get; set; }
        public String quantity { get; set; }
        public String total { get; set; }
    }
    public class BuyOrders
    {
        public String price { get; set; }
        public String quantity { get; set; }
        public String total { get; set; }
    }
    public class Balances
    {
        public JToken held { get; set; }
        public JToken available { get; set; }
    }
    public class Order
    {
        public Sellorders sellorders { get; set; }
        public BuyOrders buyorders { get; set; }
    }
    public class AllHttpRequests
    {
        public String nonce { get; set; }
        public String PrivateKey { get; set; }
        public String PublicKey { get; set; }
        private string limit = "&limit=100";
        private HttpClient client;
        private EventLog eventlog1 { get; set; }
        private List<Currency> allAvailableCurrencies = new List<Currency>();
        private Balances allBalances = new Balances();
        private ListBox lv { get; set; }



        
        public AllHttpRequests(EventLog eventlog1,ListBox consolebox)
        {
            this.eventlog1 = eventlog1;
            eventlog1.WriteEntry("Initializing All data for the HttpRequests.");
            System.Net.ServicePointManager.Expect100Continue = false;
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.cryptsy.com/");
            lv = consolebox;
        }
        public List<Currency> AllAvailableCurrency
        {
            get
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                eventlog1.WriteEntry("Making a Request to get Available Currencies on host: " + client.BaseAddress.ToString() + "api/v2/currencies");
                HttpResponseMessage response = client.GetAsync("api/v2/currencies").Result;
                if(IsSuccessFullRequest(response))
                {
                    JToken obj = ReadSuccessfullContent(response);
                    allAvailableCurrencies = JsonConvert.DeserializeObject<List<Currency>>(obj.ToString());
                    eventlog1.WriteEntry("All Currencies receivied successfully with " + allAvailableCurrencies.Count + " Items.");
                }
                else
                {
                    eventlog1.WriteEntry("Failed to load all Currencies because of JsonParsing");
                }
                return allAvailableCurrencies;
            }
        }
        public Balances AllBalance
        {
            get
            {
                nonce = DateTime.Now.Millisecond.ToString();
                String msg = "nonce=" + nonce + limit;
                String hmac_balance = EncryptData(msg);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("key", PublicKey);
                client.DefaultRequestHeaders.Add("sign", hmac_balance);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                eventlog1.WriteEntry("Making a Request to get Balance on host: " + client.BaseAddress.ToString() + "api/v2/balances?"+msg);
                HttpResponseMessage response = client.GetAsync("api/v2/balances?"+msg).Result;
                if (IsSuccessFullRequest(response))
                {
                    JToken obj = ReadSuccessfullContent(response);
                    allBalances.available = obj["available"];
                    allBalances.held = obj["held"];
                    eventlog1.WriteEntry("Available Balance: "+allBalances.available.ToString());
                    eventlog1.WriteEntry("Held Balance: " + allBalances.held.ToString());
                }
                else
                {
                    eventlog1.WriteEntry("Failed to load all Currencies because of JsonParsing");
                }
                return allBalances;
            }
        }
        







        public Boolean Withdraw(String balance,String address,String coinid)
        {
            Boolean IsSuccess=false;
            String noncet = DateTime.Now.Millisecond.ToString();
            eventlog1.WriteEntry("start withdrawing from the user.");
            String msg = "nonce=" + noncet + limit + "&quantity=" + balance + "&address=" + address + "&notificationtoken=token";
            String hmac_withdraw = EncryptData(msg);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("key", PublicKey);
            client.DefaultRequestHeaders.Add("sign", hmac_withdraw);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            eventlog1.WriteEntry("Making the Request to the https://api.cryptsy.com/api/v2/withdraw/.");
            HttpResponseMessage newresponse = client.GetAsync(new Uri("https://api.cryptsy.com/api/v2/withdraw/" + coinid + "?" + msg)).Result;
            if(IsSuccessFullRequest(newresponse))
            {
                JToken data = ReadSuccessfullContent(newresponse);
                eventlog1.WriteEntry(data.ToString());
            }
            else
            {
                eventlog1.WriteEntry("Withdrawl of the selected currency failed.");
            }
            return IsSuccess;
        }
        public Boolean PlaceOrder(String exchangefrom,String exchangeto,String balance,Boolean isSellOrder)
        {
            String noncer = DateTime.Now.Millisecond.ToString();
            Market market = GetMarket(exchangefrom,exchangeto,0);
            Order order = GetMarketOrders(market.id.ToString());
            String msg;
            if (order != null)
            {
                if (isSellOrder)
                {
                    eventlog1.WriteEntry("Exhange from " + exchangefrom + " to " + exchangeto + " of balance " + balance + " using market " + market.id + " at sell order price of " + order.sellorders.price + " has been initiated.");
                    msg = "nonce=" + noncer + "&quantity=" + balance + "&ordertype=sell&marketid=" + market.id + "&price=" + order.sellorders.price;
                }
                else
                {
                    eventlog1.WriteEntry("Exhange from " + exchangefrom + " to " + exchangeto + " of balance " + balance + " using market " + market.id + " at buy order price of " + order.buyorders.price + " has been initiated.");
                    msg = "nonce=" + noncer + "&quantity=" + balance + "&ordertype=buy&marketid=" + market.id.ToString() + "&price=" + order.buyorders.price;
                }
                String hmac_withdraw = EncryptData(msg);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("key", PublicKey);
                client.DefaultRequestHeaders.Add("sign", hmac_withdraw);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                eventlog1.WriteEntry("Making the Request to the https://api.cryptsy.com/api/v2/order?" + msg);
                List<KeyValuePair<String,String>> param=new List<KeyValuePair<string,string>>();
                param.Add(new KeyValuePair<String,String>("nonce","noncer"));
                param.Add(new KeyValuePair<String,String>("quantity",balance));

                
                param.Add(new KeyValuePair<String,String>("marketid",market.id.ToString()));
                
                if(isSellOrder)
                {
                    param.Add(new KeyValuePair<String,String>("ordertype","sell"));
                    param.Add(new KeyValuePair<String,String>("price",order.sellorders.price));
                }
                else
                {
                    param.Add(new KeyValuePair<String,String>("ordertype","buy"));
                    param.Add(new KeyValuePair<String,String>("price",order.buyorders.price));
                }
                HttpContent content=new FormUrlEncodedContent(param);
                HttpResponseMessage newresponse = client.PostAsync(new Uri("https://api.cryptsy.com/api/v2/order?" + msg),content).Result;
                if (IsSuccessFullRequest(newresponse))
                {
                    JToken data = ReadSuccessfullContent(newresponse);
                    if (isSellOrder)
                    {
                        eventlog1.WriteEntry("Sell Order Placed Successfully");
                        eventlog1.WriteEntry(data.ToString());
                    }
                    else
                    {
                        eventlog1.WriteEntry("Buy Order Placed Successfully");
                        eventlog1.WriteEntry(data.ToString());
                    }
                    return true;
                }
                else
                {
                    if (isSellOrder)
                    {
                        eventlog1.WriteEntry("Sell Order Placement Failed.");
                    }
                    else
                    {
                        eventlog1.WriteEntry("Buy Order placement Failed.");
                    }
                    return false;
                }
            }
            else
            {
                eventlog1.WriteEntry("Order is Null.");
                return false;
            }
        }
        private Order GetMarketOrders(String marketId)
        {
            nonce = DateTime.Now.Millisecond.ToString();
            String message = "nonce=" + nonce + "&method=marketorders&marketid=" + marketId;
            client.DefaultRequestHeaders.Accept.Clear();
            String hmac_withdraw = EncryptData(message);
            client.DefaultRequestHeaders.Add("key", PublicKey);
            client.DefaultRequestHeaders.Add("sign", hmac_withdraw);
            //List<KeyValuePair<String, String>> param = new List<KeyValuePair<string, string>>();
            //param.Add(new KeyValuePair<string, string>("nonce", nonce));
            //param.Add(new KeyValuePair<string, string>("method", "marketorders"));
            //param.Add(new KeyValuePair<string, string>("marketid", marketId));
            //HttpContent content = new FormUrlEncodedContent(param);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            HttpResponseMessage response = client.GetAsync(new Uri("https://api.cryptsy.com/api/v2/markets/"+marketId+"/orderbook")).Result;
            if (IsSuccessFullRequest(response))
            {
                JToken data = ReadSuccessfullContent(response);
                //JObject obj = JObject.Parse(response.Content.ReadAsStringAsync().Result.ToString());
                Order order = new Order();
                JToken selltoken = data["sellorders"][0];
                JToken buytoken = data["buyorders"][0];
                eventlog1.WriteEntry("Market Sell Data is: " + selltoken.ToString());
                eventlog1.WriteEntry("Market Buy Data is: " + buytoken.ToString());
                order.sellorders = JsonConvert.DeserializeObject<Sellorders>(selltoken.ToString());
                order.buyorders = JsonConvert.DeserializeObject<BuyOrders>(buytoken.ToString());
                return order;
            }
            else
            {
                return null;
            }
        }
        private Market GetMarket(string from, string to, int trial)
        {
            eventlog1.WriteEntry("start geting Market Id.");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            eventlog1.WriteEntry("Making the Request to the https://api.cryptsy.com/api/v2/markets/" + from + "_" + to);
            HttpResponseMessage newresponse = client.GetAsync(new Uri("https://api.cryptsy.com/api/v2/markets/" + from + "_" + to)).Result;
            if(IsSuccessFullRequest(newresponse))
            {
                eventlog1.WriteEntry("Market Id Found for "+from+"_"+to);
                JToken data = ReadSuccessfullContent(newresponse);
                eventlog1.WriteEntry("Market Id is: "+data.ToString());
                Market market = JsonConvert.DeserializeObject<Market>(data.ToString());
                return market;
            }
            else
            {
                if (trial == 0)
                {
                    eventlog1.WriteEntry("Market Id Not Found for " + from + "_" + to);
                    eventlog1.WriteEntry("Searching Market Data for " + to + "_" + from);
                    return GetMarket(to, from, 1);
                }
                else
                {
                    eventlog1.WriteEntry("No Market Id Found.");
                    return null;
                }
            }
        }
        private JToken ReadSuccessfullContent(HttpResponseMessage message)
        {
            JObject obj = JObject.Parse(message.Content.ReadAsStringAsync().Result);
            return obj["data"];
        }
        private Boolean IsSuccessFullRequest(HttpResponseMessage message)
        {
            Boolean result = false;
            eventlog1.WriteEntry("Response recieved with the status code " + message.StatusCode.ToString());
            if (message.IsSuccessStatusCode)
            {
                eventlog1.WriteEntry("Reading the Response Content.");
                JObject obj = JObject.Parse(message.Content.ReadAsStringAsync().Result);
                String valu = obj["success"].ToString();
                Boolean isSuccess;
                if (valu == "False"||valu=="0")
                {
                    eventlog1.WriteEntry(obj["error"].ToString());
                    isSuccess = false;
                }
                else
                {
                    try
                    {
                        eventlog1.WriteEntry(obj["data"].ToString());
                    }
                    catch(Exception ess)
                    {
                        //throw;
                    }
                    isSuccess = true;
                }
                result = isSuccess;
            }
            else
            {
                eventlog1.WriteEntry("Failure Because of Wrong request and the Status Code: "+message.StatusCode.ToString());
                result = false;
            }
            return result;
        }
        private String EncryptData(String message)
        {
            eventlog1.WriteEntry("Message for the Encryption is: " + message);
            eventlog1.WriteEntry("Encryption of Message Started.");
            Encoding encoding = Encoding.UTF8;
            HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(PrivateKey));
            byte[] getsigndata = hmac.ComputeHash(encoding.GetBytes(message));
            string sbinary = "";
            for (int i = 0; i < getsigndata.Length; i++)
            {
                sbinary += getsigndata[i].ToString("X2");
            }
            eventlog1.WriteEntry("Encryption Done.");
            eventlog1.WriteEntry("Encrypted Result is: "+sbinary.ToLower());
            return sbinary.ToLower();
        }
    }
}
