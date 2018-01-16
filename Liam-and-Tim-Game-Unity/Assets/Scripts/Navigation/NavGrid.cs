using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid {
  public int m_width;
  public int m_height;
  private GridCell[,] m_grid;
  private float m_dx;
  private float m_dy;
  private Vector3 m_topLeftCorner;

  public NavGrid(int w, int h, float dx, float dy, Vector3 center) {
    m_width = w;
    m_height = h;
    m_dx = dx;
    m_dy = dy;
    m_topLeftCorner = new Vector3(center.x - dx * (int) (m_width - 1) / 2,
                                  center.y + dy * (int) (m_height - 1) / 2,
                                  center.z);
    m_grid = new GridCell[h, w];
    for (int r = 0; r < h; r++) {
      for (int c = 0; c < w; c++) {
        m_grid[r,c] = new GridCell();
      }
    }
  }

  public void SetGridCell(int r, int c, float cost, bool isWall) {
    m_grid[r,c].Cost(cost);
    m_grid[r,c].IsWall(isWall);
  }
  public GridCell GetGridCell(int r, int c) { return m_grid[r,c]; }
  public GridCell GetGridCell(Vector2 pos) {
    Vector2 p = WorldToGridCoord(pos);
    return GetGridCell((int) p.x, (int) p.y);
  }

  public Vector2 WorldToGridCoord(Vector2 pos) {
    float r = (m_topLeftCorner.y - pos.y) / m_dy;
    float c = (pos.x - m_topLeftCorner.x) / m_dx;
    return new Vector2(Mathf.RoundToInt(r),  Mathf.RoundToInt(c));
  }

  public Vector3 GridCoordToWorld(NavCell cell) {
    return GridCoordToWorld(cell.Pos());
  }
  public Vector3 GridCoordToWorld(Vector2 p) {
    return GridCoordToWorld((int) p.x, (int) p.y);
  }
  public Vector3 GridCoordToWorld(int row, int col) {
    Vector3 w = m_topLeftCorner;
    w.x += m_dx * col;
    w.y -= m_dy * row;
    w.z = m_topLeftCorner.z;
    return w;
  }

  public bool IsValidAndNotAWall(NavCell from, NavCell to) {
    Vector2 new_pos = to.Pos();
    int nr = (int) new_pos.x;
    int nc = (int) new_pos.y;
    if (0 <= nc && nc < m_width &&
        0 <= nr && nr < m_height &&
        !m_grid[nr,nc].IsWall()) {
      int or = (int) from.Pos().x;
      int oc = (int) from.Pos().y;
      if (or != nr || oc != nc) {
        return !m_grid[or,nc].IsWall() && !m_grid[nr, oc].IsWall();
      } else {
        return true;
      }
    }
    return false;
  }

  private float Cost(NavCell node) {
    Vector2 pos = node.Pos();
    int r = (int) pos.x;
    int c = (int) pos.y;
    return m_grid[r, c].Cost();
  }

  public List<NavCell> GetNeighbors(NavCell node) {
    Vector2 pos = node.Pos();
    int r = (int) pos.x;
    int c = (int) pos.y;
    List<NavCell> potential = new List<NavCell> {
      new NavCell(new Vector2(r, c-1)),
      new NavCell(new Vector2(r, c+1)),
      new NavCell(new Vector2(r+1, c)),
      new NavCell(new Vector2(r-1, c)),
      new NavCell(new Vector2(r+1, c-1)),
      new NavCell(new Vector2(r+1, c+1)),
      new NavCell(new Vector2(r-1, c+1)),
      new NavCell(new Vector2(r-1, c-1)),
    };
    List<NavCell> neighbors = new List<NavCell>();
    for (int i = 0; i < potential.Count; i++) {
      if (IsValidAndNotAWall(node, potential[i])) {
        NavCell n = potential[i];
        int nr = (int) n.Pos().x;
        int nc = (int) n.Pos().y;
        // change cost depending on whether the neighbor is diagonal or not
        if (r != nr && c != nc) {
          n.G(node.G() + .7071f*Cost(node) + .7071f*Cost(n));
        } else {
          n.G(node.G() + .5f*Cost(node) + .5f*Cost(n));
        }
        n.Parent(node);
        neighbors.Add(n);
      }
    }
    return neighbors;
  }
}
