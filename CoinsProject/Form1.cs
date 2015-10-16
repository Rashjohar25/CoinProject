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
        private static List<Currency> allCurrencies = new List<Currency>();
        public static AllHttpRequests allrequests;
        public Form1()
        {
            InitializeComponent();
            allrequests= new AllHttpRequests(eventLog1,listBox1);
        }
        private void RunAction()
        {
            eventLog1.WriteEntry("Button Action Started.");
            eventLog1.WriteEntry("Public Key is "+text_publickey.Text.ToString());
            eventLog1.WriteEntry("Private Key is " + text_privatekey.Text.ToString());
            eventLog1.WriteEntry("Exchange from is " + exchangefrom.Text.ToString());
            eventLog1.WriteEntry("Exchange to is " + exchangeto.Text.ToString());
            eventLog1.WriteEntry("Withdraw from is " + withdrawfrom.Text.ToString());
            eventLog1.WriteEntry("Withdraw to is " + text_withdrawlto.Text.ToString());
            if (text_publickey.Text != "" && text_privatekey.Text != "")
            {
                if (exchangefrom.Text != "Select" && exchangeto.Text != "Select" && exchangefrom.Text != "" && exchangeto.Text != "")
                {
                    if (exchangefrom.Items.Contains(exchangefrom.Text) && exchangeto.Items.Contains(exchangeto.Text))
                    {
                        if (withdrawfrom.Text != ""&&withdrawfrom.Text!="Select"&&withdrawfrom.Items.Contains(withdrawfrom.Text)&& text_withdrawlto.Text != "")
                        {
                            PerformOperations(text_publickey.Text.ToString(), text_privatekey.Text.ToString(), exchangefrom.Text.ToString(), exchangeto.Text.ToString(), withdrawfrom.Text.ToString(), text_withdrawlto.Text.ToString());
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
            eventLog1.WriteEntry("Controls Enable.");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                eventLog1.WriteEntry("Start Button Clicked.");
                button1.Enabled = false;
                RunAction();
                button1.Enabled = true;
            }
            else
            {
                eventLog1.WriteEntry("Stop Button Clicked.");
                button1.Enabled = false;
                StopTimer();
                button1.Enabled = true;
            }
        }
        private void LoadStart()
        {
            eventLog1.WriteEntry("Loading Started.");
            toolStrip_stop.Enabled = false;
            toolStrip_start.Enabled = true;
            allCurrencies = new List<Currency>();
            allCurrencies = allrequests.AllAvailableCurrency;
            eventLog1.WriteEntry("All Currencies receivied successfully with " + allCurrencies.Count + " Items.");
            foreach (var item in allCurrencies)
            {
                eventLog1.WriteEntry("Adding Item " + item.id + " {" + item.name + "}" + "to all the drop down list.");
                exchangefrom.Items.Add(item.name);
                exchangeto.Items.Add(item.name);
                withdrawfrom.Items.Add(item.name);
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            eventLog1.WriteEntry("Loading Initialize.");
            LoadStart();
            
        }
        private void PerformOperations(String publickey,String privatekey,String ExchangeFrom,String ExchangeTo, String  WithdrawlFrom, String WithdrawlTo)
        {
            eventLog1.WriteEntry("Performing Start Action.");
            _exch_from = ExchangeFrom;
            _exch_to = ExchangeTo;
            _with_from = WithdrawlFrom;
            _with_to = WithdrawlTo;
            _pub_key = publickey;
            _pri_key = privatekey;
            allrequests.PrivateKey = privatekey;
            allrequests.PublicKey = publickey;
            Balances all = allrequests.AllBalance;
            ExchangeMoney(all);
            Withdrawl();
            eventLog1.WriteEntry("Starting the Service.");
            StartTimer();
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
        private void ExchangeMoney(Balances balances)
        {
            eventLog1.WriteEntry("Exchange of Currencies Action Started.");
            Currency _exchangefrom=allCurrencies.Find(l => l.name == _exch_from);
            double _from_balance= balances.available.SelectToken(_exchangefrom.id.ToString()).Value<double>();
            eventLog1.WriteEntry("Your Current Balance for "+_exchangefrom.name+" Coins is: "+_from_balance);
            if(_from_balance>0)
            {
                eventLog1.WriteEntry("Valid Balance of " + _exchangefrom.name + " Coins. Exchange Operation is forwarding.");
            }
            else
            {
                eventLog1.WriteEntry("Invalid Balance of " + _exchangefrom.name + " Coins. No Exchange is Possible.");
                return;
            }
            Boolean IsSellSuccess= allrequests.PlaceOrder(_exchangefrom.code, "btc", _from_balance.ToString(), true);
            if(IsSellSuccess)
            {
                Currency _exchangeto = allCurrencies.Find(l => l.name == _exch_to);
                balances = allrequests.AllBalance;
                double _to_balance = balances.available.SelectToken("3").Value<double>();
                eventLog1.WriteEntry("Your Current Balance for btc Coins is: " + _to_balance);
                if (_to_balance > 0)
                {
                    eventLog1.WriteEntry("Valid Balance of btc Coins. Exchange Operation is forwarding.");
                }
                else
                {
                    eventLog1.WriteEntry("Invalid Balance of btc Coins. No Exchange is Possible.");
                    return;
                }
                Boolean IsBuySuccess = allrequests.PlaceOrder( "btc",_exchangeto.code, _to_balance.ToString(), false);
                if(!IsBuySuccess)
                {
                    eventLog1.WriteEntry("Buy Order Failed.");
                }
                else
                {
                    eventLog1.WriteEntry("Buy Order Placed.");
                }
            }
            else
            {
                eventLog1.WriteEntry("Buy Can not be processed without performing Sell.");
            }

        }
        private void Withdrawl()
        {
            eventLog1.WriteEntry("Withdrawl Action Started.");
            Balances balances = allrequests.AllBalance;
            Currency withdrawlcurrency=allCurrencies.Find(l => l.name == _with_from);
            double bal = balances.available.SelectToken(withdrawlcurrency.id.ToString()).Value<double>();
            if(bal>0)
            {
                allrequests.Withdraw(bal.ToString(), text_withdrawlto.Text.ToString(), withdrawlcurrency.id.ToString());
            }
            else
            {
                eventLog1.WriteEntry("Invalid Balance of "+withdrawlcurrency.name+" Coins. No Withdrawl is Possible.");
                return;
            }
            
        }
        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {
            listBox1.Items.Add(DateTime.Now.ToString() + "    " + e.Entry.Message);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
