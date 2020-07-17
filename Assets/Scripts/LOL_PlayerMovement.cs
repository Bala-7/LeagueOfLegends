using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LOL_PlayerMovement : MonoBehaviour
{
    private NavMeshAgent _nav;
    private Transform _body;    // Body gameobject HAS to be the first child in the list
    private Animator _ac;

    public GameObject castPreviewRange;

    public GameObject qSpellPreview;
    public GameObject qSpell;

    public GameObject wSpellPreview;
    public GameObject wSpell;
    private void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _body = transform.GetChild(0);
        _ac = _body.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _nav.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                _nav.SetDestination(hit.point);
        }


        HandleQAttack();
        HandleWAttack();


        
        _ac.SetBool("Run", IsMoving());
        


    }

    private void LateUpdate()
    {
        if(IsMoving()) transform.rotation = Quaternion.LookRotation(_nav.velocity.normalized);
    }

    private bool IsMoving() { return (_nav.velocity != Vector3.zero); }

    private void OnDrawGizmos()
    {
        RaycastHit hitAtk;
        Ray rayAtk = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayAtk, out hitAtk, Mathf.Infinity))
        {
            Gizmos.DrawSphere(hitAtk.point, 0.025f);
        }
    }

    private void HandleQAttack() {
        if (Input.GetKey(KeyCode.Q))
        {
            qSpellPreview.SetActive(true);
            castPreviewRange.SetActive(true);

            RaycastHit hitAtk;
            Ray rayAtk = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayAtk, out hitAtk, Mathf.Infinity))
            {
                float floorHeight = hitAtk.point.y;
                Vector3 center = new Vector3(transform.position.x, floorHeight, transform.position.z);

                Vector3 dir = hitAtk.point - center;
                dir = new Vector3(dir.x, 0, dir.z);
                Vector3 castDir = qSpellPreview.transform.position - center;
                castDir = new Vector3(castDir.x, 0, castDir.z);
                float sAngle = Vector3.SignedAngle(dir, castDir, Vector3.up);

                //Debug.DrawRay(center, dir, Color.red);
                //Debug.DrawRay(center, castDir * 50, Color.blue);

                int sign = (sAngle >= 0) ? 1 : -1;
                float angle = Mathf.Abs(sAngle);
                if (angle > 0.3f) qSpellPreview.transform.RotateAround(center, Vector3.up, -sign * angle);



                Debug.Log(angle);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q)) {
           

            _nav.velocity = Vector3.zero;
            _nav.ResetPath();
            _ac.Play("Spell_Q");

            RaycastHit hitAtk;
            Ray rayAtk = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayAtk, out hitAtk, Mathf.Infinity))
            {
                float transformHeight = transform.position.y;
                float floorHeight = hitAtk.point.y;
                Vector3 point = new Vector3(hitAtk.point.x, transformHeight, hitAtk.point.z);



                transform.LookAt(point);

                Vector3 center = new Vector3(transform.position.x, floorHeight, transform.position.z);

                Vector3 dir = hitAtk.point - center;
                dir = new Vector3(dir.x, 0, dir.z);
                GameObject spell = Instantiate(qSpell, transform.position, Quaternion.identity);
                spell.GetComponent<Rigidbody>().velocity = dir.normalized * 5.0f;
                Debug.Log(dir.normalized);
            }

            

        }
        else
        {
            qSpellPreview.SetActive(false);
            castPreviewRange.SetActive(false);
        }
    }


    private void HandleWAttack()
    {
        if (Input.GetKey(KeyCode.W))
        {
            wSpellPreview.SetActive(true);
            castPreviewRange.SetActive(true);

            RaycastHit hitAtk;
            Ray rayAtk = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayAtk, out hitAtk, Mathf.Infinity))
            {
                wSpellPreview.transform.position = hitAtk.point + new Vector3(0, 0.02f, 0);

            }
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _nav.velocity = Vector3.zero;
            _nav.ResetPath();
            _ac.Play("Spell_W");

            RaycastHit hitAtk;
            Ray rayAtk = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayAtk, out hitAtk, Mathf.Infinity))
            {
                float transformHeight = transform.position.y;
                float floorHeight = hitAtk.point.y;
                Vector3 point = new Vector3(hitAtk.point.x, floorHeight + 0.01f, hitAtk.point.z);
                
                transform.LookAt(point);

                GameObject spell = Instantiate(wSpell, point, Quaternion.identity);
            }
            
        }
        else
        {
            wSpellPreview.SetActive(false);
            castPreviewRange.SetActive(false);
        }
    }
}
