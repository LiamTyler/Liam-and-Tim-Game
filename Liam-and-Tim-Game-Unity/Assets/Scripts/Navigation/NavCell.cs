using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavCell  {
  public NavCell() {
    Init(new Vector2(0, 0), 0, 0, null);
  }
  public NavCell(Vector2 pos) {
    Init(pos, 0, 0, null);
  }
  public NavCell(Vector2 pos, float f, float g, NavCell parent) {
    Init(pos, f, g, parent);
  }
  void Init(Vector2 pos, float f, float g, NavCell parent) {
    m_position = pos;
    m_f = f;
    m_g = g;
    m_parent = parent;
  }

  public Vector2 Pos() { return m_position; }
  public float F() { return m_f; }
  public float G() { return m_g; }
  public NavCell Parent() { return m_parent; }

  public void Pos(Vector2 p) { m_position = p; }
  public void F(float f) { m_f = f; }
  public void G(float g) { m_g = g; }
  public void Parent(NavCell p) { m_parent = p; }

  private Vector2 m_position;
  private float m_f;
  private float m_g;
  public NavCell m_parent;
}
