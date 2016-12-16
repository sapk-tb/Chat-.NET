using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer refreshTimer;
        private int lastMessageId = 0;
        private RemotingInterface.IRemotChaine LeRemot;
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;

            // l'objet LeRemot  récupére ici la référence de l'objet du serveur
            // on donne l'URI (serveur, port, classe du serveur)  et le nom de l'interface
            //LeRemot = (RemotingInterface.IRemotChaine)Activator.GetObject(typeof(RemotingInterface.IRemotChaine), "tcp://localhost:12345/Serveur");       
            refreshTimer = new Timer();
            refreshTimer.Tick += new EventHandler(refreshTimer_Tick);

            // Sets the timer interval to 5 seconds.
            refreshTimer.Interval = 5000;
            refreshTimer.Start();
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (sendBtn.IsEnabled)
            {
                //We are  logged 
                Logout_Click(sender, null);
                return;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            pseudoBox.IsEnabled = false;
            hostBox.IsEnabled = false;
            portBox.IsEnabled = false;
            LeRemot = (RemotingInterface.IRemotChaine)Activator.GetObject(typeof(RemotingInterface.IRemotChaine), "tcp://"+hostBox.Text+ ":" + portBox.Text + "/Serveur");
            //textBlock.Text = LeRemot.Hello().ToString();
            try
            {
                serverMessBox.Text = LeRemot.Hello();
            }
            catch
            {
                //Server seems NOK
            }
            if (LeRemot.Login(pseudoBox.Text))
            {
                messageBox.IsEnabled = true;
                messageBox.Text = "";
                sendBtn.IsEnabled = true;
                //Login ok
                refreshUI();
            } else
            {
                pseudoBox.IsEnabled = true;
                hostBox.IsEnabled = true;
                portBox.IsEnabled = true;
                //Login fail
                System.Windows.MessageBox.Show("Failed to login", "Warning !");
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (LeRemot.Logout(pseudoBox.Text))
            {
                //Logged out
                //Reset UI
                pseudoBox.IsEnabled = true;
                hostBox.IsEnabled = true;
                portBox.IsEnabled = true;
                serverMessBox.Text = "";
                messageBox.IsEnabled = false;
                sendBtn.IsEnabled = false;
                chatBox.Items.Clear();
                usersBox.Items.Clear();
            }
            else
            {
                //Failed
                System.Windows.MessageBox.Show("Failed to disconnect", "Warning !");
            }
        }
        private void addToListBox(System.Windows.Controls.ListBox listBox, string str)
        {
            var l = new ListBoxItem();l.Content = str;
            listBox.Items.Add(l);
        }
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            refreshUI();
        }
        private void refreshUI()
        {
            if (!sendBtn.IsEnabled)
            {
                //We are not logged 
                return;
            }
            //chatBox.Items.Clear(); //TODO only update from last point
            LeRemot.GetMessagesSince(lastMessageId).ForEach(delegate (string m)
            {
                addToListBox(chatBox,m);
                lastMessageId++;
            });
            usersBox.Items.Clear();
            LeRemot.GetUsers().ForEach(delegate (string m)
            {
                addToListBox(usersBox, m);
            });
        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {

            if (LeRemot.SendMessage(pseudoBox.Text, messageBox.Text))
            {
                //Success
                System.Windows.MessageBox.Show("Message delivered", "Success !");
                messageBox.Text = "";
            }
            else
            {
                //Failed
                System.Windows.MessageBox.Show("Failed to send message", "Warning !");
            }
            refreshUI();
        }
            /*
    private void button_Click(object sender, RoutedEventArgs e)
    {
       textBlock.Text = LeRemot.Hello().ToString();
    }
    */
        }
}
