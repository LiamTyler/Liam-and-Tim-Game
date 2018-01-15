using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AStar : MonoBehaviour {

  public class Cell {
    public int row;
    public int col;
    public float f;
    public float g;
    public Cell parent;
    public Cell(int r, int c) {
      row = r; col = c;
      f = 0; g = 0; parent = null;
    }
    public Cell() {
      row = 0; col = 0;
      f = 0; g = 0; parent = null;
    }
  }

  public class PathCell {
    public Cell cell;
    public GameObject square;
  }

  public string m_Filename;
  public GameObject m_StartSquare;
  public GameObject m_FinishSquare;
  public GameObject m_WallSquare;
  public GameObject m_WalkSquare;

  private int m_Width;
  private int m_Height;
  private int[,] m_Grid;
  private Vector2Int m_TopLeftCorner;
  private GameObject m_Start;
  private GameObject m_Finish;
  private Stack<PathCell> m_Path;

  private

	// Use this for initialization
	void Start () {
    LoadMap();
	}

  public void LoadMap() {
    m_Path = new Stack<PathCell>();
	  StreamReader reader = new StreamReader(m_Filename);
    string line;
    line = reader.ReadLine();
    string[] subs = line.Split(' ');
    m_Width = int.Parse(subs[0]);
    m_Height = int.Parse(subs[1]);
    m_Grid = new int[m_Height,m_Width];
    Vector2Int mid = new Vector2Int((int) m_Width / 2, (int)m_Height / 2);
    m_TopLeftCorner = new Vector2Int(-mid.x, mid.y);
    for (int r = 0; r < m_Height; r++) {
      line = reader.ReadLine();
      char[] chars = line.ToCharArray();
      for (int c = 0; c < m_Width; c++) {
        char ch = chars[c];
        int f;
        if (ch == 'S') {
          m_Start = Instantiate(m_StartSquare, GridCoordToWorld(r, c), Quaternion.identity);
          f = 1;
        } else if (ch == 'F') {
          m_Finish = Instantiate(m_FinishSquare,  GridCoordToWorld(r, c), Quaternion.identity);
          f = 1;
        } else if (ch == 'W') {
          f = 99999999;
          Instantiate(m_WallSquare,  GridCoordToWorld(r, c), Quaternion.identity);
        } else {
          f = int.Parse(ch.ToString());
          // Instantiate(m_WalkSquare, new Vector3(c - mid[0], r - mid[1], -.1f), Quaternion.identity);
        }
        m_Grid[r,c] = f;
      }
    }
    reader.Close();
  }

  Vector2Int WorldToGridCoord(Vector2 pos) {
    Vector2Int w = new Vector2Int(Mathf.RoundToInt(pos.x),  Mathf.RoundToInt(pos.y));
    return new Vector2Int(m_TopLeftCorner.y - w.y, w.x - m_TopLeftCorner.x);
  }

  Vector3 GridCoordToWorld(int row, int col) {
    Vector3 w;
    w.x = col + m_TopLeftCorner.x;
    w.y = m_TopLeftCorner.y - row;
    w.z = -.1f;
    return w;
  }

  float Heuristic(Cell current, Cell goal) {
    float dy = current.row - goal.row;
    float dx = current.col - goal.col;
    return Mathf.Abs(dx) + Mathf.Abs(dy);
  }

  Cell GetLowestCost(List<Cell> list) {
    int LowestIndex = 0;
    for (int i = 1; i < list.Count; i++) {
      if (list[i].f < list[LowestIndex].f) {
        LowestIndex = i;
      }
    }
    Cell lowest = list[LowestIndex];
    list.RemoveAt(LowestIndex);
    return lowest;
  }

  bool IsValidAndNotAWall(Cell node) {
    int r = node.row;
    int c = node.col;
    if (0 <= c && c < m_Width &&
        0 <= r && r < m_Height &&
        m_Grid[r,c] != 99999999)
      return true;
    return false;
  }

  List<Cell> GetNeighbors(Cell node) {
    int r = node.row;
    int c = node.col;
    List<Cell> potential = new List<Cell> {
      new Cell(r, c-1),
      new Cell(r, c+1),
      new Cell(r+1, c),
      new Cell(r-1, c),
    };
    List<Cell> neighbors = new List<Cell>();
    for (int i = 0; i < potential.Count; i++) {
      if (IsValidAndNotAWall(potential[i])) {
        Cell n = potential[i];
        n.g = node.g + .5f*m_Grid[node.row,node.col] + .5f*m_Grid[n.row,n.col];
        n.parent = node;
        neighbors.Add(potential[i]);
      }
    }
    return neighbors;
  }

  bool CellInList(List<Cell> list, Cell node, out int index) {
    index = -1;
    for (int i = 0; i < list.Count; i++) {
      if (list[i].row == node.row && list[i].col == node.col) {
        index = i;
        return true;
      }
    }   
    return false;
  }

  public Cell PathFind() {
    List<Cell> open_list = new List<Cell>();
    List<Cell> closed_list = new List<Cell>();
    Cell goal = new Cell();
    Vector2Int p = WorldToGridCoord(m_Finish.transform.position);
    goal.row = p.x;
    goal.col = p.y;
    Cell start = new Cell();
    p = WorldToGridCoord(m_Start.transform.position);
    start.row = p.x;
    start.col = p.y;
    start.g = 0;
    start.f = start.g + Heuristic(start, goal);
    open_list.Add(start);
    
    while (open_list.Count != 0) {
      Cell current = GetLowestCost(open_list);
      if (current.row == goal.row && current.col == goal.col) {
        return current;
      }
      closed_list.Add(current);
      List<Cell> neighbors = GetNeighbors(current);
      for(int i = 0; i < neighbors.Count; i++) {
        Cell neighbor = neighbors[i];
        int index;
        if (!CellInList(closed_list, neighbor, out index)) {
          if (!CellInList(open_list, neighbor, out index)) {
            neighbor.f = neighbor.g + Heuristic(neighbor, current);
            open_list.Add(neighbor);
          } else {
            Cell open_neighbor = open_list[index];
            if (neighbor.g < open_neighbor.g) {
              open_neighbor.g = neighbor.g;
              open_neighbor.parent = current;
            }
          }
        }
      }
    }
    return null;
  }

  void ClearPath() {
    while (m_Path.Count != 0) {
      PathCell c = m_Path.Pop();
      if (c.square != null)
        Destroy(c.square);
    }
  }

	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
      ClearPath();
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mp.x = Mathf.RoundToInt(mp.x);
      mp.y = Mathf.RoundToInt(mp.y);
      m_Start.transform.position = new Vector3(mp.x, mp.y, -.1f);
    }
    if (Input.GetMouseButtonUp(1)) {
      ClearPath();
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mp.x = Mathf.RoundToInt(mp.x);
      mp.y = Mathf.RoundToInt(mp.y);
      m_Finish.transform.position = new Vector3(mp.x, mp.y, -.1f);
    }
    if (Input.GetKeyUp(KeyCode.Space)) {
      Cell ret = PathFind();
      if (ret == null) {
        Debug.Log("No path found");
      } else {
        Debug.Log("Path found");
        m_Path.Push(new PathCell { cell = ret, square = null });
        ret = ret.parent;
        while (ret.parent != null) {
          GameObject sq = Instantiate(m_WalkSquare, GridCoordToWorld(ret.row, ret.col), Quaternion.identity);
          m_Path.Push(new PathCell { square = sq, cell = ret });
          ret = ret.parent;
        }
      }
    }
	}
}
