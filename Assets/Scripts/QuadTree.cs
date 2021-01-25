using System.Collections.Generic;
using UnityEngine;
//https://gamedevelopment.tutsplus.com/tutorials/quick-tip-use-quadtrees-to-detect-likely-collisions-in-2d-space--gamedev-374
public class QuadTree
{
    private QuadTree[] children;
    [SerializeField] List<Rect> objectList;
    [SerializeField] Rect bounds;
    [SerializeField] float maxObjs = 1;
    [SerializeField] int maxLevel = 5;
    private int level;
    bool divided;

    private float width;
    private float height;
    Vector3 position;

    public QuadTree(int level, Rect bounds)
    {
        this.level = level;
        objectList = new List<Rect>();
        this.bounds = bounds;
        children = new QuadTree[4];
        width = bounds.max.x - bounds.max.x;
        height = bounds.max.y - bounds.max.y;
        ShowBoundries();
    }

    public void Clear()
    {
        objectList.Clear();

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i] != null)
            {
                children[i].Clear();
                children[i] = null;
            }
        }
    }
    void Split()
    {
        float subWidth = (bounds.width / 2);
        float subHeight = (bounds.height / 2);
        float x = bounds.x;
        float y = bounds.y;

        children[0] = new QuadTree(level + 1, new Rect(x + subWidth, y, subWidth, subHeight));
        children[1] = new QuadTree(level + 1, new Rect(x, y, subWidth, subHeight));
        children[2] = new QuadTree(level + 1, new Rect(x, y + subHeight, subWidth, subHeight));
        children[3] = new QuadTree(level + 1, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));


    }
    int GetIndex(Rect pRect)
    {
        int i = -1;
        float verticalMidpoint = bounds.x + (bounds.width / 2);
        float horizontalMidpoint = bounds.y + (bounds.height / 2);

        bool topQuadrant = pRect.y <= horizontalMidpoint;
        bool bottomQuadrant = (pRect.y - pRect.height) >= horizontalMidpoint;

        // Check if object is in just right quad
        if (pRect.x + pRect.width > verticalMidpoint)
        {
            if (topQuadrant)
            {
                i = 0;
            }
            else if (bottomQuadrant)
            {
                i = 3;
            }

        }
        // Check if object is in just left quad
        else if (pRect.x - pRect.width < verticalMidpoint)
        {
            if (topQuadrant)
            {
                i = 1;
            }
            else if (bottomQuadrant)
            {
                i = 2;
            }

        }
        return i;
    }
    public void Insert(Rect pRect)
    {
        if (children[0] != null)
        {
            int i = GetIndex(pRect);
            if (i != -1)
            {
                children[i].Insert(pRect);
            }

            return;
        }
        objectList.Add(pRect);

        if (objectList.Count > maxObjs && level < maxLevel)
        {
            if (children[0] == null)
            {
                Split();
            }
            int i = 0;
            while (i < objectList.Count)
            {
                int index = GetIndex(objectList[i]);
                if (index != -1)
                {
                    children[index].Insert(objectList[i]);
                    objectList.RemoveAt(i);
                }
                else
                    i++;
            }


        }
    }
    public void ShowBoundries()
    {
        float x = bounds.x;
        float y = bounds.y;
        float w = bounds.width;
        float h = bounds.height;

        Vector2 bottomLeftPoint = new Vector2(x, y);
        Vector2 bottomRightPoint = new Vector2(x + w, y);
        Vector2 topRightPoint = new Vector2(x + w, y + h);
        Vector2 topLeftPoint = new Vector2(x, y + h);

        Debug.DrawLine(bottomLeftPoint, bottomRightPoint, Color.red, 100f);   //bottomLine
        Debug.DrawLine(bottomLeftPoint, topLeftPoint, Color.red, 100f);       //leftLine
        Debug.DrawLine(bottomRightPoint, topRightPoint, Color.red, 100f);     //rightLine
        Debug.DrawLine(topLeftPoint, topRightPoint, Color.red, 100f);         //topLine
    }
    public void ShowBound(float x, float y, float h, float w)
    {
        Vector2 bottomLeftPoint = new Vector2(x, y);
        Vector2 bottomRightPoint = new Vector2(x + w, y);
        Vector2 topRightPoint = new Vector2(x + w, y + h);
        Vector2 topLeftPoint = new Vector2(x, y + h);

        Debug.DrawLine(bottomLeftPoint, bottomRightPoint, Color.green, 50f);   //bottomLine
        Debug.DrawLine(bottomLeftPoint, topLeftPoint, Color.green, 50f);       //leftLine
        Debug.DrawLine(bottomRightPoint, topRightPoint, Color.green, 50f); //rightLine
        Debug.DrawLine(topLeftPoint, topRightPoint, Color.green, 50f);     //topLine
    }

}
