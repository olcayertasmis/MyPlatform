using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator chestAnim;
    [SerializeField] private float speed;

    private Score score;

    private bool isRunning;
    private float flip = 1f;
    private float horizontalValue;

    private bool isDead;

    public Action OnDead;

    public bool isStart;

    private Health health;

    private Animator playerAnim;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerAnim = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();
        score = transform.GetComponent<Score>();
        health = transform.GetComponent<Health>();
    }

    private void FixedUpdate()
    {
        if (isDead || !isStart) return;

        if (health.currentHealth == 0) ToDeath();


        horizontalValue = Move();

        CheckRunning();
    }

    #region Karakter hareket işlemleri

    private float Move()
    {
        horizontalValue = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalValue * speed, rb.velocity.y);
        FlipDirection();
        return horizontalValue;
    }

    #endregion

    #region Karakter Döndürme İşlemleri

    private void FlipDirection()
    {
        if (horizontalValue > 0) flip = 1f;

        else if (horizontalValue < 0) flip = -1f;

        transform.localScale = new Vector3(flip, 1f, 1f);
    }

    #endregion

    #region Koşma Animasyonu İşlemleri

    private void CheckRunning()
    {
        if (horizontalValue != 0) isRunning = true;

        else isRunning = false;

        playerAnim.SetBool("KosuyorMu", isRunning);
    }

    #endregion

    #region Ölme İşlemleri

    private void ToDeath()
    {
        OnDead?.Invoke();

        playerAnim.SetTrigger("Death");
        SoundManager.Instance.PlaySfx("GameOver");
        isDead = true;
        score.ResetScore();
    }

    #endregion

    #region Coin Toplama İşlemleri

    private void CollectItem(Collider2D collision)
    {
        Destroy(collision.gameObject);
        SoundManager.Instance.PlaySfx("Item");
        score.IncreasingScore(5);
    }

    #endregion

    #region Next level işlemleri

    private void NextLevelChest(Collision2D other)
    {
        if (!other.gameObject.CompareTag("NextLevel")) return;

        SoundManager.Instance.PlaySfx("NextLevel");
        chestAnim.SetBool("isChest", true);
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2.5f);
        score.ScoreUpdate();
        PlayerPrefs.SetInt("beforescore", PlayerPrefs.GetInt("score"));
        GameManager.Instance.NextLevel();
    }

    #endregion

    #region Collider - Collision İşlemleri

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item")) CollectItem(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NextLevelChest(collision);
        if (collision.gameObject.CompareTag("Enemy")) health.TakeDamage();
        if (collision.gameObject.CompareTag("DeathSpace")) ToDeath();

        if (collision.gameObject.CompareTag("Head"))
        {
            Destroy(collision.gameObject, 0.25f);
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }

    #endregion
}