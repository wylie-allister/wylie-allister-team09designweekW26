//using UnityEngine;
//using UnityEngine.SocialPlatforms.Impl;

//public class CarrotPile : MonoBehaviour
//{
//    public int collectScore = 0;
//    public int score = 0;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    public void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.gameObject.tag == "Player" && collectScore == 1)
//        {
//            score++;
//            collectScore = 0;
//            Debug.Log($"{score}");
//        }

//        if (other.gameObject.tag == "Player" && collectScore == 2)
//        {
//            score += 2;
//            collectScore = 0;
//            Debug.Log($"{score}");
//        }    
//    }
//}
// Don't need to use this since collectable does all the work now!
