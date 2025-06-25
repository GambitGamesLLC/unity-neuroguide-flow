using UnityEngine;

public class FadeInSetActive : MonoBehaviour
{

    public GameObject fadeScreen;

    void Start()
    {

        if (fadeScreen != null)
        {
            fadeScreen.SetActive(true);
        }

    }
}