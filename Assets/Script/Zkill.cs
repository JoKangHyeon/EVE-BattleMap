using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using WebSocketSharp;

public class Zkill : MonoBehaviour
{

    private string zkillServer = "wss://zkillboard.com/websocket/";
    public WebSocket m_Socket = null;

    public CsvParser parser;
    public List<Kill> kills;
    // Start is called before the first frame update
    void Start()
    {
        parser = this.gameObject.GetComponent<CsvParser>();
        
        try
        {
            m_Socket = new WebSocket(zkillServer);
            m_Socket.OnMessage += Recv;
            m_Socket.OnClose += CloseConnect;
            m_Socket.OnOpen += ws_connected;
            m_Socket.Connect();
            m_Socket.Send("{\"action\":\"sub\",\"channel\":\"killstream\"}");
            kills = new List<Kill>();
        }
        catch
        {
            Debug.Log("웹소캣 생성 오류");
        }
    }

    public void Connect()
    {
        try
        {
            if(m_Socket==null || !m_Socket.IsAlive)
            {
                m_Socket.Connect();
            }
        }catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    public void ws_connected(object sender, EventArgs e)
    {
        Debug.Log("OPENED");
    }

    public void wsSendMessage(string msg)
    {

        try
        {
            m_Socket.Send(Encoding.UTF8.GetBytes(msg));
        }catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    void CloseConnect(object sender, CloseEventArgs e)
    {
        Debug.Log(e.Reason);

        try
        {
            if (m_Socket == null) return;

            if (m_Socket.IsAlive) m_Socket.Close();
        } catch (Exception ex)
        {
            Debug.Log(ex.StackTrace);
        }
    }

    void Recv(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
        try
        {
            kills.Add(JsonUtility.FromJson<Kill>(e.Data));

        }
        catch (Exception er)
        {
            Debug.Log(er.ToString());
            Debug.Log(er.StackTrace);
        }
    }
    // Update is called once per frame
    void Update()
    {
        while (kills.Count > 0)
        {
            Kill k = kills[0];
            kills.RemoveAt(0);
            parser.starData[k.solar_system_id].starObject.GetComponent<SpriteRenderer>().color = Color.red;
            parser.starData[k.solar_system_id].starObject.GetComponent<ParticleSystem>().Play();
        }
    }
}


[Serializable]
public class Kill
{
    public Attacker[] attackers;
    public int killmail_id;
    public string killmail_time;
    public int solar_system_id;
    public Victim victim;
    public Zkb zkb;

    [Serializable]
    public class Attacker
    {
        public int damage_done;
        public int faction_id;
        public bool final_blow;
        public int security_status;
        public int ship_type_id;
    }

    [Serializable]
    public class Victim
    {
        public int character_id;
        public int corporation_id;
        public int damage_taken;
        public Item[] items;
        public Position position;
        public int ship_type_id;

        [Serializable]
        public class Item
        {
            public int flag;
            public int item_type_id;
            public int quantity_dropped;
            public int singleton;
        }

        [Serializable]
        public class Position
        {
            public float x;
            public float y;
            public float z;
        }
    }

    [Serializable]
    public class Zkb
    {
        public int locationID;
        public string hash;
        public float fittedValue;
        public float droppedValue;
        public float destroyedValue;
        public float totalValue;
        public int points;
        public bool npc;
        public bool solo;
        public bool awox;
        public string esi;
        public string url;
    }
}

