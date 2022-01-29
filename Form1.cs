using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using S22.Imap;
using MySql.Data.MySqlClient;
using MetroFramework.Forms;

namespace TicketingSystem
{
    public partial class Form1 : MetroForm
    {

        MySqlCommand com;
        MySqlConnection con;
        MySqlDataReader data;


        public Form1 f;

        public Form1()
        {
            InitializeComponent();
            f = this;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            connection();

            receiveticket();

            selection();
            selectionreply();
            selectionclose();
            selectionnote();

            //selectif();
            autogenerate();


        }

        public void selectif()
        {
            String x = "Reply";

            con.Open();
            com = new MySqlCommand("Select * from tbl_email where fromx = '" + fromtxbx.Text + "' and Status = '" + x + "'", con);
            data = com.ExecuteReader();

            while (data.Read())
            {
                    MessageBox.Show("asdasd");
                    insertreply();


                    do
                    {
                       // autogenerate();

                    } while (!data.Read());

                

                

            }
            con.Close();


        }


        public void connection()
        {

            con = new MySqlConnection("Server = localhost; database = db_ticketing; UID = root; password = 0000;");
            con.Open();
            con.Close();
        }

        public void selection()
        {
            String openx = "Open";

            con.Open();
            com = new MySqlCommand("Select * from tbl_email where status = '" + openx + "'", con);
            data = com.ExecuteReader();

            Emailviewer.Items.Clear();

            while (data.Read())
            {
                Emailviewer.Sorting = SortOrder.Descending;
                ListViewItem lv = new ListViewItem(data.GetValue(5).ToString());
                lv.SubItems.Add(data.GetValue(1).ToString());
                lv.SubItems.Add(data.GetValue(4).ToString());
                lv.SubItems.Add(data.GetValue(6).ToString());
                lv.SubItems.Add(data.GetValue(3).ToString());
     
                Emailviewer.Items.Add(lv);



            }
            con.Close();
        }

        public void selectionreply()
        {
            String closex = "Reply";
            con.Open();
            com = new MySqlCommand("Select * from tbl_email where status = '" + closex + "'" , con);
            data = com.ExecuteReader();

            Replyticketviewer.Items.Clear();

             while (data.Read())
            {
                Replyticketviewer.Sorting = SortOrder.Descending;
                ListViewItem lv = new ListViewItem(data.GetValue(5).ToString());
                lv.SubItems.Add(data.GetValue(1).ToString());
                lv.SubItems.Add(data.GetValue(4).ToString());
                lv.SubItems.Add(data.GetValue(6).ToString());
                lv.SubItems.Add(data.GetValue(3).ToString());
     
                Replyticketviewer.Items.Add(lv);


            }
            con.Close();

        }

        public void selectionclose()
        {
            String openx = "Close";
            con.Open();
            com = new MySqlCommand("Select * from tbl_email where status = '" + openx + "'", con);
            data = com.ExecuteReader();

            Closeticketviewer.Items.Clear();

            while (data.Read())
            {
                Closeticketviewer.Sorting = SortOrder.Descending;
                ListViewItem lv = new ListViewItem(data.GetValue(5).ToString());
                lv.SubItems.Add(data.GetValue(1).ToString());
                lv.SubItems.Add(data.GetValue(4).ToString());
                lv.SubItems.Add(data.GetValue(6).ToString());
                lv.SubItems.Add(data.GetValue(3).ToString());

                Closeticketviewer.Items.Add(lv);


            }
            con.Close();

        }

        public void selectionnote()
        {
            String notedx = "Noted";
            con.Open();
            com = new MySqlCommand("Select * from tbl_email where status = '" + notedx + "'", con);
            data = com.ExecuteReader();

            Notedticketviewer.Items.Clear();

            while (data.Read())
            {
                Notedticketviewer.Sorting = SortOrder.Descending;
                ListViewItem lv = new ListViewItem(data.GetValue(5).ToString());
                lv.SubItems.Add(data.GetValue(1).ToString());
                lv.SubItems.Add(data.GetValue(4).ToString());
                lv.SubItems.Add(data.GetValue(6).ToString());
                lv.SubItems.Add(data.GetValue(3).ToString());

                Notedticketviewer.Items.Add(lv);


            }
            con.Close();

        }





        public void autogenerate()
        {

            con.Open();
            com = new MySqlCommand("SELECT MAX(TicketNo)as maxid FROM tbl_email", con);
            data = com.ExecuteReader();
            data.Read();

            var id = data.GetString("maxid");
            var x = "1";
            int a = Convert.ToInt32(id);
            int b = Convert.ToInt32(x);

            int y = a + b;

            String result = y.ToString();
            String INV = "17PT" + result;

            ticketnumbertxbx.Text = INV;
            ticketnumberholder.Text = result;

            con.Close();

        }


        public void receiveticket()
        {

            String user = "anthonydedios1998@gmail.com";
            String passx = "5673676anthony";

  

            Task.Run(() =>
            {


                using (ImapClient client = new ImapClient("imap.gmail.com", 993, user, passx, AuthMethod.Login, true))
                {
                    if (client.Supports("IDLE") == false)
                    {
                        return;
                    }

                    client.NewMessage += new EventHandler<IdleMessageEventArgs>(fxx);
                    while (true) ;

                }


            });


        }

        


