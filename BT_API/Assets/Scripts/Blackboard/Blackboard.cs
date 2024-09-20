using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{   
    [SerializeField]
    private Text clock;

    private float timeOfDay = 0;
    private GameObject patron;
 

    public float TimeOfDay() { return timeOfDay; }
    public GameObject Patron() { return patron; }

    static Blackboard instance;

    public static Blackboard Instance
    {
        get
        {
            if (!instance)
            {
                Blackboard[] blackboards = GameObject.FindObjectsOfType<Blackboard>();
                if (blackboards != null)
                {
                    if (blackboards.Length == 1)
                    {
                        instance = blackboards[0];
                        return instance;
                    }
                }

                GameObject go = new GameObject("Blackboard", typeof(Blackboard));
                instance = go.AddComponent<Blackboard>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }

        set 
        {
            instance = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateClock());
    }

    public GameObject RegisterPatron(GameObject newPatron)
    {
        if(patron == null)
        {
            patron = newPatron;
        }

        return patron;
    }

    public void DeregisterPatron()
    {
        patron = null;
    }

    private IEnumerator UpdateClock()
    {
        while(true)
        {
            timeOfDay++;

            if (timeOfDay > 23)
            {
                timeOfDay = 0;            
            }
            clock.text = timeOfDay + ":00";
            yield return new WaitForSeconds(1.0f);
        }
    }
}
