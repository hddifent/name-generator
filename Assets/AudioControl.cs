using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SFX
{
    public AudioClip[] resultSound, rollerSound;
    public AudioClip rickroll, desperate;

    public bool alreadyRicked;
}

public class AudioControl : MonoBehaviour
{
    public enum Player
    {
        player, desperatePlayer
    }

    public SFX allSound;
    [SerializeField] private AudioSource player, desperatePlayer;

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.M))
        {
            StopPlayer(Player.player);
            StopPlayer(Player.desperatePlayer);
        }
    }

    public void PlayRollSFX()
    {
        player.Stop();

        if (SettingControl.Mute || allSound.rollerSound.Length == 0) { return; }

        player.clip = allSound.rollerSound[Random.Range(0, allSound.rollerSound.Length)];

        player.Play();
    }

    public void PlayRollSFX(float chance)
    {
        player.Stop();

        if (SettingControl.Mute || allSound.rollerSound.Length == 0) { return; }

        if (allSound.rickroll != null && !allSound.alreadyRicked && Random.Range(0f, 1f) < chance)
        {
            player.clip = allSound.rickroll;
            allSound.alreadyRicked = true;
        }
        else
        {
            player.clip = allSound.rollerSound[Random.Range(0, allSound.rollerSound.Length)];
        }

        player.Play();
    }

    public void PlayShowSFX()
    {
        player.Stop();

        if (SettingControl.Mute || allSound.resultSound.Length == 0) { return; }

        player.clip = allSound.resultSound[Random.Range(0, allSound.resultSound.Length)];

        player.Play();
    }

    public void PlayDesperateSFX()
    {
        desperatePlayer.Stop();

        if (SettingControl.Mute || allSound.desperate == null) { return; }

        desperatePlayer.clip = allSound.desperate;

        desperatePlayer.Play();
    }

    public void StopPlayer(Player playerCode)
    {
        if (playerCode == Player.player) { player.Stop(); }
        else if (playerCode == Player.desperatePlayer) { desperatePlayer.Stop(); }
    }

    public void SetVolume(Player playerCode, float v)
    {
        if (playerCode == Player.player) { player.volume = v; }
        else if (playerCode == Player.desperatePlayer) { desperatePlayer.volume = v; }
    }
}
