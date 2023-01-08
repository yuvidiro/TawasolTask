using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

//Making single script for both the players, changing important varaibles and compenents in inspector
public class MovingPlayer : MonoBehaviour
{
    public Transform target;
    public Camera _camera;
    public GameObject otherPlayer;
    float walkSpeed, runSpeed;
    public float boostSpeed;
    public int Range;
    private Animator _animator;
    private int _animIDSpeed;
    bool _boostCheck;
    //static helps to stop this code in other player script
    static bool _endCheck;
    public float _boostTimer;
    public TextMeshProUGUI EndText;
    public Canvas ENDUI;
    public Canvas BoostUI;
    public GameObject _endDiamond;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animIDSpeed = Animator.StringToHash("Speed");
        walkSpeed = Random.Range(4,7);
        runSpeed = Random.Range(14, 17);
        EndText = EndText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_endCheck){
            //Zoom in Camera
            if(_camera.fieldOfView >50)
            {
                _camera.fieldOfView-= 10*Time.deltaTime;
            }
            return;}
        transform.LookAt(target);
       if(_boostCheck){Boost();}
       else{move();}

      
       
    }

    void move()
    { 
            float distance = Vector3.Distance(transform.position, target.position);
            
            
            if(distance<=Range)
            {
            _animator.SetFloat(_animIDSpeed, runSpeed);
            transform.Translate(Vector3.forward *runSpeed *Time.deltaTime);
            }
            else{
            _animator.SetFloat(_animIDSpeed, walkSpeed);
            transform.Translate(Vector3.forward *walkSpeed *Time.deltaTime);
            }
    }

    public void Boost()
    {
        _boostCheck = true;
        _animator.SetFloat(_animIDSpeed, boostSpeed);
        transform.Translate(Vector3.forward *boostSpeed *Time.deltaTime);
        StartCoroutine(StopBoost());
        

    }

    IEnumerator StopBoost()
    {
        yield return new WaitForSeconds(_boostTimer);
        _boostCheck = false;
    }

    public void OnTriggerEnter(Collider other) {

        if(other.gameObject.tag == "Diamond")
        {
            _endCheck= true;
            BoostUI.enabled = false;
            _animator.SetTrigger("Holding");
            other.gameObject.SetActive(false);
            _endDiamond.SetActive(true);
            otherPlayer.GetComponent<Animator>().SetTrigger("Defeat");
            transform.eulerAngles = new Vector3(0, -90, 0);
            otherPlayer.transform.eulerAngles = new Vector3(0, -90, 0);
            StartCoroutine(ENDUIEnable());
        }
        
    }
    IEnumerator ENDUIEnable()
    {
        //change seconds value according to camerafieldofview value
        yield return new WaitForSeconds(1.2f);
        ENDUI.enabled = true;
    }

     public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale =1;
    }

   private void OnDisable() {
    _endCheck = false;
   }
}
