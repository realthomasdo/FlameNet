using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int posX;
    private int posY;

    public float temperature = 100f;

    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }

    private void Update()
    {
        float bindTemp = (temperature / 100);
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.color = new Color(1f, 1f - bindTemp, 1f - bindTemp, 1f);
    }
}
