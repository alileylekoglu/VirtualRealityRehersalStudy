using UnityEngine;
using TMPro; // Use TextMesh Pro's namespace

public class ScaleObject : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject currentObject;

    public float scaleIncrement = 1.0f;
    public Vector3 maxScale = new Vector3(5.0f, 5.0f, 5.0f);
    public Vector3 minScale = new Vector3(1.0f, 1.0f, 1.0f);

    private Vector3 originalScale;

    // Use TextMeshProUGUI instead of Text
    public TextMeshProUGUI xScaleText;
    public TextMeshProUGUI yScaleText;
    public TextMeshProUGUI zScaleText;

    private enum Axis
    {
        X,
        Y,
        Z
    }

    private void Start()
    {
        if (objects.Length > 0)
        {
            currentObject = objects[0];
            originalScale = currentObject.transform.localScale;
            UpdateScaleTexts();
        }
    }

    public void SelectObject(int index)
    {
        if (index < 0 || index >= objects.Length) return;

        if (currentObject)
        {
            currentObject.SetActive(false);
        }

        currentObject = objects[index];
        currentObject.SetActive(true);
        originalScale = currentObject.transform.localScale;
        UpdateScaleTexts();
    }

    public void AdjustScaleX(bool increase)
    {
        AdjustScale(Axis.X, increase);
        UpdateScaleTexts();
    }

    public void AdjustScaleY(bool increase)
    {
        AdjustScale(Axis.Y, increase);
        UpdateScaleTexts();
    }

    public void AdjustScaleZ(bool increase)
    {
        AdjustScale(Axis.Z, increase);
        UpdateScaleTexts();
    }

    private void AdjustScale(Axis axis, bool increase)
    {
        Vector3 newScale = currentObject.transform.localScale;
        float change = increase ? scaleIncrement : -scaleIncrement;

        switch (axis)
        {
            case Axis.X:
                newScale.x = Mathf.Clamp(newScale.x + change, minScale.x, maxScale.x);
                break;
            case Axis.Y:
                newScale.y = Mathf.Clamp(newScale.y + change, minScale.y, maxScale.y);
                break;
            case Axis.Z:
                newScale.z = Mathf.Clamp(newScale.z + change, minScale.z, maxScale.z);
                break;
        }

        currentObject.transform.localScale = newScale;
    }

    public void ResetScale(string axis)
    {
        Vector3 newScale = currentObject.transform.localScale;

        switch (axis)
        {
            case "X":
                newScale.x = originalScale.x;
                break;
            case "Y":
                newScale.y = originalScale.y;
                break;
            case "Z":
                newScale.z = originalScale.z;
                break;
        }

        currentObject.transform.localScale = newScale;
        UpdateScaleTexts();
    }

    private void UpdateScaleTexts()
    {
        if (currentObject)
        {
            xScaleText.text = "Scale: " + currentObject.transform.localScale.x.ToString("F2");
            yScaleText.text = "Scale: " + currentObject.transform.localScale.y.ToString("F2");
            zScaleText.text = "Scale: " + currentObject.transform.localScale.z.ToString("F2");
        }
    }
}
