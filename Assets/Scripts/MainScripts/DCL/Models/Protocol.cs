using UnityEngine;

namespace DCL.Models
{
    public enum CLASS_ID
    {
        TRANSFORM = 1,
        UUID_CALLBACK = 8,

        BOX_SHAPE = 16,
        SPHERE_SHAPE = 17,
        PLANE_SHAPE = 18,
        CONE_SHAPE = 19,
        CYLINDER_SHAPE = 20,
        TEXT_SHAPE = 21,
        NFT_SHAPE = 22,

        UI_WORLD_SPACE_SHAPE = 23,
        UI_SCREEN_SPACE_SHAPE = 24,
        UI_CONTAINER_RECT = 25,
        UI_CONTAINER_STACK = 26,
        UI_TEXT_SHAPE = 27,
        UI_INPUT_TEXT_SHAPE = 28,
        UI_IMAGE_SHAPE = 29,
        UI_SLIDER_SHAPE = 30,

        CIRCLE_SHAPE = 31,
        BILLBOARD = 32,
        ANIMATOR = 33,

        GLTF_SHAPE = 54,
        OBJ_SHAPE = 55,
        BASIC_MATERIAL = 64,
        PBR_MATERIAL = 65,

        HIGHLIGHT_ENTITY = 66,
        SOUND = 67
    }

    public class CallMethodComponentMessage
    {
        public string methodName;
        public object[] args;
    }

    [System.Serializable]
    public class SharedComponentAttachMessage
    {
        /// id of the affected entity
        public string entityId;
        /// name of the compoenent
        public string name;
        /// ID of the disposable component
        public string id;

        public void FromJSON(string rawJson)
        {
            entityId = default(string);
            name = default(string);
            id = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class EntityComponentCreateMessage
    {
        /// id of the affected entity
        public string entityId;
        /// name of the compoenent
        public string name;
        /// class of the component that should be instantiated
        public int classId;

        public string json;

        public void FromJSON(string rawJson)
        {
            entityId = default(string);
            name = default(string);
            json = default(string);
            classId = default(int);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class SetEntityParentMessage
    {
        /// id of the affected entity
        public string entityId;
        /// id of the parent entity
        public string parentId;

        public void FromJSON(string rawJson)
        {
            entityId = default(string);
            parentId = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class CreateEntityMessage
    {
        /// id of the new entity
        public string id;

        public void FromJSON(string rawJson)
        {
            id = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class RemoveEntityMessage
    {
        /// id of the new entity
        public string id;

        public void FromJSON(string rawJson)
        {
            id = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class EntityComponentRemoveMessage
    {
        /// id of the affected entity
        public string entityId;
        /// name of the compoenent
        public string name;

        public void FromJSON(string rawJson)
        {
            entityId = default(string);
            name = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class SharedComponentUpdateMessage
    {
        /// ID of the disposable component
        public string id;
        public string json;

        public void FromJSON(string rawJson)
        {
            id = default(string);
            json = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class SharedComponentDisposeMessage
    {
        /// ID of the disposable component to dispose
        public string id;

        public void FromJSON(string rawJson)
        {
            id = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class SharedComponentCreateMessage
    {
        /// ID of the disposable component
        public string id;
        /// name of the compoenent
        public string name;
        /// class of the component that should be instantiated
        public int classId;

        public void FromJSON(string rawJson)
        {
            id = default(string);
            name = default(string);
            classId = default(int);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }

    [System.Serializable]
    public class UUIDCallbackMessage
    {
        /// ID of the event to trigger
        public string uuid;
        /// type of the event
        public string type;

        public void FromJSON(string rawJson)
        {
            uuid = default(string);
            type = default(string);

            JsonUtility.FromJsonOverwrite(rawJson, this);
        }
    }
}
