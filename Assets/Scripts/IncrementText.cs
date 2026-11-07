using System.Collections;
using UnityEngine;
using TMPro;

public class IncrementText : MonoBehaviour
{
    public float maxLife;
    public float verticalDistance;

    public Color positiveColor;
    public Color negativeColor;
    
    private float _life;
    private TextMeshProUGUI _text;
    private Color _color;
    private Vector3 _startPos;
    private Vector3 _finalPos;
    
    
    /// <summary>
    /// Sets up the increment text, activates it, then starts the animation.
    /// </summary>
    /// <param name="amount">The amount incremented to display.</param>
    public void Trigger(int amount)
    {
        SetUp(amount);
        gameObject.SetActive(true);
        StartCoroutine(Animate());
    }

    /// <summary>
    /// Set up text, color, and position information for animation.
    /// </summary>
    /// <param name="amount">The amount incremented to display.</param>
    private void SetUp(int amount)
    {
        _text = GetComponent<TextMeshProUGUI>();
        _color = _text.color;
        
        _startPos = _text.transform.localPosition;
        _finalPos = new Vector3(_startPos.x, _startPos.y + verticalDistance, _startPos.z);

        if (amount >= 0)
        {
            _text.text = "+" + amount;
            _color = positiveColor;
        }
        else
        {
            _text.text = amount.ToString();
            _color = negativeColor;
        }
    }
    
    /// <summary>
    /// Runs the animation for _maxLife amount of time, fading the number to transparent and moving vertically up.
    /// </summary>
    private IEnumerator Animate()
    {
        while (_life < maxLife)
        {
            _life += Time.deltaTime;
            var step = _life / maxLife;
            
            transform.localPosition = Vector3.Lerp(_startPos, _finalPos, step);
            
            var color = _color;
            color.a = Mathf.Lerp(color.a, 0f, step);
            _text.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }
}
