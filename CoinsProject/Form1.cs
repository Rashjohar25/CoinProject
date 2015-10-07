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
        }
        private void RunAction()
        {
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
                            MessageBox.Show("Please Add Withdrawl Options.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Coin Options from the lists.");
                    }
                }
                else
                {
                    MessageBox.Show("Please choose coins options to exchange.");
                }
            }
            else
            {
                MessageBox.Show("Both the keys are required.");
            }
        }
        private void DisableControls()
        {
            text_privatekey.Enabled = false;
            text_publickey.Enabled = false;
            withdrawfrom.Enabled = false;
            text_withdrawlto.Enabled = false;
            exchangefrom.Enabled = false;
            exchangeto.Enabled = false;
            text_token.Enabled = false;
        }
        private void EnableControls()
        {
            text_privatekey.Enabled = true;
            text_publickey.Enabled = true;
            withdrawfrom.Enabled = true;
            text_withdrawlto.Enabled = true;
            exchangefrom.Enabled = true;
            exchangeto.Enabled = true;
            text_token.Enabled = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                RunAction();
            }
            else
            {
                StopTimer();
            }
        }
        private async Task LoadStart()
        {
            toolStrip_stop.Enabled = false;
            toolStrip_start.Enabled = true;
            allCurrencies = new List<Currency>();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("api/v2/currencies").Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;
                String reqdata = await content.ReadAsStringAsync();
                JObject obj = JObject.Parse(reqdata);
                allCurrencies = JsonConvert.DeserializeObject<List<Currency>>(obj["data"].ToString());
                foreach (var item in allCurrencies)
                {
                    exchangefrom.Items.Add(item.name);
                    exchangeto.Items.Add(item.name);
                    withdrawfrom.Items.Add(item.name);
                }
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStart().Wait();
        }
        private void PerformOperations(String publickey,String privatekey,String ExchangeFrom,String ExchangeTo, String  WithdrawlFrom, String WithdrawlTo,String token)
        {
            _exch_from = ExchangeFrom;
            _exch_to = ExchangeTo;
            _with_from = WithdrawlFrom;
            _with_to = WithdrawlTo;
            _pub_key = publickey;
            _pri_key = privatekey;
            _token = token;
            this.Hide();
            Withdrawl();
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
            this.Show();
        }
        private void toolStrip_exit_Click(object sender, EventArgs e)
        {   
            this.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Withdrawl();
        }
        private void toolStrip_start_Click(object sender, EventArgs e)
        {
            RunAction();
        }
        private void toolStrip_stop_Click(object sender, EventArgs e)
        {
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
        private static void Withdrawl()
        {
            HttpClient newclient = new HttpClient();

            int nonce = DateTime.Now.Millisecond;
            String msg = "nonce=" + nonce;
            msg = msg + "&limit=100";
            String hmac_balance = GetEncryptedData(_pri_key,msg);
            
            newclient.DefaultRequestHeaders.Accept.Clear();
            newclient.DefaultRequestHeaders.Add("key", _pub_key);
            newclient.DefaultRequestHeaders.Add("sign", hmac_balance);
            newclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            HttpResponseMessage response = newclient.GetAsync(new Uri("https://api.cryptsy.com/api/v2/balances/"+(allCurrencies.Find(l=>l.name==_with_from)).id+"?"+msg)).Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContent newcontent = response.Content;
                String reqdata = newcontent.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(reqdata);
                String valu = obj["success"].ToString();
                Boolean isSuccess;
                if (valu == "False")
                {
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                if (isSuccess)
                {
                    String MyBalance=obj["data"]["available"][(allCurrencies.Find(l => l.name == _with_from)).id.ToString()].ToString();
                    String msg_withdraw = msg + "&quantity=" + MyBalance + "&address=" + _with_to + "&notificationtoken=" + _token;
                    String hmac_withdraw = GetEncryptedData(_pri_key, msg_withdraw);
                    newclient.DefaultRequestHeaders.Accept.Clear();
                    newclient.DefaultRequestHeaders.Add("key", _pub_key);
                    newclient.DefaultRequestHeaders.Add("sign", hmac_withdraw);
                    newclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    HttpResponseMessage newresponse = newclient.GetAsync(new Uri("https://api.cryptsy.com/api/v2/balances/" + (allCurrencies.Find(l => l.name == _with_from)).id + "?" + msg_withdraw)).Result;
                    if(newresponse.IsSuccessStatusCode)
                    {
                        HttpContent content2 = response.Content;
                        String reqdata2 = content2.ReadAsStringAsync().Result;
                        JObject obj2 = JObject.Parse(reqdata2);
                        String valu2 = obj["success"].ToString();
                        Boolean isSuccess2;
                        if (valu2 == "False")
                        {
                            isSuccess2 = false;
                        }
                        else
                        {
                            isSuccess2 = true;
                        }
                        if(isSuccess2)
                        {
                            MessageBox.Show("Withdrawal of "+MyBalance+" "+_with_from+" has been completed.");
                        }
                        else
                        {
                            MessageBox.Show(obj["error"][0].ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(newresponse.StatusCode.ToString(), "Connection Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show(obj["error"][0].ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(response.StatusCode.ToString(),"Connection Error.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            /*
            HttpClient newclient = new HttpClient();
            var postData = new List<KeyValuePair<string, string>>();
            //newclient.BaseAddress = new Uri("https://api.cryptsy.com/api");

            int nonce = DateTime.Now.Millisecond;
            String msg = "nonce="+nonce;
            msg = msg + "&limit=100";
            msg = msg + "&quantity=" + _amount + "&address=" + _with_to + "&notificationtoken=makewithdrawal";
            
            postData.Add(new KeyValuePair<string, string>("method", "makewithdrawal"));
            postData.Add(new KeyValuePair<string, string>("nonce", nonce.ToString()));
            postData.Add(new KeyValuePair<string, string>("amount", _amount));
            postData.Add(new KeyValuePair<string, string>("address", _with_to));
            newclient.DefaultRequestHeaders.Accept.Clear();
            newclient.DefaultRequestHeaders.Add("key",_pub_key);
            newclient.DefaultRequestHeaders.Add("sign", sbinary);
            //newclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(""));
            
            newclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("/*"));
            //newclient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36"));
            
            HttpContent content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response = newclient.GetAsync(new Uri("https://api.cryptsy.com/api/v2/withdraw/"+(allCurrencies.Find(l=>l.name==_with_from)).id+"?"+msg).Result;
            // HttpResponseMessage response = newclient.PostAsync(new Uri("https://api.cryptsy.com/api"), content).Result;
            if (response.IsSuccessStatusCode)
            {
                HttpContent newcontent = response.Content;
                String reqdata = newcontent.ReadAsStringAsync().Result;
                JObject obj = JObject.Parse(reqdata);
                String valu=obj["success"].ToString();
                Boolean isSuccess;
                if (valu == "0")
                {
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
                if(isSuccess)
                {

                }
                else
                {
                    MessageBox.Show(obj["error"].ToString());
                }
            }
            else
            {
                MessageBox.Show(response.StatusCode.ToString());
            }*/
        }

        private void text_amount_TextChanged(object sender, EventArgs e)
        {

        }
        private static String GetEncryptedData(String key,String message)
        {
            Encoding encoding = Encoding.UTF8;
            HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(key));
            byte[] getsigndata = hmac.ComputeHash(encoding.GetBytes(message));
            string sbinary = "";
            for (int i = 0; i < getsigndata.Length; i++)
            {
                sbinary += getsigndata[i].ToString("X2");
            }
            return sbinary.ToLower();
        }
    }
}
