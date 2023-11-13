namespace HappyHourGames.MatchMaker
{
    public class MatchMaker : IMatchMakingService
    {
        private void CreateRoom()
        {
            var roomCount = PhotonNetwork.countOfRooms;
            roomCount++;
            var options = new RoomOptions() { MaxPlayers = 2, IsOpen = true ,IsVisible = true};
            PhotonNetwork.JoinOrCreateRoom(roomCount.ToString(),options,TypedLobby.Default);
        }
    
        public void QuickMatch()
        {
            var roomList =  PhotonNetwork.GetRoomList();
            RoomInfo bestRoom = null;
            foreach (var room in roomList)
            {
                if (room.PlayerCount < room.MaxPlayers)
                {
                    if (bestRoom == null)
                    {
                        bestRoom = room;
                        continue;
                   
                    }
        
                    if (room.PlayerCount > bestRoom.PlayerCount)
                    { 
                        bestRoom = room;
                    } 
                }
            }
        
            if (bestRoom != null)
            {
                PhotonNetwork.JoinRoom(bestRoom.Name);
            }
            else
            {
                CreateRoom();
            }
        }
    }
    
}
