using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int points;
    public TextMeshProUGUI pointsCounter;

    [Header("Points To Gain")]
    public int bodyShotPoints;

    void Update()
    {
        pointsCounter.text = points.ToString();
    }

    public void Purchasing(int cost)
    {
        points = points - cost;
    }

    public void Damage(int pointsToGain)
    {
        points = points + pointsToGain;
    }

    public void AddPoints(int PointsToAdd)
    {
        points = points + PointsToAdd;
    }
}