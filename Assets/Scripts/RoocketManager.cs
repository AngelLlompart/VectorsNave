using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoocketManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtMaxSpeed;
    [SerializeField] private TextMeshProUGUI _txtSpeed;
    [SerializeField] private TextMeshProUGUI _txtElevation;
    [SerializeField] private TextMeshProUGUI _txtFuel;
    [SerializeField] private TextMeshProUGUI _txtResult;
    [SerializeField] private Button _btnOK;
    
    private Rigidbody rocketRb;
    private int height = 45;
    private int maxSpeed = 20;
    private float fuel = 10;
    private bool collided = false;
    private int[] fuelLevels = {15, 15, 10};
    private float[] angle = new float[3];
    private int level = 0;
    private bool win = true;
    private float fuelSpeed = 50;
    private float rotateSpeed;

    private double previousSpeed;
    // Start is called before the first frame update
    void Start()
    {
        angle[0] = 0;
        angle[1] = Random.Range(1, -2) * Random.Range(60, 140);
        angle[2] = Random.Range(120, 240);
        _btnOK.onClick.AddListener(MainMenu);
        rocketRb = gameObject.GetComponent<Rigidbody>();
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        _txtMaxSpeed.text = "Max speed: " + maxSpeed;
        _txtFuel.text = "Fuel: " + fuel.ToString("F1");
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rocketRb.MoveRotation(rocketRb.rotation * Quaternion.Euler(new Vector3(0,0,rotateSpeed) * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rocketRb.MoveRotation(rocketRb.rotation * Quaternion.Euler(new Vector3(0,0,-rotateSpeed) * Time.deltaTime));
        }
        
        if (!collided)
        {
            _txtSpeed.text = "Speed: " +  (previousSpeed).ToString("F1");
            /*if (Input.GetKeyDown(KeyCode.Space) && fuel > 0)
            { 
                //rocketRb.AddForce(Vector3.up * speed); 
                rocketRb.velocity += Vector3.up * 3;
                fuel--;
            }*/
            if (Input.GetKey(KeyCode.Space) && fuel > 0)
            { 
                //rocketRb.velocity += Vector3.up * fuelSpeed * Time.deltaTime;
                rocketRb.velocity += (transform.up * fuelSpeed * Time.deltaTime);
                fuel -= (Time.deltaTime * 5);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _btnOK.onClick.Invoke();
            }
        }
        _txtElevation.text = "Elevation: " + (rocketRb.position.y - 2.548388f).ToString("F1");
        previousSpeed = rocketRb.velocity.magnitude;
       
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (previousSpeed < maxSpeed && collision.gameObject.CompareTag("Finish"))
        {
            if(level < 2)
            {
                EndLevel("Next level"); 
            }
            else
            {
                EndLevel("YOU WIN"); 
            }
           
        }
        else
        {
            win = false;
            EndLevel("YOU LOSE");
        }

    }
    
    private void EndLevel(string message)
    {
        collided = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _txtResult.text = message;
        _txtResult.gameObject.SetActive(true);
        _btnOK.gameObject.SetActive(true);
        //PAUSAR EL JUEGO
        Time.timeScale = 0;
    }
    private void MainMenu()
    {
        if (win && level < 2)
        { 
            level++;
            Restart();
        }
        else
        {
            #if UNITY_EDITOR
                if(EditorApplication.isPlaying) 
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            #else
                Application.Quit();
            #endif
        }
        //SceneManager.LoadScene("MainMenu");
    }
    private void Restart()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = new Vector3(0, height, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0,0,angle[level]));
        rocketRb.velocity = transform.up * 15 * level;
        rocketRb.angularVelocity = new Vector3(0, 0, 0);
        fuel = fuelLevels[level];
        _txtResult.gameObject.SetActive(false);
        _btnOK.gameObject.SetActive(false);
        Time.timeScale = 1;
        collided = false;
        rotateSpeed = 100 * (level + 1);
    }
}
