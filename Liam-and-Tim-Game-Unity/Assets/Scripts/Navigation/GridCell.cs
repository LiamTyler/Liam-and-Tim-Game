using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell {
  public GridCell() {
    Init(0, false);
  }
  public GridCell(float cost, bool wall) {
    Init(cost, wall);
  }
  void Init(float cost, bool wall) {
    m_cost = cost;
    m_isWall = wall;
  }

  public float Cost() { return m_cost; }
  public void Cost(float c) { m_cost = c; }
  public bool IsWall() { return m_isWall; }
  public void IsWall(bool b) { m_isWall = b; }

  private float m_cost;
  private bool m_isWall;
}