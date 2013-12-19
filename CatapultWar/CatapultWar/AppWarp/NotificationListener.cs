using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Collections.Generic;
using System.Windows.Threading;

namespace CatapultWar
{
    public class NotificationListener : com.shephertz.app42.gaming.multiplayer.client.listener.NotifyListener
    {
        public delegate void UICallback();
        UICallback OpponentLeftRoom = null;
        UICallback RemoteUserPaused = null;
        UICallback RemoteUserResumed = null;
        UICallback ChangeTurnCallback = null;
        UICallback GameStarted = null;
        public NotificationListener()
        {
        }
        public void AddCallBacks(UICallback uCallback, UICallback RPaused, UICallback RResumed, UICallback ChangeTurn,UICallback GStarted)
        {
            OpponentLeftRoom = uCallback;
            RemoteUserPaused = RPaused;
            RemoteUserResumed = RResumed;
            ChangeTurnCallback = ChangeTurn;
            GameStarted = GStarted;
        }
        public void RemoveCallBacks()
        {
            OpponentLeftRoom = null;
            RemoteUserPaused = null;
            RemoteUserResumed = null;
            ChangeTurnCallback = null;
        }
        public void onRoomCreated(RoomData eventObj)
        {
           
        }

        public void onRoomDestroyed(RoomData eventObj)
        {
            
        }

        public void onUserLeftRoom(RoomData eventObj, String username)
        {
            if (eventObj.getId().Equals(GlobalContext.GameRoomId))
            {
                if (!GlobalContext.localUsername.Equals(username))
                {
                    if(OpponentLeftRoom!=null)
                        Deployment.Current.Dispatcher.BeginInvoke(new UICallback(OpponentLeftRoom));            
                }
            }
        }

        public void onUserJoinedRoom(RoomData eventObj, String username)
        {
            if (!GlobalContext.localUsername.Equals(username))
            {  
                GlobalContext.opponentName = username;
                GlobalContext.joinedUsers = new[] {GlobalContext.localUsername, GlobalContext.opponentName};
            }
            if (GlobalContext.joinedUsers.Length == 2)
            {
                WarpClient.GetInstance().startGame();
            }
        }

        public void onUserLeftLobby(LobbyData eventObj, String username)
        {
            
        }

        public void onUserJoinedLobby(LobbyData eventObj, String username)
        {
            
        }

        public void onChatReceived(ChatEvent eventObj)
        {
            
        }

        public void onUpdatePeersReceived(UpdateEvent eventObj)
        {
            MoveMessage.buildMessage(eventObj.getUpdate());           
        }

        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties)
        {
            GlobalContext.tableProperties = properties;
        }
        public void onMoveCompleted(MoveEvent moveEvent)
        {
            if (moveEvent.getRoomId().Equals(GlobalContext.GameRoomId))
            { 
                if(moveEvent.getMoveData()!=null)
                MoveMessage.buildMessage(System.Text.Encoding.UTF8.GetBytes(moveEvent.getMoveData()));
                if (moveEvent.getNextTurn().ToString().Equals(GlobalContext.localUsername))
                {
                    GlobalContext.IsMyTurn = true;
                    if (RemoteUserPaused != null)
                        Deployment.Current.Dispatcher.BeginInvoke(new UICallback(ChangeTurnCallback));
                }
                else
                {
                    GlobalContext.IsMyTurn = false;
                    if (RemoteUserPaused != null)
                        Deployment.Current.Dispatcher.BeginInvoke(new UICallback(ChangeTurnCallback));
                }
            }
        }

        public void onPrivateChatReceived(string sender, string message)
        {
            throw new NotImplementedException();
        }

        public void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
        {
            GlobalContext.tableProperties = properties;
        }


        public void onUserPaused(string locid, bool isLobby, string username)
        {
             if (!GlobalContext.localUsername.Equals(username) && !isLobby)
              {
                  if (RemoteUserPaused != null)
                      Deployment.Current.Dispatcher.BeginInvoke(new UICallback(RemoteUserPaused));             
             }
        }

        public void onUserResumed(string locid, bool isLobby, string username)
        {
              if (!GlobalContext.localUsername.Equals(username) && !isLobby)
                {
                    if (RemoteUserResumed != null)
                        Deployment.Current.Dispatcher.BeginInvoke(new UICallback(RemoteUserResumed));               
                }
        }


        public void onGameStarted(string sender, string roomId, string nextTurn)
        {
            Deployment.Current.Dispatcher.BeginInvoke(new UICallback(GameStarted));  
        }

        public void onGameStopped(string sender, string roomId)
        {
            //throw new NotImplementedException();
        }
    }
}
