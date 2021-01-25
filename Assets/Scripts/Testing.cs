using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    QuadTree quad;
    [SerializeField] List<Rect> allObjectsList;
    void Start()
    {
        quad = new QuadTree(0, new Rect(Vector3.zero, new Vector3(20, 20)));
        quad.ShowBound(0, 0, 20, 20);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Debug.Log($"{mousePosition}");
            allObjectsList.Add(new Rect(mousePosition, new Vector2(0.2f, 0.2f)));
        }
        quad.Clear();
        for (int i = 0; i < allObjectsList.Count; i++)
        {
            quad.Insert(allObjectsList[i]);
        }
    }
}
