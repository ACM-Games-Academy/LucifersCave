using UnityEngine;

public class CollectBook : MonoBehaviour, IInteractable
{
    public bool finalPhaseGame;
    public GameObject finalBook;
    public GameObject bossFightHUD;
    public GameObject enemyBoss;

    void Start()
    {
        finalBook.SetActive(true);
    }

    public void Interact()
    {
        finalPhaseGame = true;
        finalBook.SetActive(false);
    }

    public void EnableBossFight()
    {

    }
}
