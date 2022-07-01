using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //[SerializeField]
    public static float maxAcceleration = 10.0f;
    public static float minAcceleration = 1.0f;
    public static bool WithMagnet = false;  //upon touching magnet item this is set to true
    private float Acceleration = 0.0f;  //initial accel is 0
    private Vector3 InitialEventPos;    //holds first click/touch position
    public static GameObject SelfObject;    //for outer reference
    private CharacterController MyBody;
    private float gravityValue = 1.0f;  //Custom float value for gravity. Custom calculations required custom gravity

    //ZPos is required for magnet work,, and may have variable use cases like calculating completion percentage of map etc.
    public static float PlayerZPos = 0.0f;  
    private Quaternion RotateTo;    //to Quaternion where character will rotate eventually
    public static int cameraRotationSpeed = 3;
    private Vector3 translatorVector; //movement vector
    private bool EscapeSequence = false; //!-- When character tries to go out this gets true
    private Animator MyAnimator;
    public static Transform myTransform;
    private ParticleSystem ps;  //For wind animation
    private ControllerColliderHit LastContact; //for jumper things (to check layer)
    private GameObject [] LimiterWalls = new GameObject[2]; // these are the walls which will prevent on air player to fall down

    void Start()
    {
        Input.simulateMouseWithTouches = true;  // Considering no need for multitouch support;;;
        InitialEventPos = Vector3.zero; //init touch pos
        SelfObject = this.gameObject;   //outer scripts communicate via SelfObject
        MyBody = GetComponent<CharacterController>();
        translatorVector = Vector3.zero;    //movement vector
        MyAnimator = GetComponentInChildren<Animator>();
        myTransform = GameObject.Find("AnimatedCharacter").transform;
        ps = GetComponent<ParticleSystem>();
        LastContact = null;
        LimiterWalls[0] = GameObject.Find("RoadWay/Base/RightWall");
        LimiterWalls[1] = GameObject.Find("RoadWay/Base/LeftWall");
    }
    

    void Update()
    {
        PlayerZPos = myTransform.position.z;  //to follow if player has reached finish line
        MyAnimator.speed = Acceleration / 10;
        if (!MyBody.isGrounded) //i tried to escape or i jumped over jumpers
        {
            SwitchSafetyWalls(true); //if i dont stand on the ground i dont want to fall down platform
            JumpOver();
        }
        else // i am touching the ground
        {
            SwitchSafetyWalls(false);
            gravityValue = 1.0f;    //reset gravity for next falls
            if (Input.GetMouseButton(0))    //left click or screen touch 
            {
                PlayRunAnimation();
                NormalMove();
            }
            else SlowingMove();
        }  
        CorrectCharacterRotation(); //rotate character as with the landshape
        ShowWindEffect(); //show wind particles if the acceleration is enough
       
    }

    void OnControllerColliderHit(ControllerColliderHit Thing)   //that i hit to trigger this func
    {
        translatorVector.y = -1.0f; //reset gravity
        EscapeSequence = false; // if i hit something then im not escaping
        switch (Thing.gameObject.layer)
        {
            case 6: //surface before fall
                EscapeSequence = true;
                break;
            case 8: //cubes, turning bars
                if (MyBody.isGrounded) Acceleration = 0.0f;    //stop
                Thing.gameObject.SetActive(false);  //remove hit object,, later this will be converted to destruction animation
                MyAnimator.Play("Base Layer.Jogging Stumble");
                break;
            case 9: //entering into swamp
                Acceleration = 4.0f;    //no way to stop or increase speed either
                break;
            case 10:    //collision with a rival
                StartCoroutine(DistortNavmeshAgent(Thing.gameObject));
                break;
            default:
                break;
        }
        LastContact = Thing;    //this is for calculating jumpable slopes
        RotateTo = Thing.gameObject.transform.rotation; //camera rotation
    }

    IEnumerator DistortNavmeshAgent(GameObject Rival)
    {
        UnityEngine.AI.NavMeshAgent Agent = Rival.GetComponent<UnityEngine.AI.NavMeshAgent>();
        Agent.speed = 1;
        Agent.Move(new Vector3(Random.Range(-2.0f,2.0f),5.0f,Random.Range(2.0f,4.0f)));
        yield return new WaitForSeconds(3.0f);
        Agent.speed = 8;
    }

    void PlayRunAnimation()
    {
        try 
        {
            if (MyAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Jogging Stumble") //if i havent collide with obstacles
            {
                MyAnimator.Play("Base Layer.Run Forward");
            }
        }
        catch 
        {
            Debug.Log("AnimatorClipInfo Clip Name Not Found!");
        }
    }

    void SlowingMove()
    {
        if (!InitialEventPos.Equals(Vector3.zero)) InitialEventPos = Vector3.zero;  //reset last touch position
        if (Acceleration > minAcceleration) Acceleration -= 0.1f;   //lower acceleration if not touching the screen
        MyBody.Move ( (Vector3.forward + Vector3.down) * (Acceleration > 0 ? Acceleration : 1.0f) * Time.deltaTime);
    }

    void NormalMove()
    {
        if (InitialEventPos.Equals(Vector3.zero)) InitialEventPos = Input.mousePosition; //reset last touch position
        if (Acceleration < maxAcceleration) Acceleration += 0.1f; //increase acceleration if touching the screen
        translatorVector = (Input.mousePosition - InitialEventPos).normalized + Vector3.forward; //generate vector from slide
        translatorVector.y = -1.0f; //to ensure the character stays on the platform
        MyBody.Move(translatorVector * Acceleration * Time.deltaTime);
    }

    void JumpOver()
    {
        if (LastContact != null && LastContact.gameObject.layer == LayerMask.NameToLayer("Jumper")) //fall from jumper
        {
        MyAnimator.Play("Base Layer.Landing");
        translatorVector.y = gravityValue;
        MyBody.Move( translatorVector * (Acceleration > 0 ? Acceleration : 1.0f) * Time.deltaTime);
        gravityValue -= 0.02f; 
        }
        else 
        {
            if (EscapeSequence)  //move over the edge
            {
                MyAnimator.Play("Base Layer.Landing");
                MyBody.Move( new Vector3(translatorVector.x*-1,gravityValue,1.0f) * (Acceleration > 0 ? Acceleration : 1.0f) * Time.deltaTime);
                gravityValue -= 0.02f;
            }
            else 
            {
                translatorVector.y = -1.0f;
                MyBody.Move( translatorVector * (Acceleration > 0 ? Acceleration : 1.0f) * Time.deltaTime);
            }
       
        }
    }

    void ShowWindEffect() 
    {
        if (Acceleration > 5.0f)
        {
            if (ps.isStopped)   //animation is not live
            {
                ps.Play();
            }
            else    //already animating 
            {
                var main = ps.main;
                main.simulationSpeed = Acceleration;    //increase animation speed with acceleration
            }
        }
        else{
            ps.Stop();  //not enough acceleration to have wind
        }        
    }

    void CorrectCharacterRotation()
    {
        //slowly rotate the camera
        if (myTransform.rotation != RotateTo) myTransform.rotation = Quaternion.Slerp(myTransform.rotation, RotateTo, cameraRotationSpeed *Time.deltaTime);
    }

    void SwitchSafetyWalls(bool WallState)
    {
        foreach (GameObject wall in LimiterWalls)
        {
            wall.SetActive(WallState);
        }
    }

}
