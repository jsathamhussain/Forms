using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MyFormsApplication
{
    public partial class Form1 : Form
    {
        BackgroundWorker bgworker;

        public Form1()
        {
            InitializeComponent();

            bgworker = new BackgroundWorker();
            bgworker.DoWork += new DoWorkEventHandler(bgworker_DoWork);
            bgworker.ProgressChanged += new ProgressChangedEventHandler(bgworker_ProgressChanged);
            bgworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgworker_RunWorkerCompleted);
            bgworker.WorkerReportsProgress = true;
            bgworker.WorkerSupportsCancellation = true;            
        }

        void bgworker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                bgworker.ReportProgress(i); // Stimulate Progress_Changed Event Handler

                if (bgworker.CancellationPending)
                {
                    e.Cancel = true;
                    bgworker.ReportProgress(0);
                    return;
                }
            }

            //Report 100% completion on operation completed
            bgworker.ReportProgress(100);
        }

        void bgworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblStart.Text = "Processing... " + progressBar1.Value.ToString() + "%";
        }

        void bgworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                lblStart.Text = "Task Cancelled";
            }
            else if (e.Error != null)
            {
                lblStart.Text = "Error Performing the task !";
            }
            else
            {
                lblStart.Text = "Processing Completed";
            }

            btnStart.Enabled = true;
            btnCancel.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnCancel.Enabled = true;
            bgworker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (bgworker.IsBusy)
            {
                bgworker.CancelAsync();

                btnStart.Enabled = true;
                btnCancel.Enabled = false;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form2 obj = new Form2();
            if (obj == null)
            {
                obj.Parent = this;
            }
            obj.Show();
            this.Hide();
        }
    }
}
