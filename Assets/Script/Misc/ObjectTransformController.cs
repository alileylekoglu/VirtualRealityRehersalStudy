using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ObjectTransformController : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform objectToTransform;
    public Slider scaleXSlider;
    public Slider scaleYSlider;
    public Slider scaleZSlider;
    public Button[] colorButtons; // Buttons for the 8 basic colors

    private Vector3 networkScale = Vector3.one;
    private Color networkColor = Color.white;

    void Start()
    {
        if (objectToTransform == null)
            objectToTransform = this.transform;

        // Initialize your sliders in the UI
        scaleXSlider.onValueChanged.AddListener(value => HandleScaleChanged(scaleXSlider.value, scaleYSlider.value, scaleZSlider.value));
        scaleYSlider.onValueChanged.AddListener(value => HandleScaleChanged(scaleXSlider.value, scaleYSlider.value, scaleZSlider.value));
        scaleZSlider.onValueChanged.AddListener(value => HandleScaleChanged(scaleXSlider.value, scaleYSlider.value, scaleZSlider.value));

        // Initialize buttons for the color changes
        for (int i = 0; i < colorButtons.Length; i++)
        {
            Color buttonColor = colorButtons[i].GetComponent<Image>().color; // Assume the button's image color is the color to apply
            colorButtons[i].onClick.AddListener(() => HandleColorChanged(buttonColor));
        }
    }

    void HandleScaleChanged(float x, float y, float z)
    {
        if (photonView.IsMine)
        {
            // Update the scale of the object
            Vector3 newScale = new Vector3(x, y, z);
            objectToTransform.localScale = newScale;

            // Synchronize the change
            photonView.RPC("UpdateScale", RpcTarget.Others, newScale);
        }
    }

    void HandleColorChanged(Color newColor)
    {
        if (photonView.IsMine)
        {
            // Update the color of the object
            Renderer renderer = objectToTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }

            // Synchronize the change
            photonView.RPC("UpdateColor", RpcTarget.Others, newColor);
        }
    }

    [PunRPC]
    void UpdateScale(Vector3 newScale)
    {
        objectToTransform.localScale = newScale;
        networkScale = newScale; // Store the network scale for synchronization
    }

    [PunRPC]
    void UpdateColor(Color newColor)
    {
        Renderer renderer = objectToTransform.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = newColor;
        }
        networkColor = newColor; // Store the network color for synchronization
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this object: send the others our data
            stream.SendNext(objectToTransform.localScale);
            stream.SendNext(objectToTransform.GetComponent<Renderer>().material.color);
        }
        else
        {
            // Network object, receive data
            networkScale = (Vector3)stream.ReceiveNext();
            networkColor = (Color)stream.ReceiveNext();

            // Immediately update scale and color without waiting for the next update cycle
            objectToTransform.localScale = networkScale;
            objectToTransform.GetComponent<Renderer>().material.color = networkColor;
        }
    }

    void Update()
    {
        // Smooth out the transition for other clients
        if (!photonView.IsMine)
        {
            objectToTransform.localScale = Vector3.Lerp(objectToTransform.localScale, networkScale, Time.deltaTime * 10);
            Renderer renderer = objectToTransform.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, networkColor, Time.deltaTime * 10);
            }
        }
    }
}
