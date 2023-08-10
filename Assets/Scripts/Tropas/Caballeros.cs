using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caballeros : MonoBehaviour {

	public Admin_Casillas ScriptAdCas;
	public bool Seleccionado;

	public string Equipo;

	public Animator AnimCaballero;

	public Collider2D Col;

	public AudioSource BocinaEfectos;
	public AudioClip SonidoEspada;

	public int CasillaActual;
	public float DistMouse;
	public bool Moviendose;
	private float TiempoAnimCaminar;

	private GameObject TropaAMatar;

	public GameObject SeñalDir;
	public SpriteRenderer RenderApunta;
	public Sprite[] SpritesApunta;
	private Vector2 DirApuntado;

	public GameObject SeñalSeleccionado;

	public bool UsandoCata;
	public Catapulta ScriptCata;

	public bool UsandoAri;
	public Ariete ScriptAri;

	public bool UsandoTor;
	public Torre ScriptTor;

	void Update () {

		//Obtener la distancia entre esta tropa y el mouse:
		DistMouse = Vector2.Distance (transform.position, Camera.main.ScreenToWorldPoint (Input.mousePosition));

		//Mostrar señal a donde se está apuntado con el mouse para moverse:
		if (Seleccionado == true && DistMouse > 0.5f && DistMouse <= 1.6f) {
			SeñalDir.SetActive (true);
			DirApuntado = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			SeñalDir.transform.right = DirApuntado;
			RenderApunta.sprite = SpritesApunta [0];
		} else if (Seleccionado == true && DistMouse > 1.6f) {
			SeñalDir.SetActive (true);
			DirApuntado = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			SeñalDir.transform.right = DirApuntado;
			RenderApunta.sprite = SpritesApunta [1];
		} else if (Seleccionado == false || DistMouse <= 0.5f) {
			SeñalDir.SetActive (false);
		}

		//Mostrar señal al estar seleccionado este caballero:
		if (Seleccionado == true) {
			SeñalSeleccionado.SetActive (true);
		} else {
			SeñalSeleccionado.SetActive (false);
		}
			
		//Moverse al seleccionar este caballero seguido de seleccionar la casilla a desplazarse:
		if (Seleccionado == true && ScriptAdCas.SeleccionandoCasilla == true && ScriptAdCas.MoviendoTropa == false) {
			TiempoAnimCaminar = 0;
			ScriptAdCas.Casillas [CasillaActual].enabled = true;

			if(Equipo == "Rojo")
				ScriptAdCas.CasillasOcupadasRojo [CasillaActual] = false;
			if(Equipo == "Azul")
			    ScriptAdCas.CasillasOcupadasAzul [CasillaActual] = false;

			if (UsandoCata == true) {
				ScriptAdCas.CasillasOcupadasCata [CasillaActual] = false;
			}
			if (UsandoAri == true) {
				ScriptAdCas.CasillasOcupadasAri [CasillaActual] = false;
			}
			if (UsandoTor == true) {
				ScriptAdCas.CasillasOcupadasTor [CasillaActual] = false;
			}
			
			CasillaActual = ScriptAdCas.CasillaSeleccionada;

			if (UsandoCata == true) {
				ScriptCata.CasillaActual = CasillaActual;
			}
			if (UsandoAri == true) {
				ScriptAri.CasillaActual = CasillaActual;
			}
			if (UsandoTor == true) {
				ScriptTor.CasillaActual = CasillaActual;
			}

			if(Equipo == "Rojo")
			    ScriptAdCas.CasillasOcupadasRojo [ScriptAdCas.CasillaSeleccionada] = true;
			if(Equipo == "Azul")
				ScriptAdCas.CasillasOcupadasAzul [ScriptAdCas.CasillaSeleccionada] = true;

			if (UsandoCata == true) {
				ScriptAdCas.CasillasOcupadasCata [ScriptAdCas.CasillaSeleccionada] = true;
			}
			if (UsandoAri == true) {
				ScriptAdCas.CasillasOcupadasAri [ScriptAdCas.CasillaSeleccionada] = true;
			}
			if (UsandoTor == true) {
				ScriptAdCas.CasillasOcupadasTor [ScriptAdCas.CasillaSeleccionada] = true;
			}

			ScriptAdCas.Casillas [CasillaActual].enabled = false;
			AnimCaballero.Play ("Caballero_Caminar");
			Moviendose = true;
			ScriptAdCas.SeleccionandoCasilla = false;
			ScriptAdCas.SeleccionandoTropa = false;
			Seleccionado = false;
		}
		if (Moviendose == true) {
			ScriptAdCas.MoviendoTropa = true;
			transform.position = Vector2.Lerp (transform.position, ScriptAdCas.Casillas [ScriptAdCas.CasillaSeleccionada].transform.position, 0.03f);
			TiempoAnimCaminar += Time.deltaTime;

			if(TiempoAnimCaminar >= 2f){

				if (UsandoCata == true) {
					AnimCaballero.Play ("Caballero_Idle");
					ScriptCata.Disparar ();
					Seleccionado = false;
					//ScriptAdCas.MoviendoTropa = false; <--------- Corección de bug de repetir turno al usar una catapulta
					Moviendose = false;
				} else {
					AnimCaballero.Play ("Caballero_Idle");
					ScriptAdCas.Turno = !ScriptAdCas.Turno;

					ScriptAdCas.CasillaRoca = 70; // correcion de bug de la torre desaparecida:

					Seleccionado = false;
					ScriptAdCas.MoviendoTropa = false;
					Moviendose = false;
				}

			}
		}
			
	}
		
	//Función para seleccionar o no al caballero (En base a su turno y si no se esta moviendo alguna otra tropa al mismo tiempo):
	void OnMouseDown(){
		if (Equipo == "Rojo" && ScriptAdCas.Turno == false && ScriptAdCas.MoviendoTropa == false && ScriptAdCas.UsandoArquero == false
		|| Equipo == "Azul" && ScriptAdCas.Turno == true && ScriptAdCas.MoviendoTropa == false && ScriptAdCas.UsandoArquero == false) {
			Seleccionado = !Seleccionado;
			if (Seleccionado == true) {
				ScriptAdCas.SeleccionandoTropa = true;
				ScriptAdCas.ScriptTropaSelect = this;
			} else {
				ScriptAdCas.SeleccionandoTropa = false;
				//ScriptAdCas.ScriptTropaSelect = null;
			}
		}
	}
		
	void OnTriggerEnter2D(Collider2D Col){
		//Matar a la tropa enemiga al colisionar con ella:
		if (ScriptAdCas.Turno == false && Equipo == "Rojo" && Col.gameObject.tag == "CabAzul") {
			TropaAMatar = Col.gameObject;
			ScriptAdCas.CasillasOcupadasAzul [CasillaActual] = false;
			Invoke ("MatarRival", 0.3f);
		}
		if (ScriptAdCas.Turno == true && Equipo == "Azul" && Col.gameObject.tag == "CabRojo") {
			TropaAMatar = Col.gameObject;
			ScriptAdCas.CasillasOcupadasRojo [CasillaActual] = false;
			Invoke ("MatarRival", 0.3f);
		}

		//Morir si se es alcanzado por las flechas:
		if(Col.gameObject.tag == "Flechas" && Equipo == "Rojo" && UsandoTor == false){
			ScriptAdCas.CasillasOcupadasRojo [CasillaActual] = false;
			Destroy (gameObject);
		}
		if(Col.gameObject.tag == "Flechas" && Equipo == "Azul" && UsandoTor == false){
			ScriptAdCas.CasillasOcupadasAzul [CasillaActual] = false;
			Destroy (gameObject);
		}

		//Morir si se es alcanzado por las rocas:
		if(Col.gameObject.tag == "Roca" && Equipo == "Rojo" && UsandoAri == false){
			if (UsandoCata == true) {
				ScriptCata.Autodestruir ();
			}
			if (UsandoTor == true) {
				ScriptTor.Autodestruir ();
			}
			ScriptAdCas.CasillasOcupadasRojo [CasillaActual] = false;
			Destroy (gameObject);
		}
		if(Col.gameObject.tag == "Roca" && Equipo == "Azul" && UsandoAri == false){
			if (UsandoCata == true) {
				ScriptCata.Autodestruir ();
			}
			if (UsandoTor == true) {
				ScriptTor.Autodestruir ();
			}
			ScriptAdCas.CasillasOcupadasAzul [CasillaActual] = false;
			Destroy (gameObject);
		}

	}
	void MatarRival(){
		BocinaEfectos.PlayOneShot (SonidoEspada);
		Destroy (TropaAMatar);
	}

}