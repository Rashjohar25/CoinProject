using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace CoinsProject
{
    public partial class Form1 : Form
    {
        private static String _exch_from="";
        private static String _exch_to = "";
        private static String _with_from = "";
        private static String _with_to = "";
        private static String _pub_key = "";
        private static String _pri_key = "";
        private static String _token = "";
        private HttpClient client;
        private static List<Currency> allCurrencies = new List<Currency>();
        public Form1()
        {
            InitializeComponent();
            System.Net.ServicePointManager.Expect100Continue = false;
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.cryptsy.com/");
            eventLog1.WriteEntry("Initializing Data");
        }
        private void RunAction()
        {
            eventLog1.WriteEntry("Button Action Started.");
            eventLog1.WriteEntry("Public Key is "+text_publickey.Text.ToString());
            eventLog1.WriteEntry("Private Key is " + text_privatekey.Text.ToString());
            eventLog1.WriteEntry("Token is " + text_token.Text.ToString());
            eventLog1.WriteEntry("Exchange from is " + exchangefrom.Text.ToString());
            eventLog1.WriteEntry("Exchange to is " + exchangeto.Text.ToString());
            eventLog1.WriteEntry("Withdraw from is " + withdrawfrom.Text.ToString());
            eventLog1.WriteEntry("Withdraw to is " + text_withdrawlto.Text.ToString());
            if (text_publickey.Text != "" && text_privatekey.Text != "" &&text_token.Text!="")
            {
                if (exchangefrom.Text != "Select" && exchangeto.Text != "Select" && exchangefrom.Text != "" && exchangeto.Text != "")
                {
                    if (exchangefrom.Items.Contains(exchangefrom.Text) && exchangeto.Items.Contains(exchangeto.Text))
                    {
                        if (withdrawfrom.Text != ""&&withdrawfrom.Text!="Select"&&withdrawfrom.Items.Contains(withdrawfrom.Text)&& text_withdrawlto.Text != "")
                        {
                            PerformOperations(text_publickey.Text.ToString(), text_privatekey.Text.ToString(), exchangefrom.Text.ToString(), exchangeto.Text.ToString(), withdrawfrom.Text.ToString(), text_withdrawlto.Text.ToString(),text_token.Text.ToString());
                        }
                        else
                        {
                            eventLog1.WriteEntry("Showing Message.");
                            MessageBox.Show("Please Add Withdrawl Options.");
                        }
                    }
                    else
                    {
                        eventLog1.WriteEntry("Showing Message.");
                        MessageBox.Show("Select Coin Options from the lists.");
                    }
                }
                else
                {
                    eventLog1.WriteEntry("Showing Message.");
                    MessageBox.Show("Please choose coins options to exchange.");
                }
            }
            else
            {
                eventLog1.WriteEntry("Showing Message.");
                MessageBox.Show("Both the keys are required.");
            }
        }
        private void DisableControls()
        {
            eventLog1.WriteEntry("Disabling the All Controls.");
            text_privatekey.Enabled = false;
            text_publickey.Enabled = false;
            withdrawfrom.Enabled = false;
            text_withdrawlto.Enabled = false;
            exchangefrom.Enabled = false;
            exchangeto.Enabled = false;
            text_token.Enabled = false;
            eventLog1.WriteEntry("Controls Disabled.");
        }
        private void EnableControls()
        {
            eventLog1.WriteEntry("Enabling the All Controls.");
            text_privatekey.Enabled = true;
            text_publickey.Enabled = true;
            withdrawfrom.Enabled = true;
            text_withdrawlto.Enabled = true;
            exchangefrom.Enabled = true;
            exchangeto.Enabled = true;
            text_token.Enabled = true;
            eventLog1.WriteEntry("Controls Enable.");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                eventLog1.WriteEntry("Start Button Clicked.");
                RunAction();
            }
            else
            {
                eventLog1.WriteEntry("Stop Button Clicked.");
                StopTimer();
            }
        }
        private async Task LoadStart()
        {
            eventLog1.WriteEntry("Loading Started.");
            toolStrip_stop.Enabled = false;
            toolStrip_start.Enabled = true;
            allCurrencies = new List<Currency>();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            eventLog1.WriteEntry("Making a new Request to " + client.BaseAddress.ToString() + "api/v2/currencies");
            HttpResponseMessage response = client.GetAsync("api/v2/currencies").Result;
            eventLog1.WriteEntry("response is "+response.StatusCode.ToString());
            if (response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;
                eventLog1.WriteEntry("Reading reponse data.");
                String reqdata = await content.ReadAsStringAsync();
                eventLog1.WriteEntry("Parsing Response Data.");
                JObject obj = JObject.Parse(reqdata);
                allCurrencies = JsonConvert.DeserializeObject<List<Currency>>(obj["data"].ToString());
                eventLog1.WriteEntry("All Currencies receivied successfully with "+allCurrencies.Count+" Items.");
                foreach (var item in allCurrencies)
                {
                    eventLog1.WriteEntry("Adding Item "+item.id+" {"+item.name+"}"+"to all the drop down list.");
                    exchangefrom.Items.Add(item.name);
                    exchangeto.Items.Add(item.name);
                    withdrawfrom.Items.Add(item.name);
                }
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("Loading Initialize.");
            LoadStart().Wait();
        }
        private void PerformOperations(String publickey,String privatekey,String ExchangeFrom,String ExchangeTo, String  WithdrawlFrom, String WithdrawlTo,String token)
        {
            eventLog1.WriteEntry("Performing Start Action.");
            _exch_from = ExchangeFrom;
            _exch_to = ExchangeTo;
            _with_from = WithdrawlFrom;
            _with_to = WithdrawlTo;
            _pub_key = publickey;
            _pri_key = privatekey;
            _token = token;
            eventLog1.WriteEntry("Hiding the Form.");
            this.Hide();
            eventLog1.WriteEntry("Form Hidden.");
            eventLog1.WriteEntry("Initialize Withdraw.");
            Withdrawl();
            eventLog1.WriteEntry("Starting the Service.");
            StartTimer();
        }
        private void exchangefrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void exchangeto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("Showing the Form.");
            this.Show();
            eventLog1.WriteEntry("Form Shown.");
        }
        private void toolStrip_exit_Click(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("Closing.");
            this.Close();
            eventLog1.WriteEntry("Closing the Form.");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("Withdrawing after Interval of the hour.");
            Withdrawl();
        }
        private void toolStrip_start_Click(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("Running the Service.");
            RunAction();
        }
        private void toolStrip_stop_Click(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("Stoping the Service.");
            StopTimer();
        }
        private void StopTimer()
        {
            timer1.Stop();
            toolStrip_stop.Enabled = false;
            toolStrip_start.Enabled = true;
            button1.Text = "Start";
            EnableControls();
        }
        private void StartTimer()
        {
            timer1.Start();
            toolStrip_stop.Enabled = true;
            toolStrip_start.Enabled = false;
            button1.Text = "Stop";
            DisableControls();
        }

        private void Withdrawl()
        {
            eventLog1.WriteEntry("Withdrawl Action Started.");
            RunV2_Withdrawal2(DateTime.Now.Millisecond.ToString());
        }

        private void text_amount_TextChanged(object sender, EventArgs e)
        {

        }
        private String GetEncryptedData(String key,String message)
        {
            eventLog1.WriteEntry("Encryption of data Started.");
            Encoding encoding = Encoding.UTF8;
            HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(key));
            byte[] getsigndata = hmac.ComputeHash(encoding.GetBytes(message));
            string sbinary = "";
            for (int i = 0; i < getsigndata.Length; i++)
            {
                sbinary += getsigndata[i].ToString("X2");
            }
            eventLog1.WriteEntry("Encryption Done.");
            return sbinary.ToLower();
        }

        private HttpResponseMessage RunV2_Balance(String nonce,String private_key,String public_key,String coinid)
        {
            eventLog1.WriteEntry("start retrieving balance of the user.");
            HttpClient newclient = new HttpClient();
            String msg = "nonce=" + nonce + "&limit=100";
            String hmac_balance = GetEncryptedData(private_key, msg);
            newclient.DefaultRequestHeaders.Accept.Clear();
            newclient.DefaultRequestHeaders.Add("key", public_key);
            newclient.DefaultRequestHeaders.Add("sign", hmac_balance);
            newclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            eventLog1.WriteEntry("Making the Request to the https://api.cryptsy.com/api/v2/balances/.");
            HttpResponseMessage response = newclient.GetAsync(new Uri("https://api.cryptsy.com/api/v2/balances/" + coinid + "?" + msg)).Result;
            return response;
        }
        private HttpResponseMessage RunV2_Withdraw(String nonce,String private_key,String public_key,String token,String coinid,String address,String amount)
        {
            eventLog1.WriteEntry("start withdrawing to the user.");
            HttpClient newclient = new HttpClient();
            String msg = "nonce=" + nonce + "&limit=100" + "&quantity=" + amount + "&address=" + address + "&notificationtoken=" + token;
            String hmac_withdraw = GetEncryptedData(private_key, msg);
            newclient.DefaultRequestHeaders.Accept.Clear();
            newclient.DefaultRequestHeaders.Add("key", public_key);
            newclient.DefaultRequestHeaders.Add("sign", hmac_withdraw);
            newclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            eventLog1.WriteEntry("Making the Request to the https://api.cryptsy.com/api/v2/withdraw/.");
            HttpResponseMessage newresponse = newclient.GetAsync(new Uri("https://api.cryptsy.com/api/v2/withdraw/" + coinid + "?" + msg)).Result;
            return newresponse;
        }
        private HttpResponseMessage RunV2_Exchange(String nonce, String private_key, String public_key,String from,String to,String sendingamount)
        {
            eventLog1.WriteEntry("start Exchanging of the currencies.");
            List<KeyValuePair<String,String>> data=new List<KeyValuePair<string,string>>();
            data.Add(new KeyValuePair<string,string>("nonce",nonce));
            data.Add(new KeyValuePair<string,string>("limit","100"));
            data.Add(new KeyValuePair<string, string>("fromcurrency", from));
            data.Add(new KeyValuePair<string, string>("tocurrency", to));
            data.Add(new KeyValuePair<string, string>("sendingamount", sendingamount));
            String msg = "nonce=" + nonce + "&limit=100" + "&fromcurrency=" + from + "&tocurrency=" + to + "&sendingamount="+sendingamount;
            String request = "https://api.cryptsy.com/api/v2/converter?"+msg;
            HttpClient newclient = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(data);
            String hmac_withdraw = GetEncryptedData(private_key, msg);
            newclient.DefaultRequestHeaders.Accept.Clear();
            newclient.DefaultRequestHeaders.Add("key", public_key);
            newclient.DefaultRequestHeaders.Add("sign", hmac_withdraw);
            newclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            eventLog1.WriteEntry("Making the Request to the https://api.cryptsy.com/api/v2/converter?"+msg+".");
            HttpResponseMessage response = newclient.PostAsync(new Uri(request),content).Result;
            return response;
        }
        private void RunV2_Withdrawal2(String nonce)
        {
            eventLog1.WriteEntry("Initialize Request to get available balance.");
            HttpResponseMessage responsea = RunV2_Balance(nonce, _pri_key, _pub_key, (allCurrencies.Find(l => l.name == _exch_from)).id.ToString());
            eventLog1.WriteEntry("Response recieved with the status code " + responsea.StatusCode.ToString());
            if (responsea.IsSuccessStatusCode)
            {
                eventLog1.WriteEntry("Reading the Response Content.");
                HttpContent newcontent = responsea.Content;
                String reqdata = newcontent.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(reqdata);
                String valu = obj["success"].ToString();
                Boolean isSuccess;
                if (valu == "False")
                {
                    eventLog1.WriteEntry("Failure Because of wrong Statement.");
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                if (isSuccess)
                {
                    eventLog1.WriteEntry("Retrieving the  available balance.");
                    String MyBalance = obj["data"]["available"][(allCurrencies.Find(l => l.name == _exch_from)).id.ToString()].ToString();
                    
                    eventLog1.WriteEntry("Current balance is " + MyBalance);
                    eventLog1.WriteEntry("Initialize request for Exchange.");
                    if (MyBalance == "0")
                    {
                        eventLog1.WriteEntry("Showing message with the Error.");
                        //MessageBox.Show("You dont have any Balance in your Account", "0 Balance.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        HttpResponseMessage exchange = RunV2_Exchange(nonce, _pri_key, _pub_key, (allCurrencies.Find(l => l.name == _exch_from)).id.ToString(), (allCurrencies.Find(l => l.name == _exch_to)).id.ToString(), MyBalance);
                        eventLog1.WriteEntry("Response recieved with Status Code " + exchange.StatusCode.ToString());
                        if (exchange.IsSuccessStatusCode)
                        {
                            HttpContent mycontent = exchange.Content;
                            eventLog1.WriteEntry("start Reading the response Content.");
                            String redata = mycontent.ReadAsStringAsync().Result;
                            JObject obje = JObject.Parse(redata);
                            String valur = obje["success"].ToString();
                            Boolean isSuccess3;
                            if (valur == "False")
                            {
                                eventLog1.WriteEntry("Failure Because of wrong Statement.");
                                isSuccess3 = false;
                            }
                            else
                            {
                                isSuccess3 = true;
                            }
                            if (isSuccess3)
                            {
                                eventLog1.WriteEntry("Exchange of Currency is Successful.");
                            }
                            else
                            {
                                eventLog1.WriteEntry("Showing message with the Error.");
                                MessageBox.Show(obje["error"][0].ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                }
                else
                {
                    eventLog1.WriteEntry("Showing message with the Error.");
                    MessageBox.Show(obj["error"][0].ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                eventLog1.WriteEntry("Showing message with the Error.");
                MessageBox.Show(responsea.StatusCode.ToString(), "Connection Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




            
            eventLog1.WriteEntry("Initialize Request to get available balance.");
            HttpResponseMessage response = RunV2_Balance(nonce, _pri_key, _pub_key, (allCurrencies.Find(l => l.name == _with_from)).id.ToString());
            eventLog1.WriteEntry("Response recieved with the status code "+response.StatusCode.ToString());
            if (response.IsSuccessStatusCode)
            {
                eventLog1.WriteEntry("Reading the Response Content.");
                HttpContent newcontent = response.Content;
                String reqdata = newcontent.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(reqdata);
                String valu = obj["success"].ToString();
                Boolean isSuccess;
                if (valu == "False")
                {
                    eventLog1.WriteEntry("Failure Because of wrong Statement.");
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                if (isSuccess)
                {
                    eventLog1.WriteEntry("Retrieving the  available balance.");
                    String MyBalance = obj["data"]["available"][(allCurrencies.Find(l => l.name == _with_from)).id.ToString()].ToString();
                    eventLog1.WriteEntry("Current balance is "+MyBalance);
                    if(MyBalance=="0")
                    {
                        eventLog1.WriteEntry("Showing message with the Error.");
                        //MessageBox.Show("You dont have any Balance in your Account", "0 Balance.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        eventLog1.WriteEntry("Initialize Request to Withdraw.");
                        HttpResponseMessage newresponse = RunV2_Withdraw(nonce, _pri_key, _pub_key, _token, (allCurrencies.Find(l => l.name == _with_from)).id.ToString(), _with_to, MyBalance);
                        eventLog1.WriteEntry("Response Recieved with status code " + newresponse.StatusCode.ToString());
                        if (newresponse.IsSuccessStatusCode)
                        {
                            eventLog1.WriteEntry("Reading the Response Content.");
                            HttpContent content2 = response.Content;
                            String reqdata2 = content2.ReadAsStringAsync().Result;
                            JObject obj2 = JObject.Parse(reqdata2);
                            String valu2 = obj["success"].ToString();
                            Boolean isSuccess2;
                            if (valu2 == "False")
                            {
                                eventLog1.WriteEntry("Failure Because of wrong Statement.");
                                isSuccess2 = false;
                            }
                            else
                            {
                                isSuccess2 = true;
                            }
                            if (isSuccess2)
                            {
                                eventLog1.WriteEntry("Showing message with Success in Withdrawl.");
                                //MessageBox.Show("Withdrawal of " + MyBalance + " " + _with_from + " has been completed.");
                            }
                            else
                            {
                                eventLog1.WriteEntry("Showing message with the Error.");
                                MessageBox.Show(obj["error"][0].ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            eventLog1.WriteEntry("Showing message with the Error.");
                            MessageBox.Show(newresponse.StatusCode.ToString(), "Connection Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    eventLog1.WriteEntry("Showing message with the Error.");
                    MessageBox.Show(obj["error"][0].ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                eventLog1.WriteEntry("Showing message with the Error.");
                MessageBox.Show(response.StatusCode.ToString(), "Connection Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {
            listBox1.Items.Add(DateTime.Now.ToString() + "    " + e.Entry.Message);
        }
    }
}
