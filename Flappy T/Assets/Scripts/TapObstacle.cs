using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TapObstacle : Obstacle
{
    public int minTaps;
    public int maxTaps;

    public StringEvent TapsUpdated;
    public UnityEvent CompletedTqps;

    private int targetTaps;
    private int currentTaps;

    void Update()
    {
          if (Input.GetMouseButtonDown(0) && !GameManager.instance.GameOver && currentTaps > 0)
          {
              currentTaps = Mathf.Max(--currentTaps, 0);
              TapsUpdated.Invoke(currentTaps.ToString());

              if (currentTaps == 0)
              {
                  CompletedTqps.Invoke();
              }
          }
    }

    public override void Reset()
    {
        base.Reset();
        targetTaps = Random.Range(minTaps, maxTaps + 1);
        currentTaps = targetTaps;
        TapsUpdated.Invoke(targetTaps.ToString());
    }
}
