using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void function(FPSController player);
public class FPSController : MonoBehaviour
{
    public Camera Cam;
    Rigidbody rb;

    public AudioClip cashClip;

    public float mouseHorizontal = 3f;
    public float mouseVertical = 2f;

    float h_mouse;
    float v_mouse;
    public float maxhp = 100;
    public float hp;
    [HideInInspector]
    public float currenthpregeneration;
    public float hpBaseRegeneration;
    public float extrahpRegeneration = 10;
    public float maxhpRefeneration = 50;

    public float moveSpeed = 5;
    public float runSpeed = 10;
    public float energy = 100;
    [HideInInspector]
    public float currentEnergy = 0;
    public float energyBaseRegeneration = 10;
    [HideInInspector]
    public float currentEnergyRegeneration = 10;
    public float extraEnergyRegeneration = 10;
    public float maxEnergyRefeneration = 50;
    public float runEnergyCost = 40;
    public float runCD = 1;
    bool canRun = true;
    float h;
    float v;
    public float reloadSpeedMult = 1;

    bool floorDetected = false;
    bool isJump = false;
    public float jumpForce = 5.0f;
    bool isRunning = false;
    int score = 0;
    public function InteractAction;
    public PlayerWeaponController weaponManager;
    public Image bloodDamagebyTime;
    public float damagePercetnageEnable;
    public float damagePercetnageDisable;
    public bool triggeredEffect;
    public bool controlable;
    void Start()
    {
        weaponManager = GetComponent<PlayerWeaponController>();
        rb = GetComponent<Rigidbody>();
        hp = maxhp;
        //esconder el mouse
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.current.UpdateScoreEvent.Invoke(score);
        currentEnergy = energy;
        StartCoroutine(RegenEnergy());
        StartCoroutine(RegenHp());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(InteractAction != null)
                InteractAction(this);
        }
    }
    private void FixedUpdate()
    {
     
    }
    public void GetDamage(float d) 
    {
        currenthpregeneration = energyBaseRegeneration;
        hp -= d;
        if (hp <= maxhp * (100 - damagePercetnageEnable)/100)
            triggeredEffect = true;
        EventManager.current.OnHitEvents.Invoke();
        if (hp <= 0) 
        {
            hp = 0;
            EventManager.current.EndGame();
        }

    }
    void Move()
    {
        if (!controlable)
            return;

        //movimiento de camara
        h_mouse = mouseHorizontal * Input.GetAxis("Mouse X");
        v_mouse = mouseVertical * -Input.GetAxis("Mouse Y");

        transform.Rotate(0, h_mouse, 0);
        Cam.transform.Rotate(v_mouse, 0, 0);

        //movimiento del player
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h == 1)
            print("");
        Vector3 direction = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * new Vector3(h, 0, v);
        //pa correr
        if(Input.GetButtonDown("Fire3")) 
        {
            isRunning = true;
        };
        if (Input.GetButtonUp("Fire3"))
        {
            isRunning = false;
        };
        if (isRunning && canRun) 
        {
            if (CanUse(runEnergyCost * Time.deltaTime))
            {
                currentEnergy -= runEnergyCost * Time.deltaTime;
                rb.AddForce(direction * runSpeed * Time.deltaTime);
            }
            else 
            {
                canRun = false;
                isRunning = false;
                Invoke("ResetRun", runCD);
            }
        }
        else
            rb.AddForce(direction * moveSpeed * Time.deltaTime);

        Vector3 floor = transform.TransformDirection(Vector3.down);

        //solo 1 salto
        if (Physics.Raycast(transform.position, floor, 1.03f))
        {
            floorDetected = true;
            //print("suelo");
        }
        else
        {
            floorDetected = false;
            //print("no suelo");
        }
        isJump = Input.GetButtonDown("Jump");

        if (isJump && floorDetected)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            EventManager.current.PlayEffect(cashClip);
            IncresePoints(1000);
        }
    }
    public void ResetRun() 
    {
        canRun = true;
    }
    public void IncresePoints(int p)
    {
        score += p;
        EventManager.current.OnGainPoints.Invoke(p);
        EventManager.current.UpdateScoreEvent.Invoke(score);
    }
    public bool HasScore(int s) 
    {
        return score >= s;
    }
    public void Buy(int s) 
    {
        score -= s;
        WeaponInfo_UI.instance.SpawnScoreTextRed(s);
        EventManager.current.UpdateScoreEvent.Invoke(score);
    }
    public bool CanUse(float e) 
    {
        return (currentEnergy >= e);
    }
    public IEnumerator RegenEnergy() 
    {
        while (true)
        {
            if (currentEnergy < energy && !isRunning) 
            {
                    currentEnergyRegeneration += extraEnergyRegeneration * Time.deltaTime;
                if (currentEnergyRegeneration >= maxEnergyRefeneration)
                    currentEnergyRegeneration = maxEnergyRefeneration;

                currentEnergy += currentEnergyRegeneration * Time.deltaTime;
                if (currentEnergy >= energy)
                    currentEnergy = energy;
            }
            else
                currentEnergyRegeneration = energyBaseRegeneration;

                yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    public IEnumerator RegenHp()
    {
        float refreshTime = 0.1f;
        currenthpregeneration = hpBaseRegeneration;
        while (true)
        {
            if (hp < maxhp)
            {
                currenthpregeneration += extrahpRegeneration * refreshTime;
                if (currenthpregeneration >= maxhpRefeneration)
                    currenthpregeneration = maxhpRefeneration;

                hp += currenthpregeneration * refreshTime;
                if (hp >= maxhp) 
                {
                    triggeredEffect = false;
                    hp = maxhp;
                }


                if (triggeredEffect)
                {
                    float min = damagePercetnageDisable / 100;
                    float hpPercentage = hp / (maxhp - min);
                    Color c = bloodDamagebyTime.color;
                    c.a = 1 - hpPercentage;
                    if (c.a < 0)
                        c.a = 0;
                    bloodDamagebyTime.color = c;
                }

            }
            else
                currenthpregeneration = hpBaseRegeneration;

            yield return new WaitForSeconds(refreshTime);
        }
    }
}
