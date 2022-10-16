using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Color color0 = Color.black;
    public Color color1 = Color.blue;

    protected const float DEAD_TIME = 0.15f;
    protected float deadTimer;

    private bool _isActive;
    public bool isActive {
        get
        {
            return _isActive;
        }
        set
        {
            if (value == isActive)
            {
                return;
            }

            _isActive = value;

            myText.color = _isActive ? color1 : color0;
        }
    }

    protected Text myText;
    
    // Start is called before the first frame update
    void Start()
    {
        _isActive = false;
        myText = GetComponent<Text>();
    }

    void Update()
    {
        deadTimer += Time.deltaTime;
    }

    public bool Toggle()
    {
        if (deadTimer < DEAD_TIME)
        {
            return false;
        }

        isActive = !isActive;
        deadTimer = 0.0f;

        return true;
    }
}
