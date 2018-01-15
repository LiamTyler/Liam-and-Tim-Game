using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AStar : MonoBehaviour {
  public string m_Filename;
  public GameObject m_StartSquare;
  public GameObject m_FinishSquare;
  public GameObject m_WallSquare;
  public GameObject m_WalkSquare;

  private int m_Width;
  private int m_Height;
  private int[,] m_Grid;
  private Vector2 m_TopLeftCorner;
  private GameObject m_Start;
  private GameObject m_Finish;

  private

	// Use this for initialization
	void Start () {
    LoadMap();
	}

  public void LoadMap() {
	  StreamReader reader = new StreamReader(m_Filename);
    string line;
    line = reader.ReadLine();
    string[] subs = line.Split(' ');
    m_Width = int.Parse(subs[0]);
    m_Height = int.Parse(subs[1]);
    m_Grid = new int[m_Height,m_Width];
    Vector2 mid = new Vector2((int) m_Width / 2, (int)m_Height / 2);
    m_TopLeftCorner = -mid;
    for (int r = 0; r < m_Height; r++) {
      line = reader.ReadLine();
      char[] chars = line.ToCharArray();
      for (int c = 0; c < m_Width; c++) {
        char ch = chars[c];
        int f;
        if (ch == 'S') {
          m_Start = Instantiate(m_StartSquare, new Vector3(c - mid[0], r - mid[1], -.1f), Quaternion.identity);
          f = 1;
        } else if (ch == 'F') {
          m_Finish = Instantiate(m_FinishSquare, new Vector3(c - mid[0], r - mid[1], -.1f), Quaternion.identity);
          f = 1;
        } else if (ch == 'W') {
          f = 99999999;
          GameObject sq = Instantiate(m_WallSquare, new Vector3(c - mid[0], r - mid[1], -.1f), Quaternion.identity);
        } else {
          f = int.Parse(ch.ToString());
        }
        m_Grid[r,c] = f;
      }
    }
    reader.Close();
  }

  Vector2 WorldToGridCoord(Vector2 pos) {
    return m_TopLeftCorner - pos;
  }

  

  public void PathFind() {
    
  }

	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mp.x = Mathf.RoundToInt(mp.x);
      mp.y = Mathf.RoundToInt(mp.y);
      m_Start.transform.position = new Vector3(mp.x, mp.y, -.1f);
    }
    if (Input.GetMouseButtonUp(1)) {
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mp.x = Mathf.RoundToInt(mp.x);
      mp.y = Mathf.RoundToInt(mp.y);
      m_Finish.transform.position = new Vector3(mp.x, mp.y, -.1f);
    }
	}
}
