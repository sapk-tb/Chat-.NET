using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace Serveur
{
    class Serveur : MarshalByRefObject, RemotingInterface.IRemotChaine
    {

        List<string> _users = new List<string>(); //Our users list
        List<string> _messages = new List<string>(); //Our users list

        static void Main(string[] args)
        {
            // Création d'un nouveau canal pour le transfert des données via un port 
            TcpChannel canal = new TcpChannel(12345);

            // Le canal ainsi défini doit être Enregistré dans l'annuaire
            ChannelServices.RegisterChannel(canal, false);

            // Démarrage du serveur en écoute sur objet en mode Singleton
            // Publication du type avec l'URI et son mode 
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Serveur), "Serveur", WellKnownObjectMode.Singleton);

            Console.WriteLine("Le serveur est bien démarré");
            // pour garder la main sur la console
            Console.ReadLine();
        }
        public override object InitializeLifetimeService()
        {
            return null;
        }
        public bool Login(string user)
        {
            //throw new NotImplementedException();
            if ("".Equals(user))
            {
                return false; //Empty
            }
            if (_users.Contains(user))
            {
                return false; //Pseudo allready exist
            }
            _users.Add(user);
            _messages.Add(DateTime.Now + "# " + user + " is connected !");
            return true;
        }
        public bool Logout(string user)
        {
            if ("".Equals(user))
            {
                return false;
            }
            _messages.Add(DateTime.Now+"# "+user + " is gone !");
            return _users.Remove(user);
        }
        public string Hello()
        {
            return "Welcome on this server";
        }

        public bool SendMessage(string user, string message)
        {
            if ("".Equals(user) || "".Equals(message))
            {
                return false; //Empty
            }
            if (!_users.Contains(user))
            {
                return false; //Pseudo not logged
            }
            _messages.Add(DateTime.Now + "# " + user + ": " + message);
            Thread.Sleep(3000);
            return true;
        }
        public List<string> GetMessages()
        {
            Thread.Sleep(3000);
            return _messages;
        }
        public List<string> GetMessagesSince(int id)
        {
            int start = id;
            if (start > _messages.Count) {
                start = _messages.Count;
            }
            Thread.Sleep(3000);
            return _messages.GetRange(start, _messages.Count - start);
        }
        public List<string> GetUsers()
        {
            Thread.Sleep(3000);
            return _users;
        }
        public int GetLastMessageId()
        {
            return _messages.Count;
        }
    }
}
