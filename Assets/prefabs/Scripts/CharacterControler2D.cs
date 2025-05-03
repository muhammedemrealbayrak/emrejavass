using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f; // Oyuncu zýpladýðýnda uygulanan kuvvet miktarý
    [Range(0, 1)][SerializeField] private float m_RollSpeed = .36f; // Yuvarlanma hareketinde maksimum hýzýn ne kadarýnýn kullanýlacaðý (1 = %100)
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f; // Hareketin ne kadar yumuþatýlacaðý
    [SerializeField] private bool m_AirControl = false; // Oyuncunun havadayken yön deðiþtirebilme durumu
    [SerializeField] private LayerMask m_WhatIsGround; // Karakterin hangi katmanlarý zemin olarak tanýyacaðýný belirten maske
    [SerializeField] private Transform m_GroundCheck; // Oyuncunun yerde olup olmadýðýný kontrol edecek nokta
    [SerializeField] private Transform m_CeilingCheck; // Oyuncunun kafasýnýn üstünde bir engel olup olmadýðýný kontrol edecek nokta
    [SerializeField] private Collider2D m_RollDisableCollider; // Yuvarlanma sýrasýnda devre dýþý býrakýlacak olan collider

    const float k_GroundedRadius = .2f; // Oyuncunun yerde olup olmadýðýný kontrol etmek için kullanýlan dairenin yarýçapý
    private bool m_Grounded; // Oyuncunun yerde olup olmadýðýný belirten deðiþken
    const float k_CeilingRadius = .2f; // Oyuncunun ayaða kalkýp kalkamayacaðýný kontrol etmek için tavan kontrol dairesinin yarýçapý
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true; // Oyuncunun hangi yöne baktýðýný belirlemek için kullanýlýr
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent; // Oyuncu yere indiðinde çalýþacak olay

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnRollEvent; // Yuvarlanma olaylarýný yönetmek için UnityEvent
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

        // Oyuncunun zeminle temasýný kontrol et
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke(); // Oyuncu ilk kez yere inmiþse olayý tetikle
            }
        }
    }

    public void Move(float move, bool Roll, bool jump)
    {
        // Eðer yuvarlanma aktif deðilse, karakterin ayaða kalkýp kalkamayacaðýný kontrol et
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
            // Yuvarlanma kontrolü
            if (Roll)
            {
                if (!m_wasRolling)
                {
                    m_wasRolling = true;
                    OnRollEvent.Invoke(true); // Yuvarlanma baþladý
                }

                move *= m_RollSpeed; // Yuvarlanýrken hýz azaltýlýr

                if (m_RollDisableCollider != null)
                    m_RollDisableCollider.enabled = false; // Collider devre dýþý
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

            // Hareket vektörü belirleniyor
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Karakterin yönünü çevirme
            if (move > 0 && !m_FacingRight)
            {
                Flip(); // Saða dön
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip(); // Sola dön
            }
        }

        // Zýplama
        if (m_Grounded && jump)
        {
            m_Grounded = false; // Yerdeyken zýplama yapýlýr
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); // Yukarý doðru kuvvet uygula
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight; // Yönü tersine çevir
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale; // X ekseninde yansý
    }
}
