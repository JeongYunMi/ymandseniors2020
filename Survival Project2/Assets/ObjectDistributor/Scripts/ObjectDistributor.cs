// Copyright 2018 Nick Alves - http://www.nickalves.com
// Object Distributor v2018.3.18

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Distributes gameObjects on the gameObject this script is attached to.</para>
/// <para>It works with axis and direction, either purely random, with edge avoidance, or based on a brightness mask.</para>
/// </summary>
[ExecuteInEditMode]
public class ObjectDistributor : MonoBehaviour
{
    #region VariablesAndProperties
    public bool m_bPreviewInScene = true;
    public int m_projectionAxis;
    public int m_projectionMethod;
    public bool m_bProjectionDirection;

    public bool m_bUseRandomXRotation = false;
    public bool m_bUseRandomYRotation = false;
    public bool m_bUseRandomZRotation = false;

    public float m_randomXRotationRange = 0;
    public float m_randomYRotationRange = 0;
    public float m_randomZRotationRange = 0;

    public int m_placementTries = 5;
    public float m_edgeAvoidance = 20;
    public Texture2D m_sampleImage;
    public float m_brightnessThreshold = .5f;
    public bool m_bInvertMask = false;

    /// <summary>
    /// Contains a gameObject, the user-specified amount, and if the up vector should be inverted.
    /// </summary>
    [System.Serializable]
    public struct PlacableObject
    {
        [SerializeField]
        private GameObject m_gameObject;
        [SerializeField]
        private uint m_amount;
        [SerializeField]
        private bool m_invertUpVector;

        public uint Amount
        {
            get { return m_amount; }
        }

        public GameObject Object
        {
            get { return m_gameObject; }
        }

        public bool InvertUp
        {
            get { return m_invertUpVector; }
        }
    }

    public PlacableObject[] m_objects;

    //Group for the placed objects
    public GameObject m_bufferObject;

    private float m_extentMagnitude;
    private Vector3 m_direction;
    private float m_extentScale;

