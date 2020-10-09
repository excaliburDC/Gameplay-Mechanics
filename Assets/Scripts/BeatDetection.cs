using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BeatDetection : MonoBehaviour
{
    public AudioSource song;
    public GameObject cube;

    float[] historyBuffer = new float[43];
    float[] curSpectrum = new float[1024];


    // Use this for initialization
    void Start()
    {
        song = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (song.isPlaying)
		{
			//compute instant sound energy
	        
	        song.GetSpectrumData(curSpectrum, 0, FFTWindow.BlackmanHarris);
			// float[] channelLeft = song.GetComponent<AudioSource>().GetSpectrumData(1024, 2, FFTWindow.Hamming);

			//float e = sumStereo(channelLeft, channelRight);
			float e = sumStereo(curSpectrum);
	
	        //compute local average sound energy
	        float E = sumLocalEnergy() / historyBuffer.Length; // E being the average local sound energy
	
	        //calculate variance
	        float sumV = 0;
	        for (int i = 0; i < 43; i++)
	            sumV += (historyBuffer[i] - E) * (historyBuffer[i] - E);
	
	        float V = sumV / historyBuffer.Length;
	        float constant = (float)((-0.0025714 * V) + 1.5142857);
	
	        float[] shiftingHistoryBuffer = new float[historyBuffer.Length]; // make a new array and copy all the values to it
	
	        for (int i = 0; i < (historyBuffer.Length - 1); i++)
	        { // now we shift the array one slot to the right
	            shiftingHistoryBuffer[i + 1] = historyBuffer[i]; // and fill the empty slot with the new instant sound energy
	        }
	
	        shiftingHistoryBuffer[0] = e;
	
	        for (int i = 0; i < historyBuffer.Length; i++)
	        {
	            historyBuffer[i] = shiftingHistoryBuffer[i]; //then we return the values to the original array
	        }
	
	        //float constant = 1.5f;
	
	        if (e > (constant * E))
	        { // now we check if we have a beat
	            cube.GetComponent<Renderer>().material.color = Color.red;
	        }
	        else
	        {
	            cube.GetComponent<Renderer>().material.color = Color.yellow;
	        }
	
	        Debug.Log("Avg local: " + E);
	        Debug.Log("Instant: " + e);
	        Debug.Log("History Buffer: " + historybuffer());
	
	        Debug.Log("sum Variance: " + sumV);
	        Debug.Log("Variance: " + V);
	
	        Debug.Log("Constant: " + constant);
	        Debug.Log("--------");
	    }
	
	    float sumStereo(float[] channel)
	    {
			float e = 0;
            for (int i = 0; i < channel.Length; i++)
            {
                float ToSquare = channel[i];
                e += (ToSquare * ToSquare);
            }
            return e;
        }
	
	    float sumLocalEnergy()
	    {
	        float E = 0;
	
	        for (int i = 0; i < historyBuffer.Length; i++)
	        {
	            E += historyBuffer[i] * historyBuffer[i];
	        }
	
	        return E;
	    }
	
	    string historybuffer()
	    {
	        string s = "";
	        for (int i = 0; i < historyBuffer.Length; i++)
	        {
	            s += (historyBuffer[i] + ",");
	        }
	        return s;
	    }
	}
}
