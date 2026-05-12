using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	[Header("Panel Main")]
	public GameObject panel_menu;
	public GameObject panel_play;
	public GameObject panel_gameover;
	public GameObject panel_hight_scores;
	public GameObject panel_tip;
	public Text txt_hight_scores;

	[Header("Game Obj")]
	public Carrot.Carrot carrot;
	public MouseLooker mouse_looker;
	public Bloom effec_bloom;
	public Shooter shooter;
	public static GameManager gm;

	public int score = 0;
	private int hight_scores = 0;

	public float startTime = 5.0f;

	public Text mainScoreDisplay;
	public Text mainTimerDisplay;

	public GameObject gameOverScoreOutline;

	public AudioSource musicAudioSource;

	public bool is_play = false;
	private bool is_show_rank = false;

	public GameObject Obj_gameover_txt;

	private float currentTime;
	[Header("Sounds")]
	public AudioClip clip_sound_click;
	public AudioSource[] sound;

	[Header("GameOver")]
	public Text txt_gameover_your_scores;
	public Text txt_gameover_hight_scores;

	void Start() {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        this.carrot.Load_Carrot(this.check_exit_game);
		this.carrot.act_after_close_all_box = this.reset_ui_game;
		currentTime = startTime;

		if (gm == null) gm = this.gameObject.GetComponent<GameManager>();

		mainScoreDisplay.text = "0";

		if (gameOverScoreOutline) gameOverScoreOutline.SetActive(false);
		Obj_gameover_txt.SetActive(false);

		this.panel_menu.SetActive(true);
		this.panel_play.SetActive(false);
		this.panel_tip.SetActive(false);
		this.panel_gameover.SetActive(false);
		this.mouse_looker.LockCursor(false);

		if (this.carrot.get_status_sound())
        {
			this.carrot.game.load_bk_music(this.musicAudioSource);
			this.shooter.set_sound(true);
		}
        else
        {
			this.shooter.set_sound(false);
        }

		this.shooter.enabled = false;

		this.hight_scores = PlayerPrefs.GetInt("game_hight_scores", 0);
		if (this.hight_scores > 0)
			this.panel_hight_scores.SetActive(true);
		else
			this.panel_hight_scores.SetActive(false);
		this.txt_hight_scores.text = this.hight_scores.ToString();
		this.carrot.change_sound_click(this.clip_sound_click);
	}

	private void check_exit_game()
    {
		if (this.is_play)
		{
			this.btn_game_back_home();
			this.carrot.set_no_check_exit_app();
		}
		else if (this.panel_gameover.activeInHierarchy)
		{
			this.btn_game_back_home();
			this.carrot.set_no_check_exit_app();
		}
		else if (this.panel_tip.activeInHierarchy)
		{
			this.btn_game_back_home();
			this.carrot.set_no_check_exit_app();
		}
	}

	private void reset_ui_game()
    {
		if(this.is_show_rank==false) this.effec_bloom.enabled = true;
		if (this.is_show_rank) this.is_show_rank = false;
    }

	public void btn_game_play()
	{
		this.score = 0;
		mainScoreDisplay.text = "0";
		this.shooter.enabled = true;
		this.carrot.ads.show_ads_Interstitial();
		this.Obj_gameover_txt.SetActive(false);
		currentTime = startTime;
		this.mouse_looker.LockCursor(true);
		this.is_play = true;
		this.panel_menu.SetActive(false);
		this.panel_play.SetActive(true);
		this.panel_gameover.SetActive(false);
		this.play_sound(0);
	}

	public void btn_game_back_home()
	{
		this.shooter.enabled = false;
		this.carrot.ads.show_ads_Interstitial();
		this.is_play = false;
		this.Obj_gameover_txt.SetActive(false);
		this.mouse_looker.LockCursor(false);
		this.panel_menu.SetActive(true);
		this.panel_play.SetActive(false);
		this.panel_gameover.SetActive(false);
		this.panel_tip.SetActive(false);
	}

	void Update() {
		if (this.is_play) {
			if (currentTime < 0) {
				EndGame();
			} else {
				currentTime -= Time.deltaTime;
				mainTimerDisplay.text = currentTime.ToString("0.00");
			}
		}
	}

	void EndGame() {
		this.is_play = false;

		mainTimerDisplay.text = "GAME OVER";

		if (gameOverScoreOutline) gameOverScoreOutline.SetActive(true);

		Obj_gameover_txt.SetActive(true);

		if (musicAudioSource) musicAudioSource.pitch = 0.5f;

        if (this.score > this.hight_scores)
        {
			this.hight_scores = this.score;
			PlayerPrefs.SetInt("game_hight_scores", this.hight_scores);
			this.txt_hight_scores.text = this.hight_scores.ToString();
			this.panel_hight_scores.SetActive(true);
			this.carrot.game.update_scores_player(this.hight_scores);
        }

		this.txt_gameover_hight_scores.text = this.hight_scores.ToString();
		this.txt_gameover_your_scores.text = this.score.ToString();

		this.panel_play.SetActive(false);
		this.panel_gameover.SetActive(true);
		this.mouse_looker.LockCursor(false);
		this.shooter.enabled = false;
		this.play_sound(1);
	}

	public void targetHit(int scoreAmount, float timeAmount)
	{
		score += scoreAmount;
		mainScoreDisplay.text = score.ToString();

		currentTime += timeAmount;

		if (currentTime < 0) currentTime = 0.0f;

		mainTimerDisplay.text = currentTime.ToString("0.00");
	}

	public void btn_rate()
	{
		this.effec_bloom.enabled = false;
		this.carrot.show_rate();
	}

	public void btn_share()
    {
		this.effec_bloom.enabled = false;
		this.carrot.show_share();
    }

	public void btn_show_login()
    {
		this.effec_bloom.enabled = false;
		this.carrot.user.show_login(after_login_user);
    }

	private void after_login_user()
    {
		this.effec_bloom.enabled = false;
    }

	public void btn_show_rank()
    {
		this.is_show_rank = true;
		this.effec_bloom.enabled = false;
		this.carrot.game.Show_List_Top_player();
    }

	public void btn_show_setting()
    {
		this.effec_bloom.enabled = false;
		this.is_play = false;
		Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
		box_setting.set_act_before_closing(before_close_setting);
	}

	private void before_close_setting(List<string> list_emp_change)
    {
		foreach(string item_change in list_emp_change)
        {
			Debug.Log(item_change);
			if (item_change == "list_bk_music") this.carrot.game.load_bk_music(this.musicAudioSource);
			if (item_change == "sound")
            {
                if (this.carrot.get_status_sound())
                {
					this.carrot.game.load_bk_music(this.musicAudioSource);
					this.shooter.set_sound(true);
                }
                else
                {
					this.musicAudioSource.Stop();
					this.shooter.set_sound(false);
				}
			}
        }
		this.effec_bloom.enabled = true;
	}

	public void play_vibrate()
    {
		this.carrot.play_vibrate();
    }

	public void play_sound(int index_sound)
    {
		if(this.carrot.get_status_sound())this.sound[index_sound].Play();
    }

	public void btn_show_tip()
    {
		this.carrot.play_sound_click();
		this.panel_menu.SetActive(false);
		this.panel_tip.SetActive(true);
    }

	public void btn_closer_tip()
    {
		this.carrot.play_sound_click();
		this.panel_menu.SetActive(true);
		this.panel_tip.SetActive(false);
    }
}
