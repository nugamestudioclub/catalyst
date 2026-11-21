using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    private RectTransform _viewport;
    private TextMeshProUGUI _outputText;
    private AudioSource _audioSource;
    private Coroutine _typingCoroutine;
    private bool _skipRequested;

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
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            _audioSource.playOnAwake = false;
            _audioSource.volume = 0.12f;
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
        if (_typingCoroutine != null && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
        {
            _skipRequested = true;
        }
    }

    public void Play()
    {
        if (_outputText == null)
        {
            return;
        }

        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }

        _skipRequested = false;
        _typingCoroutine = StartCoroutine(TypeMessage());
    }

    private void CreateOutputText()
    {
        var viewportObject = new GameObject("TypewriterViewport", typeof(RectTransform), typeof(RectMask2D));
        _viewport = (RectTransform)viewportObject.transform;
        _viewport.SetParent(logPanel, false);
        _viewport.anchorMin = Vector2.zero;
        _viewport.anchorMax = Vector2.one;
        _viewport.offsetMin = textInset;
        _viewport.offsetMax = -textInset;
        _viewport.SetAsLastSibling();

        var textObject = new GameObject("TypewriterText", typeof(RectTransform), typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));
        var textRect = (RectTransform)textObject.transform;
        textRect.SetParent(_viewport, false);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        _outputText = textObject.GetComponent<TextMeshProUGUI>();
        _outputText.font = fontAsset != null ? fontAsset : TMP_Settings.defaultFontAsset;
        _outputText.fontSize = fontSize;
        _outputText.color = textColor;
        _outputText.alignment = TextAlignmentOptions.TopLeft;
        _outputText.textWrappingMode = TextWrappingModes.Normal;
        _outputText.overflowMode = TextOverflowModes.Overflow;
        _outputText.raycastTarget = false;
        _outputText.text = string.Empty;
    }

    private IEnumerator TypeMessage()
    {
        string message = openingMessage ?? string.Empty;

        for (var i = 0; i < message.Length; i++)
        {
            if (_skipRequested)
            {
                break;
            }

            char character = message[i];
            ShowText(message[..(i + 1)] + (i < message.Length - 1 ? cursor : string.Empty));

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

        ShowText(message);
        _typingCoroutine = null;
        _skipRequested = false;
    }

    private void ShowText(string text)
    {
        _outputText.text = text;

        // Once the log overflows, stick to the bottom so the newest lines stay visible.
        _outputText.alignment = _outputText.preferredHeight > _viewport.rect.height
            ? TextAlignmentOptions.BottomLeft
            : TextAlignmentOptions.TopLeft;
    }

    private void PlayTypingSound(char character)
    {
        if (char.IsWhiteSpace(character) || _audioSource == null || typingSounds == null || typingSounds.Length == 0)
        {
            return;
        }

        AudioClip clip = typingSounds[Random.Range(0, typingSounds.Length)];
        if (clip == null)
        {
            return;
        }

        _audioSource.pitch = Random.Range(1f - pitchRandomness, 1f + pitchRandomness);
        _audioSource.PlayOneShot(clip);
    }
}
