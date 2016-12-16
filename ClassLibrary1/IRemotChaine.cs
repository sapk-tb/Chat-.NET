using System;
using System.Collections.Generic;

namespace RemotingInterface
{
    /// <summary>
    /// cette interface contiendra la déclaration de toutes les 
    /// méthodes de l'objet distribué
    /// </summary>
    public interface IRemotChaine
    {
        string Hello();
        bool Login(string user);
        bool Logout(string user);
        bool SendMessage(string user, string message);
        List<string> GetMessages();
        List<string> GetMessagesSince(int id);
        List<string> GetUsers();
    }
}
