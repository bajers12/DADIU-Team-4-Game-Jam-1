using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    public float beat;
    public bool started;
    void Start()
    {
        beat = beat / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, beat * Time.deltaTime, 0f);
    }
}
