using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; // Oyuncu z�plad���nda uygulanan kuvvet miktar�
    [Range(0, 1)][SerializeField] private float m_RollSpeed = .36f; // Yuvarlanma hareketinde maksimum h�z�n ne kadar�n�n kullan�laca�� (1 = %100)
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // Hareketin ne kadar yumu�at�laca��
    [SerializeField] private bool m_AirControl = false; // Oyuncunun havadayken y�n de�i�tirebilme durumu
    [SerializeField] private LayerMask m_WhatIsGround; // Karakterin hangi katmanlar� zemin olarak tan�yaca��n� belirten maske
    [SerializeField] private Transform m_GroundCheck; // Oyuncunun yerde olup olmad���n� kontrol edecek nokta
    [SerializeField] private Transform m_CeilingCheck; // Oyuncunun kafas�n�n �st�nde bir engel olup olmad���n� kontrol edecek nokta
    [SerializeField] private Collider2D m_RollDisableCollider; // Yuvarlanma s�ras�nda devre d��� b�rak�lacak olan collider

    const float k_GroundedRadius = .2f; // Oyuncunun yerde olup olmad���n� kontrol etmek i�in kullan�lan dairenin yar��ap�
    private bool m_Grounded; // Oyuncunun yerde olup olmad���n� belirten de�i�ken
    const float k_CeilingRadius = .2f; // Oyuncunun aya�a kalk�p kalkamayaca��n� kontrol etmek i�in tavan kontrol dairesinin yar��ap�
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true; // Oyuncunun hangi y�ne bakt���n� belirlemek i�in kullan�l�r
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent; // Oyuncu yere indi�inde �al��acak olay

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnRollEvent; // Yuvarlanma olaylar�n� y�netmek i�in UnityEvent
    private bool m_wasRolling = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnRollEvent == null)
            OnRollEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // Oyuncunun zeminle temas�n� kontrol et
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke(); // Oyuncu ilk kez yere inmi�se olay� tetikle
            }
        }
    }

    public void Move(float move, bool Roll, bool jump)
    {
        // E�er yuvarlanma aktif de�ilse, karakterin aya�a kalk�p kalkamayaca��n� kontrol et
        if (!Roll)
        {
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                Roll = true; // Tavan varsa yuvarlanma devam etsin
            }
        }

        // Karakter yerdeyse veya havada kontrol aktifse hareket ettir
        if (m_Grounded || m_AirControl)
        {
            // Yuvarlanma kontrol�
            if (Roll)
            {
                if (!m_wasRolling)
                {
                    m_wasRolling = true;
                    OnRollEvent.Invoke(true); // Yuvarlanma ba�lad�
                }

                move *= m_RollSpeed; // Yuvarlan�rken h�z azalt�l�r

                if (m_RollDisableCollider != null)
                    m_RollDisableCollider.enabled = false; // Collider devre d���
            }
            else
            {
                if (m_RollDisableCollider != null)
                    m_RollDisableCollider.enabled = true; // Collider aktif

                if (m_wasRolling)
                {
                    m_wasRolling = false;
                    OnRollEvent.Invoke(false); // Yuvarlanma bitti
                }
            }

            // Hareket vekt�r� belirleniyor
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Karakterin y�n�n� �evirme
            if (move > 0 && !m_FacingRight)
            {
                Flip(); // Sa�a d�n
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip(); // Sola d�n
            }
        }

        // Z�plama
        if (m_Grounded && jump)
        {
            m_Grounded = false; // Yerdeyken z�plama yap�l�r
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); // Yukar� do�ru kuvvet uygula
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight; // Y�n� tersine �evir
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale; // X ekseninde yans�
    }
}