        public void fxx(object sender, IdleMessageEventArgs e)
        {

                
                MailMessage m = e.Client.GetMessage(e.MessageUID, FetchOptions.Normal);
                f.Invoke((MethodInvoker)delegate
                {
                    

                    fromtxbx.Text = "";
                    subjecttxbx.Text = "";
                    bodytxbx.Text = "";


                    f.fromtxbx.AppendText(m.From + "");
                    f.subjecttxbx.AppendText(m.Subject);
                    f.bodytxbx.AppendText(m.Body);
                    f.totxbx.AppendText(m.To + "");
      
                });

                TextBox.CheckForIllegalCrossThreadCalls = false;

                this.Hide();
                this.Show();

                insert();
                selection();
                autogenerate();
            
            }

          

        public void insert()
        {
            String openx = "Open";

            con.Open();
            com = new MySqlCommand("Insert into db_ticketing.tbl_email(Subject,Body,TicketNo,TicketNoText,Fromx,Status) values ('" + subjecttxbx.Text + "','" + bodytxbx.Text + "','" + ticketnumberholder.Text + "','" + ticketnumbertxbx.Text + "','" + fromtxbx.Text + "','" + openx + "')", con);
            com.ExecuteNonQuery();
            con.Close();

        }

        public void insertreply()
        {
            String reply = "Reply";

            con.Open();
            com = new MySqlCommand("Insert into db_ticketing.tbl_email(Subject,Body,TicketNo,TicketNoText,Fromx,Status) values ('" + subjecttxbx.Text + "','" + bodytxbx.Text + "','" + ticketnumberholder.Text + "','" + ticketnumbertxbx.Text + "','" + fromtxbx.Text + "','" + reply + "')", con);
            com.ExecuteNonQuery();
            con.Close();

        }


        private void Emailviewer_MouseClick(object sender, MouseEventArgs e)
        {


            ticketnox.Text = Emailviewer.Items[Emailviewer.SelectedItems[0].Index].Text;
            subjectcopy.Text = Emailviewer.Items[Emailviewer.SelectedItems[0].Index].SubItems[1].Text;
            fromcopy.Text = Emailviewer.Items[Emailviewer.SelectedItems[0].Index].SubItems[2].Text;
            status.Text = Emailviewer.Items[Emailviewer.SelectedItems[0].Index].SubItems[3].Text;
            tickettry.Text = Emailviewer.Items[Emailviewer.SelectedItems[0].Index].SubItems[4].Text;

            /*
            Emailviewer.Enabled = false;
            Closeticketviewer.Enabled = false;
            Notedticketviewer.Enabled = false;
            Replyticketviewer.Enabled = false;
            */
            



        }


        public void updatethis()
        {
            con.Open();
            com = new MySqlCommand("Update tbl_email set Status = '" + status.Text + "' where TicketNo = " + tickettry.Text + "", con);
            com.ExecuteNonQuery();
            con.Close();
        }

        public void updatehere()
        {
            con.Open();
            com = new MySqlCommand("Update tbl_email set Status = '" + StatusCS.Text + "' where TicketNo = " + ticketno.Text + "", con);
            com.ExecuteNonQuery();
            con.Close();
        }


        private void sendtxbx_Click(object sender, EventArgs e)
        {

            updatethis();

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("anthonydedios75@gmail.com");
            msg.To.Add(fromcopy.Text);
            msg.Subject = subjectcopy.Text;

            msg.Body = bodycopy.Text;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("anthonydedios1998@gmail.com", "***************");
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;  

                client.Send(msg);
            }

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            updatethis();
        }

        private void Replyticketviewer_ItemActivate(object sender, EventArgs e)
        {

            Changestatusview.Visible = true;
            TicketnumberCS.Text = Replyticketviewer.Items[Replyticketviewer.SelectedItems[0].Index].Text;
            ToCS.Text = Replyticketviewer.Items[Replyticketviewer.SelectedItems[0].Index].SubItems[1].Text;
            SubjectCS.Text = Replyticketviewer.Items[Replyticketviewer.SelectedItems[0].Index].SubItems[2].Text;
            StatusCS.Text = Replyticketviewer.Items[Replyticketviewer.SelectedItems[0].Index].SubItems[3].Text;
            ticketno.Text = Replyticketviewer.Items[Replyticketviewer.SelectedItems[0].Index].SubItems[4].Text;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            updatehere();
            TicketnumberCS.Text = "";
            ToCS.Text = "";
            SubjectCS.Text = "";
            StatusCS.Text = "";
            selectionreply();
            selectionclose();
            selectionnote();
            Changestatusview.Visible = false;
            
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            Changestatusview.Visible = false;
            TicketnumberCS.Text = "";
            ToCS.Text = "";
            SubjectCS.Text = "";
            StatusCS.Text = "";
        }

        private void Notedticketviewer_ItemActivate(object sender, EventArgs e)
        {
            Changestatusview.Visible = true;
            TicketnumberCS.Text = Notedticketviewer.Items[Notedticketviewer.SelectedItems[0].Index].Text;
            ToCS.Text = Notedticketviewer.Items[Notedticketviewer.SelectedItems[0].Index].SubItems[1].Text;
            SubjectCS.Text = Notedticketviewer.Items[Notedticketviewer.SelectedItems[0].Index].SubItems[2].Text;
            StatusCS.Text = Notedticketviewer.Items[Notedticketviewer.SelectedItems[0].Index].SubItems[3].Text;
            ticketno.Text = Notedticketviewer.Items[Notedticketviewer.SelectedItems[0].Index].SubItems[4].Text;
        }

    }
}