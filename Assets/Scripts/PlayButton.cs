using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * GummyCinemaRPG - PlayButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to start playing the game
 */
public class PlayButton : MonoBehaviour
{
    // Defined global variables
    private Button button;
    public GameObject curtains;
    private Animator curtainsAnim;
    
    // Initialize
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        curtainsAnim = curtains.GetComponent<Animator>();
    }

    // Starting playing the game once clicked
    private void OnClick()
    {
        StartCoroutine(CinemaStart());
    }

    // For cinematic beginning
    private IEnumerator CinemaStart()
    {
        curtainsAnim.Play("Curtains_Title");
        yield return new WaitForSeconds(curtainsAnim.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene("Battle");
    }
}
