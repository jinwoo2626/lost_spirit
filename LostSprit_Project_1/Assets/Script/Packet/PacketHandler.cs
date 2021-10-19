using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class PacketHandler
{
	// 내가 입장한 상태에서 다른사람이 들어왔을때 추가
	public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame; 
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.EnterGame(pkt);


	}
	// 누군가가 나갔을때
	public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.LeaveGame(pkt);

	}
	// 주변의 플레이어들 리스트를 불러온다
	public static void S_PlayerListHandler(PacketSession session, IPacket packet)
	{
		S_PlayerList pkt = packet as S_PlayerList;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.Add(pkt);

		
	}
	// 누군가가 이동하였을때
	public static void S_BroadCastMoveHandler(PacketSession session, IPacket packet)
	{
		S_BroadCastMove pkt = packet as S_BroadCastMove;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.Move(pkt);
	}

	public static void S_BoradCastDestroyItemHandler(PacketSession session, IPacket packet)
	{
        S_BoradCastDestroyItem pkt = packet as S_BoradCastDestroyItem;
        ServerSession serverSession = session as ServerSession;

        //PlayerManager.Instance.DestroyItem(pkt);

    }
	public static void S_BroadCastGameOverHandler(PacketSession session, IPacket packet)
	{
		S_BroadCastGameOver pkt = packet as S_BroadCastGameOver;
		ServerSession serverSession = session as ServerSession;

		
	}



}

