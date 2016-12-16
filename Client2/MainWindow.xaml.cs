using System;
using System.Windows;


namespace Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RemotingInterface.IRemotChaine LeRemot;
        public MainWindow()
        {
            InitializeComponent();

            // l'objet LeRemot  récupére ici la référence de l'objet du serveur
            // on donne l'URI (serveur, port, classe du serveur)  et le nom de l'interface
            LeRemot = (RemotingInterface.IRemotChaine)Activator.GetObject(
                typeof(RemotingInterface.IRemotChaine), "tcp://localhost:12345/Serveur");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = LeRemot.Hello().ToString();
        }
    }
}
