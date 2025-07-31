using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(PlayAudio);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void PlayAudio()
    {
        AudioManager.Instance.PlaySFXClip("Button");
    }
}
