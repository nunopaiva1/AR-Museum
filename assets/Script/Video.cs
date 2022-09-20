using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour {

   // private string anim = "videoStep1.mp4", film = "filmStep1";
   // public int sceneNumber;

    // Use this for initialization
    void Start () {

    }

    public void play()
    {
        StartCoroutine(playVideo());
    }

    public IEnumerator playAnim(string nomeVideo)
    {
        //Handheld.PlayFullScreenMovie("videoStep1.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
        Handheld.PlayFullScreenMovie(nomeVideo, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);

        yield return new WaitForEndOfFrame();

        Debug.Log("O video: " + nomeVideo + " foi reproduzido com sucesso");
    }

    public IEnumerator playVideo()
    {
        Handheld.PlayFullScreenMovie("filmStep1.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
        yield return new WaitForEndOfFrame();

    }


    
}
