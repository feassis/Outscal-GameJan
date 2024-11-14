using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerView>(out PlayerView playerView))
        {
            GameService.Instance.EndGame(true);
        }
    }
}
