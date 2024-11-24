using Rokid.UXR.Module;
using UnityEngine;
using UnityEngine.Assertions;

namespace Rokid.UXR.Demo
{
    public class ImageTrackingSample : MonoBehaviour
    {
        [SerializeField]
        private GameObject leftUpAnchor;
        [SerializeField]
        private GameObject rightUpAnchor;
        [SerializeField]
        private GameObject leftBottomAnchor;
        [SerializeField]
        private GameObject rightBottomAnchor;
        [SerializeField]
        private GameObject quad;

        private bool isInitialize = false;
        void Start()
        {
            Assert.IsNotNull(leftUpAnchor);
            Assert.IsNotNull(rightUpAnchor);
            Assert.IsNotNull(leftBottomAnchor);
            Assert.IsNotNull(rightBottomAnchor);
            Assert.IsNotNull(quad);

            if (isInitialize == false)
            {
                isInitialize = true;
                leftUpAnchor.gameObject.SetActive(false);
                rightUpAnchor.gameObject.SetActive(false);
                leftBottomAnchor.gameObject.SetActive(false);
                rightBottomAnchor.gameObject.SetActive(false);
                quad.gameObject.SetActive(false);
            }
        }


        public void OnTrackedImageAdded(ARTrackedImage trackedImage)
        {
            isInitialize = true;
            leftUpAnchor.gameObject.SetActive(true);
            rightUpAnchor.gameObject.SetActive(true);
            leftBottomAnchor.gameObject.SetActive(true);
            rightBottomAnchor.gameObject.SetActive(true);
            quad.gameObject.SetActive(true);
            OnTrackedImageUpdated(trackedImage);
        }
        public void OnTrackedImageUpdated(ARTrackedImage trackedImage)
        {
            leftUpAnchor.transform.position = trackedImage.pose.position + trackedImage.pose.rotation * new Vector3(-trackedImage.bounds.extents.x, trackedImage.bounds.extents.y, 0);
            rightUpAnchor.transform.position = trackedImage.pose.position + trackedImage.pose.rotation * new Vector3(trackedImage.bounds.extents.x, trackedImage.bounds.extents.y, 0);
            leftBottomAnchor.transform.position = trackedImage.pose.position + trackedImage.pose.rotation * new Vector3(-trackedImage.bounds.extents.x, -trackedImage.bounds.extents.y, 0);
            rightBottomAnchor.transform.position = trackedImage.pose.position + trackedImage.pose.rotation * new Vector3(trackedImage.bounds.extents.x, -trackedImage.bounds.extents.y, 0);
            quad.transform.localScale = new Vector3(trackedImage.size.x / transform.parent.localScale.x, trackedImage.size.y / transform.parent.localScale.y, 1);
            quad.transform.position = trackedImage.pose.position;
            quad.transform.rotation = trackedImage.pose.rotation;
        }
        public void OnTrackedImageRemoved(ARTrackedImage trackedImage)
        {
            leftUpAnchor.gameObject.SetActive(false);
            rightUpAnchor.gameObject.SetActive(false);
            leftBottomAnchor.gameObject.SetActive(false);
            rightBottomAnchor.gameObject.SetActive(false);
            quad.gameObject.SetActive(false);
        }
    }

}
