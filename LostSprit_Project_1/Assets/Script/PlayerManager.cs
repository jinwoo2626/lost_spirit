using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    PlayerController _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();
    
    public static PlayerManager Instance { get; } = new PlayerManager();
    public int PlayerId { get; set; }
    public void Add(S_PlayerList packet)
    {
        Object objB = Resources.Load("bluePlayer");
        Object objR = Resources.Load("redPlayer");

        // Box
        //Object objCube = Resources.Load("SingleCube5");
        //GameObject goCube = Object.Instantiate(objCube) as GameObject;
        //goCube.transform.position = new Vector3(10, 1, 10);

        //_myPlayer.cubeItem = goCube;


        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go;
            if (p.attr == "fire")
            {
                if (p.isSelf != true)
                {
                    go = Object.Instantiate(objR) as GameObject;
                    Player player = go.AddComponent<Player>();
                    player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                    _players.Add(p.playerId, player);
                }
                else
                {
                    PlayerId = p.playerId;
                }
            }
            else
            {
                if (p.isSelf != true)
                {
                    go = Object.Instantiate(objB) as GameObject;
                    Player player = go.AddComponent<Player>();
                    player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                    _players.Add(p.playerId, player);
                }
                else
                {
                    PlayerId = p.playerId;
                }
            }

        }



    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (PlayerId == packet.playerId)
            return;

        Object objB = Resources.Load("bluePlayer");
        Object objR = Resources.Load("redPlayer");
        GameObject go;
        if (packet.attr == "fire")
        {
            go = Object.Instantiate(objR) as GameObject;
        }
        else
        {
            go = Object.Instantiate(objB) as GameObject;
        }

        Player player = go.AddComponent<Player>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        _players.Add(packet.playerId, player);

    }
    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if(PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            Player player = null;
            if(_players.TryGetValue(packet.playerId, out player))
            {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }   
    public void Move(S_BroadCastMove packet)
    {
               
            
        // _myPlayer.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        
        Player player = null;
        if(_players.TryGetValue(packet.playerId, out player))
        {
            player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        }
        
            
    }
    //public void DestroyItem(S_BoradCastDestroyItem packet)
    //{
    //    _myPlayer.cubeItem = null;

    //    //S_BoradCastDestroyItem pkt = packet as S_BoradCastDestroyItem;
    //    //string tag = pkt.item;
    //    //_myPlayer.DestroyItemEvent(tag);
    //}


}
