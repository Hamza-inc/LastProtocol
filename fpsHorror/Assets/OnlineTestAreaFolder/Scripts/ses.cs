using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class ses : NetworkBehaviour
{
    [SerializeField] private AudioClip voiceClip;
    int lastSample;
    AudioClip c;
    int FREQUENCY = 44100/2; //Default 44100
    int length = 1;

    void Start()
    {
        if (IsOwner)
        {
            //microphoneIcon = GameObject.Find("MicrophoneHolder").GetComponent<HoldGameobject>().Holder01;
            c = Microphone.Start(null, true, length, FREQUENCY);
            while (Microphone.GetPosition(null) < 0) { }
        }
    }

    void Update()
    {
        if (IsOwner)
        {
            if (Input.GetKey(KeyCode.V))
            {
                int pos = Microphone.GetPosition(null);
                int diff = pos - lastSample;
                if (diff > 0)
                {
                    float[] samples = new float[diff * c.channels];
                    c.GetData(samples, lastSample);
                    byte[] ba = ToByteArray(samples);

                    Cmd_SendServerRPC(ba, c.channels);
                }
                lastSample = pos;
            }

        }
    }

    [ClientRpc]
    public void Rpc_SendClientRPC(byte[] ba, int chan)
    {
        if (!IsOwner)
        {
            ReciveData(ba, chan);
        }
            
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void Cmd_SendServerRPC(byte[] ba, int chan)
    {
        //ReciveData(ba, chan);
        Rpc_SendClientRPC(ba, chan);
    }

    void ReciveData(byte[] ba, int chan)
    {
        float[] f = ToFloatArray(ba);
        GetComponent<AudioSource>().clip = AudioClip.Create("Sound", f.Length, chan, FREQUENCY, true, false);
        GetComponent<AudioSource>().clip.SetData(f, 0);
        if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
    }

    public byte[] ToByteArray(float[] floatArray)
    {
        int len = floatArray.Length * 4;
        byte[] byteArray = new byte[len];
        int pos = 0;
        foreach (float f in floatArray)
        {
            byte[] data = System.BitConverter.GetBytes(f);
            System.Array.Copy(data, 0, byteArray, pos, 4);
            pos += 4;
        }
        return byteArray;
    }

    public float[] ToFloatArray(byte[] byteArray)
    {
        int len = byteArray.Length / 4;
        float[] floatArray = new float[len];
        for (int i = 0; i < byteArray.Length; i += 4)
        {
            floatArray[i / 4] = System.BitConverter.ToSingle(byteArray, i);
        }
        return floatArray;
    }
}