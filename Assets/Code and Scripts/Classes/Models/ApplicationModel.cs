using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel : MonoBehaviour
{
    /**
     * Some of the roles of models include the following.
     * --------------------------------------------------------------
     * 1. Holding core data and state information.
     * 2. Serializing / Deserializing and converting between types
     * 3. Loading / Saving Data
     * 4. Notifying controllers of the progress of operations
    **/
    public NetworkModel network;
    public VideoModel video;
    public UsersModel users;
}