    private Bounds m_bounds;
    private Collider m_collider;
    private Renderer m_renderer;
    #endregion

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        m_collider = GetComponent<Collider>();
        if (m_renderer)
        {
            m_bounds = m_renderer.bounds;
        }
    }
    /// <summary>
    /// Determines directions, possible raycast position ranges, attaches a collider if needed, and calls the specified placement method
    /// </summary>
    public void Execute()
    {
        if (m_bufferObject)
        {
            CleanUp();
        }
        m_bufferObject = new GameObject("Distributed Objects");
        m_bufferObject.transform.parent = gameObject.transform;
        m_bufferObject.transform.position = Vector3.zero;

        bool bHadCollider = true;

        if (m_collider == null)
        {
            bHadCollider = false;
            m_collider = gameObject.AddComponent<MeshCollider>();

            //Notify the system that the collider was attached
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
       
        Vector3 objProjectionCenter = GetBounds().center;
        Vector3 objExtent = GetBounds().extents;

        //Scales the projection when the edge avoidance method is chosen
        m_extentScale = (m_projectionMethod == 1) ? m_extentScale = ((100 - m_edgeAvoidance) / 100) : 1;

        Vector4 rayRange = Vector4.zero; //x, y = min, max width ; z, w = min, max height

        //Sets center to the respective center of the side that's being projected from and adjusts the range accordingly
        switch (m_projectionAxis)
        {
            case 0: //X
                objProjectionCenter.x += m_bProjectionDirection ? -objExtent.x : objExtent.x;
                rayRange.z = objProjectionCenter.y - objExtent.y * m_extentScale;
                rayRange.w = objProjectionCenter.y + objExtent.y * m_extentScale;
                rayRange.x = objProjectionCenter.z - objExtent.z * m_extentScale;
                rayRange.y = objProjectionCenter.z + objExtent.z * m_extentScale;
                m_extentMagnitude = objExtent.x * 2;
                m_direction = m_bProjectionDirection ? Vector3.right : Vector3.left;
                break;
            case 1: //Y
                rayRange.z = objProjectionCenter.x - objExtent.x * m_extentScale;
                rayRange.w = objProjectionCenter.x + objExtent.x * m_extentScale;
                objProjectionCenter.y += m_bProjectionDirection ? -objExtent.y : objExtent.y;
                rayRange.x = objProjectionCenter.z - objExtent.z * m_extentScale;
                rayRange.y = objProjectionCenter.z + objExtent.z * m_extentScale;
                m_extentMagnitude = objExtent.y * 2;
                m_direction = m_bProjectionDirection ? Vector3.up : Vector3.down;
                break;
            case 2: //Z
                rayRange.x = objProjectionCenter.x - objExtent.x * m_extentScale;
                rayRange.y = objProjectionCenter.x + objExtent.x * m_extentScale;
                rayRange.z = objProjectionCenter.y - objExtent.y * m_extentScale;
                rayRange.w = objProjectionCenter.y + objExtent.y * m_extentScale;
                objProjectionCenter.z += m_bProjectionDirection ? -objExtent.z : objExtent.z;
                m_extentMagnitude = objExtent.z * 2;
                m_direction = m_bProjectionDirection ? Vector3.forward : Vector3.back;
                break;
            default:
                break;
        }
        switch (m_projectionMethod)
        {
            case 2:
                BrightnessTextureBased(objProjectionCenter, rayRange, m_collider);
                break;
            default: //Used for option 'true random', 'avoid edges', and default
                Random(objProjectionCenter, rayRange, m_collider);
                break;
        }
        if (!bHadCollider)
        {
            DestroyImmediate(m_collider);
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }

        if (m_bufferObject.transform.childCount == 0)
        {
            CleanUp();
            Debug.LogWarning("OBJECT DISTRIBUTOR: Couldn't place objects using the given settings");
        }

    }
    /// <summary>
    /// Randomly places gameObjects on the gameObject this script is attached to with regards to the projection direction, axis,
    /// and greyscale values of the previously assigned texture
    /// </summary>
    /// <param name="objProjectionCenter">Relative center depending on the projection direction and axis</param>
    /// <param name="range">Bound range in worldspace depending on the projection direction</param>
    /// <param name="objCollider">This gameObject's (temporary) collider</param>
    private void BrightnessTextureBased(Vector3 objProjectionCenter, Vector4 range, Collider objCollider)
    {
        Color[] samples;
        Vector2Int sampleSize;
        List<Vector2Int> validPositions = new List<Vector2Int>();

        try
        {
            //Get all pixels from the texture
            samples = m_sampleImage.GetPixels();
            sampleSize = new Vector2Int(m_sampleImage.width, m_sampleImage.height);
            for (int i = 0; i < samples.Length; ++i)
            {
                if ((m_bInvertMask ? 1 - samples[i].grayscale : samples[i].grayscale) >= m_brightnessThreshold)
                {
                    //Recalculate its pixel position
                    int x = i % sampleSize.y;
                    int y = (i - x) / sampleSize.y;

                    //Add it to the list
                    validPositions.Add(new Vector2Int(x, y));
                }
            }
        }
        catch (Exception)
        {
            Debug.LogWarning("OBJECT DISTRIBUTOR: Texture not readable. Set the texture to readable in the Texture Import Settings. The texture won't be altered.");
            return;
        }

        if (validPositions.Count == 0)
        {
            Debug.LogError("OBJECT DISTRIBUTOR: Mask texture doesn't have any valid pixels using the given settings");
            return;
        }
        //Delegate for the projection axis specific ray alignment
        Func<Vector2, Vector3> GetRayStartPos = null;
        //Delegate for the projection axis specific texture scaling and alignment
        Func<Vector2Int, Vector2, Vector4, Vector2> RemapTexToExtent = null;

        switch (m_projectionAxis)
        {
            case 0: //X
                GetRayStartPos = (position) => new Vector3(objProjectionCenter.x, position.y, position.x);
                RemapTexToExtent = (texPos, texRange, extentRange) => new Vector2(RemapFloat(texPos.x, 0, texRange.x, extentRange.x, extentRange.y),
                                                                                  RemapFloat(texPos.y, 0, texRange.y, extentRange.z, extentRange.w));
                break;
            case 1: //Y
                GetRayStartPos = (position) => new Vector3(position.x, objProjectionCenter.y, position.y);
                RemapTexToExtent = (texPos, texRange, extentRange) => new Vector2(RemapFloat(texPos.x, 0, texRange.x, extentRange.z, extentRange.w),
                                                                                  RemapFloat(texPos.y, 0, texRange.y, extentRange.x, extentRange.y));
                break;
            case 2: //Z
                GetRayStartPos = (position) => new Vector3(position.x, position.y, objProjectionCenter.z);
                RemapTexToExtent = (texPos, texRange, extentRange) => new Vector2(RemapFloat(texPos.x, 0, texRange.x, extentRange.x, extentRange.y),
                                                                                  RemapFloat(texPos.y, 0, texRange.y, extentRange.z, extentRange.w));
                break;
        }

        Vector2Int validPosition;
        RaycastHit hit = new RaycastHit();

        //For each unique object
        foreach (PlacableObject obj in m_objects)
        {
            //For how often that object is meant to be placed
            for (uint i = 0; i < obj.Amount; ++i)
            {
                //Try placing objects by shooting random rays at the base object. 
                //If nothing is hit for x tries, don't place anything
                for (int j = 0; j < m_placementTries; ++j)
                {
                    if (validPositions.Count > 0)
                    {
                        int position = UnityEngine.Random.Range(0, validPositions.Count);
                        validPosition = validPositions[position];
                        validPositions.RemoveAt(position);


                        Vector3 startPos = GetRayStartPos(RemapTexToExtent(validPosition, sampleSize, range));
                        Physics.Raycast(startPos - m_direction, m_direction, out hit, m_extentMagnitude + 2);

                        if (hit.collider == objCollider)
                        {
                            PlaceObject(hit, obj);
                            break;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("OBJECT DISTRIBUTOR: Tried placing more objects than there are positions");
                        return;
                    }
                }
            }
        }

    }
    /// <summary>
    /// Randomly places gameObjects on the gameObject this script is attached to with regards to the projection direction and axis
    /// </summary>
    /// <param name="objProjectionCenter">Relative center depending on the projection direction and axis</param>
    /// <param name="range">Bound range in worldspace depending on the projection direction</param>
    /// <param name="objCollider">This gameObject's (temporary) collider</param>
    private void Random(Vector3 objProjectionCenter, Vector4 range, Collider objCollider)
    {
        //Delegate for the projection axis specific random start position
        Func<Vector3> RandomStartPosFromDir = null;

        switch (m_projectionAxis)
        {
            case 0: //X
                RandomStartPosFromDir = () => new Vector3(objProjectionCenter.x,
                                                          UnityEngine.Random.Range(range.z, range.w),
                                                          UnityEngine.Random.Range(range.x, range.y));
                break;
            case 1: //Y
                RandomStartPosFromDir = () => new Vector3(UnityEngine.Random.Range(range.z, range.w),
                                                          objProjectionCenter.y,
                                                          UnityEngine.Random.Range(range.x, range.y));
                break;
            case 2: //Z
                RandomStartPosFromDir = () => new Vector3(UnityEngine.Random.Range(range.x, range.y),
                                                          UnityEngine.Random.Range(range.z, range.w),
                                                          objProjectionCenter.z);
                break;
        }
        RaycastHit hit = new RaycastHit();
        //For each unique object
        foreach (PlacableObject obj in m_objects)
        {
            //For how often that object is meant to be placed
            for (uint i = 0; i < obj.Amount; ++i)
            {
                //Try placing objects by shooting random rays at the base object. 
                //If nothing is hit for x tries, don't place anything
                for (int j = 0; j < m_placementTries; ++j)
                {
                    Vector3 startPos = RandomStartPosFromDir();
                    Physics.Raycast(startPos - m_direction, m_direction, out hit, m_extentMagnitude + 2);
                    if (hit.collider == objCollider)
                    {
                        PlaceObject(hit, obj);
                        break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Places the object at the hit point and aligns it with the normal direction
    /// </summary>
    /// <param name="hit">hit point on the surface of another GameObject</param>
    /// <param name="obj">Object to place</param>
    private void PlaceObject(RaycastHit hit, PlacableObject obj)
    {
        GameObject spawnedObject = Instantiate<GameObject>(obj.Object, hit.point, obj.Object.transform.rotation);
        //Rotates the object to align with the normal direction of the hit surface
        spawnedObject.transform.rotation = Quaternion.FromToRotation(spawnedObject.transform.up * (obj.InvertUp ? -1 : 1), hit.normal);
        //Adds the object to the group
        spawnedObject.transform.parent = m_bufferObject.transform;
        RandomlyRotate(spawnedObject);
    }
    /// <summary>
    /// Applies random euler rotation to object
    /// </summary>
    /// <param name="spawnedObject">Object to rotate</param>
    private void RandomlyRotate(GameObject spawnedObject)
    {
        Vector3 randomEulerRotation = new Vector3(m_bUseRandomXRotation ? UnityEngine.Random.Range(-m_randomXRotationRange, m_randomXRotationRange) : 0,
                                                  m_bUseRandomYRotation ? UnityEngine.Random.Range(-m_randomYRotationRange, m_randomYRotationRange) : 0,
                                                  m_bUseRandomZRotation ? UnityEngine.Random.Range(-m_randomZRotationRange, m_randomZRotationRange) : 0);

        spawnedObject.transform.rotation *= Quaternion.Euler(randomEulerRotation);
    }
    /// <summary>
    /// Removes unsaved distributed objects by destroying their group
    /// </summary>
    public void CleanUp()
    {
        DestroyImmediate(m_bufferObject);
        GetBounds();
    }
    /// <summary>
    /// Saves distributed objects
    /// </summary>
    public void Save()
    {
        m_bufferObject = null;
    }
    /// <summary>
    /// Returns render bounds of the gameObject this script is attached to
    /// </summary>
    public Bounds GetBounds()
    {
        if (m_renderer)
        {
            return m_renderer.bounds;
        }
        //In case we're not working with a normal gameObject that has a normal renderer (Terrain for example)
        if (m_collider)
        {
            return m_collider.bounds;
        }
        return m_bounds;
    }
    /// <summary>
    /// Sets the bounds of this gameObject manually
    /// </summary>
    /// <param name="inBounds">the bounds the object should have</param>
    public void SetBounds(Bounds inBounds)
    {
        m_bounds = inBounds;
    }
    /// <summary>
    /// Remaps a given value from range a-b to range c-d
    /// </summary>
    /// <param name="value">value between a and b</param>
    /// <param name="a">lower bound of the first range</param>
    /// <param name="b">upper bound of the first range</param>
    /// <param name="c">lower bound of the second range</param>
    /// <param name="d">upper bound of the second range</param>
    /// <returns></returns>
    private float RemapFloat(float value, float a, float b, float c, float d)
    {
        return (value - a) * (d - c) / (b - a) + c;
    }
}
