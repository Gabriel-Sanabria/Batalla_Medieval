using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casillas : MonoBehaviour {

	public Admin_Casillas ScriptAdCas;
	public Admin_Partida ScriptAdPart;
	public int NumCasilla;
	public Collider2D ColCasilla;

	//Cuando esta casilla se seleccione:
	void OnMouseDown(){
		//Pasar el valor de la casilla al admin (Solo si la distancia de la tropa seleccionada con el mouse abarca una casilla):
		if (ScriptAdCas.SeleccionandoTropa == true && ScriptAdCas.ScriptTropaSelect.DistMouse < 1.6f || ScriptAdPart.PartidaIniciada == false) {
			ScriptAdCas.CasillaSeleccionada = NumCasilla;

			//Insertar la maquina seleccionada en caso de que no haya empezado la partida:
			if (ScriptAdPart.PartidaIniciada == false)
				ScriptAdPart.InsertarMaquina ();

			//Si ya empezó la partida, activar el booleano "SeleccionandoCasilla" para poder mover las tropas:
			if(ScriptAdPart.PartidaIniciada == true)
			ScriptAdCas.SeleccionandoCasilla = true;
		}
	}

	//Detectar cuando una roca golpee esta casilla:
	void OnTriggerEnter2D(Collider2D ColCasilla){
		if (ColCasilla.gameObject.tag == "Roca") {
			ScriptAdCas.CasillaRoca = NumCasilla;
		}
	}

}