using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialStandView : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject interactableText;
    private TrialStandController controller;

    private PlayerView playerView;

    public void Interact()
    {
        controller.Interact();
    }


    private void Update()
    {
        if(playerView == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void SetController(TrialStandController controller)
    {
        this.controller = controller;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerView>(out PlayerView player))
        {
            playerView = player;
            interactableText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerView>(out PlayerView player))
        {
            playerView = null;
            interactableText.SetActive(false);
        }
    }
}

public class TrialStandController
{
    private TrialStandView view;
    private TrialStandModel model;

    private UIService uiService;

    public TrialStandController(TrialStandView view, TrialStandModel model, Vector3 spawnPos, UIService uiService)
    {
        this.view = GameObject.Instantiate<TrialStandView>(view, spawnPos, Quaternion.identity);
        this.view.SetController(this);
        this.model = model;
        this.uiService = uiService;
    }

    public void Interact()
    {
        uiService.OpenTrialUI(model.Type);
    }
}
