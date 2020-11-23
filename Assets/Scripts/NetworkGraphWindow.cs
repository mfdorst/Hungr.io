using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGraphWindow : MonoBehaviour
{
    [SerializeField] private Sprite nodeSprite;
    [SerializeField] private float nodeSize;
    [SerializeField] private float edgeWidth;
    private RectTransform _graphContainer;
    private void Awake()
    {
        _graphContainer = transform.Find("Graph Container").GetComponent<RectTransform>();
        CreateNetwork(new List<int> {5, 10, 5});
    }

    private List<Vector2> GetNodePositions(List<int> layerSizes, float viewWidth, float viewHeight)
    {
        var positions = new List<Vector2>();
        var xInterval = viewWidth / (layerSizes.Count + 1);
        for (var i = 0; i < layerSizes.Count; i++)
        {
            var yInterval = viewHeight / (layerSizes[i] + 1);
            for (var j = 0; j < layerSizes[i]; j++)
            {
                positions.Add(new Vector2((i + 1) * xInterval, (j + 1) * yInterval));
            }
        }
        return positions;
    }

    private GameObject CreateNode(Vector2 position)
    {
        var node = new GameObject("Node", typeof(Image));
        node.transform.SetParent(_graphContainer, false);
        node.GetComponent<Image>().sprite = nodeSprite;
        var rectTransform = node.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        rectTransform.sizeDelta = new Vector2(nodeSize, nodeSize);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, y: 0);
        return node;
    }

    private void CreateNodes(List<Vector2> nodePositions)
    {
        foreach (var nodePosition in nodePositions)
        {
            CreateNode(nodePosition);
        }
    }

    private void CreateEdge(Vector2 firstVertex, Vector2 secondVertex)
    {
        var edge = new GameObject("Edge", typeof(Image));
        edge.transform.SetParent(_graphContainer, false);
        var rectTransform = edge.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(100, edgeWidth);
        rectTransform.anchoredPosition = firstVertex;
    }

    private void CreateEdges(List<Vector2> nodePositions, List<int> layerSizes)
    {
        var startingIndex = 0;
        for (var i = 0; i < layerSizes.Count - 1; i++)
        {
            var leftLayerSize = layerSizes[i];
            var rightLayerSize = layerSizes[i + 1];
            for (var j = 0; j < leftLayerSize; j++)
            {
                for (var k = 0; k < rightLayerSize; k++)
                {
                    CreateEdge(nodePositions[startingIndex + j], nodePositions[startingIndex + leftLayerSize + k]);
                }
            }

            startingIndex += leftLayerSize;
        }
    }

    private void CreateNetwork(List<int> layerSizes)
    {
        var rect = GetComponent<RectTransform>().rect;
        var nodePositions = GetNodePositions(layerSizes, rect.width, rect.height);
        CreateNodes(nodePositions);
        CreateEdges(nodePositions, layerSizes);
    }
}
