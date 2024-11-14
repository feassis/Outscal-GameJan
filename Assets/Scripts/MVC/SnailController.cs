using UnityEngine;
using UnityEngine.Rendering;

public class SnailController 
{
    private SnailModel model;
    private SnailView view;

    public SnailController(SnailModel model, SnailView view, Vector3 spawnPos)
    {
        this.model = model;
        this.view = GameObject.Instantiate<SnailView>(view, spawnPos, Quaternion.identity);
        this.view.transform.position = spawnPos;
        this.view.SetController(this);
    }

    public Transform GetTarget() => model.Target;
    public float GetFollowRange() => model.FollowRange;

    public void TouchPlayer() => GameService.Instance.EndGame(false);
}

