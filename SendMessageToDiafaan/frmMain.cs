using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMessageToDiafaan
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtMessage.Text.Trim()))
            {
                await sendHttpsMessage(txtMessage.Text);
            }
            else
            {
                lblResultInfo.Text = "متنی جهت ارسال وارد نشده است";
            }
        }

        private async Task sendHttpsMessage(string message)
        {
            btnSend.Enabled = false;
            var cancellationTokenSource = new CancellationTokenSource();
            var client = new RestClient("https://Mehrnia-MSI:9710/http/send-message");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("to", "+989163085306");
            request.AddParameter("message-type", "sms.automatic");
            request.AddParameter("message", message);
            request.AddParameter("username", "admin");
            request.AddParameter("password", "kabinet95");

            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
            (
               delegate { return true; }
            );
            IRestResponse response = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);

            if (response.IsSuccessful)
            {
                var context = response.Content.Split(':')[1];
                lblResultInfo.Text=$"پیام با شناسه {context} ارسال شد";
                btnSend.Enabled = true;
            }
            else
            {
                lblResultInfo.Text = "پیام ارسال نشد";
                btnSend.Enabled = true;
            }
        }

        private async Task sendHttpMessage(string message)
        {
            btnSend.Enabled = false;
            var cancellationTokenSource = new CancellationTokenSource();
            var client = new RestClient("httpd://Mehrnia-MSI:9710/http/send-message");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("to", "+989163085306");
            request.AddParameter("message-type", "sms.automatic");
            request.AddParameter("message", message);
            request.AddParameter("username", "admin");
            request.AddParameter("password", "kabinet95");

            IRestResponse response = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);

            if (response.IsSuccessful)
            {
                var context = response.Content.Split(':')[1];
                lblResultInfo.Text = $"پیام با شناسه {context} ارسال شد";
                btnSend.Enabled = true;
            }
            else
            {
                lblResultInfo.Text = "پیام ارسال نشد";
                btnSend.Enabled = true;
            }
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
            lblWordCount.Text = $"تعداد کاراکتر:{txtMessage.Text.Length + 1}";
        }
    }
}
