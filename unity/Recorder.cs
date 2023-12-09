#if UNITY_EDITOR
using System.IO;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

namespace UnityEngine.Recorder.Examples
{
    public class Recorder : MonoBehaviour
    {
        RecorderController m_RecorderController;

        private void OnEnable()
        {
            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            m_RecorderController = new RecorderController(controllerSettings);
            var mediaOutputFolder = Path.Combine(Application.dataPath, "..", "SampleRecordings");

            var imageRecorder = ScriptableObject.CreateInstance<ImageRecorderSettings>();
            imageRecorder.name = "My Image Recorder";
            imageRecorder.Enabled = true;
            imageRecorder.OutputFormat = ImageRecorderSettings.ImageRecorderOutputFormat.PNG;
            imageRecorder.CaptureAlpha = false;
            imageRecorder.OutputFile = Path.Combine(mediaOutputFolder, "image_") + DefaultWildcard.Take;
            imageRecorder.imageInputSettings = new GameViewInputSettings
            {
                OutputWidth = 1920,
                OutputHeight = 1080,
            };

            controllerSettings.AddRecorderSettings(imageRecorder);
            controllerSettings.SetRecordModeToSingleFrame(0);
        }

        private void OnGUI()
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                m_RecorderController.PrepareRecording();
                m_RecorderController.StartRecording();
            }
        }
    }
}
#endif