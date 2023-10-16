using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvParser : MonoBehaviour
{

    public GameObject starBase;
    public Dictionary<int, Star> starData;

    public GameObject lineBase;
    public List<Line> lineData;

    // Start is called before the first frame update
    void Start()
    {
        starData=ReadStar();
        foreach (Star st in starData.Values)
        {
            st.gen(starBase);
        }

        lineData = ReadLine();
        foreach (Line l in lineData)
        {
            l.gen(lineBase,starData);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Line> ReadLine()
    {
        string[] ta = Resources.Load<TextAsset>("mapSolarSystemJumps").text.Split('\n');
        List<Line> dic = new List<Line>();

        for(int i = 0; i < ta.Length; i++)
        {
            if (ta[i].Length == 0) continue;

            Line l = new Line();
            string[] splited = ta[i].Split(',');

            l.fromregionid = int.Parse(splited[0]);
            l.fromconstellationid = int.Parse(splited[1]);
            l.fromsolarsystemid = int.Parse(splited[2]);
            l.tosolarsystemid = int.Parse(splited[3]);
            l.toconstellationid = int.Parse(splited[4]);
            l.toregionid = int.Parse(splited[5]);

            dic.Add(l);
        }

        return dic;
    }

    public Dictionary<int, Star> ReadStar()
    {
        string[] ta = Resources.Load<TextAsset>("mapSolarSystems").text.Split('\n');
        Dictionary<int, Star> dic = new Dictionary<int, Star>();

        for(int i=0; i < ta.Length; i++)
        {
            if (ta[i].Length == 0) continue;

            Star s = new Star();
            string[] splited = ta[i].Split(',');
            //print(ta[i]);

            s.regionid = int.Parse(splited[0]);
            s.constellationid = int.Parse(splited[1]);
            s.solarsystemid = int.Parse(splited[2]);
            s.solarsystemname = splited[3];

            s.x = float.Parse(splited[4]) / 500000000000000;
            s.y = float.Parse(splited[5]) / 500000000000000;
            s.z = float.Parse(splited[6]) / 600000000000000;
            s.xmin = float.Parse(splited[7]);
            s.xmax = float.Parse(splited[8]);
            s.ymin = float.Parse(splited[9]);
            s.ymax = float.Parse(splited[10]);
            s.zmin = float.Parse(splited[11]);
            s.zmax = float.Parse(splited[12]);
            s.luminosity = float.Parse(splited[13]);

            s.border = splited[14] == "1" ? true : false;
            s.fringe = splited[15] == "1" ? true : false;
            s.corridor = splited[16] == "1" ? true : false;
            s.hub = splited[17] == "1" ? true : false;
            s.international = splited[18] == "1" ? true : false;
            s.regional = splited[19] == "1" ? true : false;
            s.constellation = splited[20] == "1" ? true : false;

            s.security = float.Parse(splited[21]);
            //22 NULL
            s.radius = float.Parse(splited[23]);
            s.suntypeid = int.Parse(splited[24]);
            s.securityclass = splited[25];

            dic.Add(s.solarsystemid, s);
        }

        return dic;
    }
}

public class Star
{
    public int regionid, constellationid, solarsystemid, suntypeid;
    public string solarsystemname, securityclass;
    public float x, y, z, xmin, xmax, ymin, ymax, zmin, zmax, luminosity, security, radius;
    public bool border, fringe, corridor, hub, international, regional, constellation;
    //NULL DATA : FACTIONID

    public GameObject starObject;

    public GameObject gen(GameObject starBase)
    {
        GameObject go = GameObject.Instantiate<GameObject>(starBase);
        go.transform.position = new Vector3(x, y, z);
        starObject = go;
        return go;
    }
}

public class Line
{
    public int fromregionid, fromconstellationid, fromsolarsystemid, tosolarsystemid, toconstellationid, toregionid;
    public GameObject lineObject;

    public GameObject gen(GameObject lineBase, Dictionary<int,Star> dict)
    {
        GameObject go = GameObject.Instantiate<GameObject>(lineBase);

        LineRenderer lr = go.GetComponent<LineRenderer>();
        lr.SetPosition(0, dict[fromsolarsystemid].starObject.transform.position);
        lr.SetPosition(1, dict[tosolarsystemid].starObject.transform.position);

        if (fromregionid != toregionid)
        {
            lr.startColor = Color.blue;
            lr.endColor = Color.blue;
        }

        lineObject = go;
        return go;
    }
}