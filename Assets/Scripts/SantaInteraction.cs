using UnityEngine;

public class SantaInteraction : MonoBehaviour
{
    public GameObject questionPanel;
    public GameObject pressEText;

    private bool playerInRange = false;

    void Start()
    {
        questionPanel.SetActive(false);
        pressEText.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenQuestionPanel();
        }
    }

    void OpenQuestionPanel()
    {
        questionPanel.SetActive(true);
        pressEText.SetActive(false);
    }

    public void CloseQuestionPanel()
    {
        questionPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressEText.SetActive(false);
        }
    }
}
