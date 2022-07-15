using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TakeScreencapture : MonoBehaviour
{
    [SerializeField] private string folder;
    [SerializeField] private resFactor resolutionFactor;
    
    private enum resFactor { x1, x2, x4, x8 };
    private int resFacInt;
    private int imgCounter;
    private int vidCounter;
    private int frameCounter;
    private string dataPath;
    private bool recAni;
    

    // Start is called before the first frame update
    void Start()
    {
        if (resolutionFactor == resFactor.x1) {
            resFacInt = 1;
        } else if (resolutionFactor == resFactor.x2) {
            resFacInt = 2;
        } else if (resolutionFactor == resFactor.x4) {
            resFacInt = 4;
        } else {
            resFacInt = 8;
        }

        if (String.IsNullOrEmpty(folder))
        {
            print ("Please add folder path in inpsector.");
            this.enabled = false;
            return;
        }

        imgCounter = 0;
        vidCounter = 0;
        recAni = false;

        print("Screencapture function enabled. In GameView, press the 's' key to save the current frame. To start and stop video capture press 'a'.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            if (recAni == false)
            {
                frameCounter = 0;
                recAni = true;
                print ("video recording started.");
            } else {
                recAni = false;
                vidCounter++;
                print ("video recording ended.");
            }
        }
        if (recAni == true)
        {
            string fileName = "vid_0" + vidCounter + "frame_0" + frameCounter + ".png";
            dataPath = folder + "/" + fileName;
            ScreenCapture.CaptureScreenshot(dataPath, resFacInt);
            frameCounter++;
            return;
        } else if (Input.GetKeyDown("s")) {
            string fileName = "img_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_0" + imgCounter + ".png";
            dataPath = folder + "/" + fileName;
            ScreenCapture.CaptureScreenshot(dataPath, resFacInt);
            print(fileName + " has been saved to: " + folder);
            imgCounter++;
            return;
        }
    }
}
