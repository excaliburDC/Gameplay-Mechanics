using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumSwap : MonoBehaviour
{
    public int numSamples;

    private float[] curSpectrum;
    private float[] prevSpectrum;

    private void Start()
    {
        curSpectrum = new float[numSamples];
        prevSpectrum = new float[numSamples];
    }

    public void setCurSpectrum(float[] spectrum)
    {
        curSpectrum.CopyTo(prevSpectrum, 0);
        spectrum.CopyTo(curSpectrum, 0);
    }

    float calculateRectifiedSpectralFlux()
    {
        float sum = 0f;

        // Aggregate positive changes in spectrum data
        for (int i = 0; i < numSamples; i++)
        {
            sum += Mathf.Max(0f, curSpectrum[i] - prevSpectrum[i]);
        }
        return sum;
    }
}
