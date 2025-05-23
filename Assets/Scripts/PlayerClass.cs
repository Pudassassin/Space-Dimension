using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClass : MonoBehaviour
{
    public float moveSpeed = 7f;

    private GameObject bulletPrefab;

    InputAction PI_Move, PI_Attack, PI_MouseAim, PI_Ascend, PI_Descend;
    bool attackPressed = false;

    public float maxHeight = 1.0f, minHeight = 6.0f;

    public float fireRate = 2.5f;

    float attackCooldown
    {
        get
        { return 1 / fireRate; }
    }

    float attackTimer = 0.0f;

    void Start()
    {
        // โหลด Bullet Prefab จาก Resources/Prefabs/Bullets
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
        if (bulletPrefab == null)
        {
            Debug.LogWarning("ไม่พบ Bullet Prefab ที่ Resources/Prefabs/Bullet");
        }

        PI_Move = InputSystem.actions.FindAction("Move");
        PI_Attack = InputSystem.actions.FindAction("Attack");
        PI_Ascend = InputSystem.actions.FindAction("Ascend");
        PI_Descend = InputSystem.actions.FindAction("Descend");
        // PI_MouseAim = InputSystem.actions.FindAction("MouseAim");
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        // implement InputSystem
        Vector2 moveVector = PI_Move.ReadValue<Vector2>();

        // horizontal
        float moveX = moveVector.x;
        float moveY = moveVector.y;

        // vertical
        float moveVert;
        if (PI_Ascend.IsPressed())
        {
            moveVert = 1.0f;
        }
        else if (PI_Descend.IsPressed())
        {
            moveVert = -1.0f;
        }
        else
        {
            moveVert = 0.0f;
        }

        Vector3 control = new Vector3(moveX, moveVert, moveY);
        Debug.Log(control);

        if (control.magnitude < 0.1f)
        {
            return;
        }

        Vector3 delta = new Vector3(moveX, moveVert, moveY).normalized * moveSpeed * Time.deltaTime;
        Debug.Log(delta);
        Vector3 newPos = transform.position + delta;
        // newPos.y = Mathf.Clamp(newPos.y, minHeight, maxHeight);

        transform.position = newPos;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);
        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }

    void Shoot()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackCooldown)
        {
            attackTimer = attackCooldown;
        }

        // 'Attack' button from InputSystem
        if (PI_Attack.IsPressed())
        {
            // limit to fire rate
            if (attackTimer >= attackCooldown)
            {
                // trigger attack
                if (bulletPrefab != null)
                {
                    Instantiate(bulletPrefab, transform.position, transform.rotation);
                }

                attackTimer = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            FindObjectOfType<MainLogic>()?.GetDamage();
        }
    }
}
 