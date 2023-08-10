using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_Arqueros : MonoBehaviour {

	public string Equipo;
	public GameObject[] Arqueros;
	public bool[] ArquerosActivos;
	public Arqueros ScriptArqueroSelect;
	public Admin_Casillas ScriptAdCas;

	public GameObject BotonesRojo;
	public GameObject BotonesAzul;

	public GameObject FlechasRojas;
	public GameObject FlechasAzules;
	private GameObject FlechasInstanciadas;

	public Vector2[] PosInicialRojas;
	public Vector2[] PosInicialAzules;

	public Animator[] AnimArqueros;

	public AudioSource BocinaArqueros;

	public bool Disparando;
	public GameObject ArqueroDisparando;
	private float TiempoDisparo;

	void Update () {

		//Seleccionar solo un arquero a la vez:
		foreach(GameObject Arquero in Arqueros){
			if (Arquero != null) {
				Arqueros ScriptArquero = Arquero.GetComponent<Arqueros> ();
				if (ScriptArquero != ScriptArqueroSelect) {
					ArquerosActivos [ScriptArquero.NumArquero] = false;
				}
			}
		}

		//Activar o desactivar los botones de las flechas:
		if (Disparando == false && ArqueroDisparando == null) {

			if (Equipo == "Rojo" && ArquerosActivos [0] == true && ArquerosActivos [1] == false) {
				BotonesRojo.SetActive (true);
			}
			if (Equipo == "Rojo" && ArquerosActivos [1] == true && ArquerosActivos [0] == false) {
				BotonesRojo.SetActive (true);
			}
			if (Equipo == "Rojo" && ArquerosActivos [0] == false && ArquerosActivos [1] == false) {
				BotonesRojo.SetActive (false);
			}

			if (Equipo == "Azul" && ArquerosActivos [0] == true && ArquerosActivos [1] == false) {
				BotonesAzul.SetActive (true);
			}
			if (Equipo == "Azul" && ArquerosActivos [1] == true && ArquerosActivos [0] == false) {
				BotonesAzul.SetActive (true);
			}
			if (Equipo == "Azul" && ArquerosActivos [0] == false && ArquerosActivos [1] == false) {
				BotonesAzul.SetActive (false);
			}

		}

		//Desactivar automaticamente los arqueros del equipo cuando no sea su turno:
		if (Equipo == "Rojo" && ScriptAdCas.Turno == true || Equipo == "Azul" && ScriptAdCas.Turno == false) {
			ArquerosActivos [0] = false;
			ArquerosActivos [1] = false;
		}

		//Disparar flechas (y ejecutar animaciones):
		if (Disparando == true) {

			if (ArquerosActivos [0] == true) {
				AnimArqueros [0].Play ("Arquero_Disparo");
				ArqueroDisparando = AnimArqueros [0].gameObject;

				if(Arqueros [1] != null)
				Arqueros [1].GetComponent<Collider2D> ().enabled = false;	
			}
			if (ArquerosActivos [1] == true) {
				AnimArqueros [1].Play ("Arquero_Disparo");
				ArqueroDisparando = AnimArqueros [1].gameObject;

				if(Arqueros [0] != null)
				Arqueros [0].GetComponent<Collider2D> ().enabled = false;
			}

			TiempoDisparo += Time.deltaTime;

			if (TiempoDisparo <= 1.65f) {
			FlechasInstanciadas.transform.position = 
			new Vector2 (FlechasInstanciadas.transform.position.x, FlechasInstanciadas.transform.position.y - 5f * Time.deltaTime);
				ScriptAdCas.UsandoArquero = true;
				BocinaArqueros.enabled = true;
			}

			if (TiempoDisparo > 1.65f) {
				
				if(ArquerosActivos[0] == true && Arqueros [1] != null)
					Arqueros [1].GetComponent<Collider2D> ().enabled = true;
				if(ArquerosActivos[1] == true && Arqueros [0] != null)
					Arqueros [0].GetComponent<Collider2D> ().enabled = true;
				
				Invoke ("DestruirFlechas", 0.2f);
				TiempoDisparo = 0;
				ScriptAdCas.Turno = !ScriptAdCas.Turno;
				BocinaArqueros.enabled = false;
				Disparando = false;
			}
		}

	}

	//FUNCIONES DISPARO ROJAS:
	public void Linea1Rojo(){
		FlechasInstanciadas = Instantiate (FlechasRojas);
		FlechasInstanciadas.transform.position = PosInicialRojas [0];
		Disparando = true;
		BotonesRojo.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea2Rojo(){
		FlechasInstanciadas = Instantiate (FlechasRojas);
		FlechasInstanciadas.transform.position = PosInicialRojas [1];
		Disparando = true;
		BotonesRojo.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea3Rojo(){
		FlechasInstanciadas = Instantiate (FlechasRojas);
		FlechasInstanciadas.transform.position = PosInicialRojas [2];
		Disparando = true;
		BotonesRojo.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea4Rojo(){
		FlechasInstanciadas = Instantiate (FlechasRojas);
		FlechasInstanciadas.transform.position = PosInicialRojas [3];
		Disparando = true;
		BotonesRojo.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea5Rojo(){
		FlechasInstanciadas = Instantiate (FlechasRojas);
		FlechasInstanciadas.transform.position = PosInicialRojas [4];
		Disparando = true;
		BotonesRojo.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}

	//FUNCIONES DISPARO AZULES:
	public void Linea1Azul(){
		FlechasInstanciadas = Instantiate (FlechasAzules);
		FlechasInstanciadas.transform.position = PosInicialAzules [0];
		Disparando = true;
		BotonesAzul.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea2Azul(){
		FlechasInstanciadas = Instantiate (FlechasAzules);
		FlechasInstanciadas.transform.position = PosInicialAzules [1];
		Disparando = true;
		BotonesAzul.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea3Azul(){
		FlechasInstanciadas = Instantiate (FlechasAzules);
		FlechasInstanciadas.transform.position = PosInicialAzules [2];
		Disparando = true;
		BotonesAzul.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea4Azul(){
		FlechasInstanciadas = Instantiate (FlechasAzules);
		FlechasInstanciadas.transform.position = PosInicialAzules [3];
		Disparando = true;
		BotonesAzul.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
	public void Linea5Azul(){
		FlechasInstanciadas = Instantiate (FlechasAzules);
		FlechasInstanciadas.transform.position = PosInicialAzules [4];
		Disparando = true;
		BotonesAzul.SetActive (false);
		ArqueroDisparando.GetComponent<Collider2D> ().enabled = false;
	}
		
	//Función para destruir las flechas lanzadas:
	void DestruirFlechas(){
		ScriptAdCas.UsandoArquero = false;
		Destroy (ArqueroDisparando);
		Destroy (FlechasInstanciadas);
	}

}