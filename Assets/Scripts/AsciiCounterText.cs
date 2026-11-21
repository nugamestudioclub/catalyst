using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// Shows the number in this text as big ASCII digits, with dim leading zeros.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public sealed class AsciiCounterText : MonoBehaviour, ITextPreprocessor
{
    private const int GlyphRows = 10;

    // Invisible char at both ends so every row has the same width when centered.
    private const string EdgeMarker = "<color=#00000000>.</color>";

    private static readonly string[][] Glyphs =
    {
        new[] { " ___ ", "##\"##", "## ##", "## ##", "## ##", "## ##", "## ##", "## ##", "##_##", " \"\"\" " },
        new[] { " __  ", "\"##  ", " ##  ", " ##  ", " ##  ", " ##  ", " ##  ", " ##  ", "_##_ ", "\"\"\"\" " },
        new[] { "____ ", "\"\"\"##", "   ##", "   ##", "   ##", " ### ", "##   ", "##   ", "##___", "\"\"\"\"\"" },
        new[] { "____ ", "\"\"\"##", "   ##", "   ##", "   ##", " ### ", "   ##", "   ##", "___##", "\"\"\"\" " },
        new[] { "__ __", "## ##", "## ##", "## ##", "## ##", "#####", "   ##", "   ##", "   ##", "   \"\"" },
        new[] { "_____", "##\"\"\"", "##   ", "##   ", "##   ", "#### ", "   ##", "   ##", "___##", "\"\"\"\" " },
        new[] { " ____", "##\"\"\"", "##   ", "##   ", "##   ", "#### ", "## ##", "## ##", "##_##", " \"\"\" " },
        new[] { "_____", "\"\"\"##", "   ##", "   ##", "  ## ", "  ## ", " ##  ", " ##  ", " ##  ", " \"\"  " },
        new[] { " ___ ", "##\"##", "## ##", "## ##", "## ##", " ### ", "## ##", "## ##", "##_##", " \"\"\" " },
        new[] { " ___ ", "##\"##", "## ##", "## ##", "## ##", " ####", "   ##", "   ##", "___##", "\"\"\"\" " },
    };

    [SerializeField, Min(1)] private int minDigits = 8;
    [SerializeField] private Color unlitColor = new Color(0f, 0.21f, 0f, 1f);

    [Tooltip("Shadow text that copies this number.")]
    [SerializeField] private TMP_Text mirror;

    private readonly StringBuilder _builder = new StringBuilder();
    private TMP_Text _text;
    private string _source;
    private string _result;

    private void OnEnable()
    {
        _text = GetComponent<TMP_Text>();
        _text.textPreprocessor = this;
        _text.havePropertiesChanged = true;
    }

    private void OnDisable()
    {
        if (!_text) return;

        if (_text.textPreprocessor == this) _text.textPreprocessor = null;
        _text.havePropertiesChanged = true;
    }

    private void OnValidate()
    {
        _source = null;
        if (_text) _text.havePropertiesChanged = true;
    }

    private void LateUpdate()
    {
        if (mirror && mirror.text != _text.text) mirror.text = _text.text;
    }

    public string PreprocessText(string text)
    {
        if (text == _source) return _result;

        _source = text;
        _result = IsNumber(text) ? BuildCounter(text) : text;
        return _result;
    }

    private static bool IsNumber(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;

        foreach (char character in text)
        {
            if (character is < '0' or > '9') return false;
        }

        return true;
    }

    private string BuildCounter(string digits)
    {
        string padded = digits.PadLeft(minDigits, '0');
        int unlitCount = padded.Length - digits.Length;
        string unlitTag = $"<color=#{ColorUtility.ToHtmlStringRGBA(unlitColor)}>";

        _builder.Clear();
        for (var row = 0; row < GlyphRows; row++)
        {
            if (row > 0) _builder.Append('\n');

            _builder.Append(EdgeMarker).Append(unlitTag);
            for (var i = 0; i < padded.Length; i++)
            {
                if (i == unlitCount) _builder.Append("</color>");
                if (i > 0) _builder.Append(' ');
                _builder.Append(Glyphs[padded[i] - '0'][row]);
            }

            _builder.Append(EdgeMarker);
        }

        return _builder.ToString();
    }
}
