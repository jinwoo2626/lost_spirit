using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	ServerSession _session = new ServerSession();

	public void Send(ArraySegment<byte> senBuff)
    {
		_session.Send(senBuff);
    }
    void Start()
    {
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = ipHost.AddressList[0];
		IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

		Connector connector = new Connector();

		connector.Connect(endPoint,	
			() => { return _session; });
		
	}

    void Update()
    {
		List<IPacket> list = PacketQueue.Instance.PopAll();
		
		
		foreach (IPacket packet in list)
		{
			PacketManager.Instance.HandlePacket(_session, packet);
			
		}		
    }
	public void FirePlayerStart()
	{
		C_Enter enterPacket = new C_Enter();
		enterPacket.attr = "fire";
		enterPacket.posX = 0;
		enterPacket.posY = 1;
		enterPacket.posZ = 10;
		Send(enterPacket.Write());
	}

	public void WaterPlayerStart()
	{
		C_Enter enterPacket = new C_Enter();
		enterPacket.attr = "water";
		enterPacket.posX = 0;
		enterPacket.posY = 1;
		enterPacket.posZ = 5;
		Send(enterPacket.Write());
	}

    public void MovePlayer(float posX, float posY, float posZ)
    {
		C_Move movePacket = new C_Move();
		movePacket.posX = posX;
		movePacket.posY = posY;
		movePacket.posZ = posZ;
		Send(movePacket.Write());
	}
	public void DestroyObject(String tag)
    {
		C_DestroyItem packet = new C_DestroyItem();
		packet.item = tag;
		Send(packet.Write());
	}

	public void GameOverEvent()
    {
		C_GameOver packet = new C_GameOver();
		Send(packet.Write());
    }

}
