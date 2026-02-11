using UnityEngine;
using UnityEngine.Rendering;

public class Collectable : MonoBehaviour
{


    public CarrotPile carrotP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && carrotP.collectScore <= 2)
        {
            Debug.Log("Ping");
            Destroy(gameObject);
            carrotP.collectScore++;
        }
    }
}
