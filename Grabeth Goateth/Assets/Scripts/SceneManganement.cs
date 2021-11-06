using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManganement : MonoBehaviour
{
    //fields
    public Player playerScript;
    public GameObject[] guardArray;
    public AudioClip goatClip;
    private AudioSource audioSource;
    private float audioTimer = 0;
    private bool visionIncreased = false;
    // Start is called before the first frame update
    void Start()
    {
        //finds all of the guards in the scene
        guardArray = GameObject.FindGameObjectsWithTag("Guard");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (playerScript.hasGoat)
        {
            //when the player has the goat increase's the vision of each guard by 1
            if (!visionIncreased)
            {
                foreach (GameObject guard in guardArray)
                {
                    guard.GetComponent<GuardScript>().visionRadius += 1;
                }
                visionIncreased = true;
            }
            
            //plays a goat sound every 5 seconds
            if(audioTimer <= 0)
            {
                audioSource.Play();
                audioTimer = 5;
            }

            audioTimer -= Time.deltaTime;
            
            
        }

        if(playerScript.flashLight.enabled == true)
        {
            //checks each guard to see if they are within range of the player's stun
            foreach(GameObject guard in guardArray)
            {
                float distance = Mathf.Pow((playerScript.gameObject.transform.position.x - guard.transform.position.x), 2) + Mathf.Pow((playerScript.gameObject.transform.position.y - guard.transform.position.y), 2);
                distance = Mathf.Sqrt(distance);

                if (distance < 50)
                {
                    //stops the guard from moving and turns them yellow as a visual cue
                    guard.GetComponent<GuardScript>().canMove = false;
                    guard.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }
    }

    //I saw this in the unity documentation, it may not be necessary.
    private IEnumerator playAudio()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        }
}
