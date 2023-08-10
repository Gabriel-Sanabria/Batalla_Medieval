using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Musica : MonoBehaviour {

	public static Musica ScriptMisma;
	private Admin_Partida ScriptAdPart;

	private int IndexEscena = 0;

	public AudioSource BocinaMusica;
	public AudioClip[] Canciones;

	public bool Muteado;
	private float TiempoMuerto;

	public Image ImgBotonMut;
	public Sprite[] SpritesBotonMut;

	void Awake(){
		//Conservar siempre solo una instancia de este objeto:
		if (ScriptMisma == null) {
			ScriptMisma = this;
			DontDestroyOnLoad (gameObject);
		} else if (ScriptMisma != this) {
			Destroy (gameObject);
		}
	}

	void Start(){
		//Obtener la ultima configuración del mute:
		if (PlayerPrefs.GetInt ("BatMedMute", 0) == 0) {
			Muteado = false;
			ImgBotonMut.sprite = SpritesBotonMut [0];
			BocinaMusica.enabled = true;
		} else {
			Muteado = true;
			ImgBotonMut.sprite = SpritesBotonMut [1];
			BocinaMusica.enabled = false;
		}
	}
		
	void Update () {

		//Si la escena abierta es la del menú, reproducir la canción 0, de lo contrario reproducir la canción correspondiente al mapa:
		if (SceneManager.GetActiveScene().buildIndex == 0)
			IndexEscena = 0;
		else
			IndexEscena = PlayerPrefs.GetInt("BatMedMapa", 0) + 1;

		//Obtener la script administradora de la partida (si no se esta en la escena del menu):
		if (IndexEscena != 0)
		{
			ScriptAdPart = GameObject.Find("Campo de Batalla").GetComponent<Admin_Partida>();
		}
		else {
			ScriptAdPart = null;
		}

		//Mientras no este muteada la musica:
		if (Muteado == false)
		{
			//Si se esta en la escena del menu:
			if (IndexEscena == 0)
			{
				//Regular el volumen de la musica en 0.15f:
				BocinaMusica.volume = 0.25f;

				//Buclear en la canción 0:
				if (BocinaMusica.isPlaying == false)
				{
					BocinaMusica.PlayOneShot(Canciones[0]);
				}
				TiempoMuerto = 0;
			}

			//Si NO se esta en la escena del menú:
			if (IndexEscena != 0)
			{
				//mientras no empiece la partida:
				if (ScriptAdPart.PartidaIniciada == false && ScriptAdPart.PartidaFinalizada == false)
				{

					//Hacer fundido de salida y buclear en la canción 0:
					if (BocinaMusica.isPlaying == false)
					{
						BocinaMusica.PlayOneShot(Canciones[0]);
					}
					if (BocinaMusica.volume > 0.15f)
					{
						BocinaMusica.volume -= 0.3f * Time.deltaTime;
					}
				}

				//Cuando empiece la partida:
				if (ScriptAdPart.PartidaIniciada == true && ScriptAdPart.PartidaFinalizada == false)
				{

					TiempoMuerto += Time.deltaTime;

					//Hacer fundido de salida y detener la musica cuando el volumen llegue a 0:
					if (TiempoMuerto <= 2.5f && BocinaMusica.volume > 0)
					{
						BocinaMusica.volume -= 0.3f * Time.deltaTime;
					}
					if (TiempoMuerto <= 2.5f && BocinaMusica.volume <= 0)
					{
						BocinaMusica.Stop();
					}

					//Despues de 2.5 seg, hacer fundido de entrada y buclear en la canción del mapa:
					if (TiempoMuerto > 2.5f && BocinaMusica.volume <= 0.50f)
					{
						BocinaMusica.volume += 0.3f * Time.deltaTime;
					}
					if (BocinaMusica.isPlaying == false && ScriptAdPart.Menu == false)
					{
						BocinaMusica.PlayOneShot(Canciones[IndexEscena]);
					}
				}

				//Al finalizar la partida:
				if (ScriptAdPart.PartidaFinalizada == true)
				{

					TiempoMuerto = 0;

					//Hacer fundido de salida:
					if (BocinaMusica.volume > 0)
					{
						BocinaMusica.volume -= 0.3f * Time.deltaTime;
					}
					if (BocinaMusica.volume <= 0)
					{
						BocinaMusica.Stop();
					}
				}

			}
		}
	}
		
	//Función para mutear la musica:
	public void Mutear(){
		Muteado = !Muteado;

		//Si no se esta en mute:
		if (Muteado == false) {
			ImgBotonMut.sprite = SpritesBotonMut [0];
			BocinaMusica.enabled = true;

			//Reproducir la canción adecuada según el caso:
			if (ScriptAdPart == null || ScriptAdPart != null && ScriptAdPart.PartidaIniciada == false)
			BocinaMusica.PlayOneShot (Canciones [0]);
			if(ScriptAdPart != null && ScriptAdPart.PartidaIniciada == true)
			BocinaMusica.PlayOneShot (Canciones [IndexEscena]);

			PlayerPrefs.SetInt("BatMedMute", 0);
		}

		//Si se esta en mute:
		if (Muteado == true) {
			ImgBotonMut.sprite = SpritesBotonMut [1];
			BocinaMusica.enabled = false;
			PlayerPrefs.SetInt("BatMedMute", 1);
		}
	}


}