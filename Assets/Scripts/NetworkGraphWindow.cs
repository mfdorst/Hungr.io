using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGraphWindow : MonoBehaviour
{
    [SerializeField] private Sprite nodeSprite;
    [SerializeField] private float nodeSize;
    private RectTransform _graphContainer;
    private void Awake()
    {
        _graphContainer = transform.Find("Graph Container").GetComponent<RectTransform>();
        CreateNetwork(new List<int> {5, 10, 5});
    }

    private void CreateNode(Vector2 anchoredPosition)
    {
        var node = new GameObject("Node", typeof(Image));
        node.transform.SetParent(_graphContainer, false);
        node.GetComponent<Image>().sprite = nodeSprite;
        var rectTransform = node.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(nodeSize, nodeSize);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, y: 0);
    }

    private void CreateLayer(int nodeCount, float x)
    {
        var rectTransform = GetComponent<RectTransform>();
        var viewHeight = rectTransform.rect.height;
        var interval = viewHeight / (nodeCount + 1f);
        for (var i = 1; i <= nodeCount; i++)
        {
            CreateNode(new Vector2(x, i * interval));
        }
    }

    private void CreateNetwork(List<int> layers)
    {
        var rectTransform = GetComponent<RectTransform>();
        var viewWidth = rectTransform.rect.width;
        var interval = viewWidth / (layers.Count + 1f);
        for (var i = 0; i < layers.Count; i++)
        {
            CreateLayer(layers[i], (i + 1) * interval);
        }
    }
}
