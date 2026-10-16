using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class BasicUITypewriter : MonoBehaviour
{
    [Header("Log Display")]
    [SerializeField] private RectTransform logPanel;
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField, TextArea(4, 8)] private string openingMessage;
    [SerializeField] private Color textColor = new Color(0.62f, 1f, 0.62f, 1f);
    [SerializeField, Min(8f)] private float fontSize = 24f;
    [SerializeField] private Vector2 textInset = new Vector2(28f, 18f);

    [Header("Typing")]
    [SerializeField, Min(0f)] private float characterDelay = 0.035f;
    [SerializeField, Min(0f)] private float linePause = 0.15f;
    [SerializeField] private string cursor = "_";
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private AudioClip[] typingSounds;
    [SerializeField, Range(0f, 0.5f)] private float pitchRandomness = 0.08f;

    private TextMeshProUGUI outputText;
    private AudioSource audioSource;
    private Coroutine typingCoroutine;
    private bool skipRequested;

    private void Awake()
    {
        if (logPanel == null)
        {
            GameObject logObject = GameObject.Find("Log");
            if (logObject != null)
            {
                logPanel = logObject.GetComponent<RectTransform>();
            }
        }

        if (logPanel == null)
        {
            Debug.LogError("BasicUITypewriter: Assign the Log panel before using the typewriter.", this);
            enabled = false;
            return;
        }

        CreateOutputText();

        if (typingSounds != null && typingSounds.Length > 0)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.playOnAwake = false;
            audioSource.volume = 0.12f;
        }
    }

    private void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    private void Update()
    {
        if (typingCoroutine != null && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            skipRequested = true;
        }
    }

    public void Play()
    {
        if (outputText == null)
        {
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        skipRequested = false;
        typingCoroutine = StartCoroutine(TypeMessage());
    }

    private void CreateOutputText()
    {
        GameObject textObject = new GameObject("TypewriterText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.SetParent(logPanel, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = textInset;
        textRect.offsetMax = -textInset;
        textRect.SetAsLastSibling();

        outputText = textObject.GetComponent<TextMeshProUGUI>();
        outputText.font = fontAsset != null ? fontAsset : TMP_Settings.defaultFontAsset;
        outputText.fontSize = fontSize;
        outputText.color = textColor;
        outputText.alignment = TextAlignmentOptions.TopLeft;
        outputText.textWrappingMode = TextWrappingModes.Normal;
        outputText.overflowMode = TextOverflowModes.Overflow;
        outputText.raycastTarget = false;
        outputText.text = string.Empty;
    }

    private IEnumerator TypeMessage()
    {
        string visibleText = string.Empty;
        string message = openingMessage ?? string.Empty;

        for (int i = 0; i < message.Length; i++)
        {
            if (skipRequested)
            {
                break;
            }

            char character = message[i];
            visibleText += character;
            outputText.text = visibleText + (i < message.Length - 1 ? cursor : string.Empty);

            PlayTypingSound(character);

            float delay = character == '\n' ? linePause : characterDelay;
            if (delay > 0f)
            {
                yield return new WaitForSeconds(delay);
            }
            else
            {
                yield return null;
            }
        }

        outputText.text = message;
        typingCoroutine = null;
        skipRequested = false;
    }

    private void PlayTypingSound(char character)
    {
        if (char.IsWhiteSpace(character) || audioSource == null || typingSounds == null || typingSounds.Length == 0)
        {
            return;
        }

        AudioClip clip = typingSounds[Random.Range(0, typingSounds.Length)];
        if (clip == null)
        {
            return;
        }

        audioSource.pitch = Random.Range(1f - pitchRandomness, 1f + pitchRandomness);
        audioSource.PlayOneShot(clip);
    }
}
