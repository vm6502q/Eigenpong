using UnityEngine;
using UnityEngine.UI;

public enum UIScrollAxis
{
    X = 0,
    Y = 1,
    Z = 2,
    I = 0,
    C = 1,
    A = 2
}

public class UIScroll : MonoBehaviour
{
    public UIScrollAxis axis { get; private set; }
    public GameObject x;
    public GameObject y;
    public GameObject z;

    protected const float DEAD_TIME = 0.15f;
    protected float deadTimer;

    // Start is called before the first frame update
    void Start()
    {
        axis = UIScrollAxis.X;
        x.SetActive(true);
        y.SetActive(false);
        z.SetActive(false);
    }

    void Update()
    {
        deadTimer += Time.deltaTime;
    }

    public void SetActive(UIScrollAxis axisChoice)
    {
        axis = axisChoice;

        x.SetActive(axis == UIScrollAxis.X);
        y.SetActive(axis == UIScrollAxis.Y);
        z.SetActive(axis == UIScrollAxis.Z);
    }

    public bool Scroll(bool up)
    {
        if (deadTimer < DEAD_TIME)
        {
            return false;
        }

        deadTimer = 0.0f;

        axis = up
            ? (axis == UIScrollAxis.X ? UIScrollAxis.Y : (axis == UIScrollAxis.Y ? UIScrollAxis.Z : UIScrollAxis.X))
            : (axis == UIScrollAxis.X ? UIScrollAxis.Z : (axis == UIScrollAxis.Y ? UIScrollAxis.X : UIScrollAxis.Y));

        SetActive(axis);

        return true;
    }
}
