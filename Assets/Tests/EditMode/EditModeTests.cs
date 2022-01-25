using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditModeTests
    {
        public GameObject ant = GameObject.Find("Manager");
        
        
        [Test]
        public void PitchShouldBeEqualOne()
        { 
            var script = ant.GetComponent<Play>();

            script.generated.pitch = 2;
            script.normalPitch();

            Assert.AreEqual(1, script.generated.pitch);
        }

        [Test]
        public void WWWShouldReturnDescrubedPath()
        {
            var script = ant.GetComponent<Play>();
            string path = "\\project\\abc";
            
            WWW request = script.GetAudioFromFile(path);

            Assert.AreEqual("http://localhost/project/abc", request.url);
        }
        
        [Test]
        public void DifferenceShouldBeNegativeTest()
        {
            var script = ant.GetComponent<Play>();
            script.sampleNote = 0;
            script.trackNote = 11;
            
            script.findDifference();
            
            Assert.AreEqual(-1, script.differenceBetween);
        }
        
        [Test]
        public void ScaleShouldBeCMajorTest()
        {
            var script = ant.GetComponent<Play>();
            script.majorProgression[0] = 2;
            script.majorProgression[1] = 2;
            script.majorProgression[2] = 1;
            script.majorProgression[3] = 2;
            script.majorProgression[4] = 2;
            script.majorProgression[5] = 2;
            script.majorProgression[6] = 1;
            script.differenceBetween = 0;
            script.sampleNote = 0;
            script.trackNote = 0;
            script.isMajor = true;
            script.fillScale();

            Debug.Log(script.scale);
            
            int[] cMajorScale = new int[8];
            cMajorScale[0] = 0;
            cMajorScale[1] = 2;
            cMajorScale[2] = 4;
            cMajorScale[3] = 5;
            cMajorScale[4] = 7;
            cMajorScale[5] = 9;
            cMajorScale[6] = 11;
            cMajorScale[7] = 12;
            
            Assert.AreEqual(cMajorScale, script.scale);
        }
    }
    
}